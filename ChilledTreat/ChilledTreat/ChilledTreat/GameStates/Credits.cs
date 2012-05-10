using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ChilledTreat.GameStates
{
	class Credits : GameState
	{
		readonly SpriteFont _creditsFont;
		readonly string[] _creditsContent;
		private int _shift;
		Color _fontColor;
		readonly InputHandler _input = InputHandler.Instance;
		readonly FrameInfo _frameInfo = FrameInfo.Instance;
		private int scrolling = 0;

		public Credits(SpriteBatch spriteBatch, ContentManager content)
			: base(spriteBatch, content)
		{
			// Credits content
			_creditsFont = content.Load<SpriteFont>("Fonts/CreditsFont");
			_fontColor = Color.Salmon;
			_creditsContent = new[] {
				".....CREDITS.....",
				"Programmers",
				" ",
				"Simen Bekkhus",
				"Vegard Strand",
				"Martin Lehmann",
				"Steinar Skogly",
				" ",
				"Designers",
				" ",
				"Simen Bekkhus",
				"Vegard Strand",
				"Martin Lehmann",
				"Steinar Skogly",
				" ",
				"Sources",
				" ",
				"blogcdn.com/joystiq.com/media/2006/06/reticule.jpg",
				"tactical-life.com/online/wp-content/uploads/2008/11/bullet.gif",
				"soundbible.com/tags-gun.html",
				"dragoart.com/tuts/pics/9/5990/33809/how-to-draw-an-easy-heart-step-6.jpg",
				"filterforge.com/filters/9452.jpg",
				"gimp.pixtuts.com/images/tutorials/fire-explosion/fire-explosion.jpg",
				"sdb.drshnaps.com/objects/17/1513/Sprite/MIStanSheet.png",
				"schwar.fxhome.com/flare/machine_gun_seed_1.jpg",
				"switchonthecode.com/tutorials/csharp-tutorial-xml-serialization",
				"pacdv.com/sounds/voices-1.html",
				"media.photobucket.com/image/recent/USCMC/nationstates/Cartridges_pistol.png",
				"freewebs.com/callpoll100/photos/Sprites/DoomWeaponsPC.png",
				"i41.tinypic.com/5vx078.png",
		};
	}

		// Methods

		public override void Update()
		{
			// Logic
			_shift = 0;

			if (_input.IsAbortPressed())
			{
				Game1.ChangeState(GameState.Menu);
				scrolling = 0;
			}

			scrolling--;
		}

		public override void Draw()
		{
			// DRAW!!!! LåååL
			foreach (string creditEntry in _creditsContent)
			{
				_shift++;
				SpriteBatch.DrawString(_creditsFont, creditEntry, new Vector2(320, 680 + (_shift * 50) + (scrolling)), Color.White);
			}
		}
	}
}
