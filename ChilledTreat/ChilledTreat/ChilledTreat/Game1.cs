// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Game1.cs" company="X&A Team">
//   Copyright (c) X&A Team. All rights reserved
// </copyright>
// <summary>
//   This is the main type for your game
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat
{
    using System.Collections.Generic;

    using ChilledTreat.GameClasses;
    using ChilledTreat.GameStates;

    using Microsoft.Xna.Framework;
#if XBOX
    using Microsoft.Xna.Framework.GamerServices;
#endif
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        #region Fields
        /// <summary>
        /// This is where the user can set their own GameScreen Width.
        /// The aspect of this vs the height should be 16:9, or very close, for the scale to work as it should
        /// </summary>
        public const int GameScreenWidth = 1280; // Game1.Instance.GraphicsDevice.DisplayMode.Width;

        /// <summary>
        /// This is where the user can set their own GameScreen Height.
        /// The aspect of this vs the width should be 16:9, or very close, for the scale to work as it should
        /// </summary>
        public const int GameScreenHeight = 720; // Game1.Instance.GraphicsDevice.DisplayMode.Height;

        /// <summary>
        /// This scale is used to scale everything drawn if the user changes the height and width of the game for the original
        /// </summary>
        public const float GameScale = ((GameScreenWidth / 1280f) + (GameScreenHeight / 720f)) / 2f;

        /// <summary>
        /// This is the static instance of the game
        /// </summary>
        private static Game1 instance;

        /// <summary>
        /// This gets the current frameinfo
        /// </summary>
        private readonly FrameInfo frameInfo = FrameInfo.Instance;

        /// <summary>
        /// This gets the current input-handler
        /// </summary>
        private readonly InputHandler inputHandler = InputHandler.Instance;

        /// <summary>
        /// This is a list of all the gamestates available
        /// </summary>
        private readonly List<GameState> gameStates = new List<GameState>();

        /// <summary>
        /// This is the active gamestate
        /// </summary>
        private GameState activeGameState;

        /// <summary>
        /// This is the nextstate, the state waiting to be applied
        /// </summary>
        private GameState nextState;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Game1"/> class.
        /// </summary>
        public Game1()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.Graphics.PreferredBackBufferWidth = GameScreenWidth;
            this.Graphics.PreferredBackBufferHeight = GameScreenHeight;

            // Graphics.IsFullScreen = true;
#if XBOX
            Components.Add(new GamerServicesComponent(this));
#endif

            IsMouseVisible = true;

            instance = this;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the instance of Game1
        /// </summary>
        public static Game1 Instance
        {
            get { return instance ?? (instance = new Game1()); }
        }

        /// <summary>
        /// Gets the Sprite-batch
        /// </summary>
        public SpriteBatch SpriteBatch { get; private set; }

        /// <summary>
        /// Gets the graphics-device
        /// </summary>
        public GraphicsDeviceManager Graphics { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// This method changes the current gamestate
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public static void ChangeState(int index)
        {
            // If the gamestate you're changing to is NOT ingame, stop vibrating (This is to avoid testing it every other place)
            if (index != GameState.InGame)
            {
                InputHandler.Instance.StopVibrate();
            }

            if (index < instance.gameStates.Count)
            {
                instance.nextState = instance.gameStates[index];
            }
        }

        /// <summary> 
        /// Add a new instance of the InGame class.
        /// When a new game is started the last instance of the InGame class is replaced by a new one, and
        /// the player and enemyhandler is reset.
        /// </summary>
        public static void NewGame()
        {
            instance.gameStates[GameState.InGame] = new InGame();
            Player.ResetPlayer();
            EnemyHandler.ResetEnemyHandler();
            NewGameOver();
        }

        /// <summary>
        /// Used to reset Credits
        /// </summary>
        public static void NewCredits()
        {
            instance.gameStates[GameState.Credits] = new Credits();
        }

        /// <summary>
        /// Used to reset LeaderBoard
        /// </summary>
        public static void NewLeaderBoard()
        {
            instance.gameStates[GameState.LeaderBoard] = new LeaderBoard();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);

            // GameStates added
            this.gameStates.Add(new Splash());
            this.gameStates.Add(new Menu());
            this.gameStates.Add(new Credits());
            this.gameStates.Add(new PauseMenu());
            this.gameStates.Add(new GameOver());
            this.gameStates.Add(new Instructions());
            this.gameStates.Add(new LeaderBoard());
            this.gameStates.Add(new InGame());

            // Splashscreen state set as activegamestate at startup
            this.activeGameState = this.gameStates[GameState.Splash];
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
            this.frameInfo.GameTime = gameTime;

            // InputHandler updated to handle input 
            this.inputHandler.Update();

            // Used to change between gamestates
            if (this.nextState != null)
            {
                this.activeGameState = this.nextState;
                this.nextState = null;
            }

            // Update method called in the activegamestate
            this.activeGameState.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            this.SpriteBatch.Begin();

            // Draw method in the activegamestate called
            this.activeGameState.Draw();

            this.SpriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Used to reset GameOver
        /// </summary>
        private static void NewGameOver()
        {
            instance.gameStates[GameState.GameOver] = new GameOver();
        }
        #endregion
    }
}