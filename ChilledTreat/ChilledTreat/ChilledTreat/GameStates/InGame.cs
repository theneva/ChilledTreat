using ChilledTreat.GameClasses;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameStates
{
	class InGame : GameState
	{
		// Fields
		readonly Player _player;
		readonly EnemyHandler _enemies = EnemyHandler.Instance;
		readonly InputHandler _input = InputHandler.Instance;

		// Constructor
		public InGame(SpriteBatch spriteBatch, ContentManager content)
			: base (spriteBatch, content)
		{
			// CONTENT LOAD
			_player = new Player(spriteBatch, content);
			_enemies.Add(new Enemy(spriteBatch, content));

			Game1.Instance.IsMouseVisible = false;
		}

		// Methods

		public override void Update()
		{
			// LOGIC
			_player.Update();
			_enemies.Update();

			if (_input.IsAbortPressed() || Game1.Instance.IsActive == false)
			{
				Game1.Instance.IsMouseVisible = true;
				//Game1.ChangeState(3); VS:
				Game1.ChangeState(GameState.PauseMenu);
			}
		}

		public override void Draw()
		{
			// DRAW THAT SHIT
			_enemies.Draw();
			_player.Draw();
		}
	}
}