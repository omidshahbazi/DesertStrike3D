//Rambo Team
using System;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using RamboTeam.Common;

namespace RamboTeam.Server
{
	class Room : LogicObjects
	{
		public const int MAX_PLAYER_COUNT = 2;

		private BufferStream buffer = null;
		private List<NetworkingPlayer> players = null;

		public NetworkingPlayer PilotPlayer
		{
			get;
			private set;
		}

		public NetworkingPlayer CoPilotPlayer
		{
			get;
			private set;
		}

		public bool IsFull
		{
			get { return players.Count == MAX_PLAYER_COUNT; }
		}

		public Room(Application Application) :
			base(Application)
		{
			buffer = new BufferStream(new byte[64]);

			players = new List<NetworkingPlayer>();
		}

		public void HandleRequest(BufferStream Buffer, NetworkingPlayer Player)
		{
			byte command = Buffer.ReadByte();

			if (command == Commands.Room.SYNC_CHOPTER_TRANSFORM ||
				command == Commands.Room.SYNC_PILOT_FIRE ||
				command == Commands.Room.SYNC_ENEMY_FIRE)
			{
				if (CoPilotPlayer != null)
					Send(CoPilotPlayer, Buffer);
			}
			else if (command == Commands.Room.SYNC_CO_PILOT_FIRE)
			{
				if (PilotPlayer != null)
					Send(PilotPlayer, Buffer);
			}
			else if (command == Commands.Room.BECOME_PILOT)
			{
				if (PilotPlayer == null)
				{
					PilotPlayer = Player;

					buffer.Reset();
					buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.BECOME_PILOT);
					Send(PilotPlayer, buffer);

					buffer.Reset();
					buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.PILOT_RESERVED);
					SendToAll(PilotPlayer);

					if (CoPilotPlayer != null && CoPilotPlayer.IPEndPointHandle == Player.IPEndPointHandle)
					{
						CoPilotPlayer = null;

						buffer.Reset();
						buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.CO_PILOT_RELEASED);
						SendToAll();
					}
				}
			}
			else if (command == Commands.Room.BECOME_CO_PILOT)
			{
				if (CoPilotPlayer == null)
				{
					CoPilotPlayer = Player;

					buffer.Reset();
					buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.BECOME_CO_PILOT);
					Send(CoPilotPlayer, buffer);

					buffer.Reset();
					buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.CO_PILOT_RESERVED);
					SendToAll(CoPilotPlayer);

					if (PilotPlayer != null && PilotPlayer.IPEndPointHandle == Player.IPEndPointHandle)
					{
						PilotPlayer = null;

						buffer.Reset();
						buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.PILOT_RELEASED);
						SendToAll();
					}
				}
			}
		}

		public void AddPlayer(NetworkingPlayer Player)
		{
			players.Add(Player);
		}

		public void HandlePlayerDisconnection(NetworkingPlayer Player)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.END_GAME);

			if (Player == PilotPlayer)
			{
				if (CoPilotPlayer != null)
					Send(CoPilotPlayer, buffer);
			}
			else
			{
				if (PilotPlayer != null)
					Send(PilotPlayer, buffer);
			}
		}

		public bool ContainsPlayer(NetworkingPlayer Player)
		{
			for (int i = 0; i < players.Count; ++i)
			{
				if (players[i].IPEndPointHandle == Player.IPEndPointHandle)
					return true;
			}

			return false;
		}

		private void SendToAll(NetworkingPlayer Except = null)
		{
			for (int i = 0; i < players.Count; ++i)
				if (players[i] != Except)
					Send(players[i], buffer);
		}

		public override string ToString()
		{
			return (PilotPlayer == null ? "[No Player]" : PilotPlayer.IPEndPointHandle.ToString()) + " with " + (CoPilotPlayer == null ? "[No Player]" : CoPilotPlayer.IPEndPointHandle.ToString());
		}
	}
}