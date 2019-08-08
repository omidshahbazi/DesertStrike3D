//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;
using System;

namespace RamboTeam.Client
{
	public delegate void MessageReceivedEventHandler(NetworkingPlayer Player, Binary Frame);

	public class Client
	{
#if USING_TCP
		private TCPClient socket = null;
#else
		private UDPClient socket = null;
#endif

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
			socket.playerConnected += OnPlayerConnected;
			socket.playerDisconnected += OnPlayerDisconnected;
			socket.playerAccepted += OnPlayerAccepted;
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

#if DEBUG
		private void OnServerAccepted(NetWorker Sender)
		{
			Console.WriteLine("Server accepted.");
		}

		private void OnPlayerConnected(NetworkingPlayer Player, NetWorker Sender)
		{
			Console.WriteLine("Player [" + Player.IPEndPointHandle + "] connected.");
		}

		private void OnPlayerDisconnected(NetworkingPlayer Player, NetWorker Sender)
		{
			Console.WriteLine("Player [" + Player.IPEndPointHandle + "] disconnected.");
		}

		private static void OnPlayerAccepted(NetworkingPlayer Player, NetWorker Sender)
		{
			Console.WriteLine("Player [" + Player.IPEndPointHandle + "] accepted.");
		}
#endif

		private void OnBinaryMessageReceived(NetworkingPlayer Player, Binary Frame, NetWorker Sender)
		{
			if (OnMessageReceived != null)
				OnMessageReceived(Player, Frame);
		}
	}
}