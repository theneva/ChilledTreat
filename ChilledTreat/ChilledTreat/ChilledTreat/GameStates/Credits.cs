using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ChilledTreat.GameStates
{
	class Credits : GameState
	{
		// Fields
		readonly InputHandler _input = InputHandler.Instance;
		readonly FrameInfo _frameInfo = FrameInfo.Instance;

		readonly SpriteFont _creditsFont;
		readonly string[] _creditsContent;
		readonly Color _fontColor;
		int _lineBreak = 0;
		double _currentTime;

		public Credits(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// Credits content
			_creditsFont = content.Load<SpriteFont>("fonts/menuFont");
			_fontColor = Color.Salmon;
			_creditsContent = new[] { "Linje----------------", "Linje----------------", "Linje----------------", "Linje----------------" };
		}

		// Methods

		public override void Update()
		{
			// Logic
			if (_input.IsAbortPressed())
			{
				Game1.ChangeState(1);
			}

			_lineBreak = 0;

			_currentTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;

		}

		public override void Draw()
		{
			// DRAW!!!! LåååL
			foreach (string creditEntry in _creditsContent)
			{
				_lineBreak += 100;
				SpriteBatch.DrawString(_creditsFont, creditEntry, new Vector2(360, _lineBreak), _fontColor);
			}
		}
	}
}
