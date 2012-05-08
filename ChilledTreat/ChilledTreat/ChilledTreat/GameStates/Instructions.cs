using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ChilledTreat.GameStates
{
	class Instructions : GameState
	{
		// Fields
		readonly InputHandler _input = InputHandler.Instance;
		readonly FrameInfo _frameInfo = FrameInfo.Instance;

		readonly SpriteFont _InstructionsFont;
		readonly string[] _InstructionsContent;
		readonly Color _fontColor;
		int _lineBreak = 0;
		double _currentTime;

		public Instructions(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// Credits content
			_InstructionsFont = content.Load<SpriteFont>("Fonts/credInsFont");
			_fontColor = Color.Salmon;
			_InstructionsContent = new[] { 
				"Shoot Stan from Monkey Island with Mouse1",
 				" ",
				"Press space to take cover", 
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
				Game1.ChangeState(GameState.Menu);
			}

			_lineBreak = 0;

			_currentTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;

		}

		public override void Draw()
		{
			// DRAW!!!! LåååL
			foreach (string creditEntry in _InstructionsContent)
			{
				_lineBreak += 60;
			 	SpriteBatch.DrawString(_InstructionsFont, creditEntry, new Vector2(40, _lineBreak), _fontColor);
			}
		}
	}
}