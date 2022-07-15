using Microsoft.Xna.Framework.Input;

namespace MonoGameTestGame
{
    public static class Input
    {
        public static Keys Up = Keys.Up;
        public static Keys Right = Keys.Right;
        public static Keys Down = Keys.Down;
        public static Keys Left = Keys.Left;
        public static Keys A = Keys.Space;
        public static Keys B = Keys.LeftAlt;
        public static Keys X = Keys.X;
        public static Keys Y = Keys.Y;
        public static Keys Start = Keys.Enter;
        public static Keys Select = Keys.Back;
        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _previousKeyboardState;
        private static MouseState _currentMouseState;
        private static MouseState _previousMouseState;
        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }
        public static bool IsPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }
        public static bool JustPressed(Keys key)
        {
            return _previousKeyboardState.IsKeyUp(key) && _currentKeyboardState.IsKeyDown(key);
        }
        public static bool JustReleased(Keys key)
        {
            return _previousKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key);
        }
        public static bool JustPressedMouseLeft()
        {
            return _previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed;
        }
    }
}