//Rambo Team
using System;
using System.Diagnostics;
using UnityEngine;

namespace RamboTeam.Client
{
	public class NetworkSender
	{
		private const int BUFFER_SIZE = 128;

		private byte[] buffer = null;
		private int index = 0;

		public NetworkSender()
		{
			buffer = new byte[BUFFER_SIZE];
		}

		public void PutInt32(int Value)
		{
			PutBytes(BitConverter.GetBytes(Value));
		}

		public void PutFloat32(float Value)
		{
			PutBytes(BitConverter.GetBytes(Value));
		}

		public void PutVector3(Vector3 Value)
		{
			PutFloat32(Value.x);
			PutFloat32(Value.y);
			PutFloat32(Value.z);
		}

		public void PutBytes(params byte[] Data)
		{
			if (Data == null)
				return;

			for (int i = 0; i < Data.Length; ++i)
			{
				buffer[index++] = Data[i];

				UnityEngine.Debug.Assert(index <= BUFFER_SIZE, "Out of Buffer size.");
			}
		}

		public void BeginSend()
		{
			index = 0;
		}

		public void EndSend()
		{
			NetworkLayer.Instance.Send(buffer, index);
		}
	}
}