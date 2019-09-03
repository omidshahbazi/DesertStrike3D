//Rambo Team
using RamboTeam.Common;
using UnityEngine;

namespace RamboTeam.Client
{
	public delegate void NetworkEventHandler();
	public delegate void SyncChopterTransformEventHandler(Vector3 Position, Vector3 Rotation);
	public delegate void SyncChopterShotEventHandler(Vector3 Position, Vector3 Direction);
	public delegate void SyncEnemyShotEventHandler(Vector3 Position, Vector3 Direction);

	public static class NetworkCommands
	{
		private static BufferStream buffer = new BufferStream(new byte[64]);

		public static event NetworkEventHandler OnConnected;
		public static event NetworkEventHandler OnConnectionLost;
		public static event NetworkEventHandler OnConnectionRestored;

		public static event NetworkEventHandler OnJoinedToRoom;
		public static event NetworkEventHandler OnPilot;
		public static event NetworkEventHandler OnCoPilot;

		public static event NetworkEventHandler OnEndGame;
		public static event SyncChopterTransformEventHandler OnSyncChopterTransform;
		public static event SyncChopterShotEventHandler OnSyncChopterShotHellfire;
		public static event SyncChopterShotEventHandler OnSyncChopterShotHydra;
		public static event SyncChopterShotEventHandler OnSyncChopterShotGatling;
		public static event SyncChopterShotEventHandler OnSyncChopterShotMachinegun;

		public static event SyncEnemyShotEventHandler OnSyncAntiAircraftShot;
		public static event SyncEnemyShotEventHandler OnSyncM3VDAShot;
		public static event SyncEnemyShotEventHandler OnSyncMissleLauncherShot;
		public static event SyncEnemyShotEventHandler OnSyncRifleManShot;
		public static event SyncEnemyShotEventHandler OnSyncRPGManShot;

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

		public static void SyncChopterShotHellfire(Vector3 Position, Vector3 Direction)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_PILOT_FIRE);
			buffer.WriteBytes(Commands.Bullet.HELLFIRE);
			WriteVector3(Position);
			WriteVector3(Direction);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void SyncChopterShotHydra(Vector3 Position, Vector3 Direction)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_PILOT_FIRE);
			buffer.WriteBytes(Commands.Bullet.HYDRA);
			WriteVector3(Position);
			WriteVector3(Direction);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void SyncChopterShotGatling(Vector3 Position, Vector3 Direction)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_PILOT_FIRE);
			buffer.WriteBytes(Commands.Bullet.GATLING);
			WriteVector3(Position);
			WriteVector3(Direction);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void SyncChopterShotMachinegun(Vector3 Position, Vector3 Direction)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_CO_PILOT_FIRE);
			buffer.WriteBytes(Commands.Bullet.MACHINEGUN);
			WriteVector3(Position);
			WriteVector3(Direction);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void SyncAntiAircraftShot(Vector3 Position, Vector3 Direction)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_ENEMY_FIRE);
			buffer.WriteBytes(Commands.Enemy.ANTI_AIRCRAFT);
			WriteVector3(Position);
			WriteVector3(Direction);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void SyncM3VDAShot(Vector3 Position, Vector3 Direction)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_ENEMY_FIRE);
			buffer.WriteBytes(Commands.Enemy.M3VDA);
			WriteVector3(Position);
			WriteVector3(Direction);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void SyncMissleLauncherShot(Vector3 Position, Vector3 Direction)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_ENEMY_FIRE);
			buffer.WriteBytes(Commands.Enemy.MISSLE_LAUNCHER);
			WriteVector3(Position);
			WriteVector3(Direction);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void SyncRifleManShot(Vector3 Position, Vector3 Direction)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_ENEMY_FIRE);
			buffer.WriteBytes(Commands.Enemy.RIFLE_MAN);
			WriteVector3(Position);
			WriteVector3(Direction);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void SyncRPGManShot(Vector3 Position, Vector3 Direction)
		{
			buffer.Reset();
			buffer.WriteBytes(Commands.Category.ROOM);
			buffer.WriteBytes(Commands.Room.SYNC_ENEMY_FIRE);
			buffer.WriteBytes(Commands.Enemy.RPG_MAN);
			WriteVector3(Position);
			WriteVector3(Direction);

			NetworkLayer.Instance.Send(buffer);
		}

		public static void HandleConnected()
		{
			if (OnConnected != null)
				OnConnected();
		}

		public static void HandleConnectionLost()
		{
			if (OnConnectionLost != null)
				OnConnectionLost();
		}

		public static void HandleConnectionRestored()
		{
			if (OnConnectionRestored != null)
				OnConnectionRestored();
		}

		public static void HandleJoinedToRoom()
		{
			if (OnJoinedToRoom != null)
				OnJoinedToRoom();
		}

		public static void HandlePilot()
		{
			if (OnPilot != null)
				OnPilot();
		}

		public static void HandleCoPilot()
		{
			if (OnCoPilot != null)
				OnCoPilot();
		}

		public static void HandleEndGame()
		{
			if (OnSyncChopterTransform != null)
				OnEndGame();
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

			Vector3 dir = Vector3.zero;
			dir.x = Buffer.ReadFloat32();
			dir.y = Buffer.ReadFloat32();
			dir.z = Buffer.ReadFloat32();

			if (OnSyncChopterShotHellfire != null)
				OnSyncChopterShotHellfire(pos, dir);
		}

		public static void HandleSyncChopterShotHydra(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			Vector3 dir = Vector3.zero;
			dir.x = Buffer.ReadFloat32();
			dir.y = Buffer.ReadFloat32();
			dir.z = Buffer.ReadFloat32();

			if (OnSyncChopterShotHydra != null)
				OnSyncChopterShotHydra(pos, dir);
		}

		public static void HandleSyncChopterShotGatling(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			Vector3 dir = Vector3.zero;
			dir.x = Buffer.ReadFloat32();
			dir.y = Buffer.ReadFloat32();
			dir.z = Buffer.ReadFloat32();

			if (OnSyncChopterShotGatling != null)
				OnSyncChopterShotGatling(pos, dir);
		}

		public static void HandleSyncChopterShotMachinegun(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			Vector3 dir = Vector3.zero;
			dir.x = Buffer.ReadFloat32();
			dir.y = Buffer.ReadFloat32();
			dir.z = Buffer.ReadFloat32();

			if (OnSyncChopterShotMachinegun != null)
				OnSyncChopterShotMachinegun(pos, dir);
		}

		public static void HandleSyncAntiAircraftShot(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			Vector3 dir = Vector3.zero;
			dir.x = Buffer.ReadFloat32();
			dir.y = Buffer.ReadFloat32();
			dir.z = Buffer.ReadFloat32();

			if (OnSyncAntiAircraftShot != null)
				OnSyncAntiAircraftShot(pos, dir);
		}

		public static void HandleSyncM3VDAShot(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			Vector3 dir = Vector3.zero;
			dir.x = Buffer.ReadFloat32();
			dir.y = Buffer.ReadFloat32();
			dir.z = Buffer.ReadFloat32();

			if (OnSyncM3VDAShot != null)
				OnSyncM3VDAShot(pos, dir);
		}

		public static void HandleSyncMissleLauncherShot(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			Vector3 dir = Vector3.zero;
			dir.x = Buffer.ReadFloat32();
			dir.y = Buffer.ReadFloat32();
			dir.z = Buffer.ReadFloat32();

			if (OnSyncMissleLauncherShot != null)
				OnSyncMissleLauncherShot(pos, dir);
		}

		public static void HandleSyncRifleManShot(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			Vector3 dir = Vector3.zero;
			dir.x = Buffer.ReadFloat32();
			dir.y = Buffer.ReadFloat32();
			dir.z = Buffer.ReadFloat32();

			if (OnSyncRifleManShot != null)
				OnSyncRifleManShot(pos, dir);
		}

		public static void HandleSyncRPGManShot(BufferStream Buffer)
		{
			Vector3 pos = Vector3.zero;
			pos.x = Buffer.ReadFloat32();
			pos.y = Buffer.ReadFloat32();
			pos.z = Buffer.ReadFloat32();

			Vector3 dir = Vector3.zero;
			dir.x = Buffer.ReadFloat32();
			dir.y = Buffer.ReadFloat32();
			dir.z = Buffer.ReadFloat32();

			if (OnSyncRPGManShot != null)
				OnSyncRPGManShot(pos, dir);
		}

		private static void WriteVector3(Vector3 Value)
		{
			buffer.WriteFloat32(Value.x);
			buffer.WriteFloat32(Value.y);
			buffer.WriteFloat32(Value.z);
		}
	}
}