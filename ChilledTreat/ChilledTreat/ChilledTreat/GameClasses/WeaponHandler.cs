using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameClasses
{
	/// <summary>
	/// A class for handling the weapons in the game
	/// </summary>
	public class WeaponHandler
	{
		#region Fields
		private readonly Weapon[] _weapons;
		private Weapon _currentWeapon;
		private int _currentWeaponIndex;
		internal double CurrentTime, StartReloadTime, StartShootTime;
		internal Vector2 ReticulePosition, PointerPosition;
		internal Rectangle HitBox;

		public bool Splash
		{
			get { return _currentWeapon.Splash; }
		}

		internal Player.State PlayerState = Player.Instance.PlayerState;
		internal readonly FrameInfo FrameInfo = FrameInfo.Instance;
		private readonly InputHandler _input = InputHandler.Instance;

		private static WeaponHandler _instance;

		public static WeaponHandler Instance
		{
			get { return _instance ?? (_instance = new WeaponHandler()); }
		}
		#endregion

		#region Constructor
		private WeaponHandler()
		{
			//This demonstrates the power of OOP. To add a weapon, with it's on attributes, simply add it to the
			//list (making sure the name matches a folder in the Weapon folder, to load textures and sounds)
			_weapons = new []
			           	{
							new Weapon("Minigun", 10000, 1000, 50, true, true),
							new Weapon("Pistol", 10, 100, 5, false, false),
							new Weapon("Rifle", 30, 50, 10, true, false)
						};
#if XBOX
			ReticulePosition = new Vector2(Game1.GameScreenWidth / 2, Game1.GameScreenHeight / 2);
#endif
			//If godmode is active, use the minigun. If not, use the normal weapon
			if (!Tools.GameConstants.GodMode)
				_currentWeaponIndex = 1;

			_currentWeapon = _weapons[_currentWeaponIndex];
		}
		#endregion

		#region Update & Draw
		public void Update()
		{
			CurrentTime = FrameInfo.GameTime.TotalGameTime.TotalMilliseconds;


			//PointerPosition = _input.PointerLocation();
			ReticulePosition = _input.PointerLocation();

			//Do not allow weapon-swapping while reloading
			if (_input.IsSwitchWeaponPressed() && PlayerState != Player.State.Reloading)
				ChangeWeapon();
			
			//
			if (CurrentTime - StartShootTime > _currentWeapon.DelayBetweenShots && PlayerState == Player.State.Waiting && PlayerState != Player.State.Reloading)
				PlayerState = Player.State.Alive;

			if (ReticulePosition.X < 0)
				ReticulePosition = new Vector2(0, ReticulePosition.Y);
			else if (ReticulePosition.X > Game1.GameScreenWidth)
				ReticulePosition = new Vector2(Game1.GameScreenWidth, ReticulePosition.Y);

			if (ReticulePosition.Y < 0)
				ReticulePosition = new Vector2(ReticulePosition.X, 0);
			else if (ReticulePosition.Y > Game1.GameScreenHeight)
				ReticulePosition = new Vector2(ReticulePosition.X, Game1.GameScreenHeight);

			if (PlayerState == Player.State.Alive &&
				(_input.IsShootPressed() && !_currentWeapon.IsWeaponAutomatic ||
				 _input.IsShootDown() && _currentWeapon.IsWeaponAutomatic) && !Player.Instance.InCover)
			{
				StartShootTime = FrameInfo.GameTime.TotalGameTime.TotalMilliseconds;

				//Set up a hitbox. Unless splash damage is activated, this isn't used directly
				//The bigger your rate of fire is, the bigger your hitbox will be.
				//If splash is activated, this makes it less probable to hit where you aim
				HitBox = new Rectangle((int)ReticulePosition.X - _currentWeapon.RateOfFire * 5,
					(int)ReticulePosition.Y - _currentWeapon.RateOfFire * 5, _currentWeapon.RateOfFire * 10, _currentWeapon.RateOfFire * 10);

				//If splash-damage is used, this sets up a usable hit-box. Inside of the overall hit-box, create a randomly
				//placed hit-box 10x10 pixels within the former hitbox to use for the actual collision test
				if (!_currentWeapon.Splash)
					HitBox = new Rectangle(EnemyHandler.Random.Next(HitBox.X, HitBox.X + HitBox.Width),
						EnemyHandler.Random.Next(HitBox.Y, HitBox.Y + HitBox.Height), 10, 10);

				PlayerState = Player.State.Shooting;
			}

			if (_input.IsReloadPressed() && PlayerState == Player.State.Alive &&
				_currentWeapon.CurrentAmmo < _currentWeapon.MaxAmmo)
			{
				PlayerState = Player.State.Reloading;
				StartReloadTime = FrameInfo.GameTime.TotalGameTime.TotalMilliseconds;
				_currentWeapon.PlayReloadSound = true;
			}

			if (PlayerState == Player.State.Shooting)
				_currentWeapon.Shoot();

			if (PlayerState == Player.State.Reloading)
				_currentWeapon.Reload();

			_currentWeapon.Update();
		}

		public void Draw()
		{
			_currentWeapon.Draw();
		}
		#endregion

		#region Methods for changing the current weapon and restarting the instance
		private void ChangeWeapon()
		{
			//If going to the next weapon takes you out of the array, start over
			if (_currentWeaponIndex + 1 == _weapons.Count())
				//If God-mode is activated, allow the use of the god weapon. If not, start at index 1
				_currentWeaponIndex = Tools.GameConstants.GodMode ? 0 : 1;
			else _currentWeaponIndex++;
			_currentWeapon = _weapons[_currentWeaponIndex];
		}

		public static void ResetWeapons()
		{
			_instance = new WeaponHandler();
		}
		#endregion
	}

	/// <summary>
	/// An internal class, for giving each weapon seperate values, textures and sounds
	/// </summary>
	internal class Weapon
	{
		#region Fields

		private readonly SpriteBatch _spriteBatch;
		internal int CurrentAmmo;
		internal readonly int MaxAmmo, RateOfFire;
		private int _timesDrawnMuzzleFlare, _damage;
		private readonly int _defaultDamage;
		internal readonly double DelayBetweenShots;
		internal bool PlayReloadSound;
		internal readonly bool IsWeaponAutomatic, Splash;
		private bool _drawMuzzleFlare, _vibrating;
		private readonly Texture2D _reticuleTexture, _cartridgeTexture, _usedCartridgeTexture, _weaponTexture;
		private readonly Vector2 _halfReticuleTexture;
		private Vector2 _vectorWeaponToReticule, _weaponPosition;
		private readonly Texture2D[] _cartridges;
		private readonly Vector2[] _cartridgePositions;
		private readonly SoundEffect _shotSound, _reloadSound;
		private float _gunRotation;
		private readonly Rectangle _weaponDrawSource, _firedWeaponDrawSource;

		#endregion

		#region Constructor
		/// <summary>
		/// Create a weapon
		/// </summary>
		/// <param name="weaponName">The name of the weapon, used for loading images and sounds, and finding the object later. Be VERY careful with the spelling</param>
		/// <param name="maxAmmo">Maximum ammo of this weapon</param>
		/// <param name="damage">The default damage </param>
		/// <param name="rateOfFire">The amount of bullets fireable per second</param>
		/// <param name="automatic">Indicates whether or not the weapon is automatic</param>
		/// <param name="splash">Indicates whether ot not the weapon delas splashdamage </param>
		internal Weapon(String weaponName, int maxAmmo, int damage, int rateOfFire, bool automatic, bool splash)
		{
			//To avoid an excess of parameters, get these directly
			_spriteBatch = Game1.Instance.SpriteBatch;
			ContentManager content = Game1.Instance.Content;

			//Set the variables of the instance in accordance with the parameters
			MaxAmmo = maxAmmo;
			_defaultDamage = damage;
			DelayBetweenShots = 1000f/rateOfFire;
			RateOfFire = rateOfFire;
			IsWeaponAutomatic = automatic;
			Splash = splash;

			CurrentAmmo = maxAmmo;

			_reticuleTexture = content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/Reticule");
			_cartridgeTexture = content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/Cartridge");
			_usedCartridgeTexture = content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/UsedCartridge");
			_weaponTexture = content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/WeaponTexture");
			_shotSound = content.Load<SoundEffect>("./Weapons/" + weaponName + "/Sounds/Shot");
			_reloadSound = content.Load<SoundEffect>("./Weapons/" + weaponName + "/Sounds/Reload");

			_halfReticuleTexture = new Vector2(_reticuleTexture.Width/2f, _reticuleTexture.Height/2f);

			_cartridges = new Texture2D[maxAmmo];
			_cartridgePositions = new Vector2[maxAmmo];
			_weaponDrawSource = new Rectangle(0, 0, _weaponTexture.Width/2, _weaponTexture.Height);
			_firedWeaponDrawSource = new Rectangle(_weaponTexture.Width/2, 0, _weaponTexture.Width/2, _weaponTexture.Height);

			//Set the weapon 30 pixels underneath the game-window, so that, when you rotate the weapon, you can't see the bottom of it
			_weaponPosition = new Vector2(Game1.GameScreenWidth / 2, Game1.GameScreenHeight + 30);

			//Fill the positions-array for the cartridges, so I can iterate over it, placing them where they should be on the screen
			for (int i = 0; i < _cartridgePositions.Length; i++)
				_cartridgePositions[i] = new Vector2(i*_cartridgeTexture.Width * Game1.GameScale, Game1.GameScreenHeight - _cartridgeTexture.Height * Game1.GameScale);

			//Fill the texture-array for the cartridges, so the correct textures are drawn in the loop in Draw
			for (int i = 0; i < _cartridges.Length; i++) 
				_cartridges[i] = _cartridgeTexture;
		}
		#endregion

		#region Update & Draw
		internal void Update()
		{
			if (_timesDrawnMuzzleFlare >= 5)
			{
				_drawMuzzleFlare = false;
				_timesDrawnMuzzleFlare = 0;
			}

			if (WeaponHandler.Instance.CurrentTime - WeaponHandler.Instance.StartShootTime > 100 && _vibrating)
			{
				InputHandler.Instance.StopVibrate();
				_vibrating = false;
			}

			_vectorWeaponToReticule = new Vector2((_weaponPosition.X - WeaponHandler.Instance.ReticulePosition.X),
											(_weaponPosition.Y - WeaponHandler.Instance.ReticulePosition.Y));

			//Minus X, to rotate the right way
			_gunRotation = (float) Math.Atan2(-_vectorWeaponToReticule.X, _vectorWeaponToReticule.Y);
		}

		internal void Draw()
		{
			if (!Player.Instance.InCover)
			{
				_spriteBatch.Draw(_reticuleTexture, WeaponHandler.Instance.ReticulePosition - _halfReticuleTexture, _reticuleTexture.Bounds, Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);

				if (_drawMuzzleFlare)
				{
					_spriteBatch.Draw(_weaponTexture, _weaponPosition, _firedWeaponDrawSource, Color.White, _gunRotation,
									  new Vector2(_weaponTexture.Width/4f, _weaponTexture.Height), Game1.GameScale, SpriteEffects.None, 0);

					_timesDrawnMuzzleFlare++;
				}
				else
					_spriteBatch.Draw(_weaponTexture, _weaponPosition, _weaponDrawSource, Color.White, _gunRotation,
									  new Vector2(_weaponTexture.Width / 4f, _weaponTexture.Height), Game1.GameScale, SpriteEffects.None, 0);
			}

			for (int i = 0; i < _cartridges.Length; i++)
				_spriteBatch.Draw(_cartridges[i], _cartridgePositions[i], _cartridgeTexture.Bounds, Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
		}
		#endregion

		#region Reload and Shoot method
		/// <summary>
		/// Reload the current weapon
		/// It adds a timer before reverting the player state to Alive, to allowing the firing of the weapon again
		/// </summary>
		internal void Reload()
		{
			if (PlayReloadSound)
			{
				_reloadSound.Play();

				PlayReloadSound = false;
			}

			if (WeaponHandler.Instance.CurrentTime - WeaponHandler.Instance.StartReloadTime < _reloadSound.Duration.TotalMilliseconds) 
				return;
			CurrentAmmo = MaxAmmo;

			for (int i = 0; i < _cartridges.Length; i++) 
				_cartridges[i] = _cartridgeTexture;

			WeaponHandler.Instance.PlayerState = Player.State.Alive;
		}

		/// <summary>
		/// Shoot the current weapon
		/// </summary>
		internal void Shoot()
		{
			_shotSound.Play();
			_drawMuzzleFlare = true;

			_vibrating = true;
			if(IsWeaponAutomatic)
				InputHandler.Instance.StartSoftVibrate();
			else
				InputHandler.Instance.StartHardVibrate();

			//The damage dealt is a random number between 2/3 of the default damage and the default damage
			_damage = EnemyHandler.Random.Next((_defaultDamage * 2 / 3), _defaultDamage);

			//Shoot at the enemy
			EnemyHandler.Instance.FiredAt(WeaponHandler.Instance.HitBox, _damage);

			//Set the array of textures to appear used when firing a shot
			_cartridges[--CurrentAmmo] = _usedCartridgeTexture;

			//If the weapon's out of ammo, and the player's not currently reloading
			if (CurrentAmmo == 0 && WeaponHandler.Instance.PlayerState != Player.State.Reloading)
			{
				WeaponHandler.Instance.PlayerState = Player.State.Reloading;

				WeaponHandler.Instance.StartReloadTime = WeaponHandler.Instance.FrameInfo.GameTime.TotalGameTime.TotalMilliseconds;
				PlayReloadSound = true;
			}
			else 
				WeaponHandler.Instance.PlayerState = Player.State.Waiting;

			if (WeaponHandler.Instance.PlayerState != Player.State.Reloading &&
			    WeaponHandler.Instance.PlayerState != Player.State.Waiting &&
			    WeaponHandler.Instance.PlayerState != Player.State.Dead)
					WeaponHandler.Instance.PlayerState = Player.State.Alive;
		}
		#endregion
	}
}