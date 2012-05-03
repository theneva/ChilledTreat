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
        int menuPos;
        InputHandler input = InputHandler.Instance;

        public Menu(SpriteBatch spriteBatch, ContentManager content)
            : base(spriteBatch, content)
        {
            // Menu content
            menuFont = Content.Load<SpriteFont>("fonts/menuFont");
            fontColor = Color.Aqua;
            string[] strings = {"New Game", "Levels", "Instructions", "Credits", "EXIT"};
            menuItems = strings;
            menuPos = 0;
            selectedItem = new float[menuItems.Length];
            for (int i = 0; i < selectedItem.Length; i++)
            {
                selectedItem[i] = 1f;
            }
            selectedItem[menuPos] = 1.3f;
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
            for (int i = 0; i < selectedItem.Length; i++) selectedItem[i] = 1f;
            selectedItem[menuPos] = 1.3f;
        }
        public override void Draw()
        {
            int yPos = 100;
            // Draw here

            for (int i = 0; i < menuItems.Length; i++)
            {
                SpriteBatch.DrawString(menuFont, menuItems[i], new Vector2(150, yPos), fontColor, 0, new Vector2(0, 0), selectedItem[i], SpriteEffects.None, 0);
                yPos += 100;
            }


        }

    }
}
