//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;

namespace RamboTeam.Client
{
	public delegate void ConnectionEventHandler();
	public delegate void MessageReceivedEventHandler(NetworkingPlayer Player, Binary Frame);

	public class Client
	{
#if USING_TCP
		private TCPClient socket = null;
#else
		private UDPClient socket = null;
#endif

		private bool isReconnecting = false;

		public event ConnectionEventHandler OnConnected;
		public event ConnectionEventHandler OnConnectionLost;
		public event ConnectionEventHandler OnConnectionRestored;
		public event MessageReceivedEventHandler OnMessageReceived;

		public bool IsConnected
		{
			get;
			private set;
		}

		public void Connect()
		{
#if USING_TCP
			socket = new TCPClient();
#else
			socket = new UDPClient();
#endif

			socket.Connect(Constants.SERVER_IP, Constants.PORT_NUMBER);

			socket.serverAccepted += OnConnetedEvent;
			socket.disconnected += OnDisconnectedEvent;
			socket.binaryMessageReceived += OnBinaryMessageReceived;
		}

		private void Disconnect()
		{
			if (socket == null)
				return;

			socket.serverAccepted -= OnConnetedEvent;
			socket.disconnected -= OnDisconnectedEvent;
			socket.binaryMessageReceived -= OnBinaryMessageReceived;

			socket.Disconnect(false);
		}

		public void Send(BufferStream Buffer)
		{
#if USING_TCP
			socket.Send(new Binary(socket.Time.Timestep, true, Buffer.Buffer, Receivers.All, Constants.BINARY_FRAME_GROUP_ID, true));
#else
			socket.Send(new Binary(socket.Time.Timestep, false, Buffer.Buffer, Receivers.All, Constants.BINARY_FRAME_GROUP_ID, false), false);
#endif
		}

		private void OnConnetedEvent(NetWorker Sender)
		{
			IsConnected = true;

			if (isReconnecting)
			{
				if (OnConnectionRestored != null)
					OnConnectionRestored();

				isReconnecting = false;
			}
			else if (OnConnected != null)
				OnConnected();
		}

		private void OnDisconnectedEvent(NetWorker Sender)
		{
			IsConnected = false;

			if (OnConnectionLost != null)
				OnConnectionLost();

			Disconnect();

			isReconnecting = true;
			Connect();
		}

		private void OnBinaryMessageReceived(NetworkingPlayer Player, Binary Frame, NetWorker Sender)
		{
			if (OnMessageReceived != null)
				OnMessageReceived(Player, Frame);
		}
	}
}