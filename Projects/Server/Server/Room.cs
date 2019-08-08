//Rambo Team
using System;
using BeardedManStudios.Forge.Networking;
using RamboTeam.Common;

namespace RamboTeam.Server
{
	class Room : LogicObjects
	{
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
		}

		public void HandleRequest(BufferStream Buffer, NetworkingPlayer Player)
		{
			byte command = Buffer.ReadByte();

			if (command == Commands.Room.SYNC_CHOPTER_TRANSFORM)
			{
				System.Console.WriteLine("Sync");

				if (SecondaryPlayer != null)
					Send(SecondaryPlayer, Buffer);
			}
		}

		public void SetSecondaryPlayer(NetworkingPlayer Player)
		{
			SecondaryPlayer = Player;
		}
	}
}