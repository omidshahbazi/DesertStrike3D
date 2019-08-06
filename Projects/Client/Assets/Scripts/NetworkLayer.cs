//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;

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

		protected override void Awake()
		{
			base.Awake();

			Instance = this;

			Connect("127.0.0.1");

			Send(new byte[] { 10, 20, 40 });
		}

		public void Connect(string Host)
		{
			client = new Client();
			client.Connect(Host);

			client.OnMessageReceived += Client_OnMessageReceived;
		}

		public void Send(byte[] Buffer)
		{
			client.Send(Buffer);
		}

		private void Client_OnMessageReceived(NetworkingPlayer Player, Binary Frame)
		{
		}
	}
}