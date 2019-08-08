//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;

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

		public bool IsPilot
		{
			get;
			private set;
		}

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

		public void Send(BufferStream Buffer)
		{
			client.Send(Buffer);
		}

		private void Client_OnMessageReceived(NetworkingPlayer Player, Binary Frame)
		{
			BufferStream buffer = new BufferStream(Frame.StreamData.byteArr);

			byte category = buffer.ReadByte();
			byte command = buffer.ReadByte();

			if (category == Commands.Category.LOBBY)
			{

			}
			else if (category == Commands.Category.ROOM)
			{
				if (command == Commands.Room.MASTER)
				{
					IsPilot = true;
				}
				else if (command == Commands.Room.SECONDARY)
				{
					IsPilot = false;
				}
				else if (command == Commands.Room.SYNC_CHOPTER_TRANSFORM)
				{

				}
			}
		}
	}
}