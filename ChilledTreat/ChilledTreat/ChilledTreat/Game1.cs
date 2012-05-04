using System.Collections.Generic;
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
		public static Game1 Instance = null;

		// public for å kunne brukes andre steder
		public GraphicsDeviceManager Graphics;
		public SpriteBatch SpriteBatch;

		public int GameScreenWidth = 1280;
		public int GameScreenHeight = 720;

		readonly FrameInfo _frameInfo = FrameInfo.Instance;
		readonly InputHandler _inputHandler = InputHandler.Instance;

		readonly List<GameState> _gameStates = new List<GameState>();
		GameState _activeGameState;

		GameState _nextState;

        Enemy testEnemy;

		public static void ChangeState(int index)
		{
			if (index < Instance._gameStates.Count)
			{
				Instance._nextState = Instance._gameStates[index];
			}
		}

		public Game1()
		{
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			Graphics.PreferredBackBufferWidth = GameScreenWidth;
			Graphics.PreferredBackBufferHeight = GameScreenHeight;

			Instance = this;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

            testEnemy = new Enemy(SpriteBatch, Content);

			base.Initialize();
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
			// ex: GameStates.add(new InGame(Sprite...., con);

			_gameStates.Add(new Splash(SpriteBatch, Content, 1));
			_gameStates.Add(new Menu(SpriteBatch, Content));
            //_gameStates.Add(new Credits(SpriteBatch, Content, 1));
			_gameStates.Add(new InGame(SpriteBatch, Content));
			_gameStates.Add(new PauseMenu(SpriteBatch, Content));

			_activeGameState = _gameStates[0];
			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
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
				this.Exit();

			// TODO: Add your update logic here

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
			// TODO: Add your drawing code here

			GraphicsDevice.Clear(Color.WhiteSmoke);

			SpriteBatch.Begin();

			_activeGameState.Draw();
            
            testEnemy.Draw();

			SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
