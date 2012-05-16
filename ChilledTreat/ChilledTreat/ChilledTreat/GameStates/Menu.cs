// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Menu.cs" company="X'nA Team">
//   Copyright (c) X'nA Team. All rights reserved
// </copyright>
// <summary>
//   The main menu-screen
// </summary>
// <author>Vegard Strand</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.GameStates
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The main menu-screen
    /// </summary>
    public class Menu : GameState
    {
        #region Fields
        /// <summary>
        /// The font used for drawing text
        /// </summary>
        private readonly SpriteFont menuFont;

        /// <summary>
        /// List of all the items in the menu
        /// </summary>
        private readonly string[] menuItems;

        /// <summary>
        /// TODO: Why is this an array???
        /// </summary>
        private readonly float[] selectedItem;

        /// <summary>
        /// TODO: Why is this an array???
        /// </summary>
        private readonly float[] yPos;

        /// <summary>
        /// The sound played when navigating the menu
        /// </summary>
        private readonly SoundEffect menuSound;

        /// <summary>
        /// Sound played when selecting an item
        /// </summary>
        private readonly SoundEffect selectSound;

        /// <summary>
        /// The currently chosen, hovered, menu-item
        /// </summary>
        private int menuPos;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        public Menu()
        {
            // Menu content
            this.menuFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/menuFont");

            // Array with menu titles
            string[] strings = { "New Game", "Instructions", "Leaderboard", "Credits", "EXIT" };
            this.menuItems = strings;

            this.menuPos = 0; // Keeps track on where in the menu the user is
            this.selectedItem = new float[this.menuItems.Length]; // Used to set a specific position for the menu item which is selected

            // Navigation sounds
            this.menuSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/buttonSound");
            this.selectSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/selectSound");

            // For loops sets X position on the menu items
            for (int i = 0; i < this.selectedItem.Length; i++)
            {
                this.selectedItem[i] = 100f * Game1.GameScale;
            }

            // The selected item (menuPos keeps track on which item is selected)
            // sets X position on the selectedItem
            this.selectedItem[this.menuPos] = 150f * Game1.GameScale;

            // Sets the Y position from which the menu will be placed
            float yStartPos = 100 * Game1.GameScale;

            // An array holds all the Y position of all the menu items.
            // The array is filled by a for loop which fills it according
            // to the numbers of menuItems
            this.yPos = new float[this.menuItems.Length];
            for (int i = 0; i < this.yPos.Length; i++)
            {
                this.yPos[i] = yStartPos;
                yStartPos += 100 * Game1.GameScale;
            }
        }
        #endregion

        #region Update&Draw
        /// <summary>
        /// Update of Menu
        /// </summary>
        internal override void Update()
        {
            // Navigation
            if (InputHandler.Instance.IsDownPressed())
            {
                this.menuSound.Play();
                this.menuPos++;
                if (this.menuPos > this.selectedItem.Length - 1)
                {
                    this.menuPos = 0;
                }
            }

            if (InputHandler.Instance.IsUpPressed())
            {
                this.menuSound.Play();
                this.menuPos--;
                if (this.menuPos < 0)
                {
                    this.menuPos = this.selectedItem.Length - 1;
                }
            }

            // The selectedItem's position is set with the menuPos
            for (int i = 0; i < this.selectedItem.Length; i++)
            {
                this.selectedItem[i] = 100f * Game1.GameScale;
            }

            this.selectedItem[this.menuPos] = 150f * Game1.GameScale;

            // The method returns when the used does not choose a menuItem
            if (!InputHandler.Instance.IsActionPressed())
            {
                return;
            }

            // GameState is changed according to what the user selects.
            this.selectSound.Play();
            if (this.menuItems[this.menuPos].Contains("EXIT"))
            {
                Game1.Instance.Exit();
            }
            else if (this.menuItems[this.menuPos].Contains("New Game"))
            {
                Game1.NewGame();
                Game1.ChangeState(InGame);
            }
            else if (this.menuItems[this.menuPos].Contains("Instructions"))
            {
                Game1.ChangeState(Instructions);
            }
            else if (this.menuItems[this.menuPos].Contains("Credits"))
            {
                Game1.NewCredits();
                Game1.ChangeState(Credits);
            }
            else if (this.menuItems[this.menuPos].Contains("Leaderboard"))
            {
                Game1.NewLeaderBoard();
                Game1.ChangeState(LeaderBoard);
            }
        }

        /// <summary>
        /// Draw of Menu
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
