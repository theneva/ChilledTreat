// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Splash.cs" company="X&A Team">
//   Copyright (c) X&A Team. All rights reserved
// </copyright>
// <summary>
//   The splash-screen
// </summary>
// <author>Vegard Strand</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.GameStates
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The splash-screen
    /// </summary>
    public class Splash : GameState
    {
        #region Fields
        /// <summary>
        /// The texture for the splash-image
        /// </summary>
        private readonly Texture2D splash;

        /// <summary>
        /// Font used for writing text
        /// </summary>
        private readonly SpriteFont welcomeFont;

        /// <summary>
        /// A string used to indicate what button the user should press to continue
        /// </summary>
        private readonly string continueInput;

        /// <summary>
        /// Should the welcome-screen be shown
        /// </summary>
        private bool welcomeScreen;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Splash"/> class.
        /// </summary>
        public Splash()
        {
            this.splash = Game1.Instance.Content.Load<Texture2D>("Images/chilledTreat");
            this.welcomeFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/welcomeFont");

            // Inform the user how to continue according to which platform is used
            this.continueInput =
#if WINDOWS
                "Press the return key to continue";
#elif XBOX
                "Press the A button to continue";

#elif WINDOWS_PHONE
                "Press the screen to continue"

#endif
        }
        #endregion

        #region Update&Draw
        /// <summary>
        /// Update of Splash
        /// </summary>
        internal override void Update()
        {
            InputHandler input = InputHandler.Instance;

            if (!input.IsActionPressed())
            {
                return;
            }

            if (!this.welcomeScreen)
            {
                this.welcomeScreen = true;
            }
            else
            {
                Game1.ChangeState(Menu);
            }
        }

        /// <summary>
        /// Draw of Splash
        /// </summary>
        internal override void Draw()
        {
            if (!this.welcomeScreen)
            {
                Game1.Instance.SpriteBatch.Draw(
                    this.splash,
                    Vector2.Zero,
                    this.splash.Bounds,
                    Color.White,
                    0,
                    Vector2.Zero,
                    Game1.GameScale,
                    SpriteEffects.None,
                    0);
            }
            else
            {
                Game1.Instance.SpriteBatch.DrawString(
                    this.welcomeFont,
                    "Welcome to Chilled Treat!",
                    new Vector2(Game1.GameScreenWidth / 10, Game1.GameScreenHeight / 5),
                    Color.White,
                    0,
                    Vector2.Zero,
                    Game1.GameScale,
                    SpriteEffects.None,
                    0);

                Game1.Instance.SpriteBatch.DrawString(
                    this.welcomeFont,
                    this.continueInput,
                    new Vector2(Game1.GameScreenWidth / 10, Game1.GameScreenHeight * 0.8f),
                    Color.White,
                    0,
                    Vector2.Zero,
                    Game1.GameScale / 2f,
                    SpriteEffects.None,
                    0);
            }
        }
        #endregion
    }
}
