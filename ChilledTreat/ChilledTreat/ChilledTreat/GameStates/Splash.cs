using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameStates
{
	class Splash : GameState
	{
		readonly Texture2D splash;
		public Splash()
		{
			// LOAD CONTENT
			splash = Game1.Instance.Content.Load<Texture2D>("Images/chilledTreat");
		}		
		public override void Update()
		{
			InputHandler input = InputHandler.Instance;

			if (input.IsActionPressed())
				Game1.ChangeState(Menu);
		}

		public override void Draw()
		{
			Game1.Instance.SpriteBatch.Draw(splash, Vector2.Zero, Color.White);
		}
	}
}
