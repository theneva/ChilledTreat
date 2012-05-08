using System.Collections.Generic;
using System.Xml;
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
		SpriteFont menuFont;
		Color fontColor;
		InputHandler input = InputHandler.Instance;

		public static void WriteHighScore()
		{
			try
			{
				String[,] scoreArray = new String[5, 2];
				
				scoreArray[0, 0] = "Simen";
				scoreArray[0, 1] = "50";
				scoreArray[0, 0] = "Martin";
				scoreArray[0, 1] = "-40";
				scoreArray[0, 0] = "Vegard";
				scoreArray[0, 1] = "10";
				scoreArray[0, 0] = "Steinar";
				scoreArray[0, 1] = "20";
				scoreArray[0, 0] = "Pluto";
				scoreArray[0, 1] = "30";

				// Creates an XML file is not exist 
				XmlTextWriter writer = new XmlTextWriter("C:\\temp\\xmltest.xml", null);
				// Starts a new document 
				writer.WriteStartDocument();
				// Add elements to the file 

				writer.WriteProcessingInstruction("Instruction", "Person Record");
				// Add elements to the file 
				writer.WriteStartElement("p", "person", "urn:person"); 
				for(int i = 0; i <= scoreArray.GetUpperBound(0); i++)
				{
					writer.WriteStartElement("Navn", "");
					writer.WriteString(scoreArray[i, 0]);
					writer.WriteEndElement();
					writer.WriteStartElement("Score", "");
					writer.WriteString(scoreArray[i, 1]);
					writer.WriteEndElement();
				}
				// Ends the document 
				writer.WriteEndDocument();

				writer.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());

			}

		}




		public GameOver(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// LOAD CONTENT
			menuFont = content.Load<SpriteFont>("Fonts/menuFont");
			fontColor = Color.RoyalBlue;
		}

		public override void Update()
		{
			if (input.IsAbortPressed())
			{
				Game1.ChangeState(GameState.Menu);
			}
		}

		public override void Draw()
		{
			SpriteBatch.DrawString(menuFont, "GAME OVER\nU DIED! LuL", new Vector2(200, 100), Color.White);
			SpriteBatch.DrawString(menuFont, "Score: " + Convert.ToString(Player.Instance.Score), new Vector2(300, 300), Color.White);
		}
	}
}
