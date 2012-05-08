using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChilledTreat.GameStates
{
	class GameState
	{
		protected SpriteBatch SpriteBatch;
		protected ContentManager Content;

		protected static int Splash = 0;
		protected internal static int Menu = 1, InGame = 6, GameOver = 4;
		protected static int Credits = 2, PauseMenu = 3, Instructions = 5;

		public GameState(SpriteBatch spriteBatch, ContentManager content)
		{
			SpriteBatch = spriteBatch;
			Content = content;
		}

		public virtual void Update() { }
		public virtual void Draw() { }
	}
}
