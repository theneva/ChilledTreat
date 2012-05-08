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
		readonly SpriteFont _menuFont;
		public static bool NewScoreToAdd { private get; set; }
		private int _shift;
		Color _fontColor;
		readonly InputHandler _input = InputHandler.Instance;
		private List<Highscore> _highScoreList;

		private static List<Highscore> CreateHighScore()
		{
			List<Highscore> highScoreList = new List<Highscore>
			                                	{
			                                		new Highscore("Per", 10),
			                                		new Highscore("Ole", 5),
			                                	};

			return highScoreList;
		}

		public GameOver(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// LOAD CONTENT
			_menuFont = content.Load<SpriteFont>("Fonts/menuFont");
			_fontColor = Color.RoyalBlue;
			_highScoreList = CreateHighScore();
		}

		public override void Update()
		{
			if (NewScoreToAdd)
			{
				_highScoreList.Add(new Highscore("Simen", Player.Instance.Score));
				_highScoreList = _highScoreList.OrderBy(x => x.Score).Reverse().ToList();
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
			SpriteBatch.DrawString(_menuFont, "GAME OVER", new Vector2(200, 100), Color.White);


			foreach (Highscore hs in _highScoreList)
			{
				SpriteBatch.DrawString(_menuFont, hs.Name, new Vector2(150, 300 + (_shift * 150)), Color.White);
				SpriteBatch.DrawString(_menuFont, Convert.ToString(hs.Score), new Vector2(750, 300 + (_shift * 150)), Color.White);
				_shift++;
			}
		}
	}

	/// <summary>
	/// This class makes objects out of the highscore a player achieves, to "easily" keep track
	/// </summary>
	internal class Highscore
	{
		//C:\\Users\\Simen B\\SkyDrive\\XNA\\HighScore for Chilled Treat.xml

		public string Name { get; private set; }
		public int Score { get; private set; }

		/// <param name="name">The name of the Player</param>
		/// <param name="score">The score of the Player</param>
		public Highscore(String name, int score)
		{
			Name = name;
			Score = score;
		}
	}
}