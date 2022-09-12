using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TapsasEngine.Utilities;

namespace TapsasEngine.Utilities
{
    public abstract class InputController
    {
        public static PointStretchRenderer Renderer;
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
        private static Keys[] _numberKeys =
        {
            Keys.D0,
            Keys.D1,
            Keys.D2,
            Keys.D3,
            Keys.D4,
            Keys.D5,
            Keys.D6,
            Keys.D7,
            Keys.D8,
            Keys.D9
        };

        public virtual void Update()
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }
        public abstract Vector2 GetDirectionVector();
        public abstract bool IsPressed(Keys key);
        public abstract bool JustPressed(Keys key);
        public abstract bool JustReleased(Keys key);
        public abstract bool IsAnyKeyPressed();
        public bool IsAnyDefinedKeyPressed()
        {
            return (
                IsPressed(A)
             || IsPressed(B)
             || IsPressed(X)
             || IsPressed(Y)
             || IsPressed(Start)
             || IsPressed(Select)
             || GetDirectionVector() != Vector2.Zero
            );
        }
        public int? AnyNumberKeyJustPressed()
        {
            for (int i = 0; i < _numberKeys.Length; i++)
            {
                if (JustPressed(_numberKeys[i]))
                {
                    return i;
                }
            }
            return null;
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
        public bool IsMouseMoving()
        {
            return  _currentMouseState.Position != _previousMouseState.Position;
        }
        public Rectangle GetMouseRectangle()
        {
            return new Rectangle(
                (int)((_currentMouseState.X - Renderer.ScreenRectangle.X) / Renderer.NativeSizeMultiplier.X),
                (int)((_currentMouseState.Y - Renderer.ScreenRectangle.Y) / Renderer.NativeSizeMultiplier.Y),
                1,
                1
            );
        }
    }
}