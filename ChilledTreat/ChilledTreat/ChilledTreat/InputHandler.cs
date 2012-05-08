using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChilledTreat
{
	class InputHandler
	{
		static InputHandler _instance;
		public static InputHandler Instance
		{
			get { return _instance ?? (_instance = new InputHandler()); }
		}

		private InputHandler()
		{ }

#if WINDOWS
		public KeyboardState KeyboardState { get; private set; }
		public KeyboardState PreviousKeyboardState { get; private set; }

		public Keys UpKey = Keys.Up;
		public Keys LeftKey = Keys.Left;
		public Keys RightKey = Keys.Right;
		public Keys DownKey = Keys.Down;
		public Keys WKey = Keys.W;
		public Keys AKey = Keys.A;
		public Keys SKey = Keys.S;
		public Keys DKey = Keys.D;

		public Keys ActionKey = Keys.Enter;
		public Keys AbortKey = Keys.Escape;

		
#endif

#if !XBOX
		public MouseState MouseState { get; private set; }
		public MouseState PreviouseMouseState { get; private set; }
#endif

#if !WINDOWSPHONE
		public GamePadState GamePadState { get; private set; }
		public GamePadState PreviousGamePadState { get; private set; }

		public Buttons LeftButton = Buttons.DPadLeft;
		public Buttons RightButton = Buttons.DPadRight;
		public Buttons DownButton = Buttons.DPadDown;
		public Buttons UpButton = Buttons.DPadUp;

		public Buttons ActionButton = Buttons.A;
		public Buttons AbortButton = Buttons.B;

		public PlayerIndex PlayerIndex = PlayerIndex.One;
#endif

		public void Update()
		{
#if WINDOWS
			PreviousKeyboardState = KeyboardState;
			KeyboardState = Keyboard.GetState();
#endif

#if !WINDOWSPHONE
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
#if !WINDOWSPHONE
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

		public bool IsLeftMouseButtonPressed ()
		{
			return ((MouseState.LeftButton == ButtonState.Pressed) && (PreviouseMouseState.LeftButton == ButtonState.Released));
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
			return IsButtonPressed(AbortButton);
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
			return IsKeyPressed(UpKey) || IsKeyPressed(WKey); ;
#elif XBOX
			return IsButtonPressed(UpButton);
#endif
		}
		public bool IsDownPressed()
		{
#if WINDOWS
			return IsKeyPressed(DownKey) || IsKeyPressed(SKey);
#elif XBOX
			return IsButtonPressed(DownButton);
#endif
		}
		public bool IsLeftPressed()
		{
#if WINDOWS
			return IsKeyPressed(LeftKey) || IsKeyPressed(AKey); ;
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
		#endregion
	}
}
