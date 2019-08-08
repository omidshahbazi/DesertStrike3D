//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;

namespace RamboTeam.Server
{
	class Application
	{
		private UDPServer socket = null;

		private Lobby lobby = null;

		public Application()
		{
			socket = new UDPServer(Constants.MAX_CONNECTION_COUNT);

			socket.playerAccepted += OnPlayerAccepted;
			socket.binaryMessageReceived += OnBinaryMessageReceived;

			lobby = new Lobby(socket);

			Log("Application created.");
		}

		public void Bind()
		{
			socket.Connect("127.0.0.1", Constants.PORT_NUMBER);

			Log("Listening for clients on port [" + Constants.PORT_NUMBER + "].");
		}

		public void Unbind()
		{
			socket.Disconnect(false);
		}

		private void OnBinaryMessageReceived(NetworkingPlayer Player, Binary Frame, NetWorker Sender)
		{
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

		private static void OnPlayerAccepted(NetworkingPlayer Player, NetWorker Sender)
		{
			Log("Player [" + Player.IPEndPointHandle + "] accepted.");
		}

		private static void Log(string Content)
		{
			System.Console.WriteLine(Content);
		}
	}
}
