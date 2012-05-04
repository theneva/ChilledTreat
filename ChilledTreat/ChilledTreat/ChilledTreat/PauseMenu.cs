using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ChilledTreat
{
	class PauseMenu : GameState
	{
		// Fields
		InputHandler _input = InputHandler.Instance;
		SpriteFont _menuFont;
		Color _fontColor;

		// Constructor
		public PauseMenu(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			_menuFont = content.Load<SpriteFont>("fonts/menuFont");
		}

		// Methods
		public override void Draw()
		{
			
		}

		public override void Update()
		{
			
		}
	}
}
