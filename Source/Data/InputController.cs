using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTestGame
{
    public abstract class InputController
    {
        public Keys Up = Keys.Up;
        public Keys Right = Keys.Right;
        public Keys Down = Keys.Down;
        public Keys Left = Keys.Left;
        public Keys A = Keys.Space;
        public Keys B = Keys.LeftAlt;
        public Keys X = Keys.X;
        public Keys Y = Keys.Y;
        public Keys Start = Keys.Enter;
        public Keys Select = Keys.Back;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        public virtual void Update()
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }
        public abstract Vector2 GetDirectionVector();
        public abstract bool IsPressed(Keys key);
        public abstract bool JustPressed(Keys key);
        public abstract bool JustReleased(Keys key);
        public bool IsAnyKeyPressed()
        {
            return (
                Input.P1.IsPressed(Input.P1.A)
             || Input.P1.IsPressed(Input.P1.B)
             || Input.P1.IsPressed(Input.P1.X)
             || Input.P1.IsPressed(Input.P1.Y)
             || Input.P1.IsPressed(Input.P1.Start)
             || Input.P1.IsPressed(Input.P1.Select)
             || Input.P1.GetDirectionVector() != Vector2.Zero
            );
        }
        public bool IsMouseLeftPressed()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed;
        }
        public bool JustPressedMouseLeft()
        {
            return _previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed;
        }
        public bool JustReleasedMouseLeft()
        {
            return  _previousMouseState.LeftButton == ButtonState.Pressed && _currentMouseState.LeftButton == ButtonState.Released;
        }
        public Rectangle GetMouseRectangle()
        {
            return new Rectangle(
                (int)((_currentMouseState.X - Static.Renderer.ScreenX) / Static.Renderer.NativeSizeMultiplier),
                (int)((_currentMouseState.Y - Static.Renderer.ScreenY) / Static.Renderer.NativeSizeMultiplier),
                1,
                1
            );
        }
    }
}