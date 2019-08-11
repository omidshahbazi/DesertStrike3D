//Rambo Team
using RamboTeam.Common;
using UnityEngine;

namespace RamboTeam.Client
{
	public delegate void NetworkEventHandler();
	public delegate void SyncChopterTransformEventHandler(Vector3 Position, Vector3 Rotation);

	public static class NetworkCommands
	{
		private static BufferStream buffer = new BufferStream(new byte[64]);

		public static event NetworkEventHandler OnJoinedToRoom;
		public static event NetworkEventHandler OnPilot;
		public static event NetworkEventHandler OnCommando;
		public static event SyncChopterTransformEventHandler OnSyncChopterTransform;

		public static void JoinToRoom()
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.LOBBY);
			buffer.WriteBytes(Commands.Lobby.JOIN_TO_ROOM);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void SyncChopterTransform(Vector3 Position, Vector3 Rotation)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_CHOPTER_TRANSFORM);
			WriteVector3(Position);
			WriteVector3(Rotation);

			NetworkLayer.Instance.Send(buffer);
		}

		//public static void SyncChopterTransform(Vector3 Position, Vector3 Rotation)
		//{
		//	buffer.Reset();
		//	buffer.WriteBytes(Commands.Category.ROOM);
		//	buffer.WriteBytes(Commands.Room.SYNC_CHOPTER_TRANSFORM);
		//	WriteVector3(Position);
		//	WriteVector3(Rotation);

		//	NetworkLayer.Instance.Send(buffer);
		//}

		public static void HandleJoinedToRoom(BufferStream Buffer)
		{
			if (OnJoinedToRoom != null)
				OnJoinedToRoom();
		}

		public static void HandlePilot(BufferStream Buffer)
		{
			if (OnPilot != null)
				OnPilot();
		}

		public static void HandleCommando(BufferStream Buffer)
		{
			if (OnCommando != null)
				OnCommando();
		}

		public static void HandleSyncChopterTransform(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			Vector3 rot = Vector3.zero;
			rot.x = Buffer.ReadFloat32();
			rot.y = Buffer.ReadFloat32();
			rot.z = Buffer.ReadFloat32();

			if (OnSyncChopterTransform != null)
				OnSyncChopterTransform(pos, rot);
		}

		private static void WriteVector3(Vector3 Value)
		{
			buffer.WriteFloat32(Value.x);
			buffer.WriteFloat32(Value.y);
			buffer.WriteFloat32(Value.z);
		}
	}
}