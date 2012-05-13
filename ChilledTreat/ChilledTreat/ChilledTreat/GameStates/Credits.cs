﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
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
		Color _fontColor;
		readonly InputHandler _input = InputHandler.Instance;
		readonly FrameInfo _frameInfo = FrameInfo.Instance;
		private double _startCreditsTimer, _currentTime;

		Song creditMusic;
		bool songstart = false;

		public Credits()
		{
			// Credits content
			creditMusic = Game1.Instance.Content.Load<Song>("Music/DubMood");
			MediaPlayer.IsRepeating = true;

			_creditsFont = Game1.Instance.Content.Load<SpriteFont>("Fonts/CreditsFont");
			_creditsFontSources = Game1.Instance.Content.Load<SpriteFont>("Fonts/urlFont");
			_fontColor = Color.Salmon;
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
				Game1.ChangeState(Menu);
				_startAnew = true;
				MediaPlayer.Stop();
			}
			_currentTime = _frameInfo.GameTime.TotalGameTime.TotalSeconds - _startCreditsTimer;

			if (!songstart)
			{
				MediaPlayer.Play(creditMusic);
				songstart = true;
			}  
		}

		public override void Draw()
		{
			// DRAW!!!! LåååL
			foreach (string creditEntry in _creditsContent)
			{
				_shift++;
				Game1.Instance.SpriteBatch.DrawString(_creditsFont, creditEntry, new Vector2((Game1.GameScreenWidth / 6) - (creditEntry.Length / 2), 680 + (_shift * 50) - (60 * (float)_currentTime)), Color.White);
			}
			if (_frameInfo.GameTime.TotalGameTime.TotalSeconds >= 5)
			{
				foreach (string creditSourcesEntry in _creditsContentSources)
				{
					_shift++;
					Game1.Instance.SpriteBatch.DrawString(_creditsFontSources, creditSourcesEntry, new Vector2(Game1.GameScreenWidth / 6, 680 + (_shift * 50) - (60 * (float)_currentTime)), Color.White);
				}
			}
		}
	}
}
