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
			public const byte MASTER = 1;
			public const byte SECONDARY = 2;
			public const byte SYNC_CHOPTER_TRANSFORM = 3;
			public const byte SYNC_PILOT_FIRE = 4;
			public const byte SYNC_CO_PILOT_FIRE = 5;

			public const byte SYNC_ENEMY_FIRE = 6;

			public const byte END_GAME = 7;
		}

		public static class Bullet
		{
			public const byte HELLFIRE = 0;
			public const byte HYDRA = 1;
			public const byte GATLING = 2;
			public const byte MACHINEGUN = 3;
		}

		public static class Enemy
		{
			public const byte ANTI_AIRCRAFT = 0;
			public const byte M3VDA = 1;
			public const byte MISSLE_LAUNCHER = 2;
			public const byte RIFLE_MAN = 3;
			public const byte RPG_MAN = 4;
		}
	}
}