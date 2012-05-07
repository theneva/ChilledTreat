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
			foreach (var e in _enemies)
			{
				e.Update();
			}
		}

		public void Draw()
		{
			foreach (var e in _enemies)
			{
				e.Draw();
			}
		}

		public void Add(Enemy e)
		{
			_enemies.Add(e);
		}

		public void Remove(Enemy e)
		{
			_enemies.Remove(e);
		}

		public void Clear()
		{
			_enemies.Clear();
		}

		// Letting the handler check if an enemy is hit, and damage that one enemy
		public void FiredAt(Rectangle attackedArea)
		{
			foreach (Enemy enemy in _enemies.Where(e => attackedArea.Intersects(e.GetRectangle())))
			{
				enemy.TakeDamage(DefaultDamage);
				break; // exactly one enemy can be damaged/killed per shot
			}
		}
	}
}
