using ChilledTreat.GameClasses;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat
{
	class InGame : GameState
	{
		// Fields
		Player player;

		// Constructor
		public InGame(SpriteBatch spriteBatch, ContentManager content)
			: base (spriteBatch, content)
		{
			// CONTENT LOAD
			player = new Player(spriteBatch, content);
		}

		// Methods

		public override void Update()
		{
			// LOGIC
			player.Update();
		}

		public override void Draw()
		{
			// DRAW THAT SHIT
			player.Draw();
		}
	}
}
