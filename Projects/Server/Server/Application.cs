//Rambo Team

using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;

namespace RamboTeam.Server
{
	class Application
	{

#if USING_TCP
		private TCPServer socket = null;
#else
		private UDPServer socket = null;
#endif

		private Lobby lobby = null;

		public Application()
		{
#if USING_TCP
			socket = new TCPServer(Constants.MAX_CONNECTION_COUNT);
#else
			socket = new UDPServer(Constants.MAX_CONNECTION_COUNT);
#endif


			socket.serverAccepted += OnServerAccepted;
			socket.playerConnected += OnPlayerConnected;
			socket.playerDisconnected += OnPlayerDisconnected;
			socket.playerAccepted += OnPlayerAccepted;
			socket.binaryMessageReceived += OnBinaryMessageReceived;

			lobby = new Lobby(this);

			Log("Application created.");
		}

		public void Bind()
		{
			socket.Connect("0.0.0.0", Constants.PORT_NUMBER);

#if USING_TCP
			Log("Listening for clients on TCP port [" + Constants.PORT_NUMBER + "].");
#else
			Log("Listening for clients on UDP port [" + Constants.PORT_NUMBER + "].");
#endif
		}

		public void Unbind()
		{
			socket.Disconnect(false);
		}

		public void Send(NetworkingPlayer Player, BufferStream Buffer)
		{
#if USING_TCP
			socket.Send(Player.TcpClientHandle, new Binary(socket.Time.Timestep, true, Buffer.Buffer, Receivers.All, Constants.BINARY_FRAME_GROUP_ID, true));
#else
			socket.Send(Player, new Binary(socket.Time.Timestep, false, Buffer.Buffer, Receivers.All, Constants.BINARY_FRAME_GROUP_ID, false), true);
#endif
		}

		private void OnServerAccepted(NetWorker Sender)
		{
			Log("Server accepted."); 
		}

		private void OnPlayerConnected(NetworkingPlayer Player, NetWorker Sender)
		{
			Log("Player [" + Player.IPEndPointHandle + "] connected.");
		}

		private void OnPlayerDisconnected(NetworkingPlayer Player, NetWorker Sender)
		{
			lobby.HandlePlayerDisconnection(Player);

			Log("Player [" + Player.IPEndPointHandle + "] disconnected.");
		}

		private static void OnPlayerAccepted(NetworkingPlayer Player, NetWorker Sender)
		{
			Log("Player [" + Player.IPEndPointHandle + "] accepted.");
		}

		private void OnBinaryMessageReceived(NetworkingPlayer Player, Binary Frame, NetWorker Sender)
		{
			if (Frame.GroupId != Constants.BINARY_FRAME_GROUP_ID)
				return;

			BufferStream buffer = new BufferStream(Frame.StreamData.byteArr);

			byte category = buffer.ReadByte();

			if (category == Commands.Category.LOBBY)
			{
				lobby.HandleRequest(buffer, Player);
			}
			else if (category == Commands.Category.ROOM)
			{
				Room room = lobby.FindRoom(Player);

				if (room == null)
					return;

				room.HandleRequest(buffer, Player);
			}
		}

		private static void Log(string Content)
		{
			System.Console.WriteLine(Content);
		}
	}
}
