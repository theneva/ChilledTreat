using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChilledTreat.GameStates
{
	class GameState
	{
		protected SpriteBatch SpriteBatch;
		protected ContentManager Content;

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
