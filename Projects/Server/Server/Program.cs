//Rambo Team
using RamboTeam.Common;
using System;
using System.Threading;

namespace RamboTeam.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Constants.SERVER_HEADER_TEXT);

			Application application = new Application();

			application.Bind();

			while (true)
			{
				Thread.Sleep(1);
			}
		}
	}
}