using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameStates
{
	class Splash : GameState
	{
		readonly Texture2D _splash;
		private bool _welcomeScreen;
		readonly SpriteFont _welcomeFont;
		public Splash()
		{
			// LOAD CONTENT
			_splash = Game1.Instance.Content.Load<Texture2D>("Images/chilledTreat");
			_welcomeFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/welcomeFont");
		}		
		public override void Update()
		{
			InputHandler input = InputHandler.Instance;

			if (!input.IsActionPressed()) return;
			if (!_welcomeScreen) _welcomeScreen = true;
			else Game1.ChangeState(Menu);
		}

		public override void Draw()
		{
			if (!_welcomeScreen)
				Game1.Instance.SpriteBatch.Draw(_splash, Vector2.Zero, _splash.Bounds,Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
			else
			{
				Game1.Instance.SpriteBatch.DrawString(_welcomeFont, "Welcome to Chilled Treat!",
					new Vector2(Game1.GameScreenWidth/10, Game1.GameScreenHeight/5), Color.White, 0,
					Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);

				Game1.Instance.SpriteBatch.DrawString(_welcomeFont, "Press " +

#if WINDOWS
					"the Return key " +
#elif XBOX
					"A " +
#endif
					"to continue.", new Vector2(Game1.GameScreenWidth / 10, Game1.GameScreenHeight * 0.8f), Color.White, 0,
					Vector2.Zero, Game1.GameScale/2f, SpriteEffects.None, 0);
			}
		}
	}
}
