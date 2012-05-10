using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameClasses
{
	/// <summary>
	/// This class represents the player, and is used as a singleton. To access it, write GameClasses.Player.Instance
	/// </summary>
	public class Player
	{
		private readonly SpriteBatch _spriteBatch;
		private readonly InputHandler _input = InputHandler.Instance;

		private readonly SoundEffect[] _injuredSounds;
		private readonly SoundEffect _diedSound;
		private int _health, _healthIn10, _heartsDrawShift;
		private double _currentTime, _timeAtDamaged;
		private readonly int _widthOfHeart;
		private bool _drawRedHaze;
		private readonly Texture2D _healthTexture, _coverTexture, _damagedTexture;
		private readonly Rectangle _fullHealthSource, _halfHealthSource, _emptyHealthSource;
		readonly SpriteFont _scoreFont;

		public bool InCover { get; private set; }

		public int Score { get; private set; }

		static Player _instance;
		public static Player Instance
		{
			get { return _instance ?? (_instance = new Player(Game1.Instance.SpriteBatch, Game1.Instance.Content)); }
		}

		public enum State
		{
			Alive,
			Shooting,
			Reloading,
			Waiting,
			Damaged,
			Dead
		}

		public State PlayerState;

		private Player(SpriteBatch spriteBatch, ContentManager content)
		{
			_spriteBatch = spriteBatch;

			_health = 100;
			Score = 0;

			//Load all textures fonts
			_healthTexture = content.Load<Texture2D>("Images/normalUsableHeart");
			_coverTexture = content.Load<Texture2D>("Images/usableCoverBox");
			_damagedTexture = content.Load<Texture2D>("Images/damagedTint");
			_scoreFont = content.Load<SpriteFont>("Fonts/ScoreFont");
			_diedSound = content.Load<SoundEffect>("Sounds/poor-baby");

			_injuredSounds = new[] { content.Load<SoundEffect>("Sounds/goddamnit"),
				content.Load<SoundEffect>("Sounds/how-dare-you"),
				content.Load<SoundEffect>("Sounds/im-in-trouble"),
				content.Load<SoundEffect>("Sounds/uh") };

			_widthOfHeart = _healthTexture.Width / 3;
			_fullHealthSource = new Rectangle(0, 0, _widthOfHeart, _healthTexture.Height);
			_halfHealthSource = new Rectangle(_widthOfHeart, 0, _widthOfHeart, _healthTexture.Height);
			_emptyHealthSource = new Rectangle(_widthOfHeart * 2, 0, _widthOfHeart, _healthTexture.Height);
			PlayerState = State.Alive;
		}

		public void Update()
		{
			_currentTime = FrameInfo.Instance.GameTime.TotalGameTime.TotalMilliseconds;
			if (PlayerState == State.Dead)
			{
				EnemyHandler.Instance.Clear();
				_diedSound.Play();
				Game1.ChangeState(GameStates.GameState.GameOver);
			}

			if (_drawRedHaze && _currentTime - _timeAtDamaged > 200)
			{
				_drawRedHaze = false;
			}

			//
			_healthIn10 = _health / 10;
			//The shift to the side, so that the hearts are not drawn on top of each other
			_heartsDrawShift = 0;

			InCover = _input.IsCoverDown();

			WeaponHandler.Instance.Update();
		}

		public void Draw()
		{
			if (InCover)
			{
				_spriteBatch.Draw(_coverTexture,
					new Vector2((Game1.Instance.GameScreenWidth - _coverTexture.Width) / 2f, Game1.Instance.GameScreenHeight - _coverTexture.Height),
					Color.White);
			}

			DrawHealth();

			_spriteBatch.DrawString(_scoreFont, Convert.ToString(Score), new Vector2(Game1.Instance.GameScreenWidth - 100, 20), Color.Black);

			WeaponHandler.Instance.Draw();

			if (_drawRedHaze) _spriteBatch.Draw(_damagedTexture, Vector2.Zero, _damagedTexture.Bounds, Color.White, 0f, Vector2.Zero, ((float)Game1.Instance.GameScreenWidth / _damagedTexture.Width), SpriteEffects.None, layerDepth: 0);
		}

		/// <summary>
		/// How much the player is damaged
		/// </summary>
		/// <param name="damage">The amount of damage recieved</param>
		public void Damaged(int damage)
		{
			if (PlayerState != State.Reloading) PlayerState = State.Damaged;
			if (InCover) damage /= 5;

			//Play a random sound when injured
			_injuredSounds[EnemyHandler.Random.Next(_injuredSounds.Length)].Play();

			_drawRedHaze = true;

			_timeAtDamaged = FrameInfo.Instance.GameTime.TotalGameTime.TotalMilliseconds;

			// TODO: Debug purposes
			Console.WriteLine("Player hit for " + damage + "points @ " + FrameInfo.Instance.GameTime.TotalGameTime.TotalSeconds);

			_health -= damage;

			if (_health <= 0)
			{
				PlayerState = State.Dead;
			}
		}

		/// <summary>
		/// Draw the correct amount of hearts (health-indicator) on the screen.
		/// It's cleaner having the logic behind it in it's own method
		/// </summary>
		private void DrawHealth()
		{
			for (int i = 0; i < _healthIn10 / 2; i++)
			{
				_spriteBatch.Draw(_healthTexture,
					new Vector2((Game1.Instance.GameScreenWidth - 300) + 60 * _heartsDrawShift, Game1.Instance.GameScreenHeight - 50),
					_fullHealthSource, Color.White);
				_heartsDrawShift++;
			}

			//Always draw 5 hearts. If the int division ends up removing the fraction (meaning we lose 0.5 hearts), draw a half-heart
			if (_healthIn10 % 2 != 0)
			{
				_spriteBatch.Draw(_healthTexture,
								  new Vector2((Game1.Instance.GameScreenWidth - 300) + 60 * _heartsDrawShift,
											  Game1.Instance.GameScreenHeight - 50), _halfHealthSource, Color.White);
				_heartsDrawShift++;
			}

			for (int i = _healthIn10 / 2; i < 5; i++)
			{
				_spriteBatch.Draw(_healthTexture,
					new Vector2((Game1.Instance.GameScreenWidth - 300) + 60 * _heartsDrawShift, Game1.Instance.GameScreenHeight - 50),
					_emptyHealthSource, Color.White);
				_heartsDrawShift++;
			}
		}

		/// <summary>
		/// Adds 1 to the player score
		/// </summary>
		public void SuccesfullKill()
		{
			Score++;
		}

		/// <summary>
		/// Reset the player's score, health and state
		/// Since the player is a singleton, the constructor is only called once. Because of that, we use this method
		/// </summary>
		public void ResetPlayer()
		{
			_health = 100;
			Score = 0;

			PlayerState = State.Alive;

			WeaponHandler.Instance.ResetWeapons();
		}
	}
}