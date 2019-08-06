using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;

namespace RamboTeam.Server
{
	class Application
	{
		private UDPServer socket = null;

		public Application()
		{
			socket = new UDPServer(Constants.MAX_CONNECTION_COUNT);

			socket.playerAccepted += OnPlayerAccepted;
			socket.binaryMessageReceived += OnBinaryMessageReceived;

			Log("Application created.");
		}

		public void Bind()
		{
			socket.Connect("127.0.0.1", Constants.PORT);

			Log("Waiting for clients on port [" + Constants.PORT + "].");
		}

		public void Unbind()
		{
			socket.Disconnect(false);
		}

		private static void OnPlayerAccepted(NetworkingPlayer Player, NetWorker Sender)
		{
			Log("Player [" + Player.IPEndPointHandle + "] accepted.");
		}

		private static void OnBinaryMessageReceived(NetworkingPlayer Player, Binary Frame, NetWorker Sender)
		{
		}

		private static void Log(string Content)
		{
			System.Console.WriteLine(Content);
		}
	}
}
