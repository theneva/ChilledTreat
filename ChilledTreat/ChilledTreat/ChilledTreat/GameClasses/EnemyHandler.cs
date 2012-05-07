using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ChilledTreat.GameClasses
{
	class EnemyHandler
	{
		readonly List<Enemy> _enemies;

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

		//public void RecievedDamage(Rectangle shot)
		//{
		//    foreach (Enemy e in _enemies.Where(e => shot.Intersects(e.GetPosition()))) Console.WriteLine("Hit!");
		//}
		//Ser ingen grunn til å ikke ha denne i Enemy lol this is why you don't code at 2 am
	}
}
