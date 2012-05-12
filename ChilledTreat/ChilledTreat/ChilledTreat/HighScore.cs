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

		static public void SerializeToXml(List<Highscore> highscores)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(List<Highscore>));
#if WINDOWS
			TextWriter textWriter = new StreamWriter(Game1.Instance.Content.RootDirectory + "/HighScore.xml");

			serializer.Serialize(textWriter, highscores);
			textWriter.Close();

#elif XBOX

using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
{

using (var stream = new IsolatedStorageFileStream(Game1.Instance.Content.RootDirectory + "/HighScore.xml", FileMode.Create, iso))
{
	serializer.Serialize(stream, highscores);

}
#endif
}
		}

		public static List<Highscore> DeserializeFromXml()
		{
			XmlSerializer deserializer = new XmlSerializer(typeof(List<Highscore>));
			TextReader textReader = new StreamReader(Game1.Instance.Content.RootDirectory + "/HighScore.xml");
			XmlSerializer scores = (List<Highscore>)deserializer.Deserialize(textReader);
			textReader.Close();
			
			return scores;
		}
	}
}
