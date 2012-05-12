using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace ChilledTreat
{
	public class Highscore
	{
		public string Name { get; set; }
		public int Score { get; set; }
		public DateTime CurrentTime { get; set; }

		/// <param name="name">The name of the Player</param>
		/// <param name="score">The score of the Player</param>
		public Highscore(String name, int score)
		{
			Name = name;
			Score = score;
			CurrentTime = DateTime.Now;
		}

		public static List<Highscore> CreateHighScore()
		{
			List<Highscore> highScoreList = Highscore.DeserializeFromXml();

			return highScoreList;
		}

		public Highscore() { }

		public static void SerializeToXml(List<Highscore> highscores)
		{
			XmlSerializer serializer = new XmlSerializer(typeof (List<Highscore>));
#if WINDOWS
			TextWriter textWriter = new StreamWriter(Game1.Instance.Content.RootDirectory + "/HighScore.xml");

#elif XBOX

			using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
			{
				using (
					IsolatedStorageFileStream textWriter =
						new IsolatedStorageFileStream(Game1.Instance.Content.RootDirectory + "/HighScore.xml", FileMode.Create, iso))
				{
#endif
					serializer.Serialize(textWriter, highscores);
					textWriter.Close();
#if XBOX

				}
			}
#endif
		}

		public static List<Highscore> DeserializeFromXml()
		{
			XmlSerializer deserializer = new XmlSerializer(typeof (List<Highscore>));
			TextReader textReader = new StreamReader(Game1.Instance.Content.RootDirectory + "/HighScore.xml");
			var scores = (List<Highscore>) deserializer.Deserialize(textReader);
			textReader.Close();

			return scores;
		}
	}
}