//Rambo Team
using BeardedManStudios.Forge.Networking;
using RamboTeam.Common;
using System.Collections.Generic;

namespace RamboTeam.Server
{
	class Lobby : LogicObjects
	{
		private List<Room> rooms = new List<Room>();

		public Lobby(Application Application) :
			base(Application)
		{
		}

		public void HandleRequest(BufferStream Buffer, NetworkingPlayer Player)
		{
			byte command = Buffer.ReadByte();

			if (command == Commands.Lobby.JOIN_TO_ROOM)
			{
				JoinToRoom(Player);
			}
		}

		private void JoinToRoom(NetworkingPlayer Player)
		{
			Room room = GetAnEmptyRoom(Player);

			if (room.MasterPlayer != Player)
				room.SetSecondaryPlayer(Player);

			//Send(Player, )
		}

		private Room GetAnEmptyRoom(NetworkingPlayer Player)
		{
			Room room = null;

			for (int i = 0; i < rooms.Count; ++i)
			{
				room = rooms[i];

				if (room.IsFull)
					continue;

				return room;
			}

			room = new Room(Application, Player);

			rooms.Add(room);

			return room;
		}

		public Room FindRoom(NetworkingPlayer Player)
		{
			for (int i = 0; i < rooms.Count; ++i)
			{
				Room room = rooms[i];

				if (room.MasterPlayer == Player || room.SecondaryPlayer == Player)
					return room;
			}

			return null;
		}
	}
}