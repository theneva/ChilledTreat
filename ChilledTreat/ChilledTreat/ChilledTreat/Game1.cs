﻿using System.Collections.Generic;
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
		public static Game1 Instance = null;

		// public for å kunne brukes andre steder
		public GraphicsDeviceManager Graphics;
		public SpriteBatch SpriteBatch;

		readonly FrameInfo _frameInfo = FrameInfo.Instance;
		readonly InputHandler _inputHandler = InputHandler.Instance;

		readonly List<GameState> _gameStates = new List<GameState>();
		GameState _activeGameState = null;

		GameState _nextState = null;

        Sprite background1;

		public static void ChangeState(int index)
		{
			if (index < Game1.Instance._gameStates.Count)
			{
				Game1.Instance._nextState = Game1.Instance._gameStates[index];
			}
		}

		public Game1()
		{
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			Graphics.PreferredBackBufferWidth = 1280;
			Graphics.PreferredBackBufferHeight = 720;

			IsMouseVisible = true;

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
            background1 = new Sprite();
            background1.Scale = 1.0f;


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

			_activeGameState = _gameStates[0];

            background1.LoadContent(this.Content, "backgroundLevel1");
            background1.Position = new Vector2(0, 0);
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
            Vector2 aDirection = new Vector2(1, 0);
            Vector2 aSpeed = new Vector2(180, 0);

            background1.Position += (aDirection.X) * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

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

			GraphicsDevice.Clear(Color.HotPink);

			SpriteBatch.Begin();

			_activeGameState.Draw();

            background1.Draw(SpriteBatch);

			SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
