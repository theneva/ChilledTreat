using Microsoft.Xna.Framework;

namespace ChilledTreat.GameStates
{
	class Splash : GameState
	{
		public Splash()
		{
			// LOAD CONTENT
		}		
		public override void Update()
		{
			InputHandler input = InputHandler.Instance;

			if (input.IsActionPressed())
				Game1.ChangeState(Menu);
		}

		public override void Draw()
		{
			Game1.Instance.GraphicsDevice.Clear(Color.Salmon);
		}

	}
}
