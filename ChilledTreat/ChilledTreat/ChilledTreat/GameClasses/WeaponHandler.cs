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
		private readonly Weapon[] _weapons;
		private Weapon _currentWeapon;
		private int _currentWeaponIndex;
		internal double CurrentTime, StartReloadTime;
		private double _startShootTime;
		internal readonly InputHandler Input = InputHandler.Instance;
		internal Vector2 ReticulePosition;
		internal Rectangle HitBox;

		internal Player.State PlayerState = Player.Instance.PlayerState;

		internal readonly FrameInfo FrameInfo = FrameInfo.Instance;

		private static WeaponHandler _instance;
		public static WeaponHandler Instance
		{
			get { return _instance ?? (_instance = new WeaponHandler()); }
		}

		private WeaponHandler()
		{
			_weapons = new [] { new Weapon("Gun", 10, 10, 5, false), 
			new Weapon("Rifle", 30, 5, 10, true) };

			_currentWeapon = _weapons[_currentWeaponIndex];
		}

		public void Update()
		{
			CurrentTime = FrameInfo.GameTime.TotalGameTime.TotalMilliseconds;

			if(Input.IsSwitchWeaponPressed() && PlayerState != Player.State.Reloading) ChangeWeapon();

			ReticulePosition = new Vector2(Input.MouseState.X, Input.MouseState.Y);
			if (CurrentTime - _startShootTime > _currentWeapon.DelayBetweenShots && PlayerState != Player.State.Reloading) PlayerState = Player.State.Alive;

			if (ReticulePosition.Y < 0) ReticulePosition = new Vector2(ReticulePosition.X, 0);
			else if (ReticulePosition.Y > Game1.GameScreenHeight) ReticulePosition = new Vector2(ReticulePosition.X, Game1.GameScreenHeight);

			if (PlayerState == Player.State.Alive && (Input.IsShootPressed() && !_currentWeapon.IsWeaponAutomatic || Input.IsShootDown() && _currentWeapon.IsWeaponAutomatic) && !Player.Instance.InCover)
			{
				_startShootTime = FrameInfo.GameTime.TotalGameTime.TotalMilliseconds;

				HitBox = _currentWeapon.IsWeaponAutomatic ? new Rectangle(Input.MouseState.X - 40, Input.MouseState.Y - 40, 80, 80) : new Rectangle(Input.MouseState.X - 10, Input.MouseState.Y - 10, 20, 20);

				PlayerState = Player.State.Shooting;
			}

			if (Input.IsReloadPressed() && PlayerState == Player.State.Alive && _currentWeapon.CurrentAmmo < _currentWeapon.MaxAmmo)
			{
				PlayerState = Player.State.Reloading;
				StartReloadTime = FrameInfo.GameTime.TotalGameTime.TotalMilliseconds;
				_currentWeapon.PlayReloadSound = true;
			}

			if (PlayerState == Player.State.Shooting) _currentWeapon.Shoot();

			if (PlayerState == Player.State.Reloading) _currentWeapon.Reload();

			_currentWeapon.Update();
		}

		public void Draw()
		{
			_currentWeapon.Draw();
		}

		private void ChangeWeapon()
		{
			if (_currentWeaponIndex + 1 == _weapons.Length) _currentWeaponIndex = 0;
			else _currentWeaponIndex++;
			_currentWeapon = _weapons[_currentWeaponIndex];
		}

		public void ResetWeapons()
		{
			_currentWeapon = _weapons.First();
			foreach (Weapon w in _weapons)
			{
				w.ResetWeapon();
			}
		}
	}

	internal class Weapon
	{
		private readonly SpriteBatch _spriteBatch;

		internal readonly String WeaponName;
		internal int MaxAmmo, CurrentAmmo;
		private int _timesDrawnMuzzleFlare, _damage;
		private readonly int _defaultDamage;
		internal readonly double DelayBetweenShots;
		internal bool PlayReloadSound;
		internal readonly bool IsWeaponAutomatic;
		private bool _drawMuzzleFlare;
		private readonly Texture2D _reticuleTexture, _cartridgeTexture, _usedCartridgeTexture, _weaponTexture;
		private readonly Vector2 _halfReticuleTexture;
		private Vector2 _vectorGunToMouse;
		private readonly Texture2D[] _cartridges;
		private readonly Vector2[] _cartridgePositions;
		private readonly SoundEffect _shotSound, _reloadSound;
		private float _gunRotation;
		private readonly Rectangle _weaponPosition, _weaponDrawSource, _firedWeaponDrawSource;

		/// <summary>
		/// Create a weapon
		/// </summary>
		/// <param name="weaponName">The name of the weapon, used for loading images and sounds, and finding the object later. Be VERY careful with the spelling</param>
		/// <param name="maxAmmo">Maximum ammo of this weapon</param>
		/// <param name="damage">The default damage </param>
		/// <param name="rateOfFire">The amount of bullets fireable per second</param>
		/// <param name="automatic">Indicates whether or not the weapon is automatic</param>
		internal Weapon(String weaponName, int maxAmmo, int damage, int rateOfFire, bool automatic)
		{
			//To avoid an excess of parameters, get these directly
			_spriteBatch = Game1.Instance.SpriteBatch;
			ContentManager content = Game1.Instance.Content;

			//Set the variables of the instance in accordance with the parameters
			WeaponName = weaponName;
			MaxAmmo = maxAmmo;
			_defaultDamage = damage;
			DelayBetweenShots = 1000f / rateOfFire;
			IsWeaponAutomatic = automatic;

			CurrentAmmo = maxAmmo;

			_reticuleTexture = content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/Reticule");
			_cartridgeTexture = content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/Cartridge");
			_usedCartridgeTexture = content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/UsedCartridge");
			_weaponTexture = content.Load<Texture2D>("./Weapons/" + weaponName + "/Images/WeaponTexture");
			_shotSound = content.Load<SoundEffect>("./Weapons/" + weaponName + "/Sounds/Shot");
			_reloadSound = content.Load<SoundEffect>("./Weapons/" + weaponName + "/Sounds/Reload");

			_halfReticuleTexture = new Vector2(_reticuleTexture.Width / 2f, _reticuleTexture.Height / 2f);

			_cartridges = new Texture2D[maxAmmo];
			_cartridgePositions = new Vector2[maxAmmo];
			_weaponDrawSource = new Rectangle(0, 0, _weaponTexture.Width / 2, _weaponTexture.Height);
			_firedWeaponDrawSource = new Rectangle(_weaponTexture.Width / 2, 0, _weaponTexture.Width / 2, _weaponTexture.Height);
			_weaponPosition = new Rectangle(Game1.GameScreenWidth / 2, Game1.GameScreenHeight + 40, _weaponTexture.Width / 2, _weaponTexture.Height);

			//Fill the positions-array for the cartridges, so I can iterate over it, placing them where they should be on the screen
			for (int i = 0; i < _cartridgePositions.Length; i++) _cartridgePositions[i] = new Vector2(i * _cartridgeTexture.Width + 5, Game1.GameScreenHeight - _cartridgeTexture.Height);

			//Fill the texture-array for the cartridges, so the correct textures are drawn in the loop in Draw
			for (int i = 0; i < _cartridges.Length; i++) _cartridges[i] = _cartridgeTexture;
		}

		internal void Update()
		{
			if (_timesDrawnMuzzleFlare >= 5)
			{
				_drawMuzzleFlare = false;
				_timesDrawnMuzzleFlare = 0;
			}

			_vectorGunToMouse = new Vector2((_weaponPosition.X - WeaponHandler.Instance.Input.MouseState.X), (_weaponPosition.Y - WeaponHandler.Instance.Input.MouseState.Y));
			
			_gunRotation = (float)Math.Atan2(-_vectorGunToMouse.X, _vectorGunToMouse.Y);
		}

		internal void Draw()
		{
			if (!Player.Instance.InCover)
			{
				_spriteBatch.Draw(_reticuleTexture, WeaponHandler.Instance.ReticulePosition - _halfReticuleTexture, Color.White);

				if (_drawMuzzleFlare)
				{
					_spriteBatch.Draw(_weaponTexture, _weaponPosition, _firedWeaponDrawSource, Color.White, _gunRotation,
								   new Vector2(_weaponTexture.Width / 4f, _weaponTexture.Height), SpriteEffects.None, 1f);

					_timesDrawnMuzzleFlare++;
				}
				else
				{
					_spriteBatch.Draw(_weaponTexture, _weaponPosition, _weaponDrawSource, Color.White, _gunRotation,
								  new Vector2(_weaponTexture.Width / 4f, _weaponTexture.Height), SpriteEffects.None, 1f);
				}
			}

			for (int i = 0; i < _cartridges.Length; i++)
			{
				_spriteBatch.Draw(_cartridges[i], _cartridgePositions[i], Color.White);
			}
		}

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

			if (WeaponHandler.Instance.CurrentTime - WeaponHandler.Instance.StartReloadTime <= _reloadSound.Duration.TotalMilliseconds) return;
			CurrentAmmo = MaxAmmo;

			for (int i = 0; i < _cartridges.Length; i++) _cartridges[i] = _cartridgeTexture;

			WeaponHandler.Instance.PlayerState = Player.State.Alive;
		}

		/// <summary>
		/// Shoot the current weapon
		/// </summary>
		internal void Shoot()
		{
			//Just to make sure the player's not firing when (s)he's not supposed to
			if (WeaponHandler.Instance.PlayerState != Player.State.Shooting) return;
			_shotSound.Play();
			_drawMuzzleFlare = true;

			_damage = EnemyHandler.Random.Next((_defaultDamage / 2), _defaultDamage);

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
			else WeaponHandler.Instance.PlayerState = Player.State.Waiting;

			if (WeaponHandler.Instance.PlayerState != Player.State.Reloading && WeaponHandler.Instance.PlayerState != Player.State.Waiting && WeaponHandler.Instance.PlayerState != Player.State.Dead) WeaponHandler.Instance.PlayerState = Player.State.Alive;
		}

		internal void ResetWeapon()
		{
			CurrentAmmo = _cartridges.Length;

			for (int i = 0; i < _cartridges.Length; i++)
			{
				_cartridges[i] = _cartridgeTexture;
			}
		}
	}
}
