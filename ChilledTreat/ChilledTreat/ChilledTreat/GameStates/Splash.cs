using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameStates
{
	class Splash : GameState
	{
		readonly Texture2D splash;
		private bool _welcomeScreen;
		SpriteFont welcomeFont;
		public Splash()
		{
			// LOAD CONTENT
			splash = Game1.Instance.Content.Load<Texture2D>("Images/chilledTreat");
			welcomeFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/welcomeFont");
		}		
		public override void Update()
		{
			InputHandler input = InputHandler.Instance;

			if (input.IsActionPressed())
			{
				if (!_welcomeScreen) _welcomeScreen = true;
				else Game1.ChangeState(Menu);
			}	
		}

		public override void Draw()
		{
			if (!_welcomeScreen)
				Game1.Instance.SpriteBatch.Draw(splash, Vector2.Zero, Color.White);
			else
				Game1.Instance.SpriteBatch.DrawString(welcomeFont, "Welcome to Chilled Treat!\nPress A/Enter to continue.", new Vector2(150, 200), Color.White);	
		}
	}
}
