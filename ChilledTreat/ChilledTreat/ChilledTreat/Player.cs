using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChilledTreat
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class Player : DrawableGameComponent
	{
		int _health, _ammo;
		MouseState _currentMouseState;
		Texture2D _reticuleTexture;

		private Vector2 ReticulePosition { get; set; }

		public Player(Game game)
			: base(game)
		{
			_health = 100;
			_ammo = 10;
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public override void Initialize()
		{
			_reticuleTexture = Game.Content.Load<Texture2D>("reticule");

			base.Initialize();
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			_currentMouseState = Mouse.GetState();

			ReticulePosition = new Vector2(_currentMouseState.X, _currentMouseState.Y);

			base.Update(gameTime);
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_reticuleTexture, ReticulePosition, Color.White);

			base.Draw(gameTime);
		}
	}
}
