// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="X&A Team">
//   Copyright (c) X&A Team. All rights reserved
// </copyright>
// <summary>
//   This class represents the player, and is used as a singleton.
// </summary>
// <author>Simen Bekkhus</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.GameClasses
{
    using System;

    using ChilledTreat.Tools;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// This class represents the player, and is used as a singleton.
    /// </summary>
    /// <remarks>
    /// To access it, write GameClasses.Player.Instance
    /// </remarks>
    public class Player
    {
        #region Fields
        /// <summary>
        /// The max health of the player
        /// </summary>
        private const int MaxHealth = GameConstants.PlayerHealth;

        /// <summary>
        /// The singleton-object of the player
        /// </summary>
        private static Player instance;

        /// <summary>
        /// The width of a single heart
        /// </summary>
        private readonly int widthOfHeart;

        /// <summary>
        /// The texture of the hearts (health-indicator)
        /// </summary>
        private readonly Texture2D heartTexture;

        /// <summary>
        /// The texture of the crates used as cover
        /// </summary>
        private readonly Texture2D coverTexture;

        /// <summary>
        /// The texture drawn when damaged (red haze)
        /// </summary>
        private readonly Texture2D damagedTexture;

        /// <summary>
        /// The source-rectangle used when drawing a full heart
        /// </summary>
        private readonly Rectangle fullHealthSource;

        /// <summary>
        /// The source-rectangle used when drawing a half heart
        /// </summary>
        private readonly Rectangle halfHealthSource;

        /// <summary>
        /// The source-rectangle used when drawing an empty heart
        /// </summary>
        private readonly Rectangle emptyHealthSource;

        /// <summary>
        /// The font used for writing the player's score to the screen
        /// </summary>
        private readonly SpriteFont scoreFont;

        /// <summary>
        /// An array of the sounds used when injured
        /// </summary>
        private readonly SoundEffect[] injuredSounds;

        /// <summary>
        /// The sound effect played when the player dies
        /// </summary>
        private readonly SoundEffect diedSound;

        /// <summary>
        /// The player's current health
        /// </summary>
        private int currentHealth;

        /// <summary>
        /// The player's current health, divided into 10. It's used for drawing the hearts (health-indicator)
        /// </summary>
        private int healthIn10;

        /// <summary>
        /// The shift applied while drawing hearts. To avoid drawing them on top of each other
        /// </summary>
        private int heartsDrawShift;

        /// <summary>
        /// The current time
        /// </summary>
        private double currentTime;

        /// <summary>
        /// The time when the player was damaged
        /// </summary>
        private double timeAtDamaged;

        /// <summary>
        /// A bool indicating whether the player was damaged (to draw a damage-indicator)
        /// </summary>
        private bool damaged;
        #endregion

        #region Constructor
        /// <summary>
        /// Prevents a default instance of the <see cref="Player"/> class from being created.
        /// </summary>
        private Player()
        {
            this.currentHealth = MaxHealth;

            // Load all textures, font and sounds
            this.heartTexture = Game1.Instance.Content.Load<Texture2D>("Images/normalUsableHeart");
            this.coverTexture = Game1.Instance.Content.Load<Texture2D>("Images/usableCoverBox");
            this.damagedTexture = Game1.Instance.Content.Load<Texture2D>("Images/damagedTint");
            this.scoreFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/ScoreFont");
            this.diedSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/Player/poor-baby");

            this.injuredSounds = new[]
                {
                    Game1.Instance.Content.Load<SoundEffect>("Sounds/Player/goddamnit"),
                    Game1.Instance.Content.Load<SoundEffect>("Sounds/Player/how-dare-you"),
                    Game1.Instance.Content.Load<SoundEffect>("Sounds/Player/im-in-trouble"),
                    Game1.Instance.Content.Load<SoundEffect>("Sounds/Player/uh")
                };

            // Create the rectangle used for drawing hearts (health indicator) on the screen
            this.widthOfHeart = this.heartTexture.Width / 3;
            this.fullHealthSource = new Rectangle(0, 0, this.widthOfHeart, this.heartTexture.Height);
            this.halfHealthSource = new Rectangle(this.widthOfHeart, 0, this.widthOfHeart, this.heartTexture.Height);
            this.emptyHealthSource = new Rectangle(this.widthOfHeart * 2, 0, this.widthOfHeart, this.heartTexture.Height);

            this.PlayerState = State.Alive;
        }

        #endregion

        #region Properties
        /// <summary>
        /// These enum-states determine what's updated, and whether or not certain actions are possible
        /// </summary>
        public enum State
        {
            /// <summary>
            /// The player's alive, waiting for orders
            /// </summary>
            Alive,

            /// <summary>
            /// The player's currently shooting
            /// </summary>
            Shooting,

            /// <summary>
            /// The player's currently reloading
            /// </summary>
            Reloading,

            /// <summary>
            /// The player's currently waiting (for reload or the "cool-down" of the weapon)
            /// </summary>
            Waiting,

            /// <summary>
            /// The player's dead
            /// </summary>
            Dead
        }

        /// <summary>
        /// Gets the singleton object of the player, for use in other classes
        /// </summary>
        public static Player Instance
        {
            get { return instance ?? (instance = new Player()); }
        }

        /// <summary>
        /// Gets a value indicating whether the player's in cover or not
        /// </summary>
        public bool InCover { get; private set; }

        /// <summary>
        /// Gets the current score of the player
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Gets or sets the current state (enum) of the player
        /// <seealso cref="State"/>
        /// </summary>
        public State PlayerState { get; set; }
        #endregion

        #region Methods for determining what happens to the player
        /// <summary>
        /// Creates a new instance of the player object, to set all variables back to their default values
        /// </summary>
        public static void ResetPlayer()
        {
            instance = new Player();

            WeaponHandler.ResetWeapons();
        }

        /// <summary>
        /// Adds 1 to the player score
        /// </summary>
        public void SuccesfullKill()
        {
            this.Score++;
        }

        /// <summary>
        /// How much the player is damaged
        /// </summary>
        /// <param name="damage">The amount of damage recieved</param>
        public void Damaged(int damage)
        {
            // If you're in godmode, you're invincible
            if (GameConstants.GodMode)
            {
                return;
            }

            if (this.InCover)
            {
                damage /= 5;
            }

#if XBOX
            // Vibrate the controller
            InputHandler.Instance.StartHardVibrate();
#endif

            // Play a random sound when injured
            this.injuredSounds[GameStates.InGame.Random.Next(this.injuredSounds.Length)].Play();

            this.damaged = true;

            // Get the current time, so we can remove the red haze after a certai time interval
            this.timeAtDamaged = FrameInfo.Instance.GameTime.TotalGameTime.TotalMilliseconds;

            this.currentHealth -= damage;

            if (this.currentHealth <= 0)
            {
                this.PlayerState = State.Dead;
            }
        }
        #endregion

        #region Update & Draw
        /// <summary>
        /// The update-method of Player
        /// </summary>
        public void Update()
        {
            this.currentTime = FrameInfo.Instance.GameTime.TotalGameTime.TotalMilliseconds;

            if (this.PlayerState == State.Dead)
            {
                EnemyHandler.Instance.Clear();
                this.diedSound.Play();
                Game1.ChangeState(GameStates.GameState.GameOver);
            }

            // If the red haze (damaged) has been drawn, and the controller vibrated, for 200 ms stop it
            if (this.damaged && this.currentTime - this.timeAtDamaged > 200)
            {
                this.damaged = false;
                InputHandler.Instance.StopVibrate();
            }

            // Divide the health into 10 parts, to easily calculate how many hearts to draw on the screen
            this.healthIn10 = this.currentHealth / 10;

            // The shift to the side, so that the hearts are not drawn on top of each other
            this.heartsDrawShift = 0;

            this.InCover = InputHandler.Instance.IsCoverDown();

            WeaponHandler.Instance.Update();
        }

        /// <summary>
        /// The draw-method of Player
        /// </summary>
        public void Draw()
        {
            // If the player's in cover, draw the cover-box
            if (this.InCover)
            {
                Game1.Instance.SpriteBatch.Draw(
                    this.coverTexture,
                    new Vector2((Game1.GameScreenWidth - (this.coverTexture.Width * Game1.GameScale)) / 2f, Game1.GameScreenHeight - (this.coverTexture.Height * Game1.GameScale)),
                    this.coverTexture.Bounds,
                    Color.White,
                    0,
                    Vector2.Zero,
                    Game1.GameScale,
                    SpriteEffects.None,
                    0);
            }

            this.DrawHealth();

            // Score indicator
            Game1.Instance.SpriteBatch.DrawString(this.scoreFont, Convert.ToString(this.Score), new Vector2(Game1.GameScreenWidth - (100 * Game1.GameScale), 20 * Game1.GameScale), Color.Black, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);

            WeaponHandler.Instance.Draw();

            // If the player was damaged, draw a red haze across the screen
            if (this.damaged)
            {
                Game1.Instance.SpriteBatch.Draw(
                    this.damagedTexture,
                    Vector2.Zero,
                    this.damagedTexture.Bounds,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    Game1.GameScale,
                    SpriteEffects.None,
                    0);
            }
        }

        /// <summary>
        /// Draw the correct amount of hearts (health-indicator) on the screen.
        /// It's cleaner having the logic behind it in it's own method
        /// </summary>
        private void DrawHealth()
        {
            // The vector calculation for the X is really ugly.
            // It takes the width of the screen, subtracts the width of 5 hearts (plus 5 on each, to put some space between them), multiplies it by the scale of the game, before adding to it so that each heart drawn is shifted the necessary pixels to the right
            for (int i = 0; i < this.healthIn10 / 2; i++)
            {
                Game1.Instance.SpriteBatch.Draw(
                    this.heartTexture,
                    new Vector2(
                        Game1.GameScreenWidth - ((this.widthOfHeart + 4) * 5 * Game1.GameScale)
                        + ((this.widthOfHeart + 5) * this.heartsDrawShift * Game1.GameScale),
                        Game1.GameScreenHeight - (this.heartTexture.Height * Game1.GameScale)),
                    this.fullHealthSource,
                    Color.White,
                    0,
                    Vector2.Zero,
                    Game1.GameScale,
                    SpriteEffects.None,
                    0);
                this.heartsDrawShift++;
            }

            // Always draw 5 hearts. If the int division ends up removing the fraction (meaning we lose 0.5 hearts), draw a half-heart
            if (this.healthIn10 % 2 != 0)
            {
                Game1.Instance.SpriteBatch.Draw(
                    this.heartTexture,
                    new Vector2(
                        Game1.GameScreenWidth - ((this.widthOfHeart + 4) * 5 * Game1.GameScale)
                        + ((this.widthOfHeart + 5) * this.heartsDrawShift * Game1.GameScale),
                        Game1.GameScreenHeight - (this.heartTexture.Height * Game1.GameScale)),
                    this.halfHealthSource,
                    Color.White,
                    0,
                    Vector2.Zero,
                    Game1.GameScale,
                    SpriteEffects.None,
                    0);
                this.heartsDrawShift++;
            }

            for (int i = this.healthIn10 / 2; i < 5; i++)
            {
                Game1.Instance.SpriteBatch.Draw(
                    this.heartTexture,
                    new Vector2(
                        Game1.GameScreenWidth - ((this.widthOfHeart + 4) * 5 * Game1.GameScale)
                        + ((this.widthOfHeart + 5) * this.heartsDrawShift * Game1.GameScale),
                        Game1.GameScreenHeight - (this.heartTexture.Height * Game1.GameScale)),
                    this.emptyHealthSource,
                    Color.White,
                    0,
                    Vector2.Zero,
                    Game1.GameScale,
                    SpriteEffects.None,
                    0);
                this.heartsDrawShift++;
            }
        }
        #endregion
    }
}