// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighScore.cs" company="X&A Team">
//   Copyright (c) X&A Team. All rights reserved
// </copyright>
// <summary>
//   Highscore-class. Used for creating and managing highscores and the XML-file associated with it
// </summary>
// <author>Simen Bekkhus</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
#if XBOX
    using System.IO.IsolatedStorage;
#endif
    using System.Xml.Serialization;

    /// <summary>
    /// Highscore-class. Used for creating and managing highscores and the XML-file associated with it
    /// </summary>
    public class Highscore
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Highscore"/> class
        /// </summary>
        /// <param name="name">
        /// The name of the Player
        /// </param>
        /// <param name="score">
        /// The score of the Player
        /// </param>
        public Highscore(string name, int score)
        {
            this.Name = name;
            this.Score = score;
            this.CurrentTime = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Highscore"/> class.
        /// An empty constructor has to be used for the XML stuff
        /// </summary>
        public Highscore()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of player
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the score of the player
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the current time
        /// </summary>
        public DateTime CurrentTime { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Read the XML stored, and return a list of all entries
        /// </summary>
        /// <returns>
        /// A list of all highscores stored
        /// </returns>
        public static List<Highscore> CreateHighScore()
        {
            List<Highscore> highScoreList = DeserializeFromXml();

            return highScoreList;
        }

        /// <summary>
        /// This method serializes the list, and prints it to a file, so that it can be accessed in another session
        /// </summary>
        /// <param name="highscores">
        /// The highscores.
        /// </param>
        public static void SerializeToXml(List<Highscore> highscores)
        {
#if WINDOWS
            TextWriter writer = null;
#elif XBOX
            StreamWriter writer = null;
#endif
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Highscore>));
#if WINDOWS
                writer = new StreamWriter("HighScore.xml");
#elif XBOX
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();

            using (IsolatedStorageFileStream isoFileStream = new IsolatedStorageFileStream("HighScore.xml", FileMode.OpenOrCreate, iso))
            {
                // Write the data
                using (writer = new StreamWriter(isoFileStream))
                {
#endif
                serializer.Serialize(writer, highscores);
            }
#if XBOX
                }
            }
#endif
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// This method deserializes the XML file, and returns it in an array
        /// </summary>
        /// <returns>
        /// Returns a deserialized list of highscore-entries
        /// </returns>
        private static List<Highscore> DeserializeFromXml()
        {
            List<Highscore> scores;
#if XBOX
            scores = null;
#endif

#if WINDOWS
            if (!File.Exists("HighScore.xml"))
            {
                List<Highscore> tempLeaderBoard = new List<Highscore>();
                tempLeaderBoard.Add(new Highscore("Simen", 10));

                SerializeToXml(tempLeaderBoard);
            }

            FileStream stream = File.Open("HighScore.xml", FileMode.OpenOrCreate);
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
#elif XBOX
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("HighScore.xml", FileMode.Open, store))
                    {
                        // Read the data from the file
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Highscore>));
                        scores = (List<Highscore>)serializer.Deserialize(stream);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    store.Remove();
                }
            }
            catch (FileNotFoundException ex)
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!store.FileExists("Highscore.xml"))
                    {
                        SerializeToXml(new List<Highscore>
                                        {
                                            new Highscore("Placeholder", 1)
                                        });
                    }
                }
            }
#endif
            return scores;
        }
        #endregion
    }
}