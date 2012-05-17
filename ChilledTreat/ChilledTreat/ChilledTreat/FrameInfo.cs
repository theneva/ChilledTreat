// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameInfo.cs" company="X&A Team">
//   Copyright (c) X&A Team. All rights reserved
// </copyright>
// <summary>
//   The frameinfo contains information about the GameTime
// </summary>
// <author>Vegard Strand</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The frameinfo contains information about the GameTime
    /// </summary>
    public class FrameInfo
    {
        #region Fields
        /// <summary>
        /// The instance of the frameinfo
        /// </summary>
        public static readonly FrameInfo Instance = new FrameInfo();

        /// <summary>
        /// The current gametime
        /// </summary>
        private GameTime gameTime;
        #endregion

        #region Constructor
        /// <summary>
        /// Prevents a default instance of the <see cref="FrameInfo"/> class from being created.
        /// </summary>
        private FrameInfo()
        {
            this.gameTime = new GameTime();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets DeltaTime.
        /// </summary>
        public static double DeltaTime { get; private set; }

        /// <summary>
        /// Gets or sets GameTime.
        /// </summary>
        public GameTime GameTime
        {
            get
            {
                return this.gameTime;
            }

            set
            {
                this.gameTime = value;
                DeltaTime = this.GameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        #endregion
    }
}