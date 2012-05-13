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
			TextWriter writer = new StreamWriter("HighScore.xml");
#elif XBOX
			IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();

			using (IsolatedStorageFileStream isoFileStream = new IsolatedStorageFileStream("HighScore.xml", FileMode.OpenOrCreate, iso))
			{
				//Write the data
				using (StreamWriter writer = new StreamWriter(isoFileStream))
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
			List<Highscore> scores;
#if WINDOWS

			FileStream stream = File.Open("HighScore.xml", FileMode.OpenOrCreate, FileAccess.Read);
			try
			{
				// Read the data from the file
				XmlSerializer serializer = new XmlSerializer(typeof(List<Highscore>));
				scores = (List<Highscore>)serializer.Deserialize(stream);
			}
			finally
			{
				// Close the file
				stream.Close();
			}
			
			return scores;

#elif XBOX

			using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
			{
				using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("HighScore.xml", FileMode.Open, iso))
				{
					// Read the data from the file
					XmlSerializer serializer = new XmlSerializer(typeof(List<Highscore>));
					scores = (List<Highscore>)serializer.Deserialize(stream);
				}
			}

			return scores;

#endif
		}
		#endregion
	}
}