using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChilledTreat.GameClasses;
using ChilledTreat.GameStates;

#if XBOX
using Microsoft.Xna.Framework.GamerServices;
#endif

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

#if XBOX
			Components.Add(new GamerServicesComponent(this));
#endif

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


			// GameStates added
			_gameStates.Add(new Splash());
			_gameStates.Add(new Menu());
			_gameStates.Add(new Credits());
			_gameStates.Add(new PauseMenu());
			_gameStates.Add(new GameOver());
			_gameStates.Add(new Instructions());
			_gameStates.Add(new LeaderBoard());
		   
			// Splashscreen state set as activegamestate at startup
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
			// GameTime updated in frameinfo
			_frameInfo.GameTime = gameTime;

			// InputHandler updated to handle input 
			_inputHandler.Update();

			// Used to change between gamestates
			if (_nextState != null)
			{
				_activeGameState = _nextState;
				_nextState = null;
			}
			// Update method called in the activegamestate
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

			// Draw method in the activegamestate called
			_activeGameState.Draw();

			SpriteBatch.End();

			base.Draw(gameTime);
		}

		// Method used to change to another gamestate
		public static void ChangeState(int index)
		{
			//If the gamestate you're changing to is NOT ingame, stop vibrating (This is to avoid testing it every other place)
			if(index != GameState.InGame)
				InputHandler.Instance.StopVibrate();

			if (index < Instance._gameStates.Count)
			{
				Instance._nextState = Instance._gameStates[index];
			}
		}

		// Add a new instance of the InGame class.
		// When a new game is started the last instance
		// of the InGame class is replaced by a new one, and
		// the player and enemyhandler is reset.
		public static void NewGame()
		{
			if (Instance._gameStates.Count < GameState.InGame + 1)
			{
				Instance._gameStates.Add(new InGame());
			}
			else
			{
				Instance._gameStates[GameState.InGame] = new InGame();
			}

			Player.ResetPlayer();
			EnemyHandler.ResetEnemyHandler();
			NewGameOver();
			
		}

		// Used to reset Credits
		public static void NewCredits()
		{
			Instance._gameStates[GameState.Credits] = new Credits();
		}
		public static void NewGameOver()
		{
			Instance._gameStates[GameState.GameOver] = new GameOver();
		}
	}
}