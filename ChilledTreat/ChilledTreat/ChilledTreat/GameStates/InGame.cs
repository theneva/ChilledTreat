﻿using System;
using ChilledTreat.GameClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameStates
{
	class InGame : GameState
	{
		// Fields
		readonly Player _player;
		readonly EnemyHandler _enemies = EnemyHandler.Instance;
		readonly InputHandler _input = InputHandler.Instance;
		private readonly Random _random = new Random();

		// Constructor
		public InGame(SpriteBatch spriteBatch, ContentManager content)
			: base (spriteBatch, content)
		{
			// CONTENT LOAD
			_player = Player.Instance;
			Game1.Instance.IsMouseVisible = false;
		}

		// Methods

		// Testing
		private int _timeSinceLastAdd;
		public override void Update()
		{
			// LOGIC
			_timeSinceLastAdd += Game1.Instance.TargetElapsedTime.Milliseconds;
			if (_timeSinceLastAdd >= 500)
			{
				_timeSinceLastAdd -= 500;
				EnemyHandler.Instance.Add(new Enemy(SpriteBatch, Content, 1, new Vector2(_random.Next(1080), _random.Next(720))));
			}

			_player.Update();
			_enemies.Update();

			

			if (_input.IsAbortPressed() || Game1.Instance.IsActive == false)
			{
				Game1.ChangeState(PauseMenu);
			}
		}

		public override void Draw()
		{
			// DRAW THAT SHIT
			_enemies.Draw();
			_player.Draw();
		}
	}
}