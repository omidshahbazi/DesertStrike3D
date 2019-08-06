//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;

namespace RamboTeam.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			UDPServer networkHandle = new UDPServer(Constants.MAX_CONNECTION_COUNT);

			networkHandle.binaryMessageReceived += NetworkHandle_binaryMessageReceived;
			networkHandle.playerAccepted += NetworkHandle_playerAccepted;

			networkHandle.Connect("127.0.0.1", Constants.PORT);

			while (true)
			{
				if (System.Console.ReadLine().ToLower() == "exit")
					break;
			}

			networkHandle.Disconnect(false);
		}

		private static void NetworkHandle_binaryMessageReceived(NetworkingPlayer Player, Binary Frame, NetWorker Sender)
		{
		}

		private static void NetworkHandle_playerAccepted(NetworkingPlayer player, NetWorker sender)
		{
		}
	}
}