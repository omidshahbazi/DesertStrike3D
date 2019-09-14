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

		private NetworkingPlayer pilotPlayer = null;
		private NetworkingPlayer coPilotPlayer = null;

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
				if (coPilotPlayer != null)
					Send(coPilotPlayer, Buffer);
			}
			else if (command == Commands.Room.SYNC_CO_PILOT_FIRE)
			{
				if (pilotPlayer != null)
					Send(pilotPlayer, Buffer);
			}
			else if (command == Commands.Room.BECOME_PILOT)
			{
				if (pilotPlayer == null)
				{
					pilotPlayer = Player;

					buffer.Reset();
					buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.BECOME_PILOT);
					Send(pilotPlayer, buffer);

					buffer.Reset();
					buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.PILOT_RESERVED);
					SendToAll(pilotPlayer);

					if (coPilotPlayer != null && coPilotPlayer.IPEndPointHandle == Player.IPEndPointHandle)
					{
						coPilotPlayer = null;

						buffer.Reset();
						buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.CO_PILOT_RELEASED);
						SendToAll();
					}
				}
			}
			else if (command == Commands.Room.BECOME_CO_PILOT)
			{
				if (coPilotPlayer == null)
				{
					coPilotPlayer = Player;

					buffer.Reset();
					buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.BECOME_CO_PILOT);
					Send(coPilotPlayer, buffer);

					buffer.Reset();
					buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.CO_PILOT_RESERVED);
					SendToAll(coPilotPlayer);

					if (pilotPlayer != null && pilotPlayer.IPEndPointHandle == Player.IPEndPointHandle)
					{
						pilotPlayer = null;

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

			if (Player == pilotPlayer)
			{
				if (coPilotPlayer != null)
					Send(coPilotPlayer, buffer);
			}
			else
			{
				if (pilotPlayer != null)
					Send(pilotPlayer, buffer);
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
			return (pilotPlayer == null ? "[No Player]" : pilotPlayer.IPEndPointHandle.ToString()) + " with " + (coPilotPlayer == null ? "[No Player]" : coPilotPlayer.IPEndPointHandle.ToString());
		}
	}
}