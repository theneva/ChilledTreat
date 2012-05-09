using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
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
		readonly SpriteFont _menuFont, _scoreFont;
		public static bool NewScoreToAdd { private get; set; }
		private int _shift;
		Color _fontColor;
		readonly InputHandler _input = InputHandler.Instance;
		private List<Highscore> _highScoreList;
		char[] charList = { 'A', 'A', 'A', 'A', 'A' };
		int charListPos = 0;
		string name;
		

		public GameOver(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// LOAD CONTENT
			_menuFont = content.Load<SpriteFont>("Fonts/menuFont");
			_scoreFont = content.Load<SpriteFont>("Fonts/ScoreFont");
			_fontColor = Color.RoyalBlue;
			//_highScoreList = CreateHighScore();
			_highScoreList = Highscore.CreateHighScore();
		}

		public override void Update()
		{
			if (_input.IsDownPressed())
			{
				if (charList[charListPos] > 'A') charList[charListPos]++;
			}
			else if (_input.IsUpPressed())
			{
				if (charList[charListPos] < 'Z') charList[charListPos]--;
			}
			if (_input.IsActionPressed())
			{
				charListPos++;
				if (charListPos > charList.Length) {}
			}
			if (NewScoreToAdd)
			{
				_highScoreList.Add(new Highscore("Player", Player.Instance.Score));
				_highScoreList = _highScoreList.OrderByDescending(x => x.Score).ThenBy(x => x.CurrentTime).ToList();

				Highscore.SerializeToXml(_highScoreList);

				NewScoreToAdd = false;
			}
			_shift = 0;
			if (_input.IsAbortPressed())
			{
				Game1.ChangeState(Menu);
			}
		}

		public override void Draw()
		{
			SpriteBatch.DrawString(_menuFont, "GAME OVER", new Vector2(Game1.Instance.GameScreenWidth / 2f, 100), Color.White);



			foreach (Highscore hs in _highScoreList)
			{
				_shift++;
				SpriteBatch.DrawString(_scoreFont, Convert.ToString(_shift) + ")", new Vector2(Game1.Instance.GameScreenWidth / 3f - 25, 250 + (_shift * 50)), Color.White);
				SpriteBatch.DrawString(_scoreFont, hs.Name, new Vector2(Game1.Instance.GameScreenWidth / 3f, 250 + (_shift * 50)), Color.White);
				SpriteBatch.DrawString(_scoreFont, Convert.ToString(hs.Score), new Vector2(Game1.Instance.GameScreenWidth / 3f * 2f, 250 + (_shift * 50)), Color.White);
			}
		}
	}
}