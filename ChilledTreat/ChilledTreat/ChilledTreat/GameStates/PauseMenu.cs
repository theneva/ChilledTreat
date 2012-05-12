using ChilledTreat.GameClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ChilledTreat.GameStates
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
		readonly SoundEffect _buttonSound, _selectSound;
		int _menuPos;

		// Constructor
		public PauseMenu()
		{
			_menuFont = Game1.Instance.Content.Load<SpriteFont>("fonts/menuFont");
			_fontColor = Color.Salmon;
			string[] strings = { "Resume Game", "Main Menu" };
			_menuItems = strings;
			_selectedItem = new float[_menuItems.Length];
			_menuPos = 0;
			_buttonSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/buttonSound");
			_selectSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/selectSound");

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
			if (_input.IsDownPressed())
			{
				_menuPos++;
				if (_menuPos > _selectedItem.Length - 1) _menuPos = 0;
			}
			if (_input.IsUpPressed())
			{
				_menuPos--;
				if (_menuPos < 0) _menuPos = _selectedItem.Length - 1;
			}
			for (int i = 0; i < _selectedItem.Length; i++) _selectedItem[i] = 100f;
			_selectedItem[_menuPos] = 150f;

			if (_input.IsPausePressed() || (_input.IsAbortPressed()))
			{
				Game1.ChangeState(InGame);
				Game1.Instance.IsMouseVisible = false;
			}

			if (!_input.IsActionPressed()) return;
			if (_menuItems[_menuPos].Contains("Main Menu"))
			{
				Player.ResetPlayer();

				EnemyHandler.Instance.Clear();
				Game1.ChangeState(Menu);
			} 
			else if (_menuItems[_menuPos].Contains("Resume Game"))
			{
				Game1.ChangeState(InGame);
				Game1.Instance.IsMouseVisible = false;
			}
		}

		public override void Draw()
		{
			for (int i = 0; i < _menuItems.Length; i++)
				Game1.Instance.SpriteBatch.DrawString(_menuFont, _menuItems[i], new Vector2(_selectedItem[i], _yPos[i]), _fontColor);
		}
	}
}
