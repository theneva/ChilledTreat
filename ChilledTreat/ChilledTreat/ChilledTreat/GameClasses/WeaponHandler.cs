using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameClasses
{
	public class WeaponHandler
	{
		private readonly List<Weapon> _weapons;
		internal double CurrentTime, StartReloadTime;
		private double _startShootTime;
		internal readonly InputHandler Input = InputHandler.Instance;
		internal Vector2 ReticulePosition;
		internal Rectangle HitBox;

		internal Player.State PlayerState = Player.Instance.PlayerState;

		internal readonly FrameInfo FrameInfo = FrameInfo.Instance;

		private Weapon _currentWeapon;

		static WeaponHandler _instance;
		public static WeaponHandler Instance
		{
			get { return _instance ?? (_instance = new WeaponHandler()); }
		}

		private WeaponHandler()
		{
			_weapons = new List<Weapon> { new Weapon("Gun", 100, 10, true) };

			_currentWeapon = _weapons.First();
		}

		public void Update()
		{
			CurrentTime = FrameInfo.GameTime.TotalGameTime.TotalMilliseconds;

			ReticulePosition = new Vector2(Input.MouseState.X, Input.MouseState.Y);
			if (CurrentTime - _startShootTime > _currentWeapon.DelayBetweenShots && PlayerState != Player.State.Reloading) PlayerState = Player.State.Alive;

			if (ReticulePosition.Y < 0) ReticulePosition = new Vector2(ReticulePosition.X, 0);
			else if (ReticulePosition.Y > Game1.Instance.GameScreenHeight) ReticulePosition = new Vector2(ReticulePosition.X, Game1.Instance.GameScreenHeight);

			if (PlayerState == Player.State.Alive && (Input.IsShootPressed() && !_currentWeapon.IsWeaponAutomatic || Input.IsShootDown() && _currentWeapon.IsWeaponAutomatic) && !Player.Instance.InCover)
			{
				_startShootTime = FrameInfo.GameTime.TotalGameTime.TotalMilliseconds;

				HitBox = new Rectangle(Input.MouseState.X - 20, Input.MouseState.Y - 20, 40, 40);

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

		public void ChangeWeapon(String name)
		{
			_currentWeapon = SetCurrentWeapon(name);
		}

		private Weapon SetCurrentWeapon(String name)
		{
			return _weapons.FirstOrDefault(w => w.WeaponName.Equals(name));
		}

		public void ResetWeapons()
		{
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
		private int _timesDrawnMuzzleFlare;
		internal readonly double DelayBetweenShots;
		internal bool PlayReloadSound;
		internal readonly bool IsWeaponAutomatic;
		private bool _drawMuzzleFlare;
		private readonly Texture2D _reticuleTexture, _cartridgeTexture, _usedCartridgeTexture, _weaponTexture;
		private readonly Vector2 _halfReticuleTexture;
		private Vector2 _vectorGunToMouse;
		private readonly Texture2D[] _bullets;
		private readonly Vector2[] _bulletPositions;
		private readonly SoundEffect _shotSound, _reloadSound;
		private float _gunRotation;
		private readonly Rectangle _gunPosition, _gunSource, _firedGunSource;

		/// <summary>
		/// Create a weapon
		/// </summary>
		/// <param name="weaponName">The name of the weapon, used for loading images and sounds, and finding the object later. Be VERY careful with spelling</param>
		/// <param name="maxAmmo">Maximum ammo if this weapon</param>
		/// <param name="rateOfFire">The amount of bullets fireable per second</param>
		/// <param name="automatic">Indicates whether or not the weapon is automatic</param>
		internal Weapon(String weaponName, int maxAmmo, int rateOfFire, bool automatic)
		{
			_spriteBatch = Game1.Instance.SpriteBatch;
			ContentManager content = Game1.Instance.Content;

			_bullets = new Texture2D[maxAmmo];
			MaxAmmo = maxAmmo;
			CurrentAmmo = maxAmmo;
			WeaponName = weaponName;
			DelayBetweenShots = 1000f / rateOfFire;
			IsWeaponAutomatic = automatic;

			_reticuleTexture = content.Load<Texture2D>(weaponName + "./Images/Reticule");
			_cartridgeTexture = content.Load<Texture2D>(weaponName + "./Images/Cartridge");
			_usedCartridgeTexture = content.Load<Texture2D>(weaponName + "./Images/UsedCartridge");
			_weaponTexture = content.Load<Texture2D>(weaponName + "./Images/WeaponTexture");
			_shotSound = content.Load<SoundEffect>(weaponName + "./Sounds/Shot");
			_reloadSound = content.Load<SoundEffect>(weaponName + "./Sounds/Reload");

			_halfReticuleTexture = new Vector2(_reticuleTexture.Width / 2f, _reticuleTexture.Height / 2f);

			_bulletPositions = new Vector2[maxAmmo];
			_gunSource = new Rectangle(0, 0, _weaponTexture.Width / 2, _weaponTexture.Height);
			_firedGunSource = new Rectangle(_weaponTexture.Width / 2, 0, _weaponTexture.Width / 2, _weaponTexture.Height);
			_gunPosition = new Rectangle(Game1.Instance.GameScreenWidth / 2, Game1.Instance.GameScreenHeight + 40, _weaponTexture.Width / 2, _weaponTexture.Height);

			for (int i = 0; i < _bulletPositions.Length; i++)
			{
				_bulletPositions[i] = new Vector2(i * _cartridgeTexture.Width + 5, Game1.Instance.GameScreenHeight - _cartridgeTexture.Height);
			}

			for (int i = 0; i < _bullets.Length; i++) _bullets[i] = _cartridgeTexture;
		}

		internal void Update()
		{
			if (_timesDrawnMuzzleFlare >= 5)
			{
				_drawMuzzleFlare = false;
				_timesDrawnMuzzleFlare = 0;
			}

			_vectorGunToMouse = new Vector2((_gunPosition.X - WeaponHandler.Instance.Input.MouseState.X), (_gunPosition.Y - WeaponHandler.Instance.Input.MouseState.Y));
			
			_gunRotation = (float)Math.Atan2(-_vectorGunToMouse.X, _vectorGunToMouse.Y);
		}

		internal void Draw()
		{
			if (!Player.Instance.InCover)
			{
				_spriteBatch.Draw(_reticuleTexture, WeaponHandler.Instance.ReticulePosition - _halfReticuleTexture, Color.White);

				if (_drawMuzzleFlare)
				{
					_spriteBatch.Draw(_weaponTexture, _gunPosition, _firedGunSource, Color.White, _gunRotation,
								   new Vector2(_weaponTexture.Width / 4f, _weaponTexture.Height), SpriteEffects.None, 1f);

					_timesDrawnMuzzleFlare++;
				}
				else
				{
					_spriteBatch.Draw(_weaponTexture, _gunPosition, _gunSource, Color.White, _gunRotation,
								  new Vector2(_weaponTexture.Width / 4f, _weaponTexture.Height), SpriteEffects.None, 1f);
				}
			}

			for (int i = 0; i < _bullets.Length; i++)
			{
				_spriteBatch.Draw(_bullets[i], _bulletPositions[i], Color.White);
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

			for (int i = 0; i < _bullets.Length; i++)
			{
				_bullets[i] = _cartridgeTexture;
			}

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

			EnemyHandler.Instance.FiredAt(WeaponHandler.Instance.HitBox);

			//Set the array of textures to appear used when firing a shot
			_bullets[--CurrentAmmo] = _usedCartridgeTexture;

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
			CurrentAmmo = _bullets.Length;

			for (int i = 0; i < _bullets.Length; i++)
			{
				_bullets[i] = _cartridgeTexture;
			}
		}
	}
}
