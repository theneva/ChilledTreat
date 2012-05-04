using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ChilledTreat
{
	class PauseMenu : GameState
	{
		// Fields
		readonly InputHandler _input = InputHandler.Instance;
		readonly SpriteFont _menuFont;
		readonly Color _fontColor; 
		readonly string[] _menuItems;
		readonly float[] _selectedItem;
		readonly int[] _yPos;
		int _menuPos;

		// Constructor
		public PauseMenu(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			_menuFont = content.Load<SpriteFont>("fonts/menuFont");
			_fontColor = Color.Salmon;
			string[] strings = { "Resume Game", "Main Menu" };
			_menuItems = strings;
			_selectedItem = new float[_menuItems.Length];
			_menuPos = 0;

			for (int i = 0; i < _selectedItem.Length; i++)
			{
				_selectedItem[i] = 100f;
			}
			_selectedItem[_menuPos] = 150f;

			int yStartPos = 100;
			_yPos = new int[_menuItems.Length];
			for (int i = 0; i < _yPos.Length; i++)
			{
				_yPos[i] = yStartPos;
				yStartPos += 100;
			}
		}

		// Methods
		public override void Update()
		{
			if (_input.IsKeyPressed(_input.DownKey))
			{
				_menuPos++;
				if (_menuPos > _selectedItem.Length - 1) _menuPos = 0;
			}
			if (_input.IsKeyPressed(_input.UpKey))
			{
				_menuPos--;
				if (_menuPos < 0) _menuPos = _selectedItem.Length - 1;
			}
			for (int i = 0; i < _selectedItem.Length; i++) _selectedItem[i] = 100f;
			_selectedItem[_menuPos] = 150f;

			if (_input.IsKeyPressed(_input.ActionKey))
			{
				if (_menuItems[_menuPos].Contains("Main Menu"))
				{
					Game1.ChangeState(1);
				}
			}
		}

		public override void Draw()
		{
			for (int i = 0; i < _menuItems.Length; i++)
			{
				SpriteBatch.DrawString(_menuFont, _menuItems[i], new Vector2(_selectedItem[i], _yPos[i]), _fontColor);

			}
		}
	}
}
