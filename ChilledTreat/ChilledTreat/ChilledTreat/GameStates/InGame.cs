using System;
using ChilledTreat.GameClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameStates
{
	class InGame : GameState
	{
		// Fields
		private readonly Random _random = new Random();
		private readonly Texture2D _background;

		// Constructor
		public InGame(SpriteBatch spriteBatch, ContentManager content)
			: base (spriteBatch, content)
		{
			// CONTENT LOAD
			Game1.Instance.IsMouseVisible = false;
			_background = content.Load<Texture2D>("Images/bg");
		}

		// Methods

		/// <summary>
		/// Updates player and enemies, pauses the game if the abort key is pressed
		/// or the game loses focus
		/// </summary>
		public override void Update()
		{
			Player.Instance.Update();
			EnemyHandler.Instance.Update();

			if (InputHandler.Instance.IsAbortPressed() || Game1.Instance.IsActive == false)
				Game1.ChangeState(PauseMenu);
		}

		public override void Draw()
		{
			// DRAW THAT SHIT
			SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
			EnemyHandler.Instance.Draw();
			Player.Instance.Draw();
		}
	}
}