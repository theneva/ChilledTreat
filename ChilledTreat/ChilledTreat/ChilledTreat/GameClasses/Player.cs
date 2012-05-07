using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChilledTreat.GameClasses
{
	class Player
	{
		private readonly InputHandler _input = InputHandler.Instance;
		private readonly SpriteBatch _spriteBatch;

		private int _health, _healthIn10, _ammo, _heartsDrawShift, _timesDrawnFire;
		private readonly int _widthOfHeart;
		private double _currentTime, _startShootTime, _startReloadTime;
		private bool _playReloadSound, _drawFire;
		private readonly Texture2D _reticuleTexture, _bulletTexture, _usedBulletTexture, _gunTexture, _healthTexture, _coverTexture;
		private readonly Vector2 _halfReticuleTexture;
		private Vector2 _vectorGunToMouse, _reticulePosition;
		private readonly Texture2D[] _bullets;
		private readonly Vector2[] _bulletPositions;
		private readonly SoundEffect _gunShotSound, _gunReloadSound;
		private float _gunRotation;
		private readonly Rectangle _gunPosition, _gunSource, _firedGunSource, _fullHealthSource, _halfHealthSource, _emptyHealthSource;
		private Rectangle _hitBox;

		enum State
		{
			Alive,
			Shooting,
			Reloading,
			InCover,
			Waiting,
			Damaged,
			Dead
		}

		State _playerState;
		static Player _instance;
		public static Player Instance
		{
			get { return _instance ?? (_instance = new Player(Game1.Instance.SpriteBatch, Game1.Instance.Content)); }
		}

		private readonly FrameInfo _frameInfo = FrameInfo.Instance;

		private Player(SpriteBatch spriteBatch, ContentManager content)
		{
			_spriteBatch = spriteBatch;
			
			_health = 100;
			_ammo = 10;
			_timesDrawnFire = 0;
			_playReloadSound = false;
			_drawFire = false;
			_reticuleTexture = content.Load<Texture2D>("Images/usableReticule");
			_bulletTexture = content.Load<Texture2D>("Images/usableBullet");
			_usedBulletTexture = content.Load<Texture2D>("Images/usableUsedBullet");
			_gunTexture = content.Load<Texture2D>("Images/gunTest");
			_healthTexture = content.Load<Texture2D>("Images/normalUsableHeart");
			_coverTexture = content.Load<Texture2D>("Images/usableCoverBox");
			_gunShotSound = content.Load<SoundEffect>("Sounds/GunFire");
			_gunReloadSound = content.Load<SoundEffect>("Sounds/ReloadSound");
			_halfReticuleTexture = new Vector2(_reticuleTexture.Width / 2f, _reticuleTexture.Height / 2f);
			_bullets = new Texture2D[10];
			_bulletPositions = new Vector2[10];
			_gunSource = new Rectangle(0, 0, 80, 130);
			_firedGunSource = new Rectangle(80, 0, 80, 130);
			_gunPosition = new Rectangle(Game1.Instance.GameScreenWidth / 2, Game1.Instance.GameScreenHeight + 40, _gunTexture.Width / 2, _gunTexture.Height);
			_widthOfHeart = _healthTexture.Width / 3;
			_fullHealthSource = new Rectangle(0, 0, _widthOfHeart, _healthTexture.Height);
			_halfHealthSource = new Rectangle(_widthOfHeart, 0, _widthOfHeart, _healthTexture.Height);
			_emptyHealthSource = new Rectangle(_widthOfHeart * 2, 0, _widthOfHeart, _healthTexture.Height);
			_playerState = State.Alive;
			_hitBox = new Rectangle();

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
			if (_playerState == State.Dead) Game1.Instance.Exit();

			if (_timesDrawnFire >= 5)
			{
				_drawFire = false;
				_timesDrawnFire = 0;
			}

			_healthIn10 = _health / 10;
			//The shift to the side, so that the hearts are not drawn on top of each other
			_heartsDrawShift = 0;

			_currentTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;
			_reticulePosition = new Vector2(_input.MouseState.X, _input.MouseState.Y);
			if (_currentTime - _startShootTime > 200 && _playerState != State.Reloading) _playerState = State.Alive;

			_vectorGunToMouse = new Vector2((_gunPosition.X - _input.MouseState.X), (_gunPosition.Y - _input.MouseState.Y));
			_gunRotation = (float)Math.Atan2(-_vectorGunToMouse.X, _vectorGunToMouse.Y);

			if (_reticulePosition.X < 0) _reticulePosition = new Vector2(0, _reticulePosition.Y);
			else if (_reticulePosition.X > Game1.Instance.GameScreenWidth) _reticulePosition = new Vector2(Game1.Instance.GameScreenWidth, _reticulePosition.Y);

			if (_reticulePosition.Y < 0) _reticulePosition = new Vector2(_reticulePosition.X, 0);
			else if (_reticulePosition.Y > Game1.Instance.GameScreenHeight) _reticulePosition = new Vector2(_reticulePosition.X, Game1.Instance.GameScreenHeight);

			if (_playerState == State.Alive && _input.IsLeftMouseButtonPressed())
			{
				_startShootTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;

				_hitBox = new Rectangle(_input.MouseState.X - 20, _input.MouseState.Y - 20, 40, 40);

				_playerState = State.Shooting;
			}

			if (_input.IsKeyDown(Keys.Space)) _playerState = State.InCover;

			if (_input.IsKeyPressed(Keys.R) && _playerState == State.Alive && _ammo != 10)
			{
				_playerState = State.Reloading;
				_startReloadTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;
				_playReloadSound = true;
			}

			if (_playerState == State.Shooting) Shoot();

			if (_playerState == State.Reloading) Reload();
		}

		public void Draw()
		{
			if (_playerState != State.InCover)
			{
				_spriteBatch.Draw(_reticuleTexture, _reticulePosition - _halfReticuleTexture, Color.White);

				if (_drawFire)
				{
					_spriteBatch.Draw(_gunTexture, _gunPosition, _firedGunSource, Color.White, _gunRotation,
								   new Vector2(_gunTexture.Width / 4f, _gunTexture.Height), SpriteEffects.None, 1f);

					_timesDrawnFire++;
				}
				else
				{
					_spriteBatch.Draw(_gunTexture, _gunPosition, _gunSource, Color.White, _gunRotation,
								  new Vector2(_gunTexture.Width / 4f, _gunTexture.Height), SpriteEffects.None, 1f);
				}
			}
			else
			{
				_spriteBatch.Draw(_coverTexture,
					new Vector2((Game1.Instance.GameScreenWidth - _coverTexture.Width) / 2f, Game1.Instance.GameScreenHeight - _coverTexture.Height),
					Color.White);
			}

			for (int i = 0; i < _bullets.Length; i++)
			{
				_spriteBatch.Draw(_bullets[i], _bulletPositions[i], Color.White);
			}

			DrawHealth();
		}

		private void Shoot()
		{
			_gunShotSound.Play();
			_drawFire = true;
			_bullets[--_ammo] = _usedBulletTexture;

			EnemyHandler.Instance.FiredAt(_hitBox);

			if (_ammo == 0 && _playerState != State.Reloading)
			{
				_playerState = State.Reloading;

				_startReloadTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;
				_playReloadSound = true;
			}
			else _playerState = State.Waiting;

			if (_playerState != State.Reloading && _playerState != State.Waiting && _playerState != State.Dead) _playerState = State.Alive;

			//TODO
			//This is a test for whether or not the damage function works (and the drawing of the health indicator)
			//Damaged(5);
		}

		private void Reload()
		{
			if (_playReloadSound)
			{
				_gunReloadSound.Play();

				_playReloadSound = false;
			}

			if (_currentTime - _startReloadTime <= _gunReloadSound.Duration.TotalMilliseconds) return;
			_ammo = 10;

			for (var i = 0; i < _bullets.Length; i++)
			{
				_bullets[i] = _bulletTexture;
			}

			_playerState = State.Alive;
		}

		public void Damaged(int damage)
		{
			_playerState = State.Damaged;
			if (_playerState == State.InCover) damage /= 5;
			_health -= damage;

			if (_health <= 0)
			{
				_playerState = State.Dead;
			}
		}

		//This method determines how many hearts are drawn on the screen (i.e. how much health is left), and draws them on the screen
		private void DrawHealth()
		{
			for (int i = 0; i < _healthIn10 / 2; i++)
			{
				_spriteBatch.Draw(_healthTexture,
					new Vector2(((Game1.Instance.GameScreenWidth - 300) + 60 * _heartsDrawShift), Game1.Instance.GameScreenHeight - 50),
					_fullHealthSource, Color.White);
				_heartsDrawShift++;
			}

			//Always draw 5 hearts. If the int division ends up removing
			if (_healthIn10 % 2 != 0)
			{
				_spriteBatch.Draw(_healthTexture,
								  new Vector2(((Game1.Instance.GameScreenWidth - 300) + 60 * _heartsDrawShift),
											  Game1.Instance.GameScreenHeight - 50), _halfHealthSource, Color.White);
				_heartsDrawShift++;
			}

			for (int i = _healthIn10 / 2; i < 5; i++)
			{
				_spriteBatch.Draw(_healthTexture,
					new Vector2(((Game1.Instance.GameScreenWidth - 300) + 60 * _heartsDrawShift), Game1.Instance.GameScreenHeight - 50),
					_emptyHealthSource, Color.White);
				_heartsDrawShift++;
			}
		}
	}
}