//Rambo Team
using RamboTeam.Common;
using UnityEngine;

namespace RamboTeam.Client
{
	public static class NetworkCommands
	{
		private static BufferStream buffer = new BufferStream(new byte[64]);

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

		private static void WriteVector3(Vector3 Value)
		{
			buffer.WriteFloat32(Value.x);
			buffer.WriteFloat32(Value.y);
			buffer.WriteFloat32(Value.z);
		}
	}
}