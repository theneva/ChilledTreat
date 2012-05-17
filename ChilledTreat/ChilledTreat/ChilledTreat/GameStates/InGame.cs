// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InGame.cs" company="X&A Team">
//   Copyright (c) X&A Team. All rights reserved
// </copyright>
// <summary>
//   Screen used when in a game
// </summary>
// <author>Vegard Strand</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.GameStates
{
    using System;

    using ChilledTreat.GameClasses;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Screen used when in a game
    /// </summary>
    internal class InGame : GameState
    {
        #region Fields
        /// <summary>
        /// An instance of random, to make sure that a number really is random
        /// </summary>
        public static readonly Random Random = new Random();

        /// <summary>
        /// Texture for the background
        /// </summary>
        private readonly Texture2D background;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="InGame"/> class.
        /// </summary>
        public InGame()
        {
            Game1.Instance.IsMouseVisible = false;
            this.background = Game1.Instance.Content.Load<Texture2D>("Images/bg");
        }
        #endregion

        #region Update&Draw
        /// <summary>
        /// Updates player and enemies, pauses the game if the pause key or key is pressed
        /// or the game loses focus
        /// </summary>
        internal override void Update()
        {
            Player.Instance.Update();
            EnemyHandler.Instance.Update();

            if (InputHandler.Instance.IsPausePressed() || Game1.Instance.IsActive == false)
            {
                Game1.ChangeState(PauseMenu);
            }
#if XBOX
            if (!InputHandler.Instance.IsControllerConnected())
            {
                Game1.ChangeState(PauseMenu);
            }
#endif
        }

        /// <summary>
        /// Draws background, enemies and the player
        /// </summary>
        internal override void Draw()
        {
            Game1.Instance.SpriteBatch.Draw(this.background, Vector2.Zero, this.background.Bounds, Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);

            EnemyHandler.Instance.Draw();
            Player.Instance.Draw();
        }
        #endregion
    }
}