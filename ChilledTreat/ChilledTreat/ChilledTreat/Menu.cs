using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

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
            FrameInfo inf = FrameInfo.Instance;
            // Logic here

        }

        public override void Draw()
        {
            // Draw here

        }

    }
}
