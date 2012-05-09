using System;
using Microsoft.Xna.Framework;
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

		private int _health, _healthIn10, _heartsDrawShift;
		private readonly int _widthOfHeart;
		private readonly Texture2D _healthTexture, _coverTexture;
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
			_scoreFont = content.Load<SpriteFont>("Fonts/ScoreFont");

			_widthOfHeart = _healthTexture.Width / 3;
			_fullHealthSource = new Rectangle(0, 0, _widthOfHeart, _healthTexture.Height);
			_halfHealthSource = new Rectangle(_widthOfHeart, 0, _widthOfHeart, _healthTexture.Height);
			_emptyHealthSource = new Rectangle(_widthOfHeart * 2, 0, _widthOfHeart, _healthTexture.Height);
			PlayerState = State.Alive;
		}

		public void Update()
		{
			if (PlayerState == State.Dead)
			{
				EnemyHandler.Instance.Clear();
				Game1.ChangeState(GameStates.GameState.GameOver);
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
			if(InCover)
			{
				_spriteBatch.Draw(_coverTexture,
					new Vector2((Game1.Instance.GameScreenWidth - _coverTexture.Width) / 2f, Game1.Instance.GameScreenHeight - _coverTexture.Height),
					Color.White);
			}

			DrawHealth();

			_spriteBatch.DrawString(_scoreFont, Convert.ToString(Score), new Vector2(Game1.Instance.GameScreenWidth - 100, 20), Color.Black);

			WeaponHandler.Instance.Draw();
		}
	
		/// <summary>
		/// How much the player is damaged
		/// </summary>
		/// <param name="damage">The amount of damage recieved</param>
		public void Damaged(int damage)
		{
			if (PlayerState != State.Reloading) PlayerState = State.Damaged;
			if (InCover) damage /= 5;


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