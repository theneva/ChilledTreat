using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChilledTreat.Tools
{
	static class GameConstants
	{
		public const int PlayerHealth = 100;
		public const int PlayerDamage = 10000;

		public const int EnemyHealth = 10;
		public const int EnemyDamage = 2;


		// TODO: Simen
		public static int GunAmmo = 10;
		public static int GunRoundsPerSecond = 1;
		public static bool IsGunSplashDamage = false;
		public static bool IsGunAutomatic = false;

		public const int RifleDefaultAmmo = 20;
		public const int RifleDefaultRoundsPerSecond = 20;
		public const bool IsRifleSplashDamage = true;
		public const bool IsRifleAutomatic = true;

		public const float InitialEnemiesPerSecond = 200;
		public const int AddEnemyInterval = 5000; // milliseconds
	}
}
