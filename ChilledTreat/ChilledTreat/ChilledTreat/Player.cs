﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChilledTreat
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	class Player
	{
		int _health, _ammo;
		readonly Texture2D _reticuleTexture;
		readonly SpriteBatch _sp;
		readonly InputHandler _input = InputHandler.Instance;
		readonly Vector2 _halfTexture;

		private Vector2 ReticulePosition { get; set; }

		public Player(SpriteBatch spriteBatch, ContentManager content)
		{
			_health = 100;
			_ammo = 10;
			_reticuleTexture = content.Load<Texture2D>("reticule");
			_sp = spriteBatch;
			_halfTexture = new Vector2(_reticuleTexture.Width / 2, _reticuleTexture.Height / 2);
		}


		public void Update()
		{
			ReticulePosition = new Vector2(_input.MouseState.X, _input.MouseState.Y);


			if (ReticulePosition.X < 0) ReticulePosition = new Vector2(0, ReticulePosition.Y);
			else if (ReticulePosition.X > 1280) ReticulePosition = new Vector2(1280, ReticulePosition.Y);

			if(ReticulePosition.Y < 0) ReticulePosition = new Vector2(ReticulePosition.X, 0);
			else if (ReticulePosition.Y > 720) ReticulePosition = new Vector2(ReticulePosition.X, 720);

		}

		public void Draw()
		{
			_sp.Draw(_reticuleTexture, ReticulePosition - _halfTexture, Color.White);
		}
	}
}
