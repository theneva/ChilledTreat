using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChilledTreat.GameStates
{
	class GameState
	{
		protected SpriteBatch SpriteBatch;
		protected ContentManager Content;

		//_gameStates.Add(new Splash(SpriteBatch, Content, 1));
		//    _gameStates.Add(new Menu(SpriteBatch, Content));
		//    _gameStates.Add(new Credits(SpriteBatch, Content));
		//    _gameStates.Add(new PauseMenu(SpriteBatch, Content));
        // What is this^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
		protected static int Splash = 0, Menu = 1, Credits = 2, PauseMenu = 3;


		public GameState(SpriteBatch spriteBatch, ContentManager content)
		{
			SpriteBatch = spriteBatch;
			Content = content;
		}

		public virtual void Update() { }
		public virtual void Draw() { }
	}
}
