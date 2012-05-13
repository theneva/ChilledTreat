namespace ChilledTreat.Tools
{
	static class GameConstants
	{
		public const bool GodMode = false;
		
		public const int PlayerHealth = 100;

		public const int EnemyHealth = 10;
		public const int EnemyDamage = 15;

		public const float InitialEnemiesPerSecond = GodMode ? 20f : 2f;
		public const int AddEnemyInterval = 5000; // milliseconds
	}
}