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
		private readonly InputHandler _input = InputHandler.Instance;
		private readonly SpriteBatch _spriteBatch;

		private int _health, _healthIn10, _ammo, _fullHeartsToDraw, _emptyHeartsToDraw, _heartsDrawShift;
		private readonly int _widthOfHeart;
		private double _currentTime, _startShootTime, _startReloadTime;
		private bool _playReloadSound, _drawHalfHeart;
		private readonly Texture2D _reticuleTexture, _bulletTexture, _usedBulletTexture, _gunTexture, _healthTexture, _coverTexture;
		private readonly Vector2 _halfReticuleTexture;
		private Vector2 _vectorGunToMouse, _reticulePosition;
		private readonly Texture2D[] _bullets;
		private readonly Vector2[] _bulletPositions;
		private readonly SoundEffect _gunShotSound, _gunReloadSound;
		private float _gunRotation;
		private readonly Rectangle _gunPosition, _fullHealthSource, _halfHealthSource, _emptyHealthSource;

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
			_healthTexture = content.Load<Texture2D>("Images/normalUsableHeart");
			_coverTexture = content.Load<Texture2D>("Images/usableCoverBox");
			_gunShotSound = content.Load<SoundEffect>("Sounds/GunFire");
			_gunReloadSound = content.Load<SoundEffect>("Sounds/ReloadSound");
			_spriteBatch = spriteBatch;
			_halfReticuleTexture = new Vector2(_reticuleTexture.Width / 2f, _reticuleTexture.Height / 2f);
			_bullets = new Texture2D[10];
			_bulletPositions = new Vector2[10];
			_gunPosition = new Rectangle(Game1.Instance.GameScreenWidth / 2, Game1.Instance.GameScreenHeight + 40, _gunTexture.Width, _gunTexture.Height);
			_widthOfHeart = _healthTexture.Width/3;
			_fullHealthSource = new Rectangle(0, 0, _widthOfHeart, _healthTexture.Height);
			_halfHealthSource = new Rectangle(_widthOfHeart, 0, _widthOfHeart, _healthTexture.Height);
			_emptyHealthSource = new Rectangle(_widthOfHeart * 2, 0, _widthOfHeart, _healthTexture.Height);
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
			if(_playerState == States.Dead) Game1.Instance.Exit();

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

			if(_input.IsKeyDown(Keys.Space)) _playerState = States.InCover;

			if(_input.IsKeyPressed(Keys.R) && _playerState == States.Alive)
			{
				_playerState = States.Reloading;
				_startReloadTime = _frameInfo.GameTime.TotalGameTime.TotalMilliseconds;
				_playReloadSound = true;
			}

			if (_playerState == States.Shooting) Shoot();

			if (_playerState == States.Reloading) Reload();
		}

		public void Draw()
		{
			if (_playerState != States.InCover)
			{
				_spriteBatch.Draw(_reticuleTexture, _reticulePosition - _halfReticuleTexture, Color.White);
				_spriteBatch.Draw(_gunTexture, _gunPosition, null, Color.White, _gunRotation,
								  new Vector2(_gunTexture.Width / 2f, _gunTexture.Height), SpriteEffects.None, 1f);
			}
			else
			{
				_spriteBatch.Draw(_coverTexture,
					new Vector2((Game1.Instance.GameScreenWidth - _coverTexture.Width)  / 2f , Game1.Instance.GameScreenHeight - _coverTexture.Height),
					Color.White);
			}

			for (int i = 0; i < _bullets.Length; i++)
			{
				_spriteBatch.Draw(_bullets[i], _bulletPositions[i], Color.White);
			}

			DrawHealth();
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

			if (_playerState != States.Reloading && _playerState != States.Waiting && _playerState != States.Dead) _playerState = States.Alive;

			//TODO
			//This is a test for whether or not the damage function works (and the drawing of the health indicator)
			//Damaged(5);
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
			if (_playerState == States.InCover) damage /= 5;
			_health -= damage;

			if (_health <= 0)
			{
				_playerState = States.Dead;
			}
		}

		public void DrawHealth()
		{
			_healthIn10 = _health / 10;
			_heartsDrawShift = 0;
			_drawHalfHeart = _healthIn10 % 2 != 0;

			switch (_healthIn10)
			{
				case 10:
					_fullHeartsToDraw = 5;
					_emptyHeartsToDraw = 0;
					break;
				case 9:
					_fullHeartsToDraw = 4;
					_emptyHeartsToDraw = 0;
					break;
				case 8:
					_fullHeartsToDraw = 4;
					_emptyHeartsToDraw = 1;
					break;
				case 7:
					_fullHeartsToDraw = 3;
					_emptyHeartsToDraw = 1;
					break;
				case 6:
					_fullHeartsToDraw = 3;
					_emptyHeartsToDraw = 2;
					break;
				case 5:
					_fullHeartsToDraw = 2;
					_emptyHeartsToDraw = 2;
					break;
				case 4:
					_fullHeartsToDraw = 2;
					_emptyHeartsToDraw = 3;
					break;
				case 3:
					_fullHeartsToDraw = 1;
					_emptyHeartsToDraw = 3;
					break;
				case 2:
					_fullHeartsToDraw = 1;
					_emptyHeartsToDraw = 4;
					break;
				case 1:
					_fullHeartsToDraw = 0;
					_emptyHeartsToDraw = 4;
					break;
				case 0:
					_fullHeartsToDraw = 0;
					_emptyHeartsToDraw = 5;
					break;
			}

			
				for (int i = 0; i < _fullHeartsToDraw; i++)
				{
					_spriteBatch.Draw(_healthTexture,
						new Vector2(((Game1.Instance.GameScreenWidth - 300) + 60 * _heartsDrawShift), Game1.Instance.GameScreenHeight - 50),
						_fullHealthSource, Color.White);
					_heartsDrawShift++;
				}

				if (_drawHalfHeart)
				{
					_spriteBatch.Draw(_healthTexture,
					                  new Vector2(((Game1.Instance.GameScreenWidth - 300) + 60*_heartsDrawShift),
					                              Game1.Instance.GameScreenHeight - 50), _halfHealthSource, Color.White);
					_heartsDrawShift++;
				}

				for (int i = 0; i < _emptyHeartsToDraw; i++)
				{
					_spriteBatch.Draw(_healthTexture,
						new Vector2(((Game1.Instance.GameScreenWidth - 300) + 60 * _heartsDrawShift), Game1.Instance.GameScreenHeight - 50),
						_emptyHealthSource, Color.White);
					_heartsDrawShift++;
				}
		}
	}
}