//Rambo Team
using BeardedManStudios.Forge.Networking;
using RamboTeam.Common;

namespace RamboTeam.Server
{
	class LogicObjects
	{
		public Application Application
		{
			get;
			private set;
		}

		public LogicObjects(Application Application)
		{
			this.Application = Application;
		}

		public void Send(NetworkingPlayer Player, BufferStream Buffer)
		{
			Application.Send(Player, Buffer);
		}

		protected static void Log(string Content)
		{
			System.Console.WriteLine(Content);
		}
	}
}