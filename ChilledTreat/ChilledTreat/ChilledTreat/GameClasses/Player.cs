using System;
using ChilledTreat.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameClasses
{
	/// <summary>
	/// This class represents the player, and is used as a singleton. To access it, write GameClasses.Player.Instance
	/// </summary>
	public class Player
	{
		#region Fields

		private const int MaxHealth = GameConstants.PlayerHealth;

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
			get { return _instance ?? (_instance = new Player()); }
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

		#endregion

		private Player()
		{
			_spriteBatch = Game1.Instance.SpriteBatch;
			_health = MaxHealth;

			//Load all textures, font and sounds
			_healthTexture = Game1.Instance.Content.Load<Texture2D>("Images/normalUsableHeart");
			_coverTexture = Game1.Instance.Content.Load<Texture2D>("Images/usableCoverBox");
			_damagedTexture = Game1.Instance.Content.Load<Texture2D>("Images/damagedTint");
			_scoreFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/ScoreFont");
			_diedSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/poor-baby");

			_injuredSounds = new[] { Game1.Instance.Content.Load<SoundEffect>("Sounds/goddamnit"),
				Game1.Instance.Content.Load<SoundEffect>("Sounds/how-dare-you"),
				Game1.Instance.Content.Load<SoundEffect>("Sounds/im-in-trouble"),
				Game1.Instance.Content.Load<SoundEffect>("Sounds/uh") };

			//Create the rectangle used for drawing hearts (health indicator) on the screen
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
				_drawRedHaze = false;

			//Divide the health into 10 parts, to easily calculate how mnay hearts to draw on the screen
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
					new Vector2((Game1.GameScreenWidth - _coverTexture.Width * Game1.GameScale) / 2f, Game1.GameScreenHeight - _coverTexture.Height * Game1.GameScale),
					_coverTexture.Bounds, Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
			}

			DrawHealth();

			_spriteBatch.DrawString(_scoreFont, Convert.ToString(Score), new Vector2(Game1.GameScreenWidth - 100 * Game1.GameScale, 20 * Game1.GameScale), Color.Black, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);

			WeaponHandler.Instance.Draw();

			if (_drawRedHaze) _spriteBatch.Draw(_damagedTexture, Vector2.Zero, _damagedTexture.Bounds, Color.White, 0f, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
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

			_health -= damage;

			if (_health <= 0)
				PlayerState = State.Dead;
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
					new Vector2(Game1.GameScreenWidth - ((_widthOfHeart + 4) * 5 * Game1.GameScale) + ((_widthOfHeart + 5) * _heartsDrawShift * Game1.GameScale), Game1.GameScreenHeight - _healthTexture.Height * Game1.GameScale),
					_fullHealthSource, Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
				_heartsDrawShift++;
			}

			//Always draw 5 hearts. If the int division ends up removing the fraction (meaning we lose 0.5 hearts), draw a half-heart
			if (_healthIn10 % 2 != 0)
			{
				_spriteBatch.Draw(_healthTexture,
					new Vector2(Game1.GameScreenWidth - ((_widthOfHeart + 4) * 5 * Game1.GameScale) + ((_widthOfHeart + 5) * _heartsDrawShift * Game1.GameScale), Game1.GameScreenHeight - _healthTexture.Height * Game1.GameScale),
					_halfHealthSource, Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
				_heartsDrawShift++;
			}

			for (int i = _healthIn10 / 2; i < 5; i++)
			{
				_spriteBatch.Draw(_healthTexture,
					new Vector2(Game1.GameScreenWidth - ((_widthOfHeart + 4) * 5 * Game1.GameScale) + ((_widthOfHeart + 5) * _heartsDrawShift * Game1.GameScale), Game1.GameScreenHeight - _healthTexture.Height * Game1.GameScale),
					_emptyHealthSource, Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
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
			_health = MaxHealth;
			Score = 0;

			PlayerState = State.Alive;

			WeaponHandler.Instance.ResetWeapons();
		}
	}
}