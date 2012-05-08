using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ChilledTreat.GameStates
{
	class Instructions : GameState
	{
		// Fields
		readonly InputHandler _input = InputHandler.Instance;

		readonly SpriteFont _instructionsFont;
		readonly string[] _instructionsContent;
		readonly Color _fontColor;
		int _lineBreak;

		public Instructions(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// Credits content
			_instructionsFont = content.Load<SpriteFont>("Fonts/credInsFont");
			_fontColor = Color.Salmon;
			_instructionsContent = new[] { 
				"Shoot Stan from Monkey Island with Mouse1",
				" ",
				"Hold space to take cover", 
				"Useful to avoid taking damage while reloading", 
				" ",
				"This line of text is completely unnecessary" };
		}

		// Methods

		public override void Update()
		{
			// Logic
			if (_input.IsAbortPressed())
			{
				Game1.ChangeState(Menu);
			}

			_lineBreak = 0;
		}

		public override void Draw()
		{
			// DRAW!!!! LåååL
			foreach (string creditEntry in _instructionsContent)
			{
				_lineBreak += 60;
				SpriteBatch.DrawString(_instructionsFont, creditEntry, new Vector2(40, _lineBreak), _fontColor);
			}
		}
	}
}