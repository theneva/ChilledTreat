using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ChilledTreat.GameClasses;
using System;
using Microsoft.Xna.Framework.Audio;

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
		readonly SoundEffect _buttonSound, _selectSound;


		private List<Highscore> _highScoreList;
		readonly char[] _charList;
		int _charListPos = 0;
		string _name = "";
		int _charPos;
		bool _typing = true;
		bool _writeFile = true;


		

		public GameOver()
		{
			// LOAD CONTENT
			_menuFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/menuFont");
			_fontColor = Color.Salmon;
			_buttonSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/buttonSound");
			_selectSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/selectSound");


			_scoreFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/ScoreFont");
			_nameFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/nameFont");

			_charList = new char[6];
			for (int i = 0; i < _charList.Length; i++) _charList[i] = 'A';
			
			_highScoreList = Highscore.CreateHighScore();
		}

		public override void Update()
		{

			if (_typing)
			{
				if (_input.IsDownPressed())
				{
					_buttonSound.Play();
					_charList[_charListPos]++;
					if (_charList[_charListPos] > 'Z') _charList[_charListPos] = 'A';
				}
				else if (_input.IsUpPressed())
				{
					_buttonSound.Play();
					_charList[_charListPos]--;
					 if (_charList[_charListPos] < 'A') _charList[_charListPos] = 'Z';
				}
				else if (_input.IsRightPressed() && _charListPos != _charList.Length - 1)
				{
					_selectSound.Play();
					_charListPos++;
				}
				else if (_input.IsLeftPressed() && _charListPos != 0)
				{
					_selectSound.Play();
					_charListPos--;
				}

				if (_input.IsActionPressed())
				{
					_selectSound.Play();
					_typing = false;
				}
			}
			else if (_writeFile)
			{
				for (int i = 0; i < _charList.Length; i++)
					_name += _charList[i];
				Console.WriteLine(_name);
				_writeFile = false;
				NewScoreToAdd = true;
			}
			if (NewScoreToAdd)
			{
				_highScoreList.Add(new Highscore(_name, Player.Instance.Score));
				_highScoreList = _highScoreList.OrderByDescending(x => x.Score).ThenBy(x => x.CurrentTime).ToList();
				Highscore.SerializeToXml(_highScoreList);
				NewScoreToAdd = false;
			}
			_shift = 0;

			if (_input.IsAbortPressed())
			{
				_buttonSound.Play();
				Game1.ChangeState(Menu);
			}
				
		}

		public override void Draw()
		{
			Game1.Instance.SpriteBatch.DrawString(_menuFont, "GAME OVER", new Vector2(Game1.GameScreenWidth / 3f - 70, 100), Color.Salmon);

			if (_typing)
			{
				Game1.Instance.SpriteBatch.DrawString(_nameFont, "Name: ", new Vector2(400, 250), Color.White);
				_charPos = 570;
				for (int i = 0; i < _charList.Length; i++)
				{
					if (i != _charListPos) Game1.Instance.SpriteBatch.DrawString(_nameFont, "" + _charList[i], new Vector2(_charPos, 250), Color.White);
					else Game1.Instance.SpriteBatch.DrawString(_nameFont, "" + _charList[_charListPos], new Vector2(_charPos, 240), Color.White);
					_charPos += 50;
				}
			}
			foreach (var hs in _highScoreList)
			{
				_shift++;
				Game1.Instance.SpriteBatch.DrawString(_scoreFont, Convert.ToString(_shift) + ")", new Vector2(Game1.GameScreenWidth / 3f - 25, 300 + (_shift * 50)), Color.White);
				Game1.Instance.SpriteBatch.DrawString(_scoreFont, hs.Name, new Vector2(Game1.GameScreenWidth / 3f, 300 + (_shift * 50)), Color.White);
				Game1.Instance.SpriteBatch.DrawString(_scoreFont, Convert.ToString(hs.Score), new Vector2(Game1.GameScreenWidth / 3f * 2f, 300 + (_shift * 50)), Color.White);
			}

		}
	}
}