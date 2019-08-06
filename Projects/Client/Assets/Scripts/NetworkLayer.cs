//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using System;

namespace RamboTeam.Client
{
	public class NetworkLayer : MonoBehaviorBase
	{
		public static NetworkLayer Instance
		{
			get;
			private set;
		}

		private Client client = null;
		private byte[] outBuffer = null;

		protected override void Awake()
		{
			base.Awake();

			Instance = this;

			Connect("127.0.0.1");
		}

		public void Connect(string Host)
		{
			client = new Client();
			client.Connect(Host);

			client.OnMessageReceived += Client_OnMessageReceived;
		}

		public void Send(byte[] Buffer, int Length)
		{
			byte[] buffer = new byte[Length];
			Array.Copy(Buffer, buffer, Length);

			client.Send(buffer);
		}

		private void Client_OnMessageReceived(NetworkingPlayer Player, Binary Frame)
		{
		}
	}
}