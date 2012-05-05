using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ChilledTreat.GameStates
{
	class Splash : GameState
	{
		//Texture2D SplashTexture;

		readonly int _nextState;


		public Splash(SpriteBatch spriteBatch, ContentManager content, int nextState)
			: base(spriteBatch, content)
		{
			_nextState = nextState;
		}		
		public override void Update()
		{
			InputHandler input = InputHandler.Instance;


			if (input.IsActionPressed())
			{
				Game1.ChangeState(_nextState);
			}

		}

		public override void Draw()
		{
			//SpriteBatch.Draw(SplashTexture, Vector2.Zero, Color.White);

			Game1.Instance.GraphicsDevice.Clear(Color.Aqua);


		}

	}
}
