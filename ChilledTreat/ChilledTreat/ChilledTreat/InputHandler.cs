// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputHandler.cs" company="X&A Team">
//   Copyright (c) X&A Team. All rights reserved
// </copyright>
// <summary>
//   The input-handler
// </summary>
// <author>Vegard Strand</author>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The input-handler
    /// </summary>
    /// <remarks>Used for managing all user-input</remarks>
    internal class InputHandler
    {
        #region Fields
        // Preprocessor directives used to separate WINDOWS, WINDOWS_PHONE and XBOX
        // code. Making it possible to write code which compiles on each platform
        #region Constants
#if WINDOWS
        // Fields used for the WINDOWS platform

        /// <summary>
        /// Up key
        /// </summary>
        private const Keys UpKey = Keys.Up;

        /// <summary>
        /// Left key
        /// </summary>
        private const Keys LeftKey = Keys.Left;

        /// <summary>
        /// Right key
        /// </summary>
        private const Keys RightKey = Keys.Right;

        /// <summary>
        /// Down key
        /// </summary>
        private const Keys DownKey = Keys.Down;

        // Used for navigating as well

        /// <summary>
        /// W key
        /// </summary>
        private const Keys WKey = Keys.W;

        /// <summary>
        /// A key
        /// </summary>
        private const Keys AKey = Keys.A;

        /// <summary>
        /// S key
        /// </summary>
        private const Keys SKey = Keys.S;

        /// <summary>
        /// D key
        /// </summary>
        private const Keys DKey = Keys.D;

        /// <summary>
        /// Action key
        /// </summary>
        private const Keys ActionKey = Keys.Enter;

        /// <summary>
        /// Abort key
        /// </summary>
        private const Keys AbortKey = Keys.Escape;

        /// <summary>
        /// Reload key
        /// </summary>
        private const Keys ReloadKey = Keys.R;

        /// <summary>
        /// Space key
        /// </summary>
        private const Keys CoverKey = Keys.Space;
#endif

#if XBOX
        // TODO: Only used on XBOX for now. Full game-pad usability not implemented for WINDOWS

        /// <summary>
        /// Shoot on game-pad
        /// </summary>
        private const Buttons ShootButton = Buttons.RightTrigger;

        /// <summary>
        /// Cover on game-pad
        /// </summary>
        private const Buttons CoverButton = Buttons.LeftTrigger;

        /// <summary>
        /// Reload on game-pad
        /// </summary>
        private const Buttons ReloadButton = Buttons.X;

        /// <summary>
        /// Change weapon on game-pad
        /// </summary>
        private const Buttons ChangeWeaponButton = Buttons.Y;
#endif

#if !WINDOWS_PHONE
        /// <summary>
        /// Left on game-pad
        /// </summary>
        private const Buttons LeftButton = Buttons.DPadLeft;

        /// <summary>
        /// Right on game-pad
        /// </summary>
        private const Buttons RightButton = Buttons.DPadRight;

        /// <summary>
        /// Down on game-pad
        /// </summary>
        private const Buttons DownButton = Buttons.DPadDown;

        /// <summary>
        /// Up on game-pad
        /// </summary>
        private const Buttons UpButton = Buttons.DPadUp;

        /// <summary>
        /// B on game-pad
        /// </summary>
        private const Buttons AbortButton = Buttons.B;

        /// <summary>
        /// A on game-pad
        /// </summary>
        private const Buttons ActionButton = Buttons.A;
#endif
        #endregion

        /// <summary>
        /// The instance of the input-handler
        /// </summary>
        private static InputHandler instance;

        /// <summary>
        /// The current location of the pointer, used when using a game-pad
        /// </summary>
        private Vector2 gamePadPointerLocation;
        #endregion

        public string Per { get; set; }

        #region Constructor
        /// <summary>
        /// Prevents a default instance of the <see cref="InputHandler"/> class from being created.
        /// </summary>
        private InputHandler()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the instance of the input-handler
        /// </summary>
        public static InputHandler Instance
        {
            get { return instance ?? (instance = new InputHandler()); }
        }

#if !WINDOWS_PHONE
        /// <summary>
        /// Gets the player-index used (it's a single-player game)
        /// </summary>
        private static PlayerIndex PlayerIndex
        {
            get { return PlayerIndex.One; }
        }

        /// <summary>
        /// Gets or sets GamePadState.
        /// </summary>
        private GamePadState GamePadState { get; set; }

        /// <summary>
        /// Gets or sets PreviousGamePadState.
        /// </summary>
        private GamePadState PreviousGamePadState { get; set; }
#endif

#if WINDOWS
        /// <summary>
        /// Gets or sets the keyboard-state
        /// </summary>
        private KeyboardState KeyboardState { get; set; }

        /// <summary>
        /// Gets or sets the previous keyboard-state
        /// </summary>
        private KeyboardState PreviousKeyboardState { get; set; }
#endif

#if !XBOX
        /// <summary>
        /// Gets or sets MouseState.
        /// </summary>
        private MouseState MouseState { get; set; }

        /// <summary>
        /// Gets or sets PreviouseMouseState.
        /// </summary>
        private MouseState PreviouseMouseState { get; set; }
#endif
        #endregion

        #region WOMANS-FOR-U
        /*
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
        #endregion

        #region Update
        /// <summary>
        /// Update of Input-handler
        /// Must be called per Update for the InputHandler to work as intended
        /// </summary>
        public void Update()
        {
#if WINDOWS
            this.PreviousKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
#endif

#if !WINDOWS_PHONE
            this.PreviousGamePadState = this.GamePadState;
            this.GamePadState = GamePad.GetState(PlayerIndex);
#endif

#if !XBOX
            this.PreviouseMouseState = MouseState;
            MouseState = Mouse.GetState();
#endif
        }
        #endregion

        // The Game Specific methods region contains a collection of
        // methods that is needed specifically for this game.
        #region Game Specific methods

        /// <summary>
        /// Is action pressed
        /// </summary>
        /// <returns>
        /// Whether action-button is pressed
        /// </returns>
        public bool IsActionPressed()
        {
#if WINDOWS
            return this.IsKeyPressed(ActionKey);
#elif XBOX
            return this.IsButtonPressed(ActionButton);
#endif
        }

        /// <summary>
        /// Is abort pressed
        /// </summary>
        /// <returns>
        /// Whether abort-button is pressed
        /// </returns>
        public bool IsAbortPressed()
        {
#if WINDOWS
            return this.IsKeyPressed(AbortKey);
#elif XBOX
            return this.IsButtonPressed(AbortButton);
#endif
        }

        /// <summary>
        /// Is up pressed
        /// </summary>
        /// <returns>
        /// Whether up-button is pressed
        /// </returns>
        public bool IsUpPressed()
        {
#if WINDOWS
            return this.IsKeyPressed(UpKey) || this.IsKeyPressed(WKey);
#elif XBOX
            return this.IsButtonPressed(UpButton) || this.ThumbStickLeftUp();
#endif
        }

        /// <summary>
        /// Is down pressed
        /// </summary>
        /// <returns>
        /// Whether down-button is pressed
        /// </returns>
        public bool IsDownPressed()
        {
#if WINDOWS
            return this.IsKeyPressed(DownKey) || this.IsKeyPressed(SKey);
#elif XBOX
            return this.IsButtonPressed(DownButton) || this.ThumbStickLeftDown();
#endif
        }

        /// <summary>
        /// Is left pressed
        /// </summary>
        /// <returns>
        /// Whether left-button is pressed
        /// </returns>
        public bool IsLeftPressed()
        {
#if WINDOWS
            return this.IsKeyPressed(LeftKey) || this.IsKeyPressed(AKey);
#elif XBOX
            return this.IsButtonPressed(LeftButton);
#endif
        }

        /// <summary>
        /// Is right pressed
        /// </summary>
        /// <returns>
        /// Whether rigth-button is pressed
        /// </returns>
        public bool IsRightPressed()
        {
#if WINDOWS
            return this.IsKeyPressed(RightKey) || this.IsKeyPressed(DKey);
#elif XBOX
            return this.IsButtonPressed(RightButton);
#endif
        }

        /// <summary>
        /// Is reload pressed
        /// </summary>
        /// <returns>
        /// Whether reload-button is pressed
        /// </returns>
        public bool IsReloadPressed()
        {
#if WINDOWS
            return this.IsKeyPressed(ReloadKey);
#elif XBOX
            return this.IsButtonPressed(ReloadButton);
#endif
        }

        /// <summary>
        /// Is cover down
        /// </summary>
        /// <returns>
        /// Whether cover-button is down
        /// </returns>
        public bool IsCoverDown()
        {
#if WINDOWS
            return this.IsKeyDown(CoverKey);
#elif XBOX
            return this.IsButtonDown(CoverButton);
#endif
        }

        /// <summary>
        /// Is shoot pressed
        /// </summary>
        /// <returns>
        /// Whether shoot-button is pressed
        /// </returns>
        public bool IsShootPressed()
        {
#if WINDOWS
            return this.IsLeftMouseButtonPressed();
#elif XBOX
            return this.IsButtonPressed(ShootButton);
#endif
        }

        /// <summary>
        /// Is shoot down
        /// </summary>
        /// <returns>
        /// Whether shoot-button is down
        /// </returns>
        public bool IsShootDown()
        {
#if WINDOWS
            return this.IsLeftMouseButtonDown();
#elif XBOX
            return this.IsButtonDown(ShootButton);
#endif
        }

#if !WINDOWS_PHONE
        /// <summary>
        /// Is left thumb-stick held upwards
        /// </summary>
        /// <returns>
        /// Whether left thumb-stick is held upwards
        /// </returns>
        public bool ThumbStickLeftUp()
        {
            return GamePadState.ThumbSticks.Left.Y > 0.4 &&
                this.PreviousGamePadState.ThumbSticks.Left.Y < 0.2;
        }

        /// <summary>
        /// Is left thumb-stick held downwards
        /// </summary>
        /// <returns>
        /// Whether left thumb-stick is held downwards
        /// </returns>
        public bool ThumbStickLeftDown()
        {
            return GamePadState.ThumbSticks.Left.Y < -0.4 &&
                this.PreviousGamePadState.ThumbSticks.Left.Y > -0.2;
        }
#endif

        /// <summary>
        /// Is switch weapon pressed
        /// </summary>
        /// <returns>
        /// Whether the switch weapon-button is pressed
        /// </returns>
        public bool IsSwitchWeaponPressed()
        {
#if WINDOWS
            return this.IsKeyPressed(Keys.Tab);
#elif XBOX
            return this.IsButtonPressed(ChangeWeaponButton);
#endif
        }

        /// <summary>
        /// Used to handle input to move crosshair
        /// </summary>
        /// <returns>
        /// The relative position of the pointer on a gamepad
        /// </returns>
        public Vector2 PointerLocation()
        {
#if !XBOX
            // In the PC version the mouse is used for
            // crosshair movement
            return GamePad.GetState(PlayerIndex).IsConnected
                    ? this.GamePadPointerLocation()
                    : new Vector2(MouseState.X, MouseState.Y);
#else
            // In the XBOX version the gamepad thumbstick
            // is used to move the crosshair
            return this.GamePadPointerLocation();
#endif
        }

        /// <summary>
        /// Is pause pressed
        /// </summary>
        /// <returns>
        /// Whether the pause-button is pressed
        /// </returns>
        public bool IsPausePressed()
        {
#if WINDOWS
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
#endif
                return this.IsButtonPressed(Buttons.Start);
#if WINDOWS
            }

            return this.IsKeyPressed(Keys.Escape) || this.IsKeyPressed(Keys.P);
#endif
        }

#if !WINDOWS_PHONE
        /// <summary>
        /// Is the game-pad connected
        /// </summary>
        /// <returns>
        /// Whether the game-pad is connected
        /// </returns>
        public bool IsControllerConnected()
        {
            return GamePadState.IsConnected;
        }

        /// <summary>
        /// Start vibrating the controller in full
        /// </summary>
        public void StartHardVibrate()
        {
            GamePad.SetVibration(PlayerIndex, 1f, 1f);
        }

        /// <summary>
        /// Start vibratig the controller at half strength
        /// </summary>
        public void StartSoftVibrate()
        {
            GamePad.SetVibration(PlayerIndex, 0.5f, 0.5f);
        }

        /// <summary>
        /// Stop the vibration on the controller
        /// </summary>
        public void StopVibrate()
        {
            GamePad.SetVibration(PlayerIndex, 0, 0);
        }
#endif
        #endregion

        #region input-checkers

        // The Keyboard region contains methods used to handle
        // input from the keyboard. Each method accepts a 
        // key object as a parameter and returns a bool 
        // according to the state of the key
        #region Keyboard
#if WINDOWS
        /// <summary>
        /// Key is up
        /// </summary>
        /// <param name="key">
        /// The Key
        /// </param>
        /// <returns>
        /// Whether the key is up or not
        /// </returns>
        private bool IsKeyUp(Keys key)
        {
            return KeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Key is down
        /// </summary>
        /// <param name="key">
        /// The Key
        /// </param>
        /// <returns>
        /// Whether the key is down or not
        /// </returns>
        private bool IsKeyDown(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Key is pressed
        /// </summary>
        /// <param name="key">
        /// The Key
        /// </param>
        /// <returns>
        /// Whether the key is pressed or not
        /// </returns>
        private bool IsKeyPressed(Keys key)
        {
            return this.IsKeyDown(key) && this.PreviousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Key is released
        /// </summary>
        /// <param name="key">
        /// The Key
        /// </param>
        /// <returns>
        /// Whether the key is released or not
        /// </returns>
        private bool IsKeyReleased(Keys key)
        {
            return this.IsKeyUp(key) && this.PreviousKeyboardState.IsKeyDown(key);
        }
#endif

        #endregion

        // The GamePad region contains methods used to handle
        // input from a GamePad. Each methods accepts a
        // Button object as a parameter and returns a bool
        // according to the state of the button
        #region GamePad

#if !WINDOWS_PHONE
        /// <summary>
        /// Return a usable vector2 if GamePad is used to move crosshair
        /// </summary>
        /// <returns>
        /// The relative position of the pointer using a game-pad
        /// </returns>
        private Vector2 GamePadPointerLocation()
        {
            if (GamePadState.ThumbSticks.Left.X > 0.2)
            {
                this.gamePadPointerLocation.X += 10;
            }
            else if (GamePadState.ThumbSticks.Left.Y > 0.2)
            {
                this.gamePadPointerLocation.Y -= 10;
            }

            if (GamePadState.ThumbSticks.Left.X < -0.2)
            {
                this.gamePadPointerLocation.X -= 10;
            }
            else if (GamePadState.ThumbSticks.Left.Y < -0.2)
            {
                this.gamePadPointerLocation.Y += 10;
            }

            return this.gamePadPointerLocation;
        }
#endif

#if !WINDOWS_PHONE
        /// <summary>
        /// The button is up
        /// </summary>
        /// <param name="button">
        /// The button
        /// </param>
        /// <returns>
        /// Whether the button is up or not
        /// </returns>
        private bool IsButtonUp(Buttons button)
        {
            return GamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// The button is down
        /// </summary>
        /// <param name="button">
        /// The button
        /// </param>
        /// <returns>
        /// Whether the button is down or not
        /// </returns>
        private bool IsButtonDown(Buttons button)
        {
            return GamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// The button is pressed
        /// </summary>
        /// <param name="button">
        /// The button
        /// </param>
        /// <returns>
        /// Whether the button is pressed or not
        /// </returns>
        private bool IsButtonPressed(Buttons button)
        {
            return this.GamePadState.IsButtonDown(button) && this.PreviousGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// The button is released
        /// </summary>
        /// <param name="button">
        /// The button
        /// </param>
        /// <returns>
        /// Whether the button is released or not
        /// </returns>
        private bool IsButtonReleased(Buttons button)
        {
            return this.GamePadState.IsButtonUp(button) && this.PreviousGamePadState.IsButtonDown(button);
        }
#endif

        #endregion

        // The Mouse region contains methods which can be used
        // to handle different input from the mouse
        #region Mouse
#if !XBOX
        /// <summary>
        /// Left mouse-button (or screen on phone) is pressed
        /// </summary>
        /// <returns>
        /// Whether the mouse-button, or screen, is pressed
        /// </returns>
        private bool IsLeftMouseButtonPressed()
        {
            return (this.MouseState.LeftButton == ButtonState.Pressed) && (this.PreviouseMouseState.LeftButton == ButtonState.Released);
        }

        /// <summary>
        /// Left mouse-button (or screen on phone) is down
        /// </summary>
        /// <returns>
        /// Whether the mouse-button, or screen, is down
        /// </returns>
        private bool IsLeftMouseButtonDown()
        {
            return MouseState.LeftButton == ButtonState.Pressed;
        }
#endif

        #endregion

        #endregion

        #region Common Methods
        /// <summary>
        /// Is up down
        /// </summary>
        /// <returns>
        /// Whether up-button is down
        /// </returns>
        private bool IsUpDown()
        {
#if WINDOWS
            return this.IsKeyDown(UpKey) || this.IsButtonDown(UpButton);
#elif XBOX
            return this.IsButtonDown(UpButton);
#else 
            return false;
#endif
        }

        /// <summary>
        /// Is down down
        /// </summary>
        /// <returns>
        /// Whether down-button is down
        /// </returns>
        private bool IsDownDown()
        {
#if WINDOWS
            return this.IsKeyDown(DownKey) || this.IsButtonDown(DownButton);
#elif XBOX
            return this.IsButtonDown(DownButton);
#else
            return false;
#endif
        }

        /// <summary>
        /// Is left down
        /// </summary>
        /// <returns>
        /// Whether left-button is down
        /// </returns>
        private bool IsLeftDown()
        {
#if WINDOWS
            return this.IsKeyDown(LeftKey) || this.IsButtonDown(LeftButton);
#elif XBOX
            return this.IsButtonDown(LeftButton);
#else
            return false;
#endif
        }

        /// <summary>
        /// Is right down
        /// </summary>
        /// <returns>
        /// Whether right-button is down
        /// </returns>
        private bool IsRightDown()
        {
#if WINDOWS
            return this.IsKeyDown(RightKey) || this.IsButtonDown(RightButton);
#elif XBOX
            return this.IsButtonDown(RightButton);
#else
            return false;
#endif
        }

        /// <summary>
        /// Is abort down
        /// </summary>
        /// <returns>
        /// Whether abort-button is down
        /// </returns>
        private bool IsAbortDown()
        {
#if WINDOWS
            return this.IsKeyDown(AbortKey) || this.IsButtonDown(AbortButton);
#elif XBOX
            return this.IsButtonDown(AbortButton);
#else
            return false;
#endif
        }

        /// <summary>
        /// Is action down
        /// </summary>
        /// <returns>
        /// Whether action-button is down
        /// </returns>
        private bool IsActionDown()
        {
#if WINDOWS
            return this.IsKeyDown(ActionKey) || this.IsButtonDown(ActionButton);
#elif XBOX
            return this.IsButtonDown(ActionButton);
#else
            return false;
#endif
        }

        /// <summary>
        /// Is up up
        /// </summary>
        /// <returns>
        /// Whether up-button is up
        /// </returns>
        private bool IsUpUp()
        {
#if WINDOWS
            return this.IsKeyUp(UpKey) || this.IsButtonUp(UpButton);
#elif XBOX
            return this.IsButtonUp(UpButton);
#else
            return false;
#endif
        }

        /// <summary>
        /// Is left up
        /// </summary>
        /// <returns>
        /// Whether left-button is up
        /// </returns>
        private bool IsLeftUp()
        {
#if WINDOWS
            return this.IsKeyUp(LeftKey) || this.IsButtonUp(LeftButton);
#elif XBOX
            return this.IsButtonUp(LeftButton);
#else
            return false;
#endif
        }

        /// <summary>
        /// Is right up
        /// </summary>
        /// <returns>
        /// Whether right-button is up
        /// </returns>
        private bool IsRightUp()
        {
#if WINDOWS
            return this.IsKeyUp(RightKey) || this.IsButtonUp(RightButton);
#elif XBOX
            return this.IsButtonUp(RightButton);
#else
            return false;
#endif
        }

        /// <summary>
        /// Is down up
        /// </summary>
        /// <returns>
        /// Whether down-button is up
        /// </returns>
        private bool IsDownUp()
        {
#if WINDOWS
            return this.IsKeyUp(DownKey) || this.IsButtonUp(DownButton);
#elif XBOX
            return this.IsButtonUp(DownButton);
#else
            return false;
#endif
        }

        /// <summary>
        /// Is abort up
        /// </summary>
        /// <returns>
        /// Whether abort-button is up
        /// </returns>
        private bool IsAbortUp()
        {
#if WINDOWS
            return this.IsKeyUp(AbortKey) || this.IsButtonUp(AbortButton);
#elif XBOX
            return this.IsButtonUp(AbortButton);
#else
            return false;
#endif
        }

        /// <summary>
        /// Is action up
        /// </summary>
        /// <returns>
        /// Whether action-button is up
        /// </returns>
        private bool IsActionUp()
        {
#if WINDOWS
            return this.IsKeyUp(ActionKey) || this.IsButtonUp(ActionButton);
#elif XBOX
            return this.IsButtonUp(ActionButton);
#else
            return false;
#endif
        }
        #endregion
    }
}