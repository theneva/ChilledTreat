
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace ChilledTreat
{
	class Credits : GameState
	{
		// Fields
		int nextState;
		InputHandler input = InputHandler.Instance;

		// Constructor
		public Credits(SpriteBatch spriteBatch, ContentManager content, int NextState)
			: base(spriteBatch, content)
		{
			nextState = NextState;
		}

		// Methods

		public override void Update()
		{
			// Logic
			if (input.IsAbortPressed()) 
			{
				Game1.ChangeState(nextState);
			}
		}

		public override void Draw()
		{
			// DRAW!!!! LåååL
		}


	}
}
