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
		Vector2 _speed = new Vector2(0, 20);
		private int _damageInflicted;

		private readonly FrameInfo _frameInfo = FrameInfo.Instance;

		int _timeSinceLastFrame;
		private const int MillisecondsPerFrame = 100;

		//private SpriteEffects _walkingLeft = false; prøver med bool først


		int _health = 20;

		private Point _frameSize;
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

		// Fuck this shit

		//State WalkingLeft
		//{
		//    get
		//    {
		//        _walkingLeft = true;
		//        _currentFrame = new Point();
		//        _sheetSize = new Point();
		//        return WalkingLeft;
		//    }
		//}

		//State WalkingRight
		//{
		//    get
		//    {
		//        _walkingLeft = false;
		//        _currentFrame = new Point();
		//        _sheetSize = new Point();
		//        return WalkingRight;
		//    }
		//}

		//State Attacking
		//{
		//    get
		//    {
		//        _walkingLeft = false;
		//        _currentFrame = new Point();
		//        _sheetSize = new Point();
		//        return Attacking;
		//    }
		//}

		//State Dead
		//{
		//    get
		//    {
		//        _walkingLeft = false;
		//        _frameSize = new Point(57, 57);
		//        _currentFrame = new Point(7, 2);
		//        _sheetSize = new Point(1, 1);
		//        return Dead;
		//    }
		//}

		State _currentState;

		private Enemy(SpriteBatch spriteBatch, ContentManager content)
		{
			_spriteBatch = spriteBatch;
			_position = Vector2.Zero;
			_frameSize = new Point(41, 80);
			_texture = content.Load<Texture2D>("Images/enemy2");
			_muzzleFlare = content.Load<Texture2D>("Images/usableMuzzleFlare");
			_currentState = State.Dead;
			_drawMuzzleFlare = false;
			_timesDrawnMuzzleFlare = 0;
		}

		public Enemy(SpriteBatch spriteBatch, ContentManager content, int hp, Vector2 position)
			: this(spriteBatch, content)
		{
			_health = hp;
			_position = position;
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

			if (EnemyHandler.Random.Next(1000) == 0) // <- approximately every 8 seconds
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
				//    // Don't move
				case State.WalkingLeft:
					//        // Move right with _walkingLeft set to true so frames are flipped horizontally.
					//        MoveLeft();
					break;
				case State.WalkingRight:
					//        // Move right
					//        MoveRight();
					//        // Too far right
					//        if (_position.X > 1280) // TODO: Give each Enemy a platform and check for end of that rectangle
					//        {
					//            _walkingLeft = true;
					//            _position.X = 1280;
					//        }
					break;
				case State.Attacking:
					//        // Move "towards" player
					break;
				case State.Dead:
					Die();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}


		}

		public void Die()
		{
			// TODO: Animation through spritesheet ending in setting a boolean equal to true so the enemy can be removed from the list
		}

		public void MoveLeft()
		{
			if (_position.X >= 0)
			{


				return;
			}

			_walkingLeft = false;
			_position.X = 0;
		}

		public void MoveRight()
		{

		}


		public void Draw()
		{
			_spriteBatch.Draw(_texture, _position,
			new Rectangle(_currentFrame.X * _frameSize.X, _currentFrame.Y * _frameSize.Y, _frameSize.X, _frameSize.Y),
			Color.White, 0, Vector2.Zero, 2, _walkingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth: 0);

			if (_drawMuzzleFlare)
			{
				_spriteBatch.Draw(_muzzleFlare, new Vector2(_position.X - (_muzzleFlare.Width / 2f), _position.Y - (_muzzleFlare.Height / 2f) + (_texture.Height / 5f)), Color.White);

				_timesDrawnMuzzleFlare++;
			}
		}

		public Rectangle GetRectangle()
		{
			return new Rectangle((int)_position.X, (int)_position.Y, _frameSize.X, _frameSize.Y);
		}

		public void Attack()
		{
			_damageInflicted = EnemyHandler.Random.Next(20);

			// TODO: Debug purposes
			Console.WriteLine("Player hit for " + _damageInflicted + "points @ " + FrameInfo.Instance.GameTime.TotalGameTime.TotalSeconds);

			Player.Instance.Damaged(_damageInflicted);
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
	}
}
