using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChilledTreat.GameClasses
{
	class Enemy
	{
		readonly SpriteBatch _spriteBatch;

		readonly Texture2D _texture;
		Vector2 _speed = new Vector2(0, 20);

		
		Vector2 _position;
		int hp = 20;

		Point _frameSize = new Point(203, 228);
		Point _currentFrame = new Point(0, 0);
		Point _sheetSize = new Point(2, 1);

		public Enemy(SpriteBatch spriteBatch, ContentManager content)
		{
			_spriteBatch = spriteBatch;
			_position = Vector2.Zero;
			_texture = content.Load<Texture2D>("Images/enemy");
		}

		public void Update()
		{
			// Animation frames
			++_currentFrame.X;
				if (_currentFrame.X >= _sheetSize.X)
				{
					_currentFrame.X = 0;
					++_currentFrame.Y;
					if (_currentFrame.Y >= _sheetSize.Y)
						_currentFrame.Y = 0;
				}

				// Movement
				_position.Y -= _speed.Y;

				if (_position.Y > 200)
				{
					_speed.Y *= -1;
					_position.Y = 200;
				}
				else if (_position.Y < 0)
				{
					_speed.Y *= -1;
					_position.Y = 0;
				}
		}

		public void Draw()
		{
			_spriteBatch.Draw(_texture, _position, Color.White);
		}
	}
}
