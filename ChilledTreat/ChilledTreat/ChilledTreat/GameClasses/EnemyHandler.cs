// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnemyHandler.cs" company="X'nA Team">
//   Copyright (c) X'nA Team. All rights reserved
// </copyright>
// <summary>
//   The enemy-handler
// </summary>
// <author>Martin Lehmann</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.GameClasses
{
    using System.Collections.Generic;

    using ChilledTreat.Tools;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The enemy-handler
    /// </summary>
    public class EnemyHandler
    {
        #region Fields
        /// <summary>
        /// Time between each enemy added
        /// </summary>
        private const int AddEnemyInterval = GameConstants.AddEnemyInterval; // milliseconds

        /// <summary>
        /// Returns the singleton; defines it if null
        /// </summary>
        private static EnemyHandler instance;

        /// <summary>
        /// The list of all the enemies currently in the game
        /// </summary>
        private readonly List<Enemy> enemies;

        /// <summary>
        /// Time passed since last added enemy
        /// </summary>
        private float timeSinceLastAdd;

        /// <summary>
        /// The number of enemies generated per second
        /// </summary>
        private float enemiesPerSecond = GameConstants.InitialEnemiesPerSecond;

        /// <summary>
        /// Time passed since the last time the interval between each enemy added was increased
        /// </summary>
        private int timeSinceLastIntervalIncrease;
        #endregion

        #region Constructor
        /// <summary>
        /// Prevents a default instance of the <see cref="EnemyHandler"/> class from being created.
        /// </summary>
        private EnemyHandler()
        {
            this.enemies = new List<Enemy>();
            this.EnemyTexture = Game1.Instance.Content.Load<Texture2D>("Images/enemy");
            this.MuzzleFlareTexture = Game1.Instance.Content.Load<Texture2D>("Images/usableMuzzleFlare");
            this.SoundEffects = new List<SoundEffect>
                {
                    Game1.Instance.Content.Load<SoundEffect>("Sounds/Enemy/EnemyGrunt1"),
                    Game1.Instance.Content.Load<SoundEffect>("Sounds/Enemy/EnemyGrunt2"),
                    Game1.Instance.Content.Load<SoundEffect>("Sounds/Enemy/EnemyGrunt3"),
                    Game1.Instance.Content.Load<SoundEffect>("Sounds/Enemy/EnemyGrunt4"),
                    Game1.Instance.Content.Load<SoundEffect>("Sounds/Enemy/EnemyGrunt5")
                };
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the instance of the enemy-handler
        /// </summary>
        public static EnemyHandler Instance
        {
            get { return instance ?? (instance = new EnemyHandler()); }
        }

        /// <summary>
        /// Gets the enemy's texture
        /// </summary>
        public Texture2D EnemyTexture { get; private set; }

        /// <summary>
        /// Gets the muzzle-flare's texture
        /// </summary>
        public Texture2D MuzzleFlareTexture { get; private set; }

        /// <summary>
        /// Gets SoundEffects.
        /// </summary>
        public List<SoundEffect> SoundEffects { get; private set; }
        #endregion

        #region Reset-method
        /// <summary>
        /// Creates a new enemy handler to reset all values on "new game"
        /// </summary>
        public static void ResetEnemyHandler()
        {
            instance = new EnemyHandler();
        }
        #endregion

        #region Update
        /// <summary>
        /// Adds the given number of enemies to the screen every AddEnemyInterval milliseconds;
        /// increases that interval at that given interval.
        /// Updates each enemy in the list.
        /// </summary>
        public void Update()
        {
            if ((this.timeSinceLastIntervalIncrease += Game1.Instance.TargetElapsedTime.Milliseconds) >= AddEnemyInterval)
            {
                this.timeSinceLastIntervalIncrease -= AddEnemyInterval;
                ++this.enemiesPerSecond;
            }

            this.timeSinceLastAdd += Game1.Instance.TargetElapsedTime.Milliseconds;

            if (this.timeSinceLastAdd >= 1000 / this.enemiesPerSecond)
            {
                this.AddEnemy();
                this.timeSinceLastAdd -= 1000 / this.enemiesPerSecond;
            }

            for (int i = this.enemies.Count - 1; i >= 0; i--)
            {
                this.enemies[i].Update();
            }
        }

        #endregion

        #region Draw
        /// <summary>
        /// Draws each enemy currently in the list.
        /// Loops through the list backwards so that enemies be drawn
        /// in correct order based on spawn time.
        /// </summary>
        public void Draw()
        {
            for (int i = this.enemies.Count - 1; i >= 0; i--)
            {
                this.enemies[i].Draw();
            }
        }
        #endregion

        #region Methods
        // These are for later use

        /// <summary>
        /// Adds an enemy to the list.
        /// </summary>
        public void AddEnemy()
        {
            this.enemies.Add(new Enemy());
        }

        /// <summary>
        /// Add an enemy to the list, and it's defining health
        /// </summary>
        /// <param name="value">
        /// The health given the enemy
        /// </param>
        /// <param name="isHealth">
        /// A bool indicating if this is an enemy given custom health
        /// </param>
        public void AddEnemy(int value, bool isHealth)
        {
            this.enemies.Add(new Enemy(value, isHealth));
        }

        /// <summary>
        /// Removes an enemy from the list.
        /// </summary>
        /// <param name="enemy">The enemy to remove</param>
        public void Remove(Enemy enemy)
        {
            this.enemies.Remove(enemy);
        }

        /// <summary>
        /// Wipes (clears) the list for a fresh start.
        /// </summary>
        public void Clear()
        {
            this.enemies.Clear();
        }

        /// <summary>
        /// Lets the handler check if an enemy is hit, and damage that one enemy.
        /// </summary>
        /// <param name="attackedArea">Rectangle targeted by player's weapon</param>
        /// <param name="inflictedDamage">Damage the weapon inflicted</param>
        public void FiredAt(Rectangle attackedArea, int inflictedDamage)
        {
            // Hackish as fuck, but it works. A for-each loop won't do here because the
            // amount of enemies will change if one is killed.
            List<Enemy> enemiesReversed = this.enemies;
            enemiesReversed.Reverse();

            for (int i = this.GetNumberOfEnemies() - 1; i >= 0; i--)
            {
                if (enemiesReversed[i].GetRectangle().Intersects(attackedArea))
                {
                    if (enemiesReversed[i].Alive)
                    {
                        enemiesReversed[i].TakeDamage(inflictedDamage);
                    }

                    if (!WeaponHandler.Instance.Splash)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the amount of enemies currently in the list.
        /// </summary>
        /// <returns>The number of enemies</returns>
        private int GetNumberOfEnemies()
        {
            return this.enemies.Count;
        }
        #endregion
    }
}