using System.Collections.Generic;
using ChilledTreat.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChilledTreat.GameClasses;

namespace ChilledTreat
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		public static Game1 Instance;

		// public for å kunne brukes andre steder
		public readonly GraphicsDeviceManager Graphics;
		public SpriteBatch SpriteBatch;

		// It has to be 16:9, or very close, for the scale to work as it should
		public const int GameScreenWidth = 1280; //Game1.Instance.GraphicsDevice.DisplayMode.Width;
		public const int GameScreenHeight = 720; //Game1.Instance.GraphicsDevice.DisplayMode.Height;

		//This scale is used to scale everything drawn if the user changes the height and width of the game for the original
		public const float GameScale = (GameScreenWidth/1280f + GameScreenHeight/720f)/2f;

		readonly FrameInfo _frameInfo = FrameInfo.Instance;
		readonly InputHandler _inputHandler = InputHandler.Instance;

		readonly List<GameState> _gameStates = new List<GameState>();
		GameState _activeGameState;

		GameState _nextState;

		public Game1()
		{
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			Graphics.PreferredBackBufferWidth = GameScreenWidth;
			Graphics.PreferredBackBufferHeight = GameScreenHeight;

			//Graphics.IsFullScreen = true;

			IsMouseVisible = true;

			Instance = this;
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			SpriteBatch = new SpriteBatch(GraphicsDevice);

			// Legge til gamestates som klasser i listen
			// ex: _gameStates.add(new InGame(Sprite...., con);

			_gameStates.Add(new Splash());
			_gameStates.Add(new Menu());
			_gameStates.Add(new Credits());
			_gameStates.Add(new PauseMenu());
			_gameStates.Add(new GameOver());
			_gameStates.Add(new Instructions());
			_gameStates.Add(new LeaderBoard());
		   

			_activeGameState = _gameStates[GameState.Splash];
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();

			_frameInfo.GameTime = gameTime;

			_inputHandler.Update();

			if (_nextState != null)
			{
				_activeGameState = _nextState;
				_nextState = null;
			}

			_activeGameState.Update();


			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			
			SpriteBatch.Begin();

			_activeGameState.Draw();

			SpriteBatch.End();

			base.Draw(gameTime);
		}

		public static void ChangeState(int index)
		{
			if (index < Instance._gameStates.Count)
			{
				Instance._nextState = Instance._gameStates[index];
			}
		}

		// Kun InGame gamestatet som skal ha en metode som dette
		// ---- Den lager nytt InGame object når du starter på ny
		public static void NewGame()
		{
			if (Instance._gameStates.Count < GameState.InGame + 1)
			{
				Instance._gameStates.Add(new InGame(Instance.SpriteBatch, Instance.Content));
			}
			else
			{
				Instance._gameStates[GameState.InGame] = new InGame(Instance.SpriteBatch, Instance.Content);
			}

			Player.Instance.ResetPlayer();
			EnemyHandler.Instance.ResetEnemyHandler();
			
		}
		public static void NewCredits()
		{
			Instance._gameStates[GameState.Credits] = new Credits();
		}
	}
}