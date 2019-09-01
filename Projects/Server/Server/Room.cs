﻿//Rambo Team
using System;
using BeardedManStudios.Forge.Networking;
using RamboTeam.Common;

namespace RamboTeam.Server
{
	class Room : LogicObjects
	{
		private BufferStream buffer = null;

		public NetworkingPlayer MasterPlayer
		{
			get;
			private set;
		}

		public NetworkingPlayer SecondaryPlayer
		{
			get;
			private set;
		}

		public bool IsFull
		{
			get { return (MasterPlayer != null && SecondaryPlayer != null); }
		}

		public Room(Application Application, NetworkingPlayer MasterPlayer) :
			base(Application)
		{
			this.MasterPlayer = MasterPlayer;
			buffer = new BufferStream(new byte[64]);
		}

		public void HandleRequest(BufferStream Buffer, NetworkingPlayer Player)
		{
			byte command = Buffer.ReadByte();

			if (command == Commands.Room.SYNC_CHOPTER_TRANSFORM ||
				command == Commands.Room.SYNC_PILOT_FIRE ||
				command == Commands.Room.SYNC_ENEMY_FIRE)
			{
				if (SecondaryPlayer != null)
					Send(SecondaryPlayer, Buffer);
			}
			else if (command == Commands.Room.SYNC_CO_PILOT_FIRE)
			{
				if (MasterPlayer != null)
					Send(MasterPlayer, Buffer);
			}
		}

		public void SetSecondaryPlayer(NetworkingPlayer Player)
		{
			SecondaryPlayer = Player;
		}

		public void HandlePlayerDisconnection(NetworkingPlayer Player)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.END_GAME);

			if (Player == MasterPlayer)
			{
				if (SecondaryPlayer != null)
					Send(SecondaryPlayer, buffer);
			}
			else
			{
				if (MasterPlayer != null)
					Send(MasterPlayer, buffer);
			}
		}
	}
}