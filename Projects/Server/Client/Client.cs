//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;

namespace RamboTeam.Client
{
	public delegate void MessageReceivedEventHandler(NetworkingPlayer Player, Binary Frame);

	public class Client
	{
		private UDPClient client = null;

		public event MessageReceivedEventHandler OnMessageReceived;

		public void Connect(string Host)
		{
			client = new UDPClient();
			client.Connect("127.0.0.1", Constants.PORT);

			client.binaryMessageReceived += Client_binaryMessageReceived;
		}

		public void Send(byte[] Buffer)
		{
			client.Send(new Binary(client.Time.Timestep, false, Buffer, Receivers.Target, 1, false), true);
		}

		private void Client_binaryMessageReceived(NetworkingPlayer Player, Binary Frame, NetWorker Sender)
		{
			if (OnMessageReceived != null)
				OnMessageReceived(Player, Frame);
		}
	}
}