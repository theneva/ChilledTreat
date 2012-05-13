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
		readonly string[] _menuItems;
		readonly float[] _selectedItem, _yPos;
		readonly SoundEffect _buttonSound, _selectSound;
		int _menuPos;

		// Constructor
		public PauseMenu()
		{
			_menuFont = Game1.Instance.Content.Load<SpriteFont>("fonts/menuFont");
			string[] strings = { "Resume Game", "Main Menu" };
			_menuItems = strings;
			_selectedItem = new float[_menuItems.Length];
			_menuPos = 0;
			_buttonSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/buttonSound");
			_selectSound = Game1.Instance.Content.Load<SoundEffect>("Sounds/selectSound");

			for (int i = 0; i < _selectedItem.Length; i++)
			{
				_selectedItem[i] = 100f * Game1.GameScale;
			}
			_selectedItem[_menuPos] = 150f * Game1.GameScale;

			float yStartPos = 100f;
			_yPos = new float[_menuItems.Length];
			for (int i = 0; i < _yPos.Length; i++)
			{
				_yPos[i] = yStartPos;
				yStartPos += 100f * Game1.GameScale;
			}
		}

		// Methods
		public override void Update()
		{
			#region Navigation Logic
			// Same logic as used in the Menu class
			if (_input.IsDownPressed())
			{
				_buttonSound.Play();
				_menuPos++;
				if (_menuPos > _selectedItem.Length - 1) _menuPos = 0;
			}
			if (_input.IsUpPressed())
			{
				_buttonSound.Play();
				_menuPos--;
				if (_menuPos < 0) _menuPos = _selectedItem.Length - 1;
			}
			for (int i = 0; i < _selectedItem.Length; i++) _selectedItem[i] = 100f * Game1.GameScale;
			_selectedItem[_menuPos] = 150f * Game1.GameScale;

			if (_input.IsPausePressed() || (_input.IsAbortPressed()))
			{
				_selectSound.Play();
				Game1.ChangeState(InGame);
				Game1.Instance.IsMouseVisible = false;
			}

			if (!_input.IsActionPressed()) return;

			if (_menuItems[_menuPos].Contains("Main Menu"))
			{
				_selectSound.Play();
				Player.ResetPlayer();
				EnemyHandler.Instance.Clear();
				Game1.ChangeState(Menu);
			} 
			else if (_menuItems[_menuPos].Contains("Resume Game"))
			{
				_selectSound.Play();
				Game1.ChangeState(InGame);
				Game1.Instance.IsMouseVisible = false;
			}
			#endregion
		}

		public override void Draw()
		{
			for (int i = 0; i < _menuItems.Length; i++)
				Game1.Instance.SpriteBatch.DrawString(_menuFont, _menuItems[i], new Vector2(_selectedItem[i], _yPos[i]), Color.White, 0, Vector2.Zero, Game1.GameScale, SpriteEffects.None, 0);
		}
	}
}
