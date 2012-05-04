using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChilledTreat.GameClasses
{
	class Enemy
	{
		SpriteBatch spriteBatch;

		Texture2D texture;
		/*Vector2 speed = new Vector2(0, 20);

		*/
		Vector2 position;/*
		int hp = 20;

		Point frameSize = new Point(203, 228);
		Point currentFrame = new Point(0, 0);
		Point sheetSize = new Point(2, 1);*/

		public Enemy(SpriteBatch spriteBatch, ContentManager content)
		{
			this.spriteBatch = spriteBatch;
			//position = Vector2.Zero;
			texture = content.Load<Texture2D>("Images/enemy");
		}

		public void Update()
		{
			// Animation frames
			/*	++currentFrame.X;
				if (currentFrame.X >= sheetSize.X)
				{
					currentFrame.X = 0;
					++currentFrame.Y;
					if (currentFrame.Y >= sheetSize.Y)
						currentFrame.Y = 0;
				}

				// Movement
				position.Y -= speed.Y;

				if (position.Y > 200)
				{
					speed.Y *= -1;
					position.Y = 200;
				}
				else if (position.Y < 0)
				{
					speed.Y *= -1;
					position.Y = 0;
				}*/
		}

		public void Draw()
		{
			spriteBatch.Draw(texture,
				position,
				Color.White);
		}
	}
}
