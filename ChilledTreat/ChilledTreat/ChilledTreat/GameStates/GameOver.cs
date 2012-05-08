
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
			XmlWriter writer = null;
			try
			{

				// Create an XmlWriterSettings object with the correct options. 
				XmlWriterSettings settings = new XmlWriterSettings {Indent = true, IndentChars = ("\t"), OmitXmlDeclaration = true};

				// Create the XmlWriter object and write some content.
				writer = XmlWriter.Create("data.xml", settings);
				writer.WriteStartElement("book");
				writer.WriteElementString("item", "tesing");
				writer.WriteEndElement();

				writer.Flush();

			}
			finally
			{
				if (writer != null)
					writer.Close();
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
