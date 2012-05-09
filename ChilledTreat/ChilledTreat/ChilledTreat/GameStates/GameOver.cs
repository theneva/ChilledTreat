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

		private static List<Highscore> CreateHighScore()
		{
			List<Highscore> highScoreList = Highscore.DeserializeFromXml();

			return highScoreList;
		}

		public GameOver(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// LOAD CONTENT
			_menuFont = content.Load<SpriteFont>("Fonts/menuFont");
			_scoreFont = content.Load<SpriteFont>("Fonts/ScoreFont");
			_fontColor = Color.RoyalBlue;
			_highScoreList = CreateHighScore();
		}

		public override void Update()
		{
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

	/// <summary>
	/// This class makes objects out of the highscore a player achieves, to "easily" keep track
	/// </summary>
	public class Highscore
	{
		public string Name { get;  set; }
		public int Score { get;  set; }
		public DateTime CurrentTime { get; set; }

		/// <param name="name">The name of the Player</param>
		/// <param name="score">The score of the Player</param>
		public Highscore(String name, int score)
		{
			Name = name;
			Score = score;
			CurrentTime = DateTime.Now;
		}

		public Highscore() { }

		static public void SerializeToXml(List<Highscore> highscores)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(List<Highscore>));
			TextWriter textWriter = new StreamWriter(Game1.Instance.Content.RootDirectory + "/HighScore.xml");
			serializer.Serialize(textWriter, highscores);
			textWriter.Close();
		}

		public static List<Highscore> DeserializeFromXml()
		{
			XmlSerializer deserializer = new XmlSerializer(typeof(List<Highscore>));
			TextReader textReader = new StreamReader(Game1.Instance.Content.RootDirectory + "/HighScore.xml");
			List<Highscore> scores = (List<Highscore>)deserializer.Deserialize(textReader);
			textReader.Close();

			return scores;
		}
	}
}