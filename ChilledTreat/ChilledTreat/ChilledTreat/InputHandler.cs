using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChilledTreat
{
	internal class InputHandler
	{
		private static InputHandler _instance;

		public static InputHandler Instance
		{
			get { return _instance ?? (_instance = new InputHandler()); }
		}

		private InputHandler()
		{
		}

#if WINDOWS
		private KeyboardState KeyboardState { get; set; }
		private KeyboardState PreviousKeyboardState { get; set; }

		private const Keys UpKey = Keys.Up;
		private const Keys LeftKey = Keys.Left;
		private const Keys RightKey = Keys.Right;
		private const Keys DownKey = Keys.Down;

		/*- = = L E H M A N N  H A N D I C A P E T = = -*/
		/*Lehmann*/
		private const Keys WKey = Keys.W; /*Lehmann*/
		/*Lehmann*/
		private const Keys AKey = Keys.A; /*Lehmann*/
		/*Lehmann*/
		private const Keys SKey = Keys.S; /*Lehmann*/
		/*Lehmann*/
		private const Keys DKey = Keys.D; /*Lehmann*/
		/*- = = L E H M A N N  H A N D I C A P E T = = -*/
		/*
		 * 
		 * WOMANS FOR U
								 ********** *** &**P....
							  $$$..........  . .* &.&.&&&&
							$$$....$$$$$$$.. .. .*.&&&...*...
							$$$.$$$&&&&&&&$$$... .*.&.*....&&&
						  $$$$$$.& &&&&&&&  *.. ...*.*..&&&..&&
						 $$$$$$.& &       &&  *.. .**..&..****..
						 $$$$..&   .......  &  *...*.&&.**..&.&&&
						$$$$..& & &&&&&$$$$  & $$$$$$$$$&&&&     &
						$$$.. & ... .....$$    $$$$$$$$$$....*****
						 $$$.   ..$$$$$$$*...$$$$$$$$$$$$.......***
					   $&&&&.  .$$$$$$*    ****    **$$$..    ....&2
					   &&&&&.  .$$$.$$*     **     ..*$$$$$     ...&
					  $&&&&&&  .$*****$$.          *..$$$$.    ...&
					  &&&&&*.  &$*..../.        ****...$$$$.    ...&
					 &&&&&$$.  &$..$$****.      ./. **.$$$..    ...&
					 &&&&$$$. &&$.  *$$**    /..&&&*...$$&..    ..&
					 ****$*$& &&$           .  *&$$&..$$$&.    ...&
					 $$$$&$$$$$$         .  &      * .$$&..  ...&
					 &&&&&&$$$$$$        *  &        .$$..   ...*
				   **$*** $$$$$..       /    S      .$$&   ...*..
				  ***$** *$$$$$..       P&..$*    ...$$   ...* ..
				  ****$  $**$$$....       *       ..$$  ..**&&..
				 *****$  $**$$$$....  .*$$.*.    ...$$  ..**&&..
				 ******   ***$$$...   *$.  .**   ...$...$$&&&..
				******** $$$$MMM$.      **$      ..$$..$$$$$$.
				*******$$$$$MMMM&&.            ...*$$$....$$.
				MMM******MMMMMMM&&&&..       ..*$ $$$$$$$$$$
				 MMM*******MMMM&.&&&&&&&WWWP$$$MM*$$ $$$$$$$
				  MMMMMMMMMMMM&....&&&&&$$$$$MMMM*$$$$&&&$$$$
					&&&&&&&&&......&..&..MMMMMMMM*$$$$...&&&&$$
			   &&&&&&&&&&&&.........&..&...MMMMMM*$$$$&&&B8&&&&&&&&
			&&&&&&&&.... .&&&&......&..&...&&MMMM*.. &&&....&&&&&&&&&&&&
		  &&&&&.....&....   .&&&&....&..&..&&.MMM*&&&...........&&&&&&&&&&&&
		 &... .....&&&...      ..&&&.&.....&&&&.MM   ...................&&&&&&
		&..      ..........     ...........      *.......................&&&&&&&
		&.       .......................................................&&&&&&&&&
		&.   ....&&.....................................................&&&&&&&&&
		&.......&&&&&.................................    .&&&&&&&&&&&&&&&&&&&&&&&
		&.......&&&&&..................................           .&&&&&&&&&&&&&&&&
		*&......&&&&&.. ...............&......................        &&&&&&&&&&&&&&
		*&.......&&&&&.  ..............&.......*****......                /.$.&&&&&&&
		*&........&&&&.   ...........*.*&.*.*****.............&               $.&&&&&&
		*&.........&$M..    .......&&&**&&**.**.........                    ...$*&&&&&&
		 *&........*&MM.    .........&&**&*.**...     ...#####.              ...$**&&&&
		**&........*&MMM.   ...........&*.$**...     ...........      ...........$***&&
		 *&........**MM&.    ...........$*$$*..     ........        .............*$**&&&
		 *&........&&M..    .........*...**$&.      ......................&&&&$/***&0***&&&
		 *&&.......&M$.   .........*..*...*$&.      .........&&&&&&&B8&B8&&&&&***********&&&&
		  *&......*&*$.     .......**.**...*$.      ......OOO   ..******$$$$$$************&&&&
		  *&......&&*$      ......******...*$&.         OOOOOO   ..***&&$$$$$$$**************&&
		   $&.....&&$.      .....*****.....*$$..        OOOOOO    .***&$$$$$$$$*********$$****&&
		   *&&....&*.       ...********....*$$&..        OOOO    ..**&$$$$$$ $$$***************&&
			*&....&*.     OOO....******....*$$$&...               **B$$$$$$$  $$*****************
			*&....&*.    OOOOO*...****.....&$$MMM.....          .**MM&$$$$$$   $*****************&
			 *&...3*..   OOOOOO ...*......&&$$$MMMMM..........***M&$$$$$$$$$    $****************&
			  *&...$...   OOOO    .......&$$$$$$MMMMM**MMMMMMMM$$$$$$$$$$$$$     $****************&
			  *&.....$....       ......$$$$$$$$$$$MMMMMMMM$$$$$$$$$$$$$$$$$$      ****************
			   $&......$.............$$$$$&$$$$$$$$$$*$$$$$$$$$$*$$$$$$$$$$$$       **************
				*&.......3*.......*$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$&&$$$$        ***********
				 *&......$$$$$$$$$$$$$$$.......$$$$$$$$$$$$$$$$$$$$$*&&&.&&&**$$          ******
				 &.........$$$$$$$$$$$..................   ..$$$$$$*&........&&$$$$         **
				  &...............................          .......$.....**...&&&&*$$$$
				   &&....                                  ..........$$$$$**  ....&**$$$$$
					&&&&...                       ......................*****   ...&&&&*$$$
					 &&&&&...........        ............................*****   ...&&&&*$$$
					  &&&&&&..................&&&&&&&&.....................****    ..&&&&&&*S
					   &&&&&&&.....&&&&&&&&&&&&$$$$$$$$$**.............$$.... **    *...&&&&*$
						 &&&&&&&&&&&&$$$$$$$$$$$$$$$$$$$$**...........&..$$... ..   **...&&&&*$
										$$$$$$$$$$$$$$$$$$$**.......*$$....$$....   **...&&&&*$$
										$$$$$$...............**..$$$....  ..      ..*.  .....*$$$
									   *$$$*&.....        ....***.$$....$$...  ....**    ..&&&&&$$
									   $$$*&&&&..  .           ***$$$$...$$$  ..$.....     ..&&&*$
									   $$*&&&&&&   ...           *$$   ...   .$$......      .&&&$$
									  $$$$*&&&&&       ...               .   $$$*....        .&&&$
									  $$$$*&&&.     &&&***..             ...$$$**...         .&&$$
									 .$$$$$*&&.    .........*......     ...*$&**...         ..&&$$
									 $$$$$$$$&&.   .........*** ..... ....*$$***...         ..&&$$$
									 $$$$$$$$*&&. ..........********....$$$$****...         .&&&$$$
									 $$$$$$$$*&&............ ********$$$$$*****....        ..&&&$$$
									 $$$$$$$$$*&............. ******&$$$******.....        &&&**$$$
									 $$*$$$$$$*&&&&&....&&&&&&* **** &$******......       .....*.$$
									 $$$$$$$$$*&&&..............& ** $&*****......        .&&&**$$$
									 *$$$$$$$*&&&................&&**$$&***.......        .&&&**$$$
									  $$$$$$$$*&&&................&***$$&.........        .&&&**$$$
									  $$$$$$$$*&&......    ........&**$$$&........       ..&&&**$$.
									   $$$$$$$$*&&......    ........&*$$$$&.......       ..&&&**$$.
										$$$$$$$*&&......      ......&&*&&$&.......      ....&&&**$.
										 $$$$$$$*&&.....      .......&&&*$$&.......    .....&&**$$
										  $$$$$$**&&.....      ......&&&&*$&&......    ....&&&**$.
										  .$$$$$**&&......      ......&&&*$$&&.....   .....&&&**$
										   .$$$$**&&&.....       .....&&&*$$$&.............&&&**$
											$$$$$*&&&......       .....&&&*$$$*...........&&**$$
											 $$$$$*&&&.....       .....&&&*$$$$&..........&&*$$.
											  $$$$*&&&.....       .....&&&*$$$$&..........&&*$$
											   $$$$**&&......      .....&&&**$$$&.........&*$$.
												$$$$*&&&.....       ....&&&&*$$$$&........&*$$
												 $$$$*&&&.....      ....&&&&&*$$$&&......&*$$.
												  $$$$*&&&....       ...&&&&&*$$$$&.....&&*$$
												   $$$*&&&....       ...&&&&&*$$$$&&..&&*$$$
													$$$**&&....       ...&&&&&**$$$&...&**$
													 $$$**&&&...       ...&&&&**$$$$*..**$.
													  $$$**&&&...      ....&&&&*$$$**..**$
													   $$$**&&....      ....&&&&*$$$*...$.
														$$$**&&...       ....&&&*$$$*&..$
														 $$$**&&...       ...&&&&*$$**..*
														   $$**.....       ..&&&&*$$**..
															$**&&&..        ..&&&*$$*$..
															 $**&&...        ..&&*$$*$.
															  $$**&...        .$$$$$$$.
															   $**&....       ...$$$$$
																$$&..............$$$$
																.$.....&&&........$$
																$$... .&&&........$$
															   $$.....   ........*$$
															  $$$...&&..  ......*$$*
															 $$*......&&........*$$
															$*&...&&....&......&*$$
														   $.......&..........&&$$$
														  $*&&...  .&.........&&$$$
														 $&*&&&...     ......&&$$$$
														$$*&&&....     .....&&&$$$$
													   $$*&&&.....     .....&&$$$$$
													   $*&&&.....     .....&&$$$$$$
													  $*&&&......    ...&&&&&$$$$$$
													  **&&&......   ....&&&&$$$$$$$
													 $**&&&......   ....&&&&$$$$$$$
													 $**&&......  ....&&&&$$$$$$$$*
													$$**&&......  ....&&&$$$$$$$$$
													$$*&&&......  ....&&$$$$$$$$$*
												   $$**&&......  ...&&&$$$$$$$$$$
												   $$**&&......   ...&&$$$$$$$$$$
												   $**&&.....   ...&&&$$$$&&*$$$$
												  .$*&&.....   ...&&&$$$$$*&&*$$
												  $$**&.....  ...&&&$$$$&&&&$$$
												  &**.....    ..&&&$$$$&&&&&&$$
												  $......    ..&&&$$$$&&&& &&$$
												 $......    ..&&&$$$$$&&&  &$$
												 *.....    ..&&&$$$$$*&&   &$$
												$.....    ..&&&$$$$$$*&&  &&$
											   $.....    ..&&& $$$$$*&&&  &$$
											   $.....   ..&&&  $$$$$*&&  &&$
											  $.....   ..&&&   $$$$*&&  &&$$
											  $....   .&&&     $$$*&&   &&$
											 $$...   .&&&      $$*&&&  &&$$
											$$...   .&&&       $$&&&   &&$
		*/

		private const Keys ActionKey = Keys.Enter;
		private const Keys AbortKey = Keys.Escape;

		private const Keys ReloadKey = Keys.R;
		private const Keys CoverKey = Keys.Space;


#endif

#if !XBOX
		private MouseState MouseState { get; set; }
		private MouseState PreviouseMouseState { get; set; }
#endif

#if !WINDOWS_PHONE
		private GamePadState GamePadState { get; set; }
		private GamePadState PreviousGamePadState { get; set; }

		private const Buttons LeftButton = Buttons.DPadLeft;
		private const Buttons RightButton = Buttons.DPadRight;
		private const Buttons DownButton = Buttons.DPadDown;
		private const Buttons UpButton = Buttons.DPadUp;

		private const Buttons AbortButton = Buttons.B;
		private const Buttons ActionButton = Buttons.A;

		// Must be public
		public Buttons ShootButton = Buttons.RightTrigger;
		public Buttons CoverButton = Buttons.LeftTrigger;
		public Buttons ReloadButton = Buttons.X;
		public Buttons ChangeWeaponButton = Buttons.Y;

		public PlayerIndex PlayerIndex = PlayerIndex.One;

		//TODO: TESTING!!!!!
		private Vector2 _gamePadPointerLocation;
#endif

		public void Update()
		{
#if WINDOWS
			PreviousKeyboardState = KeyboardState;
			KeyboardState = Keyboard.GetState();
#endif

#if !WINDOWS_PHONE
			PreviousGamePadState = GamePadState;
			GamePadState = GamePad.GetState(PlayerIndex);
#endif

#if !XBOX
			PreviouseMouseState = MouseState;
			MouseState = Mouse.GetState();
#endif
		}

		#region Keyboard

#if WINDOWS
		public bool IsKeyUp(Keys key)
		{
			return KeyboardState.IsKeyUp(key);
		}

		public bool IsKeyDown(Keys key)
		{
			return KeyboardState.IsKeyDown(key);
		}

		public bool IsKeyPressed(Keys key)
		{
			return IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
		}

		public bool IsKeyReleased(Keys key)
		{
			return IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key);
		}
#endif

		#endregion

		#region GamePad

#if !WINDOWS_PHONE
		public bool IsButtonUp(Buttons button)
		{
			return GamePadState.IsButtonUp(button);
		}

		public bool IsButtonDown(Buttons button)
		{
			return GamePadState.IsButtonDown(button);
		}

		public bool IsButtonPressed(Buttons button)
		{
			return GamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button);
		}

		public bool IsButtonReleased(Buttons button)
		{
			return GamePadState.IsButtonUp(button) && PreviousGamePadState.IsButtonDown(button);
		}
#endif

		#endregion

		#region Mouse

#if !XBOX

		public bool IsLeftMouseButtonPressed()
		{
			return ((MouseState.LeftButton == ButtonState.Pressed) && (PreviouseMouseState.LeftButton == ButtonState.Released));
		}
#endif

#if !XBOX

		public bool IsLeftMouseButtonDown()
		{
			return (MouseState.LeftButton == ButtonState.Pressed);
		}
#endif

		#endregion

		#region Common Methods

		public bool IsUpDown()
		{
#if WINDOWS
			return IsKeyDown(UpKey) || IsButtonDown(UpButton);
#elif XBOX
			return IsButtonDown(UpButton);
#else 
			return false;
#endif
		}

		public bool IsDownDown()
		{
#if WINDOWS
			return IsKeyDown(DownKey) || IsButtonDown(DownButton);
#elif XBOX
			return IsButtonDown(DownButton);
#else
			return false;
#endif
		}

		public bool IsLeftDown()
		{
#if WINDOWS
			return IsKeyDown(LeftKey) || IsButtonDown(LeftButton);
#elif XBOX
			return IsButtonDown(LeftButton);
#else
			return false;
#endif
		}

		public bool IsRightDown()
		{
#if WINDOWS
			return IsKeyDown(RightKey) || IsButtonDown(RightButton);
#elif XBOX
			return IsButtonDown(RightButton);
#else
			return false;
#endif
		}

		public bool IsAbortDown()
		{
#if WINDOWS
			return IsKeyDown(AbortKey) || IsButtonDown(AbortButton);
#elif XBOX
			return IsButtonDown(AbortButton);
#else
			return false;
#endif
		}

		public bool IsActionDown()
		{
#if WINDOWS
			return IsKeyDown(ActionKey) || IsButtonDown(ActionButton);
#elif XBOX
			return IsButtonDown(ActionButton);
#else
			return false;
#endif
		}

		public bool IsUpUp()
		{
#if WINDOWS
			return IsKeyUp(UpKey) || IsButtonUp(UpButton);
#elif XBOX
			return IsButtonUp(UpButton);
#else
			return false;
#endif
		}

		public bool IsLeftUp()
		{
#if WINDOWS
			return IsKeyUp(LeftKey) || IsButtonUp(LeftButton);
#elif XBOX
			return IsButtonUp(LeftButton);
#else
			return false;
#endif
		}

		public bool IsRightUp()
		{
#if WINDOWS
			return IsKeyUp(RightKey) || IsButtonUp(RightButton);
#elif XBOX
			return IsButtonUp(RightButton);
#else
			return false;
#endif
		}

		public bool IsDownUp()
		{
#if WINDOWS
			return IsKeyUp(DownKey) || IsButtonUp(DownButton);
#elif XBOX
			return IsButtonUp(DownButton);
#else
			return false;
#endif
		}

		public bool IsAbortUp()
		{
#if WINDOWS
			return IsKeyUp(AbortKey) || IsButtonUp(AbortButton);
#elif XBOX
			return IsButtonUp(AbortButton);
#else
			return false;
#endif
		}

		public bool IsActionUp()
		{
#if WINDOWS
			return IsKeyUp(ActionKey) || IsButtonUp(ActionButton);
#elif XBOX
			return IsButtonUp(ActionButton);
#else
			return false;
#endif
		}

		#endregion

		#region COMMON METHODS FOR XBOX & WINDOWS

		public bool IsActionPressed()
		{
#if WINDOWS
			return IsKeyPressed(ActionKey);
#elif XBOX
			return IsButtonPressed(ActionButton);
#endif
		}

		public bool IsAbortPressed()
		{
#if WINDOWS
			return IsKeyPressed(AbortKey);
#elif XBOX
			return IsButtonPressed(AbortButton);
#endif
		}

		public bool IsUpPressed()
		{
#if WINDOWS
			return IsKeyPressed(UpKey) || IsKeyPressed(WKey);
			;
#elif XBOX
			return IsButtonPressed(UpButton) || ThumbStickLeftUp();
#endif
		}

		public bool IsDownPressed()
		{
#if WINDOWS
			return IsKeyPressed(DownKey) || IsKeyPressed(SKey);
#elif XBOX
			return IsButtonPressed(DownButton) || ThumbStickLeftDown();
#endif
		}

		public bool IsLeftPressed()
		{
#if WINDOWS
			return IsKeyPressed(LeftKey) || IsKeyPressed(AKey);
			;
#elif XBOX
			return IsButtonPressed(LeftButton);
#endif
		}

		public bool IsRightPressed()
		{
#if WINDOWS
			return IsKeyPressed(RightKey) || IsKeyPressed(DKey);
#elif XBOX
			return IsButtonPressed(RightButton);
#endif
		}

		public bool IsReloadPressed()
		{
#if WINDOWS
			return IsKeyPressed(ReloadKey);
#elif XBOX
			return IsButtonPressed(ReloadButton);	
#endif
		}

		public bool IsCoverDown()
		{
#if WINDOWS
			return IsKeyDown(CoverKey);
#elif XBOX
			return IsButtonDown(CoverButton);
#endif
		}

		public bool IsShootPressed()
		{
#if WINDOWS
			return IsLeftMouseButtonPressed();
#elif XBOX
			return IsButtonPressed(ShootButton);
#endif
		}

		public bool IsShootDown()
		{
#if WINDOWS
			return IsLeftMouseButtonDown();
#elif XBOX
			return IsButtonDown(ShootButton);
#endif
		}

		public bool IsSwitchWeaponPressed()
		{
#if WINDOWS
			return IsKeyPressed(Keys.Tab);
#elif XBOX
			return IsButtonPressed(ChangeWeaponButton);
#endif
		}

#if !WINDOWS_PHONE
		public bool ThumbStickLeftUp()
		{
			return GamePadState.ThumbSticks.Left.Y > 0.4 &&
				PreviousGamePadState.ThumbSticks.Left.Y < 0.2;
		}
		public bool ThumbStickLeftDown()
		{
			return GamePadState.ThumbSticks.Left.Y < -0.4 &&
				PreviousGamePadState.ThumbSticks.Left.Y > -0.2;
		}
#endif

		#endregion

		//TODO: TEST!!!!!!!!!!!!

		public Vector2 PointerLocation()
		{
#if !XBOX
			return GamePad.GetState(PlayerIndex).IsConnected
					? GamePadPointerLocation()
					: new Vector2(MouseState.X, MouseState.Y);
#else
			return GamePadPointerLocation();
#endif
		}

#if !WINDOWS_PHONE
		private Vector2 GamePadPointerLocation()
		{
			if (GamePadState.ThumbSticks.Left.X > 0.2) _gamePadPointerLocation.X += 10;
			else if (GamePadState.ThumbSticks.Left.Y > 0.2) _gamePadPointerLocation.Y -= 10;
			if (GamePadState.ThumbSticks.Left.X < -0.2) _gamePadPointerLocation.X -= 10;
			else if (GamePadState.ThumbSticks.Left.Y < -0.2) _gamePadPointerLocation.Y += 10;

			return _gamePadPointerLocation;
		}
#endif

	}
}
