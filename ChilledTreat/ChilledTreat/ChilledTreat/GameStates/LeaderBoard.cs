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
#if WINDOWS
		readonly SpriteFont _menuFont, _scoreFont;
		private int _shift;
		Color _fontColor;
		readonly InputHandler _input = InputHandler.Instance;
		private List<Highscore> _highScoreList;
		public int scrolling = 0;

		Texture2D lbTexture = new Texture2D(Game1.Instance.Graphics.GraphicsDevice, 1, 1);
		Color[] colorData = {Color.White};

		public LeaderBoard(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			_highScoreList = Highscore.CreateHighScore();
			Console.WriteLine(_highScoreList.Count);
			_menuFont = content.Load<SpriteFont>("Fonts/menuFont");
			_scoreFont = content.Load<SpriteFont>("Fonts/ScoreFont");
			_fontColor = Color.RoyalBlue;

			lbTexture.SetData<Color>(colorData);
		}

		public override void Update()
		{
			_shift = 0;
			
			if (_input.IsAbortPressed())
			{
				Game1.ChangeState(GameState.Menu);
			}
			if (_input.IsDownPressed() && scrolling < 0) scrolling++;

			else if (_input.IsUpPressed() && scrolling > -_highScoreList.Count + 10) scrolling--;
		}

		public override void Draw()
		{
			foreach (Highscore hs in _highScoreList)
			{
				_shift++;
				SpriteBatch.DrawString(_scoreFont, Convert.ToString(_shift) + ")", new Vector2(Game1.GameScreenWidth / 3f - 35, 100 + (_shift * 50) + (scrolling * 50)), Color.White);
				SpriteBatch.DrawString(_scoreFont, hs.Name, new Vector2(Game1.GameScreenWidth / 3f, 100 + (_shift * 50) + (scrolling * 50)), Color.White);
				SpriteBatch.DrawString(_scoreFont, Convert.ToString(hs.Score), new Vector2(Game1.GameScreenWidth / 3f * 2f, 100 + (_shift * 50) + (scrolling * 50)), Color.White);
			}
			//Ramme oppe og nede for å sentrere leaderboardet
			SpriteBatch.Draw(lbTexture, new Rectangle(0, 0, 1280, 150), Color.YellowGreen);
			//Kan fjernes og evt bytte fontstørrelse og linjeskift
			SpriteBatch.Draw(lbTexture, new Rectangle(0, 640, 1280, 80), Color.YellowGreen);
			SpriteBatch.DrawString(_menuFont, "Leaderboard", new Vector2(Game1.GameScreenWidth / 3f - 100, 50), Color.White);
		}
#endif
	}
}
