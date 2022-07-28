using Microsoft.Xna.Framework.Input;

namespace MonoGameTestGame
{
    public class KeyboardController : InputController
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        public KeyboardController()
        {
            Up = Keys.Up;
            Right = Keys.Right;
            Down = Keys.Down;
            Left = Keys.Left;
            A = Keys.Space;
            B = Keys.LeftAlt;
            X = Keys.X;
            Y = Keys.Y;
            Start = Keys.Enter;
            Select = Keys.Back;
        }
        public override void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }
        public override bool IsPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }
        public override bool JustPressed(Keys key)
        {
            return _previousKeyboardState.IsKeyUp(key) && _currentKeyboardState.IsKeyDown(key);
        }
        public override bool JustReleased(Keys key)
        {
            return _previousKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key);
        }
        public override bool JustPressedMouseLeft()
        {
            return _previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed;
        }
    }
}