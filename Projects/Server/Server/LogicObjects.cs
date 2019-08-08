//Rambo Team
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using RamboTeam.Common;

namespace RamboTeam.Server
{
	class LogicObjects
	{
		public UDPServer Socket
		{
			get;
			private set;
		}

		public LogicObjects(UDPServer Socket)
		{
			this.Socket = Socket;
		}

		public void Send(NetworkingPlayer Player, BufferStream Buffer)
		{
			Socket.Send(Player, new Binary(Socket.Time.Timestep, false, Buffer.Buffer, Receivers.Target, 1, false));
		}
	}
}