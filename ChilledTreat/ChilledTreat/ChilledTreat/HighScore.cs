using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

#if XBOX
using System.IO.IsolatedStorage;
#endif

namespace ChilledTreat
{
	public class Highscore
	{
		#region Fields
		public string Name { get; set; }
		public int Score { get; set; }
		public DateTime CurrentTime { get; set; }
		#endregion

		#region Constructors
		/// <param name="name">The name of the Player</param>
		/// <param name="score">The score of the Player</param>
		public Highscore(String name, int score)
		{
			Name = name;
			Score = score;
			CurrentTime = DateTime.Now;
		}

		//An empty constructor has to be used for the XML stuff
		public Highscore() { }
		#endregion

		#region Methods
		public static List<Highscore> CreateHighScore()
		{
			List<Highscore> highScoreList = DeserializeFromXml();

			return highScoreList;
		}

		//This method serializes the list, and prints it to a file, so that it can be accessed in another session
		public static void SerializeToXml(List<Highscore> highscores)
		{
			XmlSerializer serializer = new XmlSerializer(typeof (List<Highscore>));
#if WINDOWS
			TextWriter writer = new StreamWriter(Game1.Instance.Content.RootDirectory + "/HighScore.xml");
#elif XBOX
			//This shit doesn't work
			using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
			{
				using (
					IsolatedStorageFileStream writer =
						new IsolatedStorageFileStream(Game1.Instance.Content.RootDirectory + "/HighScore.xml", FileMode.Create, iso))
				{
#endif
			serializer.Serialize(writer, highscores);
			writer.Close();
#if XBOX
				}
			}
#endif
		}

		//This method deserializes the XML file, and returns it in an array
		private static List<Highscore> DeserializeFromXml()
		{
			XmlSerializer deserializer = new XmlSerializer(typeof (List<Highscore>));
			TextReader textReader = new StreamReader(Game1.Instance.Content.RootDirectory + "/HighScore.xml");
			List<Highscore> scores = (List<Highscore>) deserializer.Deserialize(textReader);
			textReader.Close();

			return scores;
		}
		#endregion
	}
}