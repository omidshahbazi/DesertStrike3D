//Rambo Team
using RamboTeam.Common;
using UnityEngine;

namespace RamboTeam.Client
{
	public delegate void NetworkEventHandler();
	public delegate void SyncChopterTransformEventHandler(Vector3 Position, Vector3 Rotation);
	public delegate void SyncChopterShotEventHandler(Vector3 Position);

	public static class NetworkCommands
	{
		private static BufferStream buffer = new BufferStream(new byte[64]);

		public static event NetworkEventHandler OnJoinedToRoom;
		public static event NetworkEventHandler OnPilot;
		public static event NetworkEventHandler OnCommando;
		public static event SyncChopterTransformEventHandler OnSyncChopterTransform;
		public static event SyncChopterShotEventHandler OnSyncChopterShotHellfire;
		public static event SyncChopterShotEventHandler OnSyncChopterShotHydra;
		public static event SyncChopterShotEventHandler OnSyncChopterShotGatling;

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

		public static void SyncChopterShotHellfire(Vector3 Position)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_CHOPTER_FIRE);
			buffer.WriteBytes(Commands.Bullet.Hellfire);
			WriteVector3(Position);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void SyncChopterShotHydra(Vector3 Position)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_CHOPTER_FIRE);
			buffer.WriteBytes(Commands.Bullet.Hydra);
			WriteVector3(Position);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void SyncChopterShotGatling(Vector3 Position)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_CHOPTER_FIRE);
			buffer.WriteBytes(Commands.Bullet.Gatling);
			WriteVector3(Position);

			NetworkLayer.Instance.Send(buffer);
		}

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

		public static void HandleSyncChopterShotHellfire(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			if (OnSyncChopterShotHellfire != null)
				OnSyncChopterShotHellfire(pos);
		}

		public static void HandleSyncChopterShotHydra(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			if (OnSyncChopterShotHydra != null)
				OnSyncChopterShotHydra(pos);
		}

		public static void HandleSyncChopterShotGatling(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			if (OnSyncChopterShotGatling != null)
				OnSyncChopterShotGatling(pos);
		}

		private static void WriteVector3(Vector3 Value)
		{
			buffer.WriteFloat32(Value.x);
			buffer.WriteFloat32(Value.y);
			buffer.WriteFloat32(Value.z);
		}
	}
}