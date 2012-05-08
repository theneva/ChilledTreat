
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using ChilledTreat.GameClasses;
using System;
namespace ChilledTreat.GameStates
{
	class GameOver : GameState
	{
		// FIELDS
		SpriteFont menuFont;
		Color fontColor;
		InputHandler input = InputHandler.Instance;


		public GameOver(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// LOAD CONTENT
			menuFont = content.Load<SpriteFont>("Fonts/menuFont");
			fontColor = Color.RoyalBlue;
		}

		public override void Update()
		{
			if (input.IsAbortPressed())
			{
				Game1.ChangeState(GameState.Menu);
			}
		}

		public override void Draw()
		{
			SpriteBatch.DrawString(menuFont, "Score: " + Convert.ToString(Player.Instance.Score), new Vector2(300, 300), Color.White);
		}
	}
}
