using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ChilledTreat.GameStates
{
	class Menu : GameState
	{
		readonly SpriteFont _menuFont;
		readonly Color _fontColor;
		readonly string[] _menuItems;
		readonly float[] _selectedItem;
		readonly int[] _yPos;
		int _menuPos;
		readonly InputHandler _input = InputHandler.Instance;

		public Menu(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// Menu content
			_menuFont = Content.Load<SpriteFont>("Fonts/menuFont");
			_fontColor = Color.Salmon;
			string[] strings = {"New Game", "Instructions", "Leaderboard", "Credits", "EXIT"};
			_menuItems = strings;
			_menuPos = 0;
			_selectedItem = new float[_menuItems.Length];
			
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

			if (_input.IsActionPressed())
			{
				if (_menuItems[_menuPos].Contains("EXIT"))
				{
					Game1.Instance.Exit();
				}
				else if (_menuItems[_menuPos].Contains("New Game"))
				{
					Game1.NewGame();
					Game1.ChangeState(GameState.InGame);
				}
				else if (_menuItems[_menuPos].Contains("Instructions"))
				{
                    Game1.ChangeState(GameState.Instructions);
				}
				else if (_menuItems[_menuPos].Contains("Credits"))
				{
					Game1.ChangeState(GameState.Credits);
				}
				else if (_menuItems[_menuPos].Contains("Leaderboard"))
				{
					Game1.ChangeState(GameState.LeaderBoard);
				}
			}
		}
		public override void Draw()
		{
			
			// Draw here
			for (int i = 0; i < _menuItems.Length; i++)
			{
				SpriteBatch.DrawString(_menuFont, _menuItems[i], new Vector2(_selectedItem[i], _yPos[i]), _fontColor, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);	
			}
		}
	}
}
