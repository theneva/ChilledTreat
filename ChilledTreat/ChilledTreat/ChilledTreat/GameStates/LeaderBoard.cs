using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ChilledTreat.GameStates
{
	class LeaderBoard : GameState
	{
		// Fields
		readonly SpriteFont _menuFont, _scoreFont;
		private int _shift;
		readonly InputHandler _input = InputHandler.Instance;
		private readonly List<Highscore> _highScoreList;
		private int _scrolling;

		readonly Texture2D _lbTexture = new Texture2D(Game1.Instance.Graphics.GraphicsDevice, 1, 1);
		readonly Color[] _colorData = { Color.White };

		// Constructor
		public LeaderBoard()
		{

			_highScoreList = Highscore.CreateHighScore();
			Console.WriteLine(_highScoreList.Count);

			_menuFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/menuFont");
			_scoreFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/ScoreFont");

			_lbTexture.SetData(_colorData);
		}

		public override void Update()
		{
			_shift = 0;

			if (_input.IsAbortPressed())
			{
				Game1.ChangeState(Menu);
			}
			// allows scrolling in the leaderboard
			if (_input.IsUpPressed() && _scrolling < 0) _scrolling++;

			else if (_input.IsDownPressed() && _scrolling > -_highScoreList.Count + 10) _scrolling--;
		}

		public override void Draw()
		{

			foreach (Highscore hs in _highScoreList)
			{
				_shift++;
				Game1.Instance.SpriteBatch.DrawString(_scoreFont, Convert.ToString(_shift) + ")", new Vector2(Game1.GameScreenWidth / 3f - 35, 100 + (_shift * 50) + (_scrolling * 50)), Color.White);
				Game1.Instance.SpriteBatch.DrawString(_scoreFont, hs.Name, new Vector2(Game1.GameScreenWidth / 3f, 100 + (_shift * 50) + (_scrolling * 50)), Color.White);
				Game1.Instance.SpriteBatch.DrawString(_scoreFont, Convert.ToString(hs.Score), new Vector2(Game1.GameScreenWidth / 3f * 2f, 100 + (_shift * 50) + (_scrolling * 50)), Color.White);
			}
			//Top/Bot Frame, this is centering the scrolling leaderboard, and hiding text under the title
			Game1.Instance.SpriteBatch.Draw(_lbTexture, new Rectangle(0, 0, 1280, 150), Color.Black);
			//Can be removed and rather tweak fontsize and spacing.
			Game1.Instance.SpriteBatch.Draw(_lbTexture, new Rectangle(0, 640, 1280, 80), Color.Black);
			Game1.Instance.SpriteBatch.DrawString(_menuFont, "Leaderboard", new Vector2(Game1.GameScreenWidth / 3f - 100, 50), Color.White);
		}
	}
}
