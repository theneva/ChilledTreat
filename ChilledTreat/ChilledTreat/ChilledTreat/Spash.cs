using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ChilledTreat
{
    class Splash : GameState
    {
        Texture2D SplashTexture;

        int NextState = 0;


        public Splash(SpriteBatch spriteBatch, ContentManager content, String texture, int nextState)
            : base(spriteBatch, content)
        {
            SplashTexture = Content.Load<Texture2D>(texture);
            NextState = nextState;

        }

        public override void Update()
        {
            InputHandler input = InputHandler.Instance;


            if (input.IsKeyPressed(input.ActionKey))
            {
                Game1.ChangeState(NextState);
            }

        }

        public override void Draw()
        {
            SpriteBatch.Draw(SplashTexture, Vector2.Zero, Color.White);
        }

    }
}
