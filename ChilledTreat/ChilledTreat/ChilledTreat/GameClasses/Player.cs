using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChilledTreat.GameClasses
{
	class Player
	{
		int _health, _ammo;
		long _currentTime, _startShootTime, _startReloadTime;
		readonly Texture2D _reticuleTexture, _bulletTexture, _usedBulletTexture;
		readonly SpriteBatch _sp;
		readonly InputHandler _input = InputHandler.Instance;
		readonly Vector2 _halfReticuleTexture;
		private readonly Texture2D[] _bullets;
		private readonly Vector2[] _bulletPositions;

		enum States
		{
			Alive,
			Shooting,
			Reloading,
			InCover,
			Dead
		}

		States _playerState;

		private readonly FrameInfo _frameInfo = FrameInfo.Instance;

		private Vector2 ReticulePosition { get; set; }

		public Player(SpriteBatch spriteBatch, ContentManager content)
		{
			_health = 100;
			_ammo = 10;
			_reticuleTexture = content.Load<Texture2D>("Images/usableReticule");
			_bulletTexture = content.Load<Texture2D>("Images/usableBullet");
			_usedBulletTexture = content.Load<Texture2D>("Images/usableUsedBullet");
			_sp = spriteBatch;
			_halfReticuleTexture = new Vector2(_reticuleTexture.Width / 2f, _reticuleTexture.Height / 2f);
			_bullets = new Texture2D[10];
			_bulletPositions = new Vector2[10];
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
			ReticulePosition = new Vector2(_input.MouseState.X, _input.MouseState.Y);

			if (ReticulePosition.X < 0) ReticulePosition = new Vector2(0, ReticulePosition.Y);
			else if (ReticulePosition.X > Game1.Instance.GameScreenWidth) ReticulePosition = new Vector2(Game1.Instance.GameScreenWidth, ReticulePosition.Y);

			if (ReticulePosition.Y < 0) ReticulePosition = new Vector2(ReticulePosition.X, 0);
			else if (ReticulePosition.Y > Game1.Instance.GameScreenHeight) ReticulePosition = new Vector2(ReticulePosition.X, Game1.Instance.GameScreenHeight);

			if (_playerState == States.Alive && _input.MouseState.LeftButton == ButtonState.Pressed && _input.PreviouseMouseState.LeftButton == ButtonState.Released)
			{
				_startShootTime = _frameInfo.GameTime.ElapsedGameTime.Milliseconds;
				_playerState = States.Shooting;
			}

			if (_playerState == States.Shooting)
			{
				_currentTime = _frameInfo.GameTime.ElapsedGameTime.Milliseconds;

				Shoot();

				if (_currentTime - _startShootTime > 200)
				{
					_playerState = States.Alive;
				}
			}

		}

		public void Draw()
		{
			_sp.Draw(_reticuleTexture, ReticulePosition - _halfReticuleTexture, Color.White);

			for (int i = 0; i < _bullets.Length; i++)
			{
				_sp.Draw(_bullets[i], _bulletPositions[i], Color.White);
			}
		}

		public void Shoot()
		{
			_playerState = States.Shooting;

			_bullets[--_ammo] = _usedBulletTexture;

			if (_ammo == 0) Reload();

			_startReloadTime = _frameInfo.GameTime.ElapsedGameTime.Milliseconds;

			_playerState = States.Alive;
		}

		public void Reload()
		{
			_playerState = States.Reloading;

			if (!(_startReloadTime - _currentTime > 1000)) return;

			_ammo = 10;
			for (int i = 0; i < _bullets.Length; i++)
			{
				_bullets[i] = _bulletTexture;
			}

			_playerState = States.Alive;
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