//Rambo Team
namespace RamboTeam.Common
{
    public static class Commands
    {
        public static class Category
		{
			public const byte LOBBY = 1;
			public const byte ROOM = 2;
        }

		public static class Lobby
		{
			public const byte JOIN_TO_ROOM = 1;
		}

        public static class Room
        {
            public const byte SYNC_CHOPTER_TRANSFORM = 1;
        }
    }
}