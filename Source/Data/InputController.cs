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

        public abstract void Update();
        public abstract bool IsPressed(Keys key);
        public abstract bool JustPressed(Keys key);
        public abstract bool JustReleased(Keys key);
        public abstract bool JustPressedMouseLeft();
    }
}