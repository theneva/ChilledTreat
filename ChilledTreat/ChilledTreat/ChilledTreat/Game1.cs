using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ChilledTreat
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 Instance = null;

        // public for å kunne brukes andre steder
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        FrameInfo FrameInfo = FrameInfo.Instance;
        InputHandler InputHandler = InputHandler.Instance;

        List<GameState> GameStates = new List<GameState>();
        GameState ActiveGameState = null;

        GameState NextState = null;

        public static void ChangeState(int index)
        {
            if (index < Game1.Instance.GameStates.Count)
            {
                Game1.Instance.NextState = Game1.Instance.GameStates[index];
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Legge til gamestates som klasser i listen
            // ex: GameStates.add(new InGame(Sprite...., con);

            GameStates.Add(new Splash(spriteBatch, Content, "splashScreen/sc2", 1));
            GameStates.Add(new Splash(spriteBatch, Content, "splashScreen/splashScreen", 0));
            // GameStates.Add(new InGame(spriteBatch, Content));

            ActiveGameState = GameStates[0];
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

            FrameInfo.GameTime = gameTime;

            InputHandler.Update();

            if (NextState != null)
            {
                ActiveGameState = NextState;
                NextState = null;
            }

            ActiveGameState.Update();


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            spriteBatch.Begin();

            ActiveGameState.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
