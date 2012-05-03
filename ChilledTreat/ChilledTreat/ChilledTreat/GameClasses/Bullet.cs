using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChilledTreat.GameClasses
{
	class Bullet
	{
		private readonly Texture2D _bulletTexture;

		private Vector2 BulletPosition { get; set; }

		readonly SpriteBatch _sp;
		readonly InputHandler _input = InputHandler.Instance;
		readonly Vector2 _halfTexture;

		private readonly FrameInfo _frameInfo = FrameInfo.Instance;

		public Bullet(SpriteBatch spriteBatch, ContentManager content)
		{
			_bulletTexture = content.Load<Texture2D>("img/usableBullet");
			_sp = spriteBatch;
			_halfTexture = new Vector2(_bulletTexture.Width / 2, _bulletTexture.Height / 2);
		}

		public void Update()
		{
			//BulletPosition += 5;
		}

		public void Draw()
		{
			_sp.Draw(_bulletTexture, BulletPosition - _halfTexture, Color.White);
		}
	}
}