using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ChilledTreat.GameClasses
{
	class EnemyHandler
	{
		readonly List<Enemy> _enemies;

		public static readonly Random _random = new Random();

		private const int DefaultDamage = 20;

		static EnemyHandler _instance;
		public static EnemyHandler Instance
		{
			get { return _instance ?? (_instance = new EnemyHandler()); }
		}

		private EnemyHandler()
		{
			_enemies = new List<Enemy>();
		}

		public int GetNumberOfEnemies()
		{
			return _enemies.Count;
		}

		public void Update()
		{
			foreach (Enemy enemy in _enemies)
			{
				enemy.Update();
			}
		}

		public void Draw()
		{
			foreach (Enemy enemy in _enemies)
			{
				enemy.Draw();
			}
		}

		public void Add(Enemy enemy)
		{
			_enemies.Add(enemy);
		}

		public void Remove(Enemy enemy)
		{
			_enemies.Remove(enemy);
		}

		public void Clear()
		{
			_enemies.Clear();
		}

		// Letting the handler check if an enemy is hit, and damage that one enemy
		public void FiredAt(Rectangle attackedArea)
		{
			//foreach (Enemy enemy in _enemies.Where(e => attackedArea.Intersects(e.GetRectangle())))
			//{
			//    enemy.TakeDamage(DefaultDamage);
			//}

			// Hackish as fuck but it works
			for (int i = _enemies.Count - 1; i >= 0; i--)
				if (_enemies[i].GetRectangle().Intersects(attackedArea))
					_enemies[i].TakeDamage(DefaultDamage);
		}
	}
}
