// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameOver.cs" company="X'nA Team">
//   Copyright (c) X'nA Team. All rights reserved
// </copyright>
// <summary>
//   Game over-screen
// </summary>
// <author>Vegard Strand</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.GameStates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ChilledTreat.GameClasses;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Game over-screen
    /// </summary>
    internal class GameOver : GameState
    {
        #region Fields
        /// <summary>
        /// Indicates whether or not a new score should be added to the leader board
        /// </summary>
        private static bool newScoreToAdd;

        /// <summary>
        /// Font used when drawing text
        /// </summary>
        private readonly SpriteFont menuFont;

        /// <summary>
        /// Font used for the Score
        /// </summary>
        private readonly SpriteFont scoreFont;

        /// <summary>
        /// Font used for drawing the letters
        /// </summary>
        private readonly SpriteFont nameFont;

        /// <summary>
        /// TODO: I have noe idea what this does
        /// Sound played when pushing a button
        /// </summary>
        private readonly SoundEffect buttonSound;

        /// <summary>
        /// Sound played when making a choice
        /// </summary>
        private readonly SoundEffect selectSound;

        /// <summary>
        /// List of all characters (A-Z)
        /// </summary>
        private readonly char[] charList;

        /// <summary>
        /// List of highscores
        /// </summary>
        private List<Highscore> highScoreList;

        /// <summary>
        /// Sideways-shift applied when drawing the leaderboard
        /// </summary>
        private int shift;

        /// <summary>
        /// Where in the list of characters the player is
        /// </summary>
        private int charListPos;

        /// <summary>
        /// Position of the current character
        /// </summary>
        private float charPos;

        /// <summary>
        /// The player's name
        /// </summary>
        private string name;

        /// <summary>
        /// The player is typing
        /// </summary>
        private bool typing = true;

        /// <summary>
        /// Write to file
        /// </summary>
        private bool writeFile = true;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="GameOver"/> class.
        /// </summary>
        public GameOver()
        {
            // LOAD CONTENT
            this.menuFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/menuFont");
            this.buttonSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/buttonSound");
            this.selectSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/selectSound");

            this.scoreFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/ScoreFont");
            this.nameFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/nameFont");

            this.charList = new char[6];
            for (int i = 0; i < this.charList.Length; i++)
            {
                this.charList[i] = 'A';
            }

            this.highScoreList = Highscore.CreateHighScore();
        }
        #endregion

        #region Update&Draw
        /// <summary>
        /// The update of GameOver
        /// </summary>
        internal override void Update()
        {
            // Typing logic region contains the logic which enables
            // the player to type his name
            if (this.typing)
            {
                if (InputHandler.Instance.IsDownPressed())
                {
                    this.buttonSound.Play();
                    this.charList[this.charListPos]++;
                    if (this.charList[this.charListPos] > 'Z')
                    {
                        this.charList[this.charListPos] = 'A';
                    }
                }
                else if (InputHandler.Instance.IsUpPressed())
                {
                    this.buttonSound.Play();
                    this.charList[this.charListPos]--;
                    if (this.charList[this.charListPos] < 'A')
                    {
                        this.charList[this.charListPos] = 'Z';
                    }
                }
                else if (InputHandler.Instance.IsRightPressed() && this.charListPos != this.charList.Length - 1)
                {
                    this.selectSound.Play();
                    this.charListPos++;
                }
                else if (InputHandler.Instance.IsLeftPressed() && this.charListPos != 0)
                {
                    this.selectSound.Play();
                    this.charListPos--;
                }

                if (InputHandler.Instance.IsActionPressed())
                {
                    this.selectSound.Play();
                    this.typing = false;
                }
            }
            else if (this.writeFile)
            {
                foreach (char t in this.charList)
                {
                    this.name += t;
                }

                this.writeFile = false;
                newScoreToAdd = true;
            }

            if (newScoreToAdd)
            {
                this.highScoreList.Add(new Highscore(this.name, Player.Instance.Score));
                this.highScoreList = this.highScoreList.OrderByDescending(x => x.Score).ThenBy(x => x.CurrentTime).ToList();
                Highscore.SerializeToXml(this.highScoreList);
                newScoreToAdd = false;
            }

            this.shift = 0;

            if (!InputHandler.Instance.IsAbortPressed())
            {
                return;
            }

            this.buttonSound.Play();
            Game1.ChangeState(Menu);
        }

        /// <summary>
        /// The draw of GameOver
        /// </summary>
        internal override void Draw()
        {
            Game1.Instance.SpriteBatch.DrawString(this.menuFont, "GAME OVER", new Vector2((Game1.GameScreenWidth / 3f) - 80, 100 * Game1.GameScale), Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
            Game1.Instance.SpriteBatch.DrawString(this.menuFont, "Your score: " + Player.Instance.Score, new Vector2(340 * Game1.GameScale, 170 * Game1.GameScale), Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);

            if (this.typing)
            {
                Game1.Instance.SpriteBatch.DrawString(this.nameFont, "Name: ", new Vector2(400, 250) * Game1.GameScale, Color.White);
                this.charPos = 570 * Game1.GameScale;
                for (int i = 0; i < this.charList.Length; i++)
                {
                    if (i != this.charListPos)
                    {
                        Game1.Instance.SpriteBatch.DrawString(
                            this.nameFont,
                            string.Empty + this.charList[i],
                            new Vector2(this.charPos, 250 * Game1.GameScale),
                            Color.White);
                    }
                    else
                    {
                        Game1.Instance.SpriteBatch.DrawString(
                            this.nameFont,
                            string.Empty + this.charList[this.charListPos],
                            new Vector2(this.charPos, 240 * Game1.GameScale),
                            Color.White);
                    }

                    this.charPos += 50 * Game1.GameScale;
                }
            }

            foreach (var hs in this.highScoreList)
            {
                Game1.Instance.SpriteBatch.DrawString(this.scoreFont, Convert.ToString(++this.shift) + ")", new Vector2((Game1.GameScreenWidth / 3f) - 25, (300 * Game1.GameScale) + (this.shift * 50)), Color.White);
                Game1.Instance.SpriteBatch.DrawString(this.scoreFont, hs.Name, new Vector2(Game1.GameScreenWidth / 3f, (300 * Game1.GameScale) + (this.shift * 50)), Color.White);
                Game1.Instance.SpriteBatch.DrawString(this.scoreFont, Convert.ToString(hs.Score), new Vector2((Game1.GameScreenWidth / 3f) * 2f, (300 * Game1.GameScale) + (this.shift * 50)), Color.White);
            }
        }

        #endregion
    }
}