using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChilledTreat
{
    class Menu : GameState
    {
        int NextState = 0;

        public Menu(SpriteBatch spriteBatch, ContentManager content, int nextState)
            : base(spriteBatch, content)
        {
            // Menu content

            NextState = nextState;
        }

        public override void Update()
        {
            // Logic here
           
        }

        public override void Draw()
        {
            // Draw here

        }

    }
}
