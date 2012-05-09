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
		readonly List<Weapon> _weapons;
		internal double _currentTime;
		private double _startShootTime;
		internal double _startReloadTime;
		internal readonly InputHandler _input = InputHandler.Instance;
		public Vector2 _reticulePosition;
		internal Rectangle _hitBox;

		public Player.State _playerState = Player.Instance.PlayerState;

		internal readonly FrameInfo _frameInfo = FrameInfo.Instance;

		private Weapon _currentWeapon;

		static WeaponHandler _instance;
		public static WeaponHandler Instance
		{
			get { return _instance ?? (_instance = new WeaponHandler()); }
		}

		private WeaponHandler()
		{
			_weapons = new List<Weapon> {new Weapon("Gun", 10)};
			ChangeWeapon("Gun");
		}

		public void Update()
		{
			_currentTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;

			_reticulePosition = new Vector2(_input.MouseState.X, _input.MouseState.Y);
			if (_currentTime - _startShootTime > 200 && _playerState != Player.State.Reloading) _playerState = Player.State.Alive;

			if (_reticulePosition.Y < 0) _reticulePosition = new Vector2(_reticulePosition.X, 0);
			else if (_reticulePosition.Y > Game1.Instance.GameScreenHeight) _reticulePosition = new Vector2(_reticulePosition.X, Game1.Instance.GameScreenHeight);

			if (_playerState == Player.State.Alive && _input.IsShootPressed() && !Player.Instance.InCover)
			{
				_startShootTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;

				_hitBox = new Rectangle(_input.MouseState.X - 20, _input.MouseState.Y - 20, 40, 40);

				_playerState = Player.State.Shooting;
			}

			if (_input.IsReloadPressed() && _playerState == Player.State.Alive && _currentWeapon._ammo != _currentWeapon._bullets.Length)
			{
				_playerState = Player.State.Reloading;
				_startReloadTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;
				_currentWeapon._playReloadSound = true;
			}

			if (_playerState == Player.State.Shooting) _currentWeapon.Shoot();

			if (_playerState == Player.State.Reloading) _currentWeapon.Reload();

			_currentWeapon.Update();
		}

		public void Draw()
		{
			_currentWeapon.Draw();
		}

		public void Reload()
		{
			_currentWeapon.Reload();
		}

		public void Shoot()
		{
			_currentWeapon.Shoot();
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
				_currentWeapon.ResetWeapon();
			}
		}
	}

	internal class Weapon
	{
		private readonly SpriteBatch _spriteBatch;

		internal readonly String WeaponName;
		public int _ammo, _timesDrawnMuzzleFlare;
		public bool _playReloadSound, _drawMuzzleFlare;
		private readonly Texture2D _reticuleTexture, _bulletTexture, _usedBulletTexture, _gunTexture;
		private readonly Vector2 _halfReticuleTexture;
		private Vector2 _vectorGunToMouse;
		public readonly Texture2D[] _bullets;
		private readonly Vector2[] _bulletPositions;
		private readonly SoundEffect _gunShotSound, _gunReloadSound;
		private float _gunRotation;
		private readonly Rectangle _gunPosition, _gunSource, _firedGunSource;

		/// <summary>
		/// Create a weapon
		/// </summary>
		/// <param name="weaponName">The name of the weapon, used for loading images and sounds, and finding the object later</param>
		/// <param name="maxAmmo">Maximum ammo if this weapon</param>
		internal Weapon(String weaponName, int maxAmmo)
		{
			_spriteBatch = Game1.Instance.SpriteBatch;
			ContentManager content = Game1.Instance.Content;

			_bullets = new Texture2D[maxAmmo];
			_ammo = maxAmmo;

			_reticuleTexture = content.Load<Texture2D>(weaponName + "./Images/usableReticule");
			_bulletTexture = content.Load<Texture2D>(weaponName + "./Images/usableBullet");
			_usedBulletTexture = content.Load<Texture2D>(weaponName + "./Images/usableUsedBullet");
			_gunTexture = content.Load<Texture2D>(weaponName + "./Images/gunTest");
			_reticuleTexture = content.Load<Texture2D>(weaponName + "./Images/usableReticule");
			_gunShotSound = content.Load<SoundEffect>(weaponName + "./Sounds/GunFire");
			_gunReloadSound = content.Load<SoundEffect>(weaponName + "./Sounds/ReloadSound");


			_halfReticuleTexture = new Vector2(_reticuleTexture.Width / 2f, _reticuleTexture.Height / 2f);

			_bulletPositions = new Vector2[_bullets.Length];
			_gunSource = new Rectangle(0, 0, _gunTexture.Width / 2, _gunTexture.Height);
			_firedGunSource = new Rectangle(_gunTexture.Width / 2, 0, _gunTexture.Width / 2, _gunTexture.Height);
			_gunPosition = new Rectangle(Game1.Instance.GameScreenWidth / 2, Game1.Instance.GameScreenHeight + 40, _gunTexture.Width / 2, _gunTexture.Height);

			for (int i = 0; i < _bulletPositions.Length; i++)
			{
				_bulletPositions[i] = new Vector2(i * _bulletTexture.Width + 5, Game1.Instance.GameScreenHeight - _bulletTexture.Height);
			}

			for (int i = 0; i < _bullets.Length; i++) _bullets[i] = _bulletTexture;

			WeaponName = weaponName;
		}

		internal void Update()
		{
			if (_timesDrawnMuzzleFlare >= 5)
			{
				_drawMuzzleFlare = false;
				_timesDrawnMuzzleFlare = 0;
			}

			_vectorGunToMouse = new Vector2((_gunPosition.X - WeaponHandler.Instance._input.MouseState.X), (_gunPosition.Y - WeaponHandler.Instance._input.MouseState.Y));
			
			_gunRotation = (float)Math.Atan2(-_vectorGunToMouse.X, _vectorGunToMouse.Y);
		}

		internal void Draw()
		{
			if (!Player.Instance.InCover)
			{
				_spriteBatch.Draw(_reticuleTexture, WeaponHandler.Instance._reticulePosition - _halfReticuleTexture, Color.White);

				if (_drawMuzzleFlare)
				{
					_spriteBatch.Draw(_gunTexture, _gunPosition, _firedGunSource, Color.White, _gunRotation,
								   new Vector2(_gunTexture.Width / 4f, _gunTexture.Height), SpriteEffects.None, 1f);

					_timesDrawnMuzzleFlare++;
				}
				else
				{
					_spriteBatch.Draw(_gunTexture, _gunPosition, _gunSource, Color.White, _gunRotation,
								  new Vector2(_gunTexture.Width / 4f, _gunTexture.Height), SpriteEffects.None, 1f);
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
			if (_playReloadSound)
			{
				_gunReloadSound.Play();

				_playReloadSound = false;
			}

			if (WeaponHandler.Instance._currentTime - WeaponHandler.Instance._startReloadTime <= _gunReloadSound.Duration.TotalMilliseconds) return;
			_ammo = _bullets.Length; // should be done in the loop vvvv  <-- (what?????)

			for (int i = 0; i < _bullets.Length; i++)
			{
				_bullets[i] = _bulletTexture;
			}

			WeaponHandler.Instance._playerState = Player.State.Alive;
		}

		/// <summary>
		/// Shoot the current weapon
		/// </summary>
		internal void Shoot()
		{
			//Just to make sure the player's not firing when (s)he's not supposed to
			if (WeaponHandler.Instance._playerState != Player.State.Shooting) return;
			_gunShotSound.Play();
			_drawMuzzleFlare = true;

			EnemyHandler.Instance.FiredAt(WeaponHandler.Instance._hitBox);

			//Set the array of textures to appear used when firing a shot
			_bullets[--_ammo] = _usedBulletTexture;

			//If the weapon's out of ammo, and the player's not currently reloading
			if (_ammo == 0 && WeaponHandler.Instance._playerState != Player.State.Reloading)
			{
				WeaponHandler.Instance._playerState = Player.State.Reloading;

				WeaponHandler.Instance._startReloadTime = WeaponHandler.Instance._frameInfo.GameTime.TotalGameTime.TotalMilliseconds;
				_playReloadSound = true;
			}
			else WeaponHandler.Instance._playerState = Player.State.Waiting;

			if (WeaponHandler.Instance._playerState != Player.State.Reloading && WeaponHandler.Instance._playerState != Player.State.Waiting && WeaponHandler.Instance._playerState != Player.State.Dead) WeaponHandler.Instance._playerState = Player.State.Alive;
		}

		internal void ResetWeapon()
		{
			_ammo = _bullets.Length;

			for (int i = 0; i < _bullets.Length; i++)
			{
				_bullets[i] = _bulletTexture;
			}
		}
	}
}
