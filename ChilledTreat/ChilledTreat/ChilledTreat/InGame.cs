using ChilledTreat.GameClasses;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat
{
	class InGame : GameState
	{
		// Fields
		Player player;
		Enemy enemy;
		InputHandler input = InputHandler.Instance;

		// Constructor
		public InGame(SpriteBatch spriteBatch, ContentManager content)
			: base (spriteBatch, content)
		{
			// CONTENT LOAD
			player = new Player(spriteBatch, content);
			enemy = new Enemy(spriteBatch, content);
			
		}

		// Methods

		public override void Update()
		{
			// LOGIC
			player.Update();
			enemy.Update();
			if (input.IsAbortPressed())
			{
				Game1.ChangeState(4);
			}
		}

		public override void Draw()
		{
			// DRAW THAT SHIT
			player.Draw();
			enemy.Draw();
		}
	}
}