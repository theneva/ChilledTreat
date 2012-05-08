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
			_InstructionsFont = content.Load<SpriteFont>("Fonts/menuFont");
			_fontColor = Color.Salmon;
			_InstructionsContent = new[] { "Skyt Stan fra Monkey Island - Mouse1", "Trykk space for å gå i cover", "Dette kan være lurt å gjøre mens du lader", "Linje----------------" };
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
				_lineBreak += 100;
				SpriteBatch.DrawString(_InstructionsFont, creditEntry, new Vector2(360, _lineBreak), _fontColor);
			}
		}
	}
}