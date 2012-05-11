using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ChilledTreat.GameClasses
{
	class EnemyHandler
	{
		readonly List<Enemy> _enemies;

		public List<SoundEffect> SoundEffects { get; protected set; }

		public static readonly Random Random = new Random();

		private const float InitialEnemiesPerSecond = 1f;

		private float _timeSinceLastAdd, _enemiesPerSecond = InitialEnemiesPerSecond;

		private int _timeSinceLastIntervalIncrease;
		private const int AddEnemyInterval = 5000; // milliseconds

		/// <summary>
		/// Returns the singleton; defines it if null
		/// </summary>
		static EnemyHandler _instance;
		public static EnemyHandler Instance
		{
			get { return _instance ?? (_instance = new EnemyHandler()); }
		}

		private EnemyHandler()
		{
			_enemies = new List<Enemy>();
			SoundEffects = new List<SoundEffect> { Game1.Instance.Content.Load<SoundEffect>("Sounds/Enemy/00000050_11025"), Game1.Instance.Content.Load<SoundEffect>("Sounds/Enemy/00000053_11025"), Game1.Instance.Content.Load<SoundEffect>("Sounds/Enemy/00000054_11025"), Game1.Instance.Content.Load<SoundEffect>("Sounds/Enemy/00000056_11025"), Game1.Instance.Content.Load<SoundEffect>("Sounds/Enemy/00000057_11025") };
		}


		/// <summary>
		/// Adds an enemy to the list
		/// </summary>
		public void AddEnemy()
		{
			_enemies.Add(new Enemy());
		}

		// These are for later use
		public void AddEnemy(int value, bool isHealth)
		{
			_enemies.Add(new Enemy(value, isHealth));
		}

		public void AddEnemy(int damage, int health)
		{
			_enemies.Add(new Enemy(health, damage));
		}


		/// <summary>
		/// Gets the amount of enemies currently in the list
		/// </summary>
		/// <returns>The number of enemies</returns>
		public int GetNumberOfEnemies()
		{
			return _enemies.Count;
		}

		/// <summary>
		/// Removes an enemy from the list
		/// </summary>
		/// <param name="enemy">The enemy to remove</param>
		public void Remove(Enemy enemy)
		{
			_enemies.Remove(enemy);
		}

		/// <summary>
		/// Wipes (clears) the list for a fresh start
		/// </summary>
		public void Clear()
		{
			_enemies.Clear();
		}

		/// <summary>
		/// Lets the handler check if an enemy is hit, and damage that one enemy
		/// </summary>
		/// <param name="attackedArea">Rectangle targeted by player's weapon</param>
		/// <param name="inflictedDamage">Damage the weapon inflicted</param>
		public void FiredAt(Rectangle attackedArea, int inflictedDamage)
		{
			// Hackish as fuck, but it works. A for-each loop won't do here because the
			// amount of enemies will change if one is killed
			for (int i = GetNumberOfEnemies() - 1; i >= 0; i--)
				if (_enemies[i].GetRectangle().Intersects(attackedArea))
				{
					if (_enemies[i].Alive)
						_enemies[i].TakeDamage(inflictedDamage);

					if (!WeaponHandler.Instance.Splash) break;
				}
		}


		public void ResetEnemyHandler()
		{
			Clear();
			_enemiesPerSecond = InitialEnemiesPerSecond;
		}

		/// <summary>
		/// Adds the given number of enemies to the screen every AddEnemyInterval milliseconds;
		/// increases that interval at that given interval.
		/// 
		/// Updates each enemy in the list.
		/// </summary>
		public void Update()
		{
			if ((_timeSinceLastIntervalIncrease += Game1.Instance.TargetElapsedTime.Milliseconds) >= AddEnemyInterval)
			{
				_timeSinceLastIntervalIncrease -= AddEnemyInterval;
				++_enemiesPerSecond;
			}

			_timeSinceLastAdd += Game1.Instance.TargetElapsedTime.Milliseconds;

			if (_timeSinceLastAdd >= 1000 / _enemiesPerSecond)
			{
				_timeSinceLastAdd -= 1000 / _enemiesPerSecond;
				AddEnemy();
			}

			for (int i = _enemies.Count - 1; i >= 0; i--)
				_enemies[i].Update();
		}

		/// <summary>
		/// Draws each enemy currently in the list.
		/// Loops through the list backwards so that enemies be drawn
		/// in correct order based on spawn time.
		/// </summary>
		public void Draw()
		{
			for (int i = _enemies.Count - 1; i >= 0; i--)
				_enemies[i].Draw();
		}
	}
}
