using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChilledTreat.GameClasses
{
	class Enemy
	{
		readonly SpriteBatch _spriteBatch;

		private bool _drawMuzzleFlare;
		private int _timesDrawnMuzzleFlare;

		readonly Texture2D _texture, _muzzleFlare;
		Vector2 _position = new Vector2(20, 40);
		Vector2 _speed = new Vector2(0, 0.5f);
		private int _damageInflicted;

		private readonly Random _random = new Random();

		private readonly FrameInfo _frameInfo = FrameInfo.Instance;

		int _timeSinceLastFrame;
		private const int MillisecondsPerFrame = 100;

		public float _scale;

		public float GetScale()
		{
			return _scale;
		}

		//private SpriteEffects _walkingLeft = false; pr�ver med bool f�rst


		int _health = 20;

		private Point _frameSize;
		Point _currentFrame;
		private Point _sheetSize;

		enum State
		{
			Attacking,
			Dead
		}

		State _currentState;

		private Enemy(SpriteBatch spriteBatch, ContentManager content)
		{
			_spriteBatch = spriteBatch;
			_frameSize = new Point(41, 80);

			_texture = content.Load<Texture2D>("Images/enemy2");
			_position = new Vector2(538  + _random.Next(192) + _scale * _texture.Width,
				// TODO: Get enemies to spawn at correct height
				Game1.Instance.Window.ClientBounds.Height - 510 - _texture.Height * _scale);

			_currentFrame = new Point(0, 0);
			_sheetSize = new Point(7, 1);

			_muzzleFlare = content.Load<Texture2D>("Images/usableMuzzleFlare");
			_currentState = State.Attacking;
			_drawMuzzleFlare = false;
			_timesDrawnMuzzleFlare = 0;
		}

		public Enemy(SpriteBatch spriteBatch, ContentManager content, int hp)
			: this(spriteBatch, content)
		{
			_health = hp;
			//_position = position;


			// TODO: Generate a scale based on y (currently 2f)
		}

		// TODO: THIS IS SHIT.
		public static Point GetOriginTexture()
		{
			return new Point(41, 80);
		}

		public Rectangle GetRectangle()
		{
			return new Rectangle((int)_position.X, (int)_position.Y, (int)(_scale * _frameSize.X), (int)(_scale * _frameSize.Y));
		}

		void Attack()
		{
			_damageInflicted = EnemyHandler.Random.Next(15, 20);
			// TODO: Debug purposes
			Player.Instance.Damaged(_damageInflicted);
			Player.Instance.Damaged(0);
		}

		// Update health, check if dead
		public void TakeDamage(int damage)
		{
			_health -= damage;
			Console.WriteLine("Enemy hit! Remaining hp: " + _health);

			if (_health > 0) return;

			_currentState = State.Dead;
			// TODO: find a better solution
			EnemyHandler.Instance.Remove(this);
			Player.Instance.SuccesfullKill();
		}

		void Die()
		{
			// TODO: Animation through spritesheet ending in setting a boolean equal to true so the enemy can be removed from the list
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
					_currentFrame.X = 0;
					++_currentFrame.Y;
					if (_currentFrame.Y >= _sheetSize.Y)
						_currentFrame.Y = 0;
				}
			}

			if (EnemyHandler.Random.Next(1000) == 0)
			{
				Attack();
				_drawMuzzleFlare = true;
			}

			if (_timesDrawnMuzzleFlare >= 5)
			{
				_drawMuzzleFlare = false;
				_timesDrawnMuzzleFlare = 0;
			}


			// Movement based on state
			switch (_currentState)
			{
				case State.Attacking:
					_position.Y += _speed.Y;
					_scale = 0.008f * (_position.Y);
					if (_position.Y > Game1.Instance.Window.ClientBounds.Height - 300)
						_position.Y = Game1.Instance.Window.ClientBounds.Height - 300;
					break;
				case State.Dead:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}



		public void Draw()
		{
			_spriteBatch.Draw(_texture, _position,
			new Rectangle(_currentFrame.X * _frameSize.X, _currentFrame.Y * _frameSize.Y, _frameSize.X, _frameSize.Y),
			Color.White, 0, origin: Vector2.Zero, scale: _scale, effects: SpriteEffects.None, layerDepth: 0);

			if (!_drawMuzzleFlare) return;

			_spriteBatch.Draw(_muzzleFlare, position: new Vector2(_position.X - (_muzzleFlare.Width / 2f), y: _position.Y - (_muzzleFlare.Height / 2f) + (_texture.Height / 5f)), color: Color.White);

			_timesDrawnMuzzleFlare++;

			//_spriteBatch.Draw();
		}



	}


}
