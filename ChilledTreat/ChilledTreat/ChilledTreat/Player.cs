using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ChilledTreat
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	class Player
	{
		int _health, _ammo;
		MouseState _currentMouseState;
		Texture2D _reticuleTexture;
		SpriteBatch sp;
		InputHandler input = InputHandler.Instance;
		Vector2 halfTexture;
		

		private Vector2 ReticulePosition { get; set; }

		public Player(SpriteBatch spriteBatch, ContentManager content)
		{
			_health = 100;
			_ammo = 10;
			_reticuleTexture = content.Load<Texture2D>("reticule");
			sp = spriteBatch;
			halfTexture = new Vector2(_reticuleTexture.Width / 2, _reticuleTexture.Height / 2);
		}


		public void Update()
		{
			ReticulePosition = new Vector2(input.MouseState.X, input.MouseState.Y) - halfTexture;
		}

		public void Draw()
		{
			sp.Draw(_reticuleTexture, ReticulePosition, Color.White);
			
			
		}
	}
}
