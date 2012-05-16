// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LeaderBoard.cs" company="X'nA Team">
//   Copyright (c) X'nA Team. All rights reserved
// </copyright>
// <summary>
//   The leaderboard-screen
// </summary>
// <author>Steinar Skogly</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.GameStates
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The leaderboard-screen
    /// </summary>
    internal class LeaderBoard : GameState
    {
        #region Fields
        /// <summary>
        /// Sprite-font used for drawing text
        /// </summary>
        private readonly SpriteFont menuFont;

        /// <summary>
        /// Sprite-font used for drawing the player's score
        /// </summary>
        private readonly SpriteFont scoreFont;

        /// <summary>
        /// List of all the highscores saved
        /// </summary>
        private readonly List<Highscore> highScoreList;

        /// <summary>
        /// The texture of color drawn to make borders
        /// </summary>
        private readonly Texture2D coverTexture = new Texture2D(Game1.Instance.Graphics.GraphicsDevice, 1, 1);

        /// <summary>
        /// The linebreak used for drawing items (and displaying the ranking)
        /// </summary>
        private int linebreak;

        /// <summary>
        /// Which way should the text be shifted (up or down)
        /// </summary>
        private int scrolling;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderBoard"/> class.
        /// </summary>
        public LeaderBoard()
        {
            this.highScoreList = Highscore.CreateHighScore();

            this.menuFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/menuFont");
            this.scoreFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/ScoreFont");

            // TODO: Is this needed?
            // this.coverTexture.SetData(new[] { Color.White });
        }
        #endregion

        #region Update&Draw
        /// <summary>
        /// The draw of Leaderboard
        /// </summary>
        internal override void Update()
        {
            this.linebreak = 0;

            if (InputHandler.Instance.IsAbortPressed())
            {
                Game1.ChangeState(Menu);
            }

            // Allows scrolling in the leaderboard
            if (InputHandler.Instance.IsUpPressed() && this.scrolling < 0)
            {
                this.scrolling++;
            }
            else if (InputHandler.Instance.IsDownPressed() && this.scrolling > -this.highScoreList.Count + 10)
            {
                this.scrolling--;
            }
        }

        /// <summary>
        /// The draw of Leaderboard
        /// </summary>
        internal override void Draw()
        {
            // Top/Bottom frame, this is centering the scrolling leaderboard, and hiding text under the tile
            Game1.Instance.SpriteBatch.Draw(this.coverTexture, new Rectangle(0, 0, Game1.GameScreenWidth, (int)(150 * Game1.GameScale)), Color.Black);
            Game1.Instance.SpriteBatch.Draw(this.coverTexture, new Rectangle(0, Game1.GameScreenHeight - (int)(80 * Game1.GameScale), Game1.GameScreenWidth, (int)(80 * Game1.GameScale)), Color.Black);
            
            // TODO: Set background color, constant, Game1
            // Can be removed and rather tweak font-size and spacing.
            Game1.Instance.SpriteBatch.DrawString(this.menuFont, "Leaderboard", new Vector2((Game1.GameScreenWidth / 3f) - (50 * Game1.GameScale), 50 * Game1.GameScale), Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
            
            foreach (Highscore highScore in this.highScoreList)
            {
                Game1.Instance.SpriteBatch.DrawString(this.scoreFont, Convert.ToString(++this.linebreak) + ")", new Vector2((Game1.GameScreenWidth / 3f) - 35, ((100 * Game1.GameScale) + (this.linebreak * 50) + (this.scrolling * 50)) * Game1.GameScale), Color.White);
                Game1.Instance.SpriteBatch.DrawString(this.scoreFont, highScore.Name, new Vector2(Game1.GameScreenWidth / 3f, ((100 * Game1.GameScale) + (this.linebreak * 50) + (this.scrolling * 50)) * Game1.GameScale), Color.White);
                Game1.Instance.SpriteBatch.DrawString(this.scoreFont, Convert.ToString(highScore.Score), new Vector2((Game1.GameScreenWidth / 3f) * 2f, ((100 * Game1.GameScale) + (this.linebreak * 50) + (this.scrolling * 50)) * Game1.GameScale), Color.White);
            }
        }
        #endregion
    }
}
