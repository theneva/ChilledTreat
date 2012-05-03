using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ChilledTreat
{
	class Player
	{
		int _health, _ammo;
		long _startTime, _currentTime;
		readonly Texture2D _reticuleTexture;
		readonly SpriteBatch _sp;
		readonly InputHandler _input = InputHandler.Instance;
		readonly Vector2 _halfTexture;

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
			_reticuleTexture = content.Load<Texture2D>("img/reticule");
			_sp = spriteBatch;
			_halfTexture = new Vector2(_reticuleTexture.Width / 2, _reticuleTexture.Height / 2);
			_playerState = States.Alive;
		}

		public void Update()
		{
			ReticulePosition = new Vector2(_input.MouseState.X, _input.MouseState.Y);

			if (ReticulePosition.X < 0) ReticulePosition = new Vector2(0, ReticulePosition.Y);
			else if (ReticulePosition.X > 1280) ReticulePosition = new Vector2(1280, ReticulePosition.Y);

			if (ReticulePosition.Y < 0) ReticulePosition = new Vector2(ReticulePosition.X, 0);
			else if (ReticulePosition.Y > 720) ReticulePosition = new Vector2(ReticulePosition.X, 720);

			if (_playerState == States.Alive && _input.MouseState.LeftButton == ButtonState.Pressed && _input.PreviouseMouseState.LeftButton != ButtonState.Pressed)
			{
				_startTime = _frameInfo.GameTime.ElapsedGameTime.Milliseconds;
				_playerState = States.Shooting;
			}

			if (_playerState == States.Shooting)
			{
				_currentTime = _frameInfo.GameTime.ElapsedGameTime.Milliseconds;

				Shoot();

				if (_currentTime - _startTime > 200)
				{
					_playerState = States.Alive;
				}
			}

		}

		public void Draw()
		{
			_sp.Draw(_reticuleTexture, ReticulePosition - _halfTexture, Color.White);
		}

		public void Shoot()
		{
			_playerState = States.Shooting;
			_ammo--;

			if (_ammo == 0) Reload();
		}

		public void Reload()
		{
			_playerState = States.Reloading;

			_ammo = 10;
		}

		public void Damaged(int damage)
		{
			_health -= damage;

			if (_health <= 0) _playerState = States.Dead;
		}
	}
}