using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TapsasEngine.Utilities;

namespace ZA6
{
    public class KeyboardController : InputController
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

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
            Select = Keys.Escape;
        }
        public override void Update()
        {
            base.Update();
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
        }
        public override Vector2 GetDirectionVector()
        {
            Vector2 dir = Vector2.Zero;

            if (_currentKeyboardState.IsKeyDown(Up))
                dir.Y = -1;
            if (_currentKeyboardState.IsKeyDown(Down))
                dir.Y = 1;
            if (_currentKeyboardState.IsKeyDown(Left))
                dir.X = -1;
            if (_currentKeyboardState.IsKeyDown(Right))
                dir.X = 1;

            return dir;
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
    }
}