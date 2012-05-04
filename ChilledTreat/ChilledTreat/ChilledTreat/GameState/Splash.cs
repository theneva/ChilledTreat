using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ChilledTreat
{
	class Splash : GameState
	{
		//Texture2D SplashTexture;

		int NextState;


		public Splash(SpriteBatch spriteBatch, ContentManager content, int nextState)
			: base(spriteBatch, content)
		{
			NextState = nextState;
		}		
		public override void Update()
		{
			InputHandler input = InputHandler.Instance;


			if (input.IsActionPressed())
			{
				Game1.ChangeState(NextState);
			}

		}

		public override void Draw()
		{
			//SpriteBatch.Draw(SplashTexture, Vector2.Zero, Color.White);

			Game1.Instance.GraphicsDevice.Clear(Color.Aqua);


		}

	}
}
