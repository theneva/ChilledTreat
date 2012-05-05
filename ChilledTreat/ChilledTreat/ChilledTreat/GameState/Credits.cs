
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
namespace ChilledTreat
{
	class Credits : GameState
	{
		// Fields
		int nextState;
		InputHandler input = InputHandler.Instance;
		FrameInfo _frameInfo = FrameInfo.Instance;

		SpriteFont creditsFont;
		string[] creditsContent;
		Color fontColor;
		int lineBreak = 0;
		double _currentTime;

		public Credits(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// Credits content
			creditsFont = content.Load<SpriteFont>("fonts/menuFont");
			fontColor = Color.Salmon;
			string[] strings = { "Linje----------------", "Linje----------------", "Linje----------------", "Linje----------------" };
			creditsContent = strings;
		}




		// Constructor
		public Credits(SpriteBatch spriteBatch, ContentManager content, int NextState)
			: base(spriteBatch, content)
		{
			nextState = NextState;
		}

		// Methods

		public override void Update()
		{
			// Logic
			if (input.IsAbortPressed())
			{
				Game1.ChangeState(1);
			}

			lineBreak = 0;

			_currentTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;

		}

		public override void Draw()
		{
			// DRAW!!!! LåååL
			for (int i = 0; i < creditsContent.Length; i++)
			{
				lineBreak += 100;
				SpriteBatch.DrawString(creditsFont, creditsContent[i], new Vector2(360, lineBreak), fontColor);
			}
		}
	}
}
