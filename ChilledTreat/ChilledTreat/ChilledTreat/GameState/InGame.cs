using ChilledTreat.GameClasses;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat
{
	class InGame : GameState
	{
		// Fields
		readonly Player _player;
		readonly Enemy _enemy;
		readonly InputHandler _input = InputHandler.Instance;

		// Constructor
		public InGame(SpriteBatch spriteBatch, ContentManager content)
			: base (spriteBatch, content)
		{
			// CONTENT LOAD
			_player = new Player(spriteBatch, content);
			_enemy = new Enemy(spriteBatch, content);

			Game1.Instance.IsMouseVisible = false;
		}

		// Methods

		public override void Update()
		{
			// LOGIC
			_player.Update();
			_enemy.Update();

			if (_input.IsAbortPressed() || Game1.Instance.IsActive == false)
			{
				Game1.Instance.IsMouseVisible = true;
				Game1.ChangeState(3);
			}
		}

		public override void Draw()
		{
			// DRAW THAT SHIT
			_enemy.Draw();
			_player.Draw();
		}
	}
}