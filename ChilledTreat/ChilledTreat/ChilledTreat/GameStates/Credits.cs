using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace ChilledTreat.GameStates
{
	class Credits : GameState
	{
		//fields
		readonly SpriteFont _creditsFont;
		readonly SpriteFont _creditsFontSources;
		readonly string[] _creditsContent;
		readonly string[] _creditsContentSources;
		private int _shift;
		private bool _startAnew = true;
		readonly InputHandler _input = InputHandler.Instance;
		readonly FrameInfo _frameInfo = FrameInfo.Instance;
		private double _startCreditsTimer, _currentTime;
		readonly Texture2D _xnaLogo, _nithLogo;
		bool _drawLogos, _songstart;
		readonly Song _creditMusic;

		public Credits()
		{
			// Credits content
			_creditMusic = Game1.Instance.Content.Load<Song>("Music/DubMood");
			MediaPlayer.IsRepeating = true;
			_xnaLogo = Game1.Instance.Content.Load<Texture2D>("Images/xnaLogo");
			_nithLogo = Game1.Instance.Content.Load<Texture2D>("Images/nithLogo");
			_creditsFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/CreditsFont");
			_creditsFontSources = Game1.Instance.Content.Load<SpriteFont>("Fonts/urlFont");
			_creditsContent = new[] {

				//People who have worked on this project
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
				" "
			};


			//sourcepaths to some of the content used in this project.
			_creditsContentSources = new[] {
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
				"i.istockimg.com/file_thumbview_approve/1096341/2/stock-photo-1096341-bullet-chain.jpg",
				"wiki.teamfortress.com/w/images/0/06/Minigun_1st_person.png"
			};
		}

		// Methods

		public override void Update()
		{
			// Logic
			_shift = 0;

			if (_startAnew)
			{
				_startCreditsTimer = _frameInfo.GameTime.TotalGameTime.TotalSeconds;
				_startAnew = false;
			}

			if (_input.IsAbortPressed())
			{
				//resets the creditsscreen
				Game1.ChangeState(Menu);
				_startAnew = true;
				MediaPlayer.Stop();
			}
			_currentTime = _frameInfo.GameTime.TotalGameTime.TotalSeconds - _startCreditsTimer;

			if (!_songstart)
			{
				//starts music player for the creditssceen
				MediaPlayer.Play(_creditMusic);
				_songstart = true;
			}
				//timer for when the logos appears at the bottom of the creditsscreen
			if (_currentTime - _startCreditsTimer > 33)
			{
				_drawLogos = true;
			}
		}

		public override void Draw()
		{
			// DRAW!!!! LåååL
			
			foreach (string creditEntry in _creditsContent)
			{
				//draws people involved in credits
				_shift++;
				Game1.Instance.SpriteBatch.DrawString(_creditsFont, creditEntry, new Vector2((Game1.GameScreenWidth / 6) - (creditEntry.Length / 2), 680 + (_shift * 50) - (60 * (float)_currentTime)), Color.White);
			}
			foreach (string creditSourcesEntry in _creditsContentSources)
			{
				//draws sources used in project
				_shift++;
				Game1.Instance.SpriteBatch.DrawString(_creditsFontSources, creditSourcesEntry, new Vector2(Game1.GameScreenWidth / 6, 680 + (_shift * 50) - (60 * (float)_currentTime)), Color.White);
			}
			if (!_drawLogos) return;
				//draws logos at the end of creditsscreen
			Game1.Instance.SpriteBatch.Draw(_nithLogo, new Vector2((Game1.GameScreenWidth - _nithLogo.Width) / 2f, (Game1.GameScreenHeight - _nithLogo.Height) / 2.5f), Color.White);
			Game1.Instance.SpriteBatch.Draw(_xnaLogo, new Vector2((Game1.GameScreenWidth - _xnaLogo.Width) / 2f, (Game1.GameScreenHeight - _xnaLogo.Height) / 1.25f), Color.White);
		}
	}
}
