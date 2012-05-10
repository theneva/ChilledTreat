using System.Collections.Generic;
using System.Linq;
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
		readonly SpriteFont _menuFont, _scoreFont, _nameFont;
		public static bool NewScoreToAdd { private get; set; }
		private int _shift;
		Color _fontColor;
		readonly InputHandler _input = InputHandler.Instance;
		private List<Highscore> _highScoreList;
		char[] charList;
		int charListPos = 0;
		string name = "";
		int charPos;
		bool typing = true;
		bool writeFile = true;
		

		public GameOver(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// LOAD CONTENT
			_menuFont = content.Load<SpriteFont>("Fonts/menuFont");
			_scoreFont = content.Load<SpriteFont>("Fonts/ScoreFont");
			_nameFont = content.Load<SpriteFont>("Fonts/nameFont");

			charList = new char[6];
			for (int i = 0; i < charList.Length; i++) charList[i] = 'A';
			

				_fontColor = Color.RoyalBlue;
			//_highScoreList = CreateHighScore();
			_highScoreList = Highscore.CreateHighScore();
		}

		public override void Update()
		{
			if (typing)
			{
				if (_input.IsDownPressed())
				{
					charList[charListPos]++;
					if (charList[charListPos] > 'Z') charList[charListPos] = 'A';
				}
				else if (_input.IsUpPressed())
				{
					charList[charListPos]--;
					 if (charList[charListPos] < 'A') charList[charListPos] = 'Z';
				}
				else if (_input.IsRightPressed() && charListPos != charList.Length - 1) charListPos++;
				else if (_input.IsLeftPressed() && charListPos != 0) charListPos--;
				if (_input.IsActionPressed()) typing = false;
			}
			else if (writeFile)
			{
				for (int i = 0; i < charList.Length; i++)
					name += charList[i];
				Console.WriteLine(name);
				writeFile = false;
				NewScoreToAdd = true;
			}
			if (NewScoreToAdd)
			{
				_highScoreList.Add(new Highscore(name, Player.Instance.Score));
				_highScoreList = _highScoreList.OrderByDescending(x => x.Score).ThenBy(x => x.CurrentTime).ToList();
				Highscore.SerializeToXml(_highScoreList);
				NewScoreToAdd = false;
			}
			_shift = 0;
			if (_input.IsAbortPressed())
				Game1.ChangeState(Menu);
		}

		public override void Draw()
		{
			SpriteBatch.DrawString(_menuFont, "GAME OVER", new Vector2(Game1.GameScreenWidth / 3f, 100), Color.White);
			SpriteBatch.DrawString(_nameFont, "Name: ", new Vector2(70, 250), Color.White);
			charPos = 250;
			for (int i = 0; i < charList.Length; i++)
			{
				if (i != charListPos) SpriteBatch.DrawString(_nameFont, "" + charList[i], new Vector2(charPos, 250), Color.White);
				else SpriteBatch.DrawString(_nameFont, "" + charList[charListPos], new Vector2(charPos, 240), Color.White);
				charPos += 50;
			}
			
			foreach (var hs in _highScoreList)
			{
				_shift++;
				SpriteBatch.DrawString(_scoreFont, Convert.ToString(_shift) + ")", new Vector2(Game1.GameScreenWidth / 3f - 25, 300 + (_shift * 50)), Color.White);
				SpriteBatch.DrawString(_scoreFont, hs.Name, new Vector2(Game1.GameScreenWidth / 3f, 300 + (_shift * 50)), Color.White);
				SpriteBatch.DrawString(_scoreFont, Convert.ToString(hs.Score), new Vector2(Game1.GameScreenWidth / 3f * 2f, 300 + (_shift * 50)), Color.White);
			}
		}
	}
}