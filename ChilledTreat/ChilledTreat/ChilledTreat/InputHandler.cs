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

		public Keys LeftKey = Keys.Left;
		public Keys RightKey = Keys.Right;
		public Keys UpKey = Keys.Up;
		public Keys DownKey = Keys.Down;

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
		public Boolean IsKeyUp(Keys key)
		{
			return KeyboardState.IsKeyUp(key);
		}
		public Boolean IsKeyDown(Keys key)
		{
			return KeyboardState.IsKeyDown(key);
		}
		public Boolean IsKeyPressed(Keys key)
		{
			return IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
		}
		public Boolean IsKeyReleased(Keys key)
		{
			return IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key);
		}
#endif
		#endregion

		#region GamePad
#if !WINDOWSPHONE
		public Boolean IsButtonUp(Buttons button)
		{
			return GamePadState.IsButtonUp(button);
		}
		public Boolean IsButtonDown(Buttons button)
		{
			return GamePadState.IsButtonDown(button);
		}
		public Boolean IsButtonPressed(Buttons button)
		{
			return GamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button);
		}
		public Boolean IsButtonReleased(Buttons button)
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

		public Boolean IsUpDown()
		{
#if WINDOWS
			return IsKeyDown(UpKey) || IsButtonDown(UpButton);
#elif XBOX
			return IsButtonDown(UpButton);
#else 
			return false;
#endif
		}
		public Boolean IsDownDown()
		{
#if WINDOWS
			return IsKeyDown(DownKey) || IsButtonDown(DownButton);
#elif XBOX
			return IsButtonDown(DownButton);
#else
			return false;
#endif
		}
		public Boolean IsLeftDown()
		{
#if WINDOWS
			return IsKeyDown(LeftKey) || IsButtonDown(LeftButton);
#elif XBOX
			return IsButtonDown(LeftButton);
#else
			return false;
#endif
		}
		public Boolean IsRightDown()
		{
#if WINDOWS
			return IsKeyDown(RightKey) || IsButtonDown(RightButton);
#elif XBOX
			return IsButtonDown(RightButton);
#else
			return false;
#endif
		}
		public Boolean IsAbortDown()
		{
#if WINDOWS
			return IsKeyDown(AbortKey) || IsButtonDown(AbortButton);
#elif XBOX
			return IsButtonDown(AbortButton);
#else
			return false;
#endif
		}
		public Boolean IsActionDown()
		{
#if WINDOWS
			return IsKeyDown(ActionKey) || IsButtonDown(ActionButton);
#elif XBOX
			return IsButtonDown(ActionButton);
#else
			return false;
#endif
		}
		public Boolean IsUpUp()
		{
#if WINDOWS
			return IsKeyUp(UpKey) || IsButtonUp(UpButton);
#elif XBOX
			return IsButtonUp(UpButton);
#else
			return false;
#endif
		}
		public Boolean IsLeftUp()
		{
#if WINDOWS
			return IsKeyUp(LeftKey) || IsButtonUp(LeftButton);
#elif XBOX
			return IsButtonUp(LeftButton);
#else
			return false;
#endif
		}
		public Boolean IsRightUp()
		{
#if WINDOWS
			return IsKeyUp(RightKey) || IsButtonUp(RightButton);
#elif XBOX
			return IsButtonUp(RightButton);
#else
			return false;
#endif
		}
		public Boolean IsDownUp()
		{
#if WINDOWS
			return IsKeyUp(DownKey) || IsButtonUp(DownButton);
#elif XBOX
			return IsButtonUp(DownButton);
#else
			return false;
#endif
		}
		public Boolean IsAbortUp()
		{
#if WINDOWS
			return IsKeyUp(AbortKey) || IsButtonUp(AbortButton);
#elif XBOX
			return IsButtonUp(AbortButton);
#else
			return false;
#endif
		}
		public Boolean IsActionUp()
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
/*		public Boolean IsActionPressed()
		{
#if WINDOWS
			return IsKeyPressed(AbortKey);
#elif XBOX
			return IsButtonPressed(AbortButton);
#endif
		}

		public Boolean IsAbortPressed()
		{
#if WINDOWS
			return IsKeyPressed(AbortKey);
#elif XBOX
			return IsButtonPressed(AbortButton);
#endif
		}
		public Boolean IsUpPressed()
		{
#if WINDOWS
			return IsKeyPressed(UpKey);
#elif XBOX
			return IsButtonPressed(UpButton);
#endif
		}
		public Boolean IsDownPressed()
		{
#if WINDOWS
			return IsKeyPressed(DownKey);
#elif XBOX
			return IsButtonPressed(DownButton);
#endif
		}
		public Boolean IsLeftPressed()
		{
#if WINDOWS
			return IsKeyPressed(LeftKey);
#elif XBOX
			return IsButtonPressed(LeftButton);
#endif
		}
		public Boolean IsRightPressed()
		{
#if WINDOWS
			return IsKeyPressed(RightKey);
#elif XBOX
			return IsButtonPressed(RightButton);
#endif
		}*/
	}
}
