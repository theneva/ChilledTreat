// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PauseMenu.cs" company="X'nA Team">
//   Copyright (c) X'nA Team. All rights reserved
// </copyright>
// <summary>
//   The pause-screen
// </summary>
// <author>Vegard Strand</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.GameStates
{
    using ChilledTreat.GameClasses;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The pause-screen
    /// </summary>
    public class PauseMenu : GameState
    {
        #region Fields
        /// <summary>
        /// Font used for drawing menu-items
        /// </summary>
        private readonly SpriteFont menuFont;

        /// <summary>
        /// Available menu-items
        /// </summary>
        private readonly string[] menuItems;

        /// <summary>
        /// TODO: Why is this an array?
        /// </summary>
        private readonly float[] selectedItem;

        /// <summary>
        /// TODO: Why is this an array?
        /// </summary>
        private readonly float[] yPos;

        /// <summary>
        /// Sound played when navigating the menu
        /// </summary>
        private readonly SoundEffect buttonSound;

        /// <summary>
        /// Sound played when selecting an item
        /// </summary>
        private readonly SoundEffect selectSound;

        /// <summary>
        /// The current position in the menu
        /// </summary>
        private int menuPos;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PauseMenu"/> class.
        /// </summary>
        public PauseMenu()
        {
            this.menuFont = Game1.Instance.Content.Load<SpriteFont>("fonts/menuFont");
            string[] strings = { "Resume Game", "Main Menu" };
            this.menuItems = strings;
            this.selectedItem = new float[this.menuItems.Length];
            this.menuPos = 0;
            this.buttonSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/buttonSound");
            this.selectSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/selectSound");

            for (int i = 0; i < this.selectedItem.Length; i++)
            {
                this.selectedItem[i] = 100f * Game1.GameScale;
            }

            this.selectedItem[this.menuPos] = 150f * Game1.GameScale;

            float yStartPos = 100f;
            this.yPos = new float[this.menuItems.Length];
            for (int i = 0; i < this.yPos.Length; i++)
            {
                this.yPos[i] = yStartPos;
                yStartPos += 100f * Game1.GameScale;
            }
        }
        #endregion

        #region Update&Draw
        /// <summary>
        /// Update of PauseMenu
        /// </summary>
        internal override void Update()
        {
            // Same logic as used in the Menu class
            if (InputHandler.Instance.IsDownPressed())
            {
                this.buttonSound.Play();
                this.menuPos++;
                if (this.menuPos > this.selectedItem.Length - 1)
                {
                    this.menuPos = 0;
                }
            }

            if (InputHandler.Instance.IsUpPressed())
            {
                this.buttonSound.Play();
                this.menuPos--;
                if (this.menuPos < 0)
                {
                    this.menuPos = this.selectedItem.Length - 1;
                }
            }

            for (int i = 0; i < this.selectedItem.Length; i++)
            {
                this.selectedItem[i] = 100f * Game1.GameScale;
            }

            this.selectedItem[this.menuPos] = 150f * Game1.GameScale;

            if (InputHandler.Instance.IsPausePressed() || InputHandler.Instance.IsAbortPressed())
            {
                this.selectSound.Play();
                Game1.ChangeState(InGame);
                Game1.Instance.IsMouseVisible = false;
            }

            if (!InputHandler.Instance.IsActionPressed())
            {
                return;
            }

            if (this.menuItems[this.menuPos].Contains("Main Menu"))
            {
                this.selectSound.Play();
                Player.ResetPlayer();
                EnemyHandler.Instance.Clear();
                Game1.ChangeState(Menu);
            } 
            else if (this.menuItems[this.menuPos].Contains("Resume Game"))
            {
                this.selectSound.Play();
                Game1.ChangeState(InGame);
                Game1.Instance.IsMouseVisible = false;
            }
        }

        /// <summary>
        /// Draw of PauseMenu
        /// </summary>
        internal override void Draw()
        {
            for (int i = 0; i < this.menuItems.Length; i++)
            {
                Game1.Instance.SpriteBatch.DrawString(
                    this.menuFont,
                    this.menuItems[i],
                    new Vector2(this.selectedItem[i], this.yPos[i]),
                    Color.White,
                    0,
                    Vector2.Zero,
                    Game1.GameScale,
                    SpriteEffects.None,
                    0);
            }
        }

        #endregion
    }
}
