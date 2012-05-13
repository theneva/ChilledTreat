using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ChilledTreat.GameStates
{
	class Menu : GameState
	{
		// Fields
		readonly SpriteFont _menuFont;
		readonly Color _fontColor;
		readonly string[] _menuItems;
		readonly float[] _selectedItem;
		readonly int[] _yPos;
		private int _menuPos;
		readonly InputHandler _input = InputHandler.Instance;
		readonly SoundEffect _menuSound, _selectSound;

		// Constructor
		public Menu()
		{
			// Menu content
			_menuFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/menuFont");
			_fontColor = Color.Salmon;

			// Array with menu titles
			string[] strings = {"New Game", "Instructions", "Leaderboard", "Credits", "EXIT"};
			_menuItems = strings;
			
			_menuPos = 0; // Keeps track on where in the menu the user is
			_selectedItem = new float[_menuItems.Length]; // Used to set a specific position for the menu item which is selected

			// Navigation sounds
			_menuSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/buttonSound");
			_selectSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/selectSound");

			// For loops sets X position on the menu items
			for (int i = 0; i < _selectedItem.Length; i++)
				_selectedItem[i] = 100f;

			// The selected item (menuPos keeps track on which item is selcted)
			// sets X positon on the selectedItem
			_selectedItem[_menuPos] = 150f;

			// Sets the Y position from which the menu will be placed
			int yStartPos = 100;

			// An array holds all the Y position of all the menu items.
			// The array is filled by a for loop which fills it according
			// to the numbers of menuItems
			_yPos = new int[_menuItems.Length];
			for (int i = 0; i < _yPos.Length; i++)
			{
				_yPos[i] = yStartPos;
				yStartPos += 100;
			}
		}

		public override void Update()
		{
			// Navigation
			if (_input.IsDownPressed())
			{
				_menuSound.Play();
				_menuPos++;
				if (_menuPos > _selectedItem.Length - 1)
					_menuPos = 0;
			}
			if (_input.IsUpPressed())
			{
				_menuSound.Play();
				_menuPos--;
				if (_menuPos < 0)
					_menuPos = _selectedItem.Length - 1;
			}

			// The selectedItem's position is set with the menuPos
			for (int i = 0; i < _selectedItem.Length; i++)
				_selectedItem[i] = 100f;
			_selectedItem[_menuPos] = 150f;

			// The method returns when the used does not choose a menuItem
			if (!_input.IsActionPressed()) return;

			// GameState is changed according to what the user selects.
			_selectSound.Play();
			if (_menuItems[_menuPos].Contains("EXIT"))
				Game1.Instance.Exit();
			else if (_menuItems[_menuPos].Contains("New Game"))
			{
				Game1.NewGame();
				Game1.ChangeState(InGame);
			}
			else if (_menuItems[_menuPos].Contains("Instructions"))
				Game1.ChangeState(Instructions);
			else if (_menuItems[_menuPos].Contains("Credits"))
			{
				Game1.NewCredits();
				Game1.ChangeState(Credits);
			}
			else if (_menuItems[_menuPos].Contains("Leaderboard"))
				Game1.ChangeState(LeaderBoard);
		}
		public override void Draw()
		{
			for (int i = 0; i < _menuItems.Length; i++)
				Game1.Instance.SpriteBatch.DrawString(_menuFont, _menuItems[i], new Vector2(_selectedItem[i], _yPos[i]), _fontColor);
		}
	}
}
