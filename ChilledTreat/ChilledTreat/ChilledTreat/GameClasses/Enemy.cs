// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Enemy.cs" company="X'nA Team">
//   Copyright (c) X'nA Team. All rights reserved
// </copyright>
// <summary>
//   The enemy-class
// </summary>
// <author>Martin Lehmann</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.GameClasses
{
    using ChilledTreat.GameStates;
    using ChilledTreat.Tools;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The enemy-class
    /// </summary>
public 
    class Enemy
    {
        #region Fields
        /// <summary>
        /// Defined animation-time
        /// </summary>
        private const int MillisecondsPerFrame = 100;

        /// <summary>
        /// The damage received to the enemy
        /// </summary>
        private readonly int damage;

        /// <summary>
        /// The texture of the enemy
        /// </summary>
        private readonly Texture2D texture;

        /// <summary>
        /// The texture of the muzzle-flare
        /// </summary>
        private readonly Texture2D muzzleFlare;

        /// <summary>
        /// The height of the road in the background texture
        /// </summary>
        private readonly int heightOfRoad;

        /// <summary>
        /// The width of the road in the background texture
        /// </summary>
        private readonly int widthOfRoad;

        /// <summary>
        /// The length from the edge to the road in the background texture
        /// </summary>
        private readonly int fromEdgeToRoad;

        /// <summary>
        /// The enemy's health
        /// </summary>
        private int health;

        /// <summary>
        /// The enemy's position
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The speed of the enemy
        /// </summary>
        private Vector2 speed;

        /// <summary>
        /// The damage the enemy inflicts on the player
        /// </summary>
        private int damageInflicted;

        /// <summary>
        /// Time since last frame
        /// </summary>
        private int timeSinceLastFrame;

        /// <summary>
        /// The size of each frame on the enemy sprite-sheet
        /// </summary>
        private Point frameSize;

        /// <summary>
        /// The point of the current frame
        /// </summary>
        private Point currentFrame;

        /// <summary>
        /// The size of the sprite-sheet
        /// </summary>
        private Point sheetSize;

        /// <summary>
        /// How far the current frame is from the origin of the first frame
        /// </summary>
        private int currentFrameOrigin;

        /// <summary>
        /// A bool for whether or not the muzzle-flare should be drawn
        /// </summary>
        private bool drawMuzzleFlare;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class. 
        /// Default constructor = default values
        /// </summary>
        public Enemy()
            : this(GameConstants.EnemyHealth, GameConstants.EnemyDamage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class. 
        /// Specifies one value and sets the other to default based on a boolean
        /// </summary>
        /// <param name="value">
        /// The value to specify
        /// </param>
        /// <param name="isHealth">
        /// Whether or not the value to specify is health
        /// </param>
        public Enemy(int value, bool isHealth)
            : this(isHealth ? value : GameConstants.EnemyHealth, isHealth ? GameConstants.EnemyHealth : value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class. 
        /// Main constructor. Assigns each field.
        /// </summary>
        /// <param name="health">
        /// Max health of enemy
        /// </param>
        /// <param name="damage">
        /// Max damage the enemy can deal
        /// </param>
        public Enemy(int health, int damage)
        {
            this.Alive = true;
            this.health = health;
            this.damage = damage;

            this.speed = new Vector2(0, 0.5f);

            this.texture = EnemyHandler.Instance.EnemyTexture;
            this.currentFrame = new Point(0, 0);
            this.frameSize = new Point(41, 58);
            this.sheetSize = new Point(6, 0);

            // Redundant but not sufficiently inefficient for a workaround
            this.Scale = 0.008f * this.position.Y;

            // 510 is the height of the road, 192 is the width of the top of the road,
            // 540 is the distance from the left border to the top of the road
            this.heightOfRoad = (int)(540 * Game1.GameScale);
            this.widthOfRoad = (int)(192 * Game1.GameScale);
            this.fromEdgeToRoad = (int)(510 * Game1.GameScale);

            this.position = new Vector2(this.fromEdgeToRoad + InGame.Random.Next(this.widthOfRoad) - (this.frameSize.X * this.Scale), Game1.GameScreenHeight - this.heightOfRoad - (this.frameSize.Y * this.Scale));

            this.muzzleFlare = EnemyHandler.Instance.MuzzleFlareTexture;
            this.drawMuzzleFlare = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether or not the enemy is Alive.
        /// </summary>
        public bool Alive { get; private set; }

        /// <summary>
        /// Gets or sets the scale of the enemy
        /// </summary>
        private float Scale { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// The texture's surrounding rectangle
        /// </summary>
        /// <returns>Returns a new rectangle based on the texture's size and position</returns>
        public Rectangle GetRectangle()
        {
            return new Rectangle((int)this.position.X, (int)this.position.Y, (int)(this.Scale * this.frameSize.X), (int)(this.Scale * this.frameSize.Y));
        }

        /// <summary>
        /// Updates health based on damage inflicted by player.
        /// </summary>
        /// <param name="inflictedDamage">The damage infilcted to the enemy</param>
        public void TakeDamage(int inflictedDamage)
        {
            this.health -= inflictedDamage;
            if (this.health <= 0)
            {
                this.Die();
            }
        }
#endregion

        #region Update
        /// <summary>
        /// Selects the proper frames to draw, attacks if possible, and controls movement
        /// </summary>
        public void Update()
        {
            this.timeSinceLastFrame += FrameInfo.Instance.GameTime.ElapsedGameTime.Milliseconds;
            if (this.timeSinceLastFrame > MillisecondsPerFrame)
            {
                this.timeSinceLastFrame -= MillisecondsPerFrame;

                // Sneaky
                this.drawMuzzleFlare = false;

                if (++this.currentFrame.X >= this.sheetSize.X)
                {
                    if (!this.Alive)
                    {
                        EnemyHandler.Instance.Remove(this);
                        return;
                    }

                    this.currentFrame.X = 0;
                    if (++this.currentFrame.Y >= this.sheetSize.Y)
                    {
                        this.currentFrame.Y = this.currentFrameOrigin;
                    }
                }
            }

            if (!this.Alive)
            {
                return;
            }

            if (InGame.Random.Next(1000) == 0 && this.position.Y > Game1.GameScreenHeight - (400 * Game1.GameScale))
            {
                this.Attack();
                this.drawMuzzleFlare = true;
            }

            this.position.Y += this.speed.Y;

            this.Scale = 0.008f * this.position.Y;

            if (this.position.Y > Game1.GameScreenHeight - (300 * Game1.GameScale))
            {
                this.position.Y = Game1.GameScreenHeight - (300 * Game1.GameScale);
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// the draw-method
        /// </summary>
        public void Draw()
        {
            Game1.Instance.SpriteBatch.Draw(
                this.texture,
                this.position,
                new Rectangle(
                    this.currentFrame.X * this.frameSize.X,
                    this.currentFrame.Y * this.frameSize.Y,
                    this.frameSize.X,
                    this.frameSize.Y),
                Color.White,
                0,
                Vector2.Zero,
                this.Scale * Game1.GameScale,
                SpriteEffects.None,
                0);

            if (!this.drawMuzzleFlare)
            {
                return;
            }

            Game1.Instance.SpriteBatch.Draw(this.muzzleFlare, new Vector2(this.position.X - (this.muzzleFlare.Width / 2f), (this.position.Y - (this.muzzleFlare.Height / 2f) + (this.currentFrame.X * this.frameSize.X / 2f)) * Game1.GameScale), Color.White);
        }

        /// <summary>
        /// Damages the player based on max damage
        /// </summary>
        private void Attack()
        {
            this.damageInflicted = InGame.Random.Next(this.damage);
            Player.Instance.Damaged(this.damageInflicted);
        }

        /// <summary>
        /// Attributes the player for the kill, plays through the sprite-sheet,
        /// and finally removes the enemy from the list
        /// </summary>
        private void Die()
        {
            this.Alive = false;

            // Play a random grunt
            EnemyHandler.Instance.SoundEffects[InGame.Random.Next(EnemyHandler.Instance.SoundEffects.Count)].Play();
            Player.Instance.SuccesfullKill();

            this.currentFrame = new Point(0, 1);
            this.currentFrameOrigin = this.frameSize.Y;

            // Width, height of dead enemies - could use some improvement
            this.frameSize = new Point(78, 60);
            this.sheetSize = new Point(3, 1);
        }
#endregion
    }
}
