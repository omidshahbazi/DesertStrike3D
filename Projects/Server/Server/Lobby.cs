﻿//Rambo Team
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

			buffer.Reset();

			if (room.MasterPlayer == Player)
			{
				buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.MASTER);

				Log("Room " + room + " created");
			}
			else
			{
				buffer.WriteBytes(Commands.Category.ROOM, Commands.Room.SECONDARY);
				room.SetSecondaryPlayer(Player);

				Log("Player joined to roo " + room);
			}

			Send(Player, buffer);
		}

		public Room FindRoom(NetworkingPlayer Player)
		{
			for (int i = 0; i < rooms.Count; ++i)
			{
				Room room = rooms[i];

				if ((room.MasterPlayer != null && room.MasterPlayer.IPEndPointHandle == Player.IPEndPointHandle) || 
					(room.SecondaryPlayer != null && room.SecondaryPlayer.IPEndPointHandle == Player.IPEndPointHandle))
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

			room = new Room(Application, Player);

			rooms.Add(room);

			return room;
		}
	}
}