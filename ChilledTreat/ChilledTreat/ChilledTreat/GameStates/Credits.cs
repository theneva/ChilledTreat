// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Credits.cs" company="X&A Team">
//   Copyright (c) X&A Team. All rights reserved
// </copyright>
// <summary>
//   The credits-screen
// </summary>
// <author>Steinar Skogly</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.GameStates
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Media;

    /// <summary>
    /// The credits-screen
    /// </summary>
    public class Credits : GameState
    {
        #region Fields
        /// <summary>
        /// Font used when writing the credits
        /// </summary>
        private readonly SpriteFont creditsFont;

        /// <summary>
        /// Font used when drawing the sources
        /// </summary>
        private readonly SpriteFont creditsFontSources;

        /// <summary>
        /// String array containing the credits
        /// </summary>
        private readonly string[] creditsContent;

        /// <summary>
        /// String array containing the sources
        /// </summary>
        private readonly string[] creditsContentSources;

        /// <summary>
        /// The texture for the XNA-logo
        /// </summary>
        private readonly Texture2D xnaLogo;

        /// <summary>
        /// The texture for the NITH-logo
        /// </summary>
        private readonly Texture2D nithLogo;

        /// <summary>
        /// The background-music
        /// </summary>
        private readonly Song creditMusic;

        /// <summary>
        /// Linebreak applied when drawing credits
        /// </summary>
        private int linebreak;

        /// <summary>
        /// Start the credits-screen over 
        /// </summary>
        private bool startAnew = true;

        /// <summary>
        /// It is time to draw the logos
        /// </summary>
        private bool drawLogos;

        /// <summary>
        /// Start playing the song
        /// </summary>
        private bool songstart;

        /// <summary>
        /// Gametime when starting the credits
        /// </summary>
        private double startCreditsTimer;

        /// <summary>
        /// Current gametime
        /// </summary>
        private double currentTime;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Credits"/> class.
        /// </summary>
        public Credits()
        {
            // Credits content
            this.xnaLogo = Game1.Instance.Content.Load<Texture2D>("Images/xnaLogo");
            this.nithLogo = Game1.Instance.Content.Load<Texture2D>("Images/nithLogo");
            this.creditsFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/CreditsFont");
            this.creditsFontSources = Game1.Instance.Content.Load<SpriteFont>("Fonts/urlFont");
            this.creditsContent = new[]
                { // People who have worked on this project
                    ".....CREDITS.....", "Programmers", " ", "Simen Bekkhus", "Vegard Strand",
                    "Martin Lehmann", "Steinar Skogly", " ", "Designers", " ", "Simen Bekkhus",
                    "Vegard Strand", "Martin Lehmann", "Steinar Skogly", " ", "Sources", " "
                };

            // Source-paths to some of the content used in this project.
            this.creditsContentSources = new[]
                {
                    "blogcdn.com/joystiq.com/media/2006/06/reticule.jpg",
                    "tactical-life.com/online/wp-content/uploads/2008/11/bullet.gif", "soundbible.com/tags-gun.html",
                    "dragoart.com/tuts/pics/9/5990/33809/how-to-draw-an-easy-heart-step-6.jpg",
                    "filterforge.com/filters/9452.jpg",
                    "gimp.pixtuts.com/images/tutorials/fire-explosion/fire-explosion.jpg",
                    "sdb.drshnaps.com/objects/17/1513/Sprite/MIStanSheet.png", "schwar.fxhome.com/flare/machine_gun_seed_1.jpg",
                    "switchonthecode.com/tutorials/csharp-tutorial-xml-serialization", "pacdv.com/sounds/voices-1.html",
                    "media.photobucket.com/image/recent/USCMC/nationstates/Cartridges_pistol.png",
                    "freewebs.com/callpoll100/photos/Sprites/DoomWeaponsPC.png", "i41.tinypic.com/5vx078.png",
                    "i.istockimg.com/file_thumbview_approve/1096341/2/stock-photo-1096341-bullet-chain.jpg",
                    "wiki.teamfortress.com/w/images/0/06/Minigun_1st_person.png"
                };

            this.creditMusic = Game1.Instance.Content.Load<Song>("Music/DubMood");
            MediaPlayer.IsRepeating = true;
        }
        #endregion

        #region Update&Draw
        /// <summary>
        /// The update of Credits
        /// </summary>
        internal override void Update()
        {
            this.currentTime = FrameInfo.Instance.GameTime.TotalGameTime.TotalSeconds - this.startCreditsTimer;

            this.linebreak = 0;

            if (this.startAnew)
            {
                this.startCreditsTimer = FrameInfo.Instance.GameTime.TotalGameTime.TotalSeconds;
                this.startAnew = false;
            }

            if (InputHandler.Instance.IsAbortPressed())
            {
                // Resets the credits-screen
                Game1.ChangeState(Menu);
                this.startAnew = true;
                MediaPlayer.Stop();
            }

            if (!this.songstart)
            {
                // Starts music player for the credits-screen
                MediaPlayer.Play(this.creditMusic);
                this.songstart = true;
            }

            // Timer for when the logos appears at the bottom of the credits-screen
            if (this.currentTime - this.startCreditsTimer > 33)
            {
                this.drawLogos = true;
            }
        }

        /// <summary>
        /// The draw of Credits
        /// </summary>
        internal override void Draw()
        {
            foreach (string creditEntry in this.creditsContent)
            {
                // Draws the names of the people involved in credits
                Game1.Instance.SpriteBatch.DrawString(this.creditsFont, creditEntry, new Vector2((Game1.GameScreenWidth / 6) - (creditEntry.Length / 2), (680 * Game1.GameScale) + (++this.linebreak * 50) - (60 * (float)this.currentTime)), Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
            }

            foreach (string creditSourcesEntry in this.creditsContentSources)
            {
                // Draws the links to the sources used in project
                Game1.Instance.SpriteBatch.DrawString(this.creditsFontSources, creditSourcesEntry, new Vector2(Game1.GameScreenWidth / 6, (680 * Game1.GameScale) + (++this.linebreak * 50) - (60 * (float)this.currentTime)), Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
            }

            if (!this.drawLogos)
            {
                return;
            }

            // Draws logos at the end of credits-screen
            Game1.Instance.SpriteBatch.Draw(this.nithLogo, new Vector2((Game1.GameScreenWidth - (this.nithLogo.Width * Game1.GameScale)) / 2f, (Game1.GameScreenHeight - (this.nithLogo.Height * Game1.GameScale)) / 2.5f), this.nithLogo.Bounds, Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
            Game1.Instance.SpriteBatch.Draw(this.xnaLogo, new Vector2((Game1.GameScreenWidth - (this.xnaLogo.Width * Game1.GameScale)) / 2f, (Game1.GameScreenHeight - (this.xnaLogo.Height * Game1.GameScale)) / 1.25f), this.xnaLogo.Bounds, Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
        }
        #endregion
    }
}
