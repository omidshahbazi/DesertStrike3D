//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;
using System;
using System.Threading;

namespace RamboTeam.Client.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Client client = new Client();
			client.Connect("127.0.0.1");

			client.OnMessageReceived += Client_OnMessageReceived;

			BufferStream buffer = new BufferStream(new byte[64]);

			buffer.WriteBytes(1, 1);
			client.Send(buffer);

			buffer.Reset();
			buffer.WriteBytes(2, 3);

			while (true)
			{
				client.Send(buffer);

				Thread.Sleep(1000);
			}
		}

		private static void Client_OnMessageReceived(NetworkingPlayer Player, Binary Frame)
		{
			Console.WriteLine("Received from Server");
		}
	}
}
