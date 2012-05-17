// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Weapon.cs" company="X&A Team">
//   Copyright (c) X&A Team. All rights reserved
// </copyright>
// <summary>
//   A weapons-class
// </summary>
// <author>Simen Bekkhus</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat.GameClasses
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A weapons-class
    /// </summary>
    public class Weapon
    {
        #region Fields
        /// <summary>
        /// The default damage a weapon does
        /// </summary>
        private readonly int defaultDamage;

        /// <summary>
        /// The texture for the reticule
        /// </summary>
        private readonly Texture2D reticuleTexture;

        /// <summary>
        /// The texture for the cartridge
        /// </summary>
        private readonly Texture2D cartridgeTexture;

        /// <summary>
        /// The texture for the used cartridge
        /// </summary>
        private readonly Texture2D usedCartridgeTexture;

        /// <summary>
        /// The texture for the weapon
        /// </summary>
        private readonly Texture2D weaponTexture;

        /// <summary>
        /// A vector used for halving where the reticule is drawn
        /// </summary>
        private readonly Vector2 halfReticuleTexture;

        /// <summary>
        /// An array of the cartridge-textures, used when drawing the ammo-indicator
        /// </summary>
        private readonly Texture2D[] cartridges;

        /// <summary>
        /// An array of the used cartridge-textures, used when drawing the ammo-indicator
        /// </summary>
        private readonly Vector2[] cartridgePositions;

        /// <summary>
        /// The sound played when a shot is fired
        /// </summary>
        private readonly SoundEffect shotSound;

        /// <summary>
        /// The sound played when the weapon's reloading
        /// </summary>
        private readonly SoundEffect reloadSound;

        /// <summary>
        /// The source-rectangle used when drawing the weapon
        /// </summary>
        private readonly Rectangle weaponDrawSource;

        /// <summary>
        /// The source-rectangle used when drawing the weapon when firing
        /// </summary>
        private readonly Rectangle firedWeaponDrawSource;

        /// <summary>
        /// The current game-time
        /// </summary>
        private int currentTime;

        /// <summary>
        /// The damage a weapon does
        /// </summary>
        private int damage;

        /// <summary>
        /// Indicates whether or not to draw a muzzle-flare
        /// </summary>
        private bool drawMuzzleFlare;

#if XBOX
        /// <summary>
        /// A bool indicating whether or not the weapon is vibrating
        /// </summary>
        private bool vibrating;
#endif

        /// <summary>
        /// The position of the weapon
        /// </summary>
        private Vector2 weaponPosition;

        /// <summary>
        /// The vector between the gun and the reticule
        /// </summary>
        private Vector2 vectorWeaponToReticule;

        /// <summary>
        /// The rotation applied to the gun, to make it follow the reticule
        /// </summary>
        private float gunRotation;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Weapon"/> class. 
        /// Create a weapon
        /// </summary>
        /// <param name="weaponName">
        /// The name of the weapon, used for loading images and sounds, and finding the object later. Be VERY careful with the spelling
        /// </param>
        /// <param name="maxAmmo">
        /// Maximum ammo of this weapon
        /// </param>
        /// <param name="damage">
        /// The default damage 
        /// </param>
        /// <param name="rateOfFire">
        /// The amount of bullets fireable per second
        /// </param>
        /// <param name="automatic">
        /// Indicates whether or not the weapon is automatic
        /// </param>
        /// <param name="splash">
        /// Indicates whether or not the weapon deals splash-damage 
        /// </param>
        public Weapon(string weaponName, int maxAmmo, int damage, int rateOfFire, bool automatic, bool splash)
        {
            // To avoid an excess of parameters, get these directly
            // Set the variables of the instance in accordance with the parameters
            this.MaxAmmo = maxAmmo;
            this.defaultDamage = damage;
            this.DelayBetweenShots = 1000f / rateOfFire;
            this.RateOfFire = rateOfFire;
            this.IsWeaponAutomatic = automatic;
            this.Splash = splash;

            this.CurrentAmmo = maxAmmo;

            this.reticuleTexture = Game1.Instance.Content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/Reticule");
            this.cartridgeTexture = Game1.Instance.Content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/Cartridge");
            this.usedCartridgeTexture = Game1.Instance.Content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/UsedCartridge");
            this.weaponTexture = Game1.Instance.Content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/WeaponTexture");
            this.shotSound = Game1.Instance.Content.Load<SoundEffect>("./Weapons/" + weaponName + "/Sounds/Shot");
            this.reloadSound = Game1.Instance.Content.Load<SoundEffect>("./Weapons/" + weaponName + "/Sounds/Reload");

            this.halfReticuleTexture = new Vector2(this.reticuleTexture.Width / 2f, this.reticuleTexture.Height / 2f);

            this.cartridges = new Texture2D[maxAmmo];
            this.cartridgePositions = new Vector2[maxAmmo];
            this.weaponDrawSource = new Rectangle(0, 0, this.weaponTexture.Width / 2, this.weaponTexture.Height);
            this.firedWeaponDrawSource = new Rectangle(this.weaponTexture.Width / 2, 0, this.weaponTexture.Width / 2, this.weaponTexture.Height);

            // Set the weapon 30 pixels underneath the game-window, so that, when you rotate the weapon, you can't see the bottom of it
            this.weaponPosition = new Vector2(Game1.GameScreenWidth / 2, Game1.GameScreenHeight + 30);

            // Fill the positions-array for the cartridges, so I can iterate over it, placing them where they should be on the screen
            for (int i = 0; i < this.cartridgePositions.Length; i++)
            {
                this.cartridgePositions[i] = new Vector2(
                    i * this.cartridgeTexture.Width * Game1.GameScale,
                    Game1.GameScreenHeight - (this.cartridgeTexture.Height * Game1.GameScale));
            }

            // Fill the texture-array for the cartridges, so the correct textures are drawn in the loop in Draw
            for (int i = 0; i < this.cartridges.Length; i++)
            {
                this.cartridges[i] = this.cartridgeTexture;
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets MaxAmmo.
        /// </summary>
        public int MaxAmmo { get; private set; }

        /// <summary>
        /// Gets the current ammo of the weapon
        /// </summary>
        public int CurrentAmmo { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to play the reload-sound
        /// </summary>
        public bool PlayReloadSound { private get; set; }

        /// <summary>
        /// Gets a value indicating whether or not the current weapon has splash-damage
        /// </summary>
        public bool Splash { get; private set; }

        /// <summary>
        /// Gets DelayBetweenShots.
        /// </summary>
        public double DelayBetweenShots { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the weapon is automatic
        /// </summary>
        public bool IsWeaponAutomatic { get; private set; }

        /// <summary>
        /// Gets RateOfFire.
        /// </summary>
        public int RateOfFire { get; private set; }
        #endregion

        #region Update & Draw
        /// <summary>
        /// The update-method of Weapon
        /// </summary>
        public void Update()
        {
            this.currentTime = (int)FrameInfo.Instance.GameTime.TotalGameTime.TotalMilliseconds;

            // To add the "shake" when firing, the time drawing muzzle-flare must be shorter on automatic
            if ((this.currentTime - WeaponHandler.Instance.StartShootTime > 100) && this.drawMuzzleFlare && !this.IsWeaponAutomatic)
            {
                this.drawMuzzleFlare = false;
#if XBOX
                InputHandler.Instance.StopVibrate();
                this.vibrating = false;
#endif
            }
            else if ((this.currentTime - WeaponHandler.Instance.StartShootTime > 50) && this.drawMuzzleFlare && this.IsWeaponAutomatic)
            {
                this.drawMuzzleFlare = false;
#if XBOX
                InputHandler.Instance.StopVibrate();
                this.vibrating = false;
#endif
            }

            // Set up a vector between the weapon and the reticule
            this.vectorWeaponToReticule = new Vector2(
                this.weaponPosition.X - WeaponHandler.Instance.ReticulePosition.X,
                this.weaponPosition.Y - WeaponHandler.Instance.ReticulePosition.Y);

            // Calculate the radian, to apply rotation to the weapon. Minus X, to rotate the right way
            this.gunRotation = (float)Math.Atan2(-this.vectorWeaponToReticule.X, this.vectorWeaponToReticule.Y);
        }

        /// <summary>
        /// The draw-method of Weapon
        /// </summary>
        public void Draw()
        {
            if (!Player.Instance.InCover)
            {
               Game1.Instance.SpriteBatch.Draw(
                   this.reticuleTexture,
                   WeaponHandler.Instance.ReticulePosition - this.halfReticuleTexture,
                   this.reticuleTexture.Bounds,
                   Color.White,
                   0,
                   Vector2.Zero,
                   Game1.GameScale,
                   SpriteEffects.None,
                   0);

                // If we're drawing the muzzle-flare, use that rectangle. If not, use the normal one
                Game1.Instance.SpriteBatch.Draw(
                    this.weaponTexture,
                    this.weaponPosition,
                    this.drawMuzzleFlare ? this.firedWeaponDrawSource : this.weaponDrawSource,
                    Color.White,
                    this.gunRotation,
                    new Vector2(this.weaponTexture.Width / 4f, this.weaponTexture.Height),
                    Game1.GameScale,
                    SpriteEffects.None,
                    0);
            }

            for (int i = 0; i < this.cartridges.Length; i++)
            {
                Game1.Instance.SpriteBatch.Draw(
                    this.cartridges[i],
                    this.cartridgePositions[i],
                    this.cartridgeTexture.Bounds,
                    Color.White,
                    0,
                    Vector2.Zero,
                    Game1.GameScale,
                    SpriteEffects.None,
                    0);
            }
        }

        #endregion

        #region Reload and Shoot method
        /// <summary>
        /// Reload the current weapon
        /// It adds a timer before reverting the player state to Alive, to allowing the firing of the weapon again
        /// </summary>
        public void Reload()
        {
            this.currentTime = (int)FrameInfo.Instance.GameTime.TotalGameTime.TotalMilliseconds;

            if (this.PlayReloadSound)
            {
                this.reloadSound.Play();

                this.PlayReloadSound = false;
            }

            if (this.currentTime - WeaponHandler.Instance.StartReloadTime
                < this.reloadSound.Duration.TotalMilliseconds)
            {
                return;
            }

            this.CurrentAmmo = this.MaxAmmo;

            for (int i = 0; i < this.cartridges.Length; i++)
            {
                this.cartridges[i] = this.cartridgeTexture;
            }

            Player.Instance.PlayerState = Player.State.Alive;
        }

        /// <summary>
        /// Shoot the current weapon
        /// </summary>
        public void Shoot()
        {
            this.shotSound.Play();
            this.drawMuzzleFlare = true;

#if XBOX
            this.vibrating = true;
#endif

            // It gets a bit too much with full vibrate on automatic weapon
            if (this.IsWeaponAutomatic)
            {
                InputHandler.Instance.StartSoftVibrate();
            }
            else
            {
                InputHandler.Instance.StartHardVibrate();
            }

            // The damage dealt is a random number between 2/3 of the default damage and the default damage
            this.damage = GameStates.InGame.Random.Next(this.defaultDamage * 2 / 3, this.defaultDamage);

            // Shoot at the enemy
            EnemyHandler.Instance.FiredAt(WeaponHandler.Instance.HitBox, this.damage);

            // Set the array of textures to appear used when firing a shot, and subtract 1 from current ammo
            this.cartridges[--this.CurrentAmmo] = this.usedCartridgeTexture;

            // If the weapon's out of ammo, and the player's not currently reloading
            if (this.CurrentAmmo == 0 && Player.Instance.PlayerState != Player.State.Reloading)
            {
                Player.Instance.PlayerState = Player.State.Reloading;

                WeaponHandler.Instance.StartReloadTime = FrameInfo.Instance.GameTime.TotalGameTime.TotalMilliseconds;
                this.PlayReloadSound = true;
            }
            else
            {
                Player.Instance.PlayerState = Player.State.Waiting;
            }

            if (Player.Instance.PlayerState != Player.State.Reloading &&
                Player.Instance.PlayerState != Player.State.Waiting &&
                Player.Instance.PlayerState != Player.State.Dead)
            {
                Player.Instance.PlayerState = Player.State.Alive;
            }
        }
        #endregion
    }
}