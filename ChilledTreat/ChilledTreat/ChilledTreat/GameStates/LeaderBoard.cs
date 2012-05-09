using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ChilledTreat.GameStates
{
	class LeaderBoard : GameState
	{
		readonly SpriteFont _menuFont, _scoreFont;
		private int _shift;
		Color _fontColor;
		readonly InputHandler _input = InputHandler.Instance;
		private List<Highscore> _highScoreList;

		public LeaderBoard(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			_highScoreList = Highscore.CreateHighScore();
			_menuFont = content.Load<SpriteFont>("Fonts/menuFont");
			_scoreFont = content.Load<SpriteFont>("Fonts/ScoreFont");
			_fontColor = Color.RoyalBlue;

		}

		public override void Update()
		{
			_shift = 0;
			
			if (_input.IsAbortPressed())
			{
				Game1.ChangeState(GameState.Menu);
			}
		}

		public override void Draw()
		{

			SpriteBatch.DrawString(_menuFont, "Leaderboard", new Vector2(Game1.Instance.GameScreenWidth / 3f - 100, 50), Color.White);
			foreach (Highscore hs in _highScoreList)
			{
				_shift++;	
				SpriteBatch.DrawString(_scoreFont, Convert.ToString(_shift) + ")", new Vector2(Game1.Instance.GameScreenWidth / 3f - 25, 150 + (_shift * 50)), Color.White);
				SpriteBatch.DrawString(_scoreFont, hs.Name, new Vector2(Game1.Instance.GameScreenWidth / 3f, 150 + (_shift * 50)), Color.White);
				SpriteBatch.DrawString(_scoreFont, Convert.ToString(hs.Score), new Vector2(Game1.Instance.GameScreenWidth / 3f * 2f, 150 + (_shift * 50)), Color.White);
			}
		}
	}
}
