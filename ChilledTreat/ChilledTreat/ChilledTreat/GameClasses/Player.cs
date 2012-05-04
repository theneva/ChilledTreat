using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace ChilledTreat.GameClasses
{
	class Player
	{
		int _health, _ammo;
		double _currentTime, _startShootTime, _startReloadTime;
		private bool _playReloadSound;
		private readonly Texture2D _reticuleTexture, _bulletTexture, _usedBulletTexture, _gunTexture;
		private readonly SpriteBatch _spriteBatch;
		private readonly InputHandler _input = InputHandler.Instance;
		private readonly Vector2 _halfReticuleTexture;
		private readonly Texture2D[] _bullets;
		private readonly Vector2[] _bulletPositions;
		private readonly SoundEffect _gunShotSound, _gunReloadSound;
		private readonly SpriteFont _healthFont;
		private readonly Color _healthFontColor;
		private Vector2 _vectorGunToMouse, _reticulePosition;
		private float _gunRotation;
		private Rectangle _gunPosition;

		enum States
		{
			Alive,
			Shooting,
			Reloading,
			InCover,
			Waiting,
			Dead
		}

		States _playerState;

		private readonly FrameInfo _frameInfo = FrameInfo.Instance;

		public Player(SpriteBatch spriteBatch, ContentManager content)
		{
			_health = 100;
			_ammo = 10;
			_playReloadSound = false;
			_reticuleTexture = content.Load<Texture2D>("Images/usableReticule");
			_bulletTexture = content.Load<Texture2D>("Images/usableBullet");
			_usedBulletTexture = content.Load<Texture2D>("Images/usableUsedBullet");
			_gunTexture = content.Load<Texture2D>("Images/gunTest");
			_gunShotSound = content.Load<SoundEffect>("Sounds/GunFire");
			_gunReloadSound = content.Load<SoundEffect>("Sounds/ReloadSound");
			_healthFont = content.Load<SpriteFont>("fonts/healthFont");
			_healthFontColor = Color.Black;
			_spriteBatch = spriteBatch;
			_halfReticuleTexture = new Vector2(_reticuleTexture.Width / 2f, _reticuleTexture.Height / 2f);
			_bullets = new Texture2D[10];
			_bulletPositions = new Vector2[10];
			_gunPosition = new Rectangle(Game1.Instance.GameScreenWidth / 2, Game1.Instance.GameScreenHeight + 40, _gunTexture.Width, _gunTexture.Height);
			_playerState = States.Alive;

			for (int i = 0; i < _bulletPositions.Length; i++)
			{
				_bulletPositions[i] = new Vector2(i * 20, Game1.Instance.GameScreenHeight - 100);
			}

			for (int i = 0; i < _bullets.Length; i++)
			{
				_bullets[i] = _bulletTexture;
			}
		}

		public void Update()
		{
			_currentTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;
			_reticulePosition = new Vector2(_input.MouseState.X, _input.MouseState.Y);
			if (_currentTime - _startShootTime > 200 && _playerState != States.Reloading) _playerState = States.Alive;

			_vectorGunToMouse = new Vector2((_gunPosition.X - _input.MouseState.X), (_gunPosition.Y - _input.MouseState.Y));
			_gunRotation = (float) Math.Atan2(- _vectorGunToMouse.X, _vectorGunToMouse.Y);

			if (_reticulePosition.X < 0) _reticulePosition = new Vector2(0, _reticulePosition.Y);
			else if (_reticulePosition.X > Game1.Instance.GameScreenWidth) _reticulePosition = new Vector2(Game1.Instance.GameScreenWidth, _reticulePosition.Y);

			if (_reticulePosition.Y < 0) _reticulePosition = new Vector2(_reticulePosition.X, 0);
			else if (_reticulePosition.Y > Game1.Instance.GameScreenHeight) _reticulePosition = new Vector2(_reticulePosition.X, Game1.Instance.GameScreenHeight);

			if (_playerState == States.Alive && _input.IsLeftMouseButtonPressed())
			{
				_startShootTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;

				_playerState = States.Shooting;
			}

			if(_input.IsKeyPressed(Keys.R) && _playerState != States.Reloading)
			{
				_playerState = States.Reloading;
				_startReloadTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;
				_playReloadSound = true;
			}

			if (_playerState == States.Shooting)
			{
				Shoot();

				//if (_currentTime - _startShootTime > 1000) _playerState = States.Alive;
			}

			if (_playerState == States.Reloading) Reload();
		}

		public void Draw()
		{
			_spriteBatch.Draw(_reticuleTexture, _reticulePosition - _halfReticuleTexture, Color.White);
			_spriteBatch.Draw(_gunTexture, _gunPosition, null, Color.White, _gunRotation, new Vector2(_gunTexture.Width / 2f, _gunTexture.Height), SpriteEffects.None, 1f);


			for (int i = 0; i < _bullets.Length; i++)
			{
				_spriteBatch.Draw(_bullets[i], _bulletPositions[i], Color.White);
			}

			_spriteBatch.DrawString(_healthFont, _health.ToString(), new Vector2(Game1.Instance.GameScreenWidth-75, Game1.Instance.GameScreenHeight - 70),_healthFontColor );
		}

		public void Shoot()
		{
			_gunShotSound.Play();
			_bullets[--_ammo] = _usedBulletTexture;

			if (_ammo == 0 && _playerState != States.Reloading)
			{
				_playerState = States.Reloading;

				_startReloadTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;
				_playReloadSound = true;
			} else _playerState = States.Waiting;

			if (_playerState != States.Reloading && _playerState != States.Waiting) _playerState = States.Alive;

		}

		public void Reload()
		{
			if (_playReloadSound)
			{
				_gunReloadSound.Play();

				_playReloadSound = false;
			}

			if (_currentTime - _startReloadTime > 2500)
			{
				_ammo = 10;

				for (int i = 0; i < _bullets.Length; i++)
				{
					_bullets[i] = _bulletTexture;
				}

				_playerState = States.Alive;
			}
		}

		public void Damaged(int damage)
		{
			_health -= damage;

			if (_health <= 0)
			{
				_playerState = States.Dead;
			}
		}
	}
}