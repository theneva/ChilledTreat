using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ChilledTreat.GameClasses;

namespace ChilledTreat.GameStates
{
	class GameOver : GameState
	{
		// Fields
		readonly SpriteFont _menuFont, _scoreFont, _nameFont;
		public static bool NewScoreToAdd { private get; set; }
		readonly InputHandler _input = InputHandler.Instance;
		readonly SoundEffect _buttonSound, _selectSound;

		private List<Highscore> _highScoreList;
		readonly char[] _charList;
		private int _shift, _charListPos;
		private float _charPos;
		private string _name;
		bool _typing = true, _writeFile = true;

		public GameOver()
		{
			// LOAD CONTENT
			_menuFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/menuFont");
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
			// Typing logic region contains the logic which enables
			// the player to type his name
			#region Typing logic
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
			#endregion

			else if (_writeFile)
			{
				for (int i = 0; i < _charList.Length; i++)
					_name += _charList[i];
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
			Game1.Instance.SpriteBatch.DrawString(_menuFont, "GAME OVER", new Vector2(Game1.GameScreenWidth / 3f - 80, 100 * Game1.GameScale), Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
			Game1.Instance.SpriteBatch.DrawString(_menuFont, "Your score: " + Player.Instance.Score, new Vector2(340 * Game1.GameScale, 170 * Game1.GameScale), Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);

			if (_typing)
			{
				Game1.Instance.SpriteBatch.DrawString(_nameFont, "Name: ", new Vector2(400, 250) * Game1.GameScale, Color.White);
				_charPos = 570 * Game1.GameScale;
				for (int i = 0; i < _charList.Length; i++)
				{
					if (i != _charListPos) Game1.Instance.SpriteBatch.DrawString(_nameFont, "" + _charList[i], new Vector2(_charPos, 250 * Game1.GameScale), Color.White);
					else Game1.Instance.SpriteBatch.DrawString(_nameFont, "" + _charList[_charListPos], new Vector2(_charPos, 240 * Game1.GameScale), Color.White);
					_charPos += 50 * Game1.GameScale;
				}
			}
			foreach (var hs in _highScoreList)
			{
				Game1.Instance.SpriteBatch.DrawString(_scoreFont, Convert.ToString(++_shift) + ")", new Vector2(Game1.GameScreenWidth / 3f - 25, 300 * Game1.GameScale + (_shift * 50)), Color.White);
				Game1.Instance.SpriteBatch.DrawString(_scoreFont, hs.Name, new Vector2(Game1.GameScreenWidth / 3f, 300 * Game1.GameScale + (_shift * 50)), Color.White);
				Game1.Instance.SpriteBatch.DrawString(_scoreFont, Convert.ToString(hs.Score), new Vector2(Game1.GameScreenWidth / 3f * 2f, 300 * Game1.GameScale + (_shift * 50)), Color.White);
			}

		}
	}
}