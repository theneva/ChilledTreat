using System;
using ChilledTreat.GameStates;
using ChilledTreat.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameClasses
{
	class Enemy
	{
		#region Fields
		public bool Alive { get; private set; }
		private int _health;
		private readonly int _damage;
		private Vector2 _position;
		readonly Texture2D _texture, _muzzleFlare;

		private Vector2 _speed;
		private int _damageInflicted;
		private int _heightOfRoad, _widthOfRoad, _fromEdgeToRoad;

		private readonly Random _random;

		int _timeSinceLastFrame;
		private const int MillisecondsPerFrame = 100;

		private float Scale { get; set; }

		private Point _frameSize;
		Point _currentFrame;
		private Point _sheetSize;
		private int _currentFrameOrigin;

		private bool _drawMuzzleFlare;
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor = default values
		/// </summary>
		public Enemy()
			: this(GameConstants.EnemyHealth, GameConstants.EnemyDamage) { }

		/// <summary>
		/// Specifies one value and sets the other to default based on a boolean
		/// </summary>
		/// <param name="value">The value to specify</param>
		/// <param name="isHealth">Whether or not the value to specify is health</param>
		public Enemy(int value, bool isHealth)
			: this(isHealth ? value : GameConstants.EnemyHealth, isHealth ? GameConstants.EnemyHealth : value) { }

		/// <summary>
		/// Main constructor. Assigns each field.
		/// </summary>
		/// <param name="health">Max health of enemy</param>
		/// <param name="damage">Max damage the enemy can deal</param>
		public Enemy(int health, int damage)
		{
			Alive = true;
			_health = health;
			_damage = damage;

			_speed = new Vector2(0, 0.5f);

			_texture = EnemyHandler.Instance.EnemyTexture;
			_currentFrame = new Point(0, 0);
			_frameSize = new Point(41, 58);
			_sheetSize = new Point(6, 0);

			// Redundant but not sufficiently inefficient for a workaround
			Scale = 0.008f * (_position.Y);

			_random = new Random();

			// 510 is the height of the road, 192 is the width of the top of the road,
			// 540 is the distance from the left border to the top of the road
			_heightOfRoad = (int) (540*Game1.GameScale);
			_widthOfRoad = (int) (192*Game1.GameScale);
			_fromEdgeToRoad = (int) (510 * Game1.GameScale);

			_position = new Vector2(_fromEdgeToRoad + _random.Next(_widthOfRoad) - _frameSize.X * Scale,
				Game1.GameScreenHeight - _heightOfRoad - _frameSize.Y * Scale);

			_muzzleFlare = EnemyHandler.Instance.MuzzleFlareTexture;
			_drawMuzzleFlare = false;
		}
		#endregion

		#region Methods
		/// <summary>
		/// The texture's surrounding rectangle
		/// </summary>
		/// <returns>Returns a new rectangle based on the texture's size and position</returns>
		public Rectangle GetRectangle()
		{
			return new Rectangle((int)_position.X, (int)_position.Y, (int)(Scale * _frameSize.X), (int)(Scale * _frameSize.Y));
		}

		/// <summary>
		/// Damages the player based on max damage
		/// </summary>
		void Attack()
		{
			_damageInflicted = InGame.Random.Next(_damage);
			Player.Instance.Damaged(_damageInflicted);
		}

		/// <summary>
		/// Updates health based on damage inflicted by player.
		/// </summary>
		/// <param name="inflictedDamage"></param>
		public void TakeDamage(int inflictedDamage)
		{
			_health -= inflictedDamage;
			if (_health <= 0)
				Die();
		}

		/// <summary>
		/// Attributes the player for the kill, plays through the spritesheet,
		/// and finally removes the enemy from the list
		/// </summary>
		void Die()
		{
			Alive = false;

			// Play a random grunt
			EnemyHandler.Instance.SoundEffects[InGame.Random.Next(EnemyHandler.Instance.SoundEffects.Count)].Play();
			Player.Instance.SuccesfullKill();

			_currentFrame = new Point(0, 1);
			_currentFrameOrigin = _frameSize.Y;
			// Width, height of dead enemies - could use some improvement
			_frameSize = new Point(78, 60);
			_sheetSize = new Point(3, 1);
		}
#endregion

		#region Update
		/// <summary>
		/// Selects the proper frames to draw, attacks if possible, and controls movement
		/// </summary>
		public void Update()
		{
			#region Animation frames
			_timeSinceLastFrame += FrameInfo.Instance.GameTime.ElapsedGameTime.Milliseconds;
			if (_timeSinceLastFrame > MillisecondsPerFrame)
			{
				_timeSinceLastFrame -= MillisecondsPerFrame;

				// Sneaky
				_drawMuzzleFlare = false;

				if (++_currentFrame.X >= _sheetSize.X)
				{
					if (!Alive)
					{
						EnemyHandler.Instance.Remove(this);
						return;
					}
					_currentFrame.X = 0;
					if (++_currentFrame.Y >= _sheetSize.Y)
						_currentFrame.Y = _currentFrameOrigin;
				}
			}
			#endregion

			if (!Alive) return;

			if (EnemyHandler.Random.Next(1000) == 0 && _position.Y > Game1.GameScreenHeight - 400)
			{
				Attack();
				_drawMuzzleFlare = true;
			}

			#region Movement
			_position.Y += _speed.Y;

			Scale = 0.008f * (_position.Y);

			if (_position.Y > Game1.GameScreenHeight - 300 * Game1.GameScale)
				_position.Y = Game1.GameScreenHeight - 300 * Game1.GameScale;
			#endregion
		}
#endregion

		#region Draw
		public void Draw()
		{
			Game1.Instance.SpriteBatch.Draw(_texture, _position,
					new Rectangle(_currentFrame.X * _frameSize.X, _currentFrame.Y * _frameSize.Y, _frameSize.X, _frameSize.Y), Color.White, 0, Vector2.Zero, Scale * Game1.GameScale, SpriteEffects.None, 0);

			#region MuzzleFlare
			if (!_drawMuzzleFlare) return;

			Game1.Instance.SpriteBatch.Draw(_muzzleFlare, new Vector2(_position.X - (_muzzleFlare.Width / 2f), (_position.Y - (_muzzleFlare.Height / 2f) + (_currentFrame.X * _frameSize.X / 2f)) * Game1.GameScale), Color.White);
			#endregion
		}
#endregion
	}
}
