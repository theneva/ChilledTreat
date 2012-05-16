// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameConstants.cs" company="X'nA Team">
//   Copyright (c) X'nA Team. All rights reserved
// </copyright>
// <summary>
//   A list of game-constants. Used for testing, and having fun
// </summary>
// <author>Martin Lehmann</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.Tools
{
    /// <summary>
    /// A list of game-constants. Used for testing, and having fun
    /// </summary>
    public static class GameConstants
    {
        /// <summary>
        /// Is the player God?
        /// </summary>
        public const bool GodMode = false;

        /// <summary>
        /// The health of the player
        /// </summary>
        public const int PlayerHealth = 100;

        /// <summary>
        /// the health of the enemy
        /// </summary>
        public const int EnemyHealth = 10;

        /// <summary>
        /// The damage the enemy does
        /// </summary>
        public const int EnemyDamage = 15;

        /// <summary>
        /// How many enemies are spawning per second
        /// </summary>
        /// <remarks>If God-mode is active, the amount is way higher than normal</remarks>
        public const float InitialEnemiesPerSecond = GodMode ? 20f : 2f;

        /// <summary>
        /// The interval an additional enemy is added
        /// </summary>
        public const int AddEnemyInterval = 5000; // milliseconds
    }
}