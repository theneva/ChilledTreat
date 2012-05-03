using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat
{
	class Menu : GameState
	{
		
		SpriteFont menuFont;
		Color fontColor;
		string[] menuItems;
		float[] selectedItem;
		int[] yPos;
		int menuPos;
		InputHandler input = InputHandler.Instance;

		public Menu(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// Menu content
			menuFont = Content.Load<SpriteFont>("fonts/menuFont");
			fontColor = Color.Salmon;
			string[] strings = {"New Game", "Instructions", "Credits", "EXIT"};
			menuItems = strings;
			menuPos = 0;
			selectedItem = new float[menuItems.Length];
			
			for (int i = 0; i < selectedItem.Length; i++)
			{
				selectedItem[i] = 100f;
			}
			selectedItem[menuPos] = 150f;

			
			int yStartPos = 100;
			yPos = new int[menuItems.Length];
			for (int i = 0; i < yPos.Length; i++)
			{
				yPos[i] = yStartPos;
				yStartPos += 100;
			}
		}

		public override void Update()
		{
			if (input.IsKeyPressed(input.DownKey))
			{
				menuPos++;
				if (menuPos > selectedItem.Length - 1) menuPos = 0;
			}
			if (input.IsKeyPressed(input.UpKey))
			{
				menuPos--;
				if (menuPos < 0) menuPos = selectedItem.Length - 1;
			}
			for (int i = 0; i < selectedItem.Length; i++) selectedItem[i] = 100f;
			selectedItem[menuPos] = 150f;

			if (input.IsKeyPressed(input.ActionKey))
			{
				if (menuItems[menuPos].Contains("EXIT"))
				{
					Game1.Instance.Exit();
				}
				else if (menuItems[menuPos].Contains("New Game"))
				{
					Game1.ChangeState(2);
				}
			}
		}
		public override void Draw()
		{
			
			// Draw here

			for (int i = 0; i < menuItems.Length; i++)
			{
				SpriteBatch.DrawString(menuFont, menuItems[i], new Vector2(selectedItem[i], yPos[i]), fontColor, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
				
			}
		}
	}
}
