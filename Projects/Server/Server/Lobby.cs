//Rambo Team
using BeardedManStudios.Forge.Networking;
using RamboTeam.Common;
using System;
using System.Collections.Generic;

namespace RamboTeam.Server
{
	class Lobby : LogicObjects
	{
		private List<Room> rooms = null;
		private BufferStream buffer = null;

		public Lobby(Application Application) :
			base(Application)
		{
			rooms = new List<Room>();
			buffer = new BufferStream(new byte[64]);
		}

		public void HandlePlayerDisconnection(NetworkingPlayer Player)
		{
			Room room = FindRoom(Player);

			if (room != null)
			{
				room.HandlePlayerDisconnection(Player);
				rooms.Remove(room);

				Log("Room " + room + " removed");
			}
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

			room.AddPlayer(Player);

			buffer.Reset();

			if (room.IsFull)
				Log("Player joined to roo " + room);
			else
				Log("Room " + room + " created");

			buffer.WriteBytes(Commands.Category.LOBBY, Commands.Lobby.JOIN_TO_ROOM);
			buffer.WriteBool(room.HasPilot);
			buffer.WriteBool(room.HasCoPilot);

			Send(Player, buffer);
		}

		public Room FindRoom(NetworkingPlayer Player)
		{
			for (int i = 0; i < rooms.Count; ++i)
			{
				Room room = rooms[i];

				if (room.ContainsPlayer(Player))
					return room;
			}

			return null;
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

			room = new Room(Application);

			rooms.Add(room);

			return room;
		}
	}
}