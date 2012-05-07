using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChilledTreat.GameClasses
{
	class Enemy
	{
		readonly SpriteBatch _spriteBatch;

		readonly Texture2D _texture;
		Vector2 _position = new Vector2(20, 40);
		Vector2 _speed = new Vector2(0, 20);
		private int _damageInflicted;
		private readonly Random _random = new Random();

		private readonly FrameInfo _frameInfo = FrameInfo.Instance;

		int _timeSinceLastFrame;
		int _millisecondsPerFrame = 100;

		//private SpriteEffects _walkingLeft = false; prøver med bool først


		int hp = 20;

		Point _frameSize = new Point(41, 80);
		Point _currentFrame = new Point(0, 0);
		Point _sheetSize = new Point(7, 1);

		//Point _frameSize = new Point(41, 80);
		//Point _currentFrame = new Point(5, 4);
		//Point _sheetSize = new Point(1, 1);

		bool _walkingLeft = false;

		enum State
		{
			WalkingLeft,
			WalkingRight,
			Attacking,
			Dead
		}

		State WalkingLeft
		{
			get
			{
				_walkingLeft = true;
				_currentFrame = new Point();
				_sheetSize = new Point();
				return WalkingLeft;
			}
		}

		State WalkingRight
		{
			get
			{
				_walkingLeft = false;
				_currentFrame = new Point();
				_sheetSize = new Point();
				return WalkingRight;
			}
		}

		State Attacking
		{
			get
			{
				_walkingLeft = false;
				_currentFrame = new Point();
				_sheetSize = new Point();
				return Attacking;
			}
		}

		State Dead
		{
			get
			{
				_walkingLeft = false;
				_frameSize = new Point(57, 57);
				_currentFrame = new Point(7, 2);
				_sheetSize = new Point(1, 1);
				return Dead;
			}
		}

		State _currentState;

		public Enemy(SpriteBatch spriteBatch, ContentManager content)
		{
			_spriteBatch = spriteBatch;
			_position = Vector2.Zero;
			_texture = content.Load<Texture2D>("Images/enemy2");
			_currentState = State.Dead;
		}

		public Enemy(SpriteBatch spriteBatch, ContentManager content, int hp, Vector2 position)
			: this(spriteBatch, content)
		{
			this.hp = hp;
			this._position = position;
		}


		public void Update()
		{
			// Animation frames
			_timeSinceLastFrame += _frameInfo.GameTime.ElapsedGameTime.Milliseconds;
			if (_timeSinceLastFrame > _millisecondsPerFrame)
			{
				_timeSinceLastFrame -= _millisecondsPerFrame;

				if (++_currentFrame.X >= _sheetSize.X)
				{
					_currentFrame.X = 0;
					++_currentFrame.Y;
					if (_currentFrame.Y >= _sheetSize.Y)
						_currentFrame.Y = 0;
				}
			}

			//if(_random.Next(1000) == 0) Shoot();
			if (_random.Next(500) == 0) Shoot(); // <- approximately every 8 seconds


			// Movement based on state
			switch (_currentState)
			{
				// Don't move
				case State.WalkingLeft:
					// Move right with _walkingLeft set to true so frames are flipped horizontally.
					break;
				case State.WalkingRight:
					// Move right
					break;
				case State.Attacking:
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
			new Rectangle(x: _currentFrame.X * _frameSize.X, y: _currentFrame.Y * _frameSize.Y, width: _frameSize.X, height: _frameSize.Y),
			Color.White, 0, origin: Vector2.Zero, scale: 2, effects: _walkingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth: 0);
		}

		public Rectangle GetRectangle()
		{
			return new Rectangle((int)_position.X, (int)_position.Y, _frameSize.X, _frameSize.Y);
		}



		public void Shoot()
		{
			_damageInflicted = _random.Next(20);

			Console.WriteLine("Player hit!");
			Console.WriteLine(FrameInfo.Instance.GameTime.TotalGameTime.TotalSeconds);

			//TODO
			//Set up a singleton of the player-object
			//Player.Damaged(_damageInflicted);
		}

		public void TakeDamage(int damage)
		{
			Console.WriteLine("Enemy hit! Remaining hp: " + hp);
			hp -= damage;
			if (hp <= 0)
				_currentState = State.Dead;
		}
	}
}
