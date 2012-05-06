using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChilledTreat.GameClasses
{
	class EnemyHandler
	{
		List<Enemy> enemies;
		
		static EnemyHandler _instance;
		public static EnemyHandler Instance
		{
			get { return _instance ?? (_instance = new EnemyHandler()); }
		}

		private EnemyHandler()
		{
			enemies = new List<Enemy>();
		}

		public int GetNumberOfEnemies()
		{
			return enemies.Count;
		}

		public void Update()
		{
			foreach (Enemy e in enemies)
			{
				e.Update();
			}
		}

		public void Draw()
		{
			foreach (Enemy e in enemies)
			{
				e.Draw();
			}
		}

		public void Add(Enemy e)
		{
			enemies.Add(e);
		}

		public void Remove(Enemy e)
		{
			enemies.Remove(e);
		}

		public void Clear()
		{
			enemies.Clear();
		}
	}
}
