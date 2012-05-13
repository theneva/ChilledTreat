using System;
using ChilledTreat.GameClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameStates
{
	class InGame : GameState
	{
		// Fields
		public static readonly Random Random = new Random();
		private readonly Texture2D _background;

		// Constructor
		public InGame()
		{
			Game1.Instance.IsMouseVisible = false;
			_background = Game1.Instance.Content.Load<Texture2D>("Images/bg");
		}

		/// <summary>
		/// Updates player and enemies, pauses the game if the abort key is pressed
		/// or the game loses focus
		/// </summary>
		public override void Update()
		{
			Player.Instance.Update();
			EnemyHandler.Instance.Update();

			if (InputHandler.Instance.IsPausePressed() || Game1.Instance.IsActive == false)
				Game1.ChangeState(PauseMenu);
#if XBOX
			if(!InputHandler.Instance.IsControllerConnected())
				Game1.ChangeState(PauseMenu);
#endif
		}

		/// <summary>
		/// Draws background, enemies and the player
		/// </summary>
		public override void Draw()
		{
			Game1.Instance.SpriteBatch.Draw(_background, Vector2.Zero, _background.Bounds, Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);

			EnemyHandler.Instance.Draw();
			Player.Instance.Draw();
		}
	}
}