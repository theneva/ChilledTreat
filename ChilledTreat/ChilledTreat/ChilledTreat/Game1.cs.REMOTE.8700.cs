using System.Collections.Generic;
using ChilledTreat.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChilledTreat
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		public static Game1 Instance;

		// public for å kunne brukes andre steder
		public GraphicsDeviceManager Graphics;
		public SpriteBatch SpriteBatch;

		public int GameScreenWidth = 800;
		public int GameScreenHeight = 600;

		readonly FrameInfo _frameInfo = FrameInfo.Instance;
		readonly InputHandler _inputHandler = InputHandler.Instance;

		readonly List<GameState> _gameStates = new List<GameState>();
		GameState _activeGameState;

		GameState _nextState;


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
			// ex: GameStates.add(new InGame(Sprite...., con);
			
			_gameStates.Add(new Splash(SpriteBatch, Content, 1));
			_gameStates.Add(new Menu(SpriteBatch, Content));
			_gameStates.Add(new Credits(SpriteBatch, Content));
			_gameStates.Add(new PauseMenu(SpriteBatch, Content));

			_activeGameState = _gameStates[0];
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
			GraphicsDevice.Clear(Color.WhiteSmoke);

			SpriteBatch.Begin();

			_activeGameState.Draw();

			SpriteBatch.End();

			base.Draw(gameTime);
		}

		public static void NewGame() 
		{
			if (Instance._gameStates.Count < 5)
			{
				Instance._gameStates.Add(new InGame(Instance.SpriteBatch, Instance.Content));
			}
			else
			{
				Instance._gameStates[4] = new InGame(Instance.SpriteBatch, Instance.Content);
			}
		}
		public static void CreditsScreen()
		{
			if (Instance._gameStates.Count < 5)
			{
				Instance._gameStates.Add(new Credits(Instance.SpriteBatch, Instance.Content));
			}
			else
			{
				Instance._gameStates[4] = new Credits(Instance.SpriteBatch, Instance.Content);
			}
		}
	}
}