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

		private long nextPingTime = 0;

		public event ConnectionEventHandler OnConnected;
		public event MessageReceivedEventHandler OnMessageReceived;

		public void Connect(string Host)
		{
#if USING_TCP
			socket = new TCPClient();
#else
			socket = new UDPClient();
#endif

			socket.Connect("127.0.0.1", Constants.PORT_NUMBER);

#if DEBUG
			socket.serverAccepted += OnServerAccepted;
#endif

			socket.binaryMessageReceived += OnBinaryMessageReceived;
		}

		public void Send(BufferStream Buffer)
		{
#if USING_TCP
			socket.Send(new Binary(socket.Time.Timestep, true, Buffer.Buffer, Receivers.All, Constants.BINARY_FRAME_GROUP_ID, true));
#else
			socket.Send(new Binary(socket.Time.Timestep, false, Buffer.Buffer, Receivers.All, Constants.BINARY_FRAME_GROUP_ID, false), false);
#endif
		}

		private void OnServerAccepted(NetWorker Sender)
		{
			if (OnConnected != null)
				OnConnected();
		}

		private void OnBinaryMessageReceived(NetworkingPlayer Player, Binary Frame, NetWorker Sender)
		{
			if (OnMessageReceived != null)
				OnMessageReceived(Player, Frame);
		}
	}
}