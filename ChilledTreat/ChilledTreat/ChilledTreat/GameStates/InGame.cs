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
		private readonly Texture2D _background;

		// Constructor
		public InGame(SpriteBatch spriteBatch, ContentManager content)
			: base (spriteBatch, content)
		{
			// CONTENT LOAD
			_player = Player.Instance;
			Game1.Instance.IsMouseVisible = false;
			_background = content.Load<Texture2D>("Images/bg");
		}

		// Methods

		// Testing
		private float _timeSinceLastAdd;
		private const float EnemiesPerSecond = 1.6f;

		public override void Update()
		{
			// LOGIC
			_timeSinceLastAdd += Game1.Instance.TargetElapsedTime.Milliseconds;

			if (_timeSinceLastAdd >= 1000 / EnemiesPerSecond)
			{
				_timeSinceLastAdd -= 1000 / EnemiesPerSecond;
				EnemyHandler.Instance.AddEnemy(SpriteBatch, Content, 20);
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
			SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
			_enemies.Draw();
			_player.Draw();
		}
	}
}