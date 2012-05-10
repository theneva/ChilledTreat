using System;
using ChilledTreat.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChilledTreat.GameClasses
{
	class Enemy
	{

		public bool Alive { get; protected set; }
		private int _health, _damage;
		Vector2 _position = new Vector2(20, 40);
		readonly Texture2D _texture, _muzzleFlare;

		Vector2 _speed = new Vector2(0, 0.5f);
		private int _damageInflicted;

		private const int DefaultDamage = 20, DefaultHealth = 1;



		private readonly Random _random = new Random();

		private readonly FrameInfo _frameInfo = FrameInfo.Instance;

		int _timeSinceLastFrame;
		private const int MillisecondsPerFrame = 100;

		public float Scale { get; private set; }



		private Point _frameSize;
		Point _currentFrame;
		private Point _sheetSize;
		private int _currentFrameOrigin;



		private bool _drawMuzzleFlare;
		private int _timesDrawnMuzzleFlare;

		/// <summary>
		/// Constructors!
		/// </summary>
		public Enemy()
			: this(DefaultHealth, DefaultDamage) { }

		public Enemy(int value, bool isHealth)
			: this(isHealth ? value : DefaultHealth, isHealth ? DefaultDamage : value) { }

		public Enemy(int health, int damage)
		{
			Alive = true;
			_health = health;
			_damage = damage;

			_texture = Game1.Instance.Content.Load<Texture2D>("Images/enemy2 - Copy");
			_currentFrame = new Point(0, 0);
			_frameSize = new Point(39, 58);
			_sheetSize = new Point(7, 1);


			// TODO: Redundant
			Scale = 0.008f * (_position.Y);


			_muzzleFlare = Game1.Instance.Content.Load<Texture2D>("Images/usableMuzzleFlare");
			_drawMuzzleFlare = false;
			_timesDrawnMuzzleFlare = 0;

			_position = new Vector2(538 + _random.Next(192) - _texture.Width * Scale,
				// TODO: Get enemies to spawn at correct height
				Game1.GameScreenHeight - 480 /*- _texture.Height * _scale*/);


		}

		public Rectangle GetRectangle()
		{
			return new Rectangle((int)_position.X, (int)_position.Y, (int)(Scale * _frameSize.X), (int)(Scale * _frameSize.Y));
		}

		void Attack()
		{
			_damageInflicted = InGame.Random.Next();

			Player.Instance.Damaged(_damageInflicted);
		}

		// Update health, check if dead
		public void TakeDamage(int inflictedDamage)
		{
			_health -= inflictedDamage;
			if (_health <= 0)
				Die();

			// TODO: Debug
			Console.WriteLine("Enemy hit! Remaining hp: " + _health);
		}



		public void Update()
		{
			// Animation frames
			_timeSinceLastFrame += _frameInfo.GameTime.ElapsedGameTime.Milliseconds;
			if (_timeSinceLastFrame > MillisecondsPerFrame)
			{
				_timeSinceLastFrame -= MillisecondsPerFrame;

				if (++_currentFrame.X >= _sheetSize.X)
				{
					if (!Alive)
					{
						EnemyHandler.Instance.Remove(this);
						return;
					}
					_currentFrame.X = 0;
					++_currentFrame.Y;
					if (_currentFrame.Y >= _sheetSize.Y)
						_currentFrame.Y = _currentFrameOrigin;
				}
			}


			if (!Alive) return;

			if (EnemyHandler.Random.Next(1000) == 0 && _position.Y > Game1.GameScreenHeight - 400)
			{
				Attack();
				_drawMuzzleFlare = true;
			}

			if (_timesDrawnMuzzleFlare >= 5)
			{
				_drawMuzzleFlare = false;
				_timesDrawnMuzzleFlare = 0;
			}


			// Movement based on life
			_position.Y += _speed.Y;

			Scale = 0.008f * (_position.Y);

			if (_position.Y > Game1.GameScreenHeight - 300)
				_position.Y = Game1.GameScreenHeight - 300;
		}

		/// <summary>
		/// Attributes the player for the kill, plays through the spritesheet,
		/// before removing the enemy from the list
		/// </summary>
		void Die()
		{
			Alive = false;
			Player.Instance.SuccesfullKill();
			_currentFrame = new Point(0, 1);
			_currentFrameOrigin = _frameSize.Y;
			_frameSize = new Point(78, 60);
			_sheetSize = new Point(3, 1);


		}

		public void Draw()
		{
			Game1.Instance.SpriteBatch.Draw(_texture, _position,
			new Rectangle(_currentFrame.X * _frameSize.X, _currentFrame.Y * _frameSize.Y, _frameSize.X, _frameSize.Y), Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);


			#region MuzzleFlare
			if (!_drawMuzzleFlare) return;

			Game1.Instance.SpriteBatch.Draw(_muzzleFlare, new Vector2(_position.X - (_muzzleFlare.Width / 2f), _position.Y - (_muzzleFlare.Height / 2f) + (_texture.Height / 5f)), Color.White);

			_timesDrawnMuzzleFlare++;
			#endregion
		}
	}
}
