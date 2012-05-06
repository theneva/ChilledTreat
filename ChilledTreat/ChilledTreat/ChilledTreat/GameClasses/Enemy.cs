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

		private readonly FrameInfo _frameInfo = FrameInfo.Instance;

		int timeSinceLastFrame = 0;
		int millisecondsPerFrame = 100;


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
			timeSinceLastFrame += _frameInfo.GameTime.ElapsedGameTime.Milliseconds;
			if (timeSinceLastFrame > millisecondsPerFrame)
			{
				timeSinceLastFrame -= millisecondsPerFrame;

				if (++_currentFrame.X >= _sheetSize.X)
				{
					_currentFrame.X = 0;
					++_currentFrame.Y;
					if (_currentFrame.Y >= _sheetSize.Y)
						_currentFrame.Y = 0;
				}
			}

			


			// Movement
			//_position.Y += _speed.Y;

			//if (_position.Y > 200)
			//{
			//    _speed.Y *= -1;
			//    _position.Y = 200;
			//}
			//else if (_position.Y < 0)
			//{
			//    _speed.Y *= -1;
			//    _position.Y = 0;
			//}
		}

		public void Draw()
		{
			_spriteBatch.Draw(_texture, _position,
			new Rectangle(_currentFrame.X * _frameSize.X,
			_currentFrame.Y * _frameSize.Y,
			_frameSize.X,
			_frameSize.Y),
			Color.White, 0, Vector2.Zero,
			2, _walkingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
		}

		public Rectangle GetPosition()
		{
			return new Rectangle((int) _position.X, (int) _position.Y, _frameSize.X, _frameSize.Y);
		}
	}
}
