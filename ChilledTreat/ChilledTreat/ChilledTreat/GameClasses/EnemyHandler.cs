﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameClasses
{
	class EnemyHandler
	{
		readonly List<Enemy> _enemies;

		public static readonly Random Random = new Random();

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

		public void AddEnemy(SpriteBatch spriteBatch, ContentManager content, int health)
		{
			_enemies.Add(new Enemy(spriteBatch, content, health, new Vector2(Random.Next(Game1.Instance.Window.ClientBounds.Width - Enemy.GetOriginTexture().X), Random.Next(Game1.Instance.Window.ClientBounds.Height - Enemy.GetOriginTexture().Y))));
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
			for (int i = GetNumberOfEnemies() - 1; i >= 0; i--)
				if (_enemies[i].GetRectangle().Intersects(attackedArea))
					_enemies[i].TakeDamage(DefaultDamage);
		}
	}
}