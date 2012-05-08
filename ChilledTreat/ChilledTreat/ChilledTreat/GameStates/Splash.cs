using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ChilledTreat.GameStates
{
	class Splash : GameState
	{
		//Texture2D SplashTexture;


		public Splash(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// LOAD CONTENT
		}		
		public override void Update()
		{
			InputHandler input = InputHandler.Instance;


			if (input.IsActionPressed())
			{
				Game1.ChangeState(GameState.Menu);
			}

		}

		public override void Draw()
		{
			//SpriteBatch.Draw(SplashTexture, Vector2.Zero, Color.White);

			Game1.Instance.GraphicsDevice.Clear(Color.Olive);


		}

	}
}
