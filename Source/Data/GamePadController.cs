using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTestGame
{
    public class GamePadController : InputController
    {
        private GamePadState _currentGamePadState;
        private GamePadState _previousGamePadState;
        private int _index;

        public GamePadController(int controllerIndex)
        {
            _index = controllerIndex;

            Up = (Keys)Buttons.DPadUp;
            Right = (Keys)Buttons.DPadRight;
            Down = (Keys)Buttons.DPadDown;
            Left = (Keys)Buttons.DPadLeft;
            A = (Keys)Buttons.A;
            B = (Keys)Buttons.B;
            X = (Keys)Buttons.X;
            Y = (Keys)Buttons.Y;
            Start = (Keys)Buttons.Start;
            Select = (Keys)Buttons.Back;

            Sys.Debug(GamePad.GetCapabilities(_index).ToString());
        }
        public override void Update()
        {
            base.Update();

            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = GamePad.GetState(_index);

            if (!_currentGamePadState.IsConnected)
            {
                Input.DisableGamePadController();
            }
        }
        public override Vector2 GetDirectionVector()
        {
            Vector2 dir = Vector2.Zero;

            if (_currentGamePadState.IsButtonDown((Buttons)Up))
                dir.Y = -1;
            if (_currentGamePadState.IsButtonDown((Buttons)Down))
                dir.Y = 1;
            if (_currentGamePadState.IsButtonDown((Buttons)Left))
                dir.X = -1;
            if (_currentGamePadState.IsButtonDown((Buttons)Right))
                dir.X = 1;

            if (dir != Vector2.Zero)
            {
                // Ignoring thumb sticks if any D-pad button is pressed
                return dir;
            }

            dir.X = _currentGamePadState.ThumbSticks.Left.X;
            dir.Y = -_currentGamePadState.ThumbSticks.Left.Y;

            return dir;
        }
        public override bool IsPressed(Keys key)
        {
            return _currentGamePadState.IsButtonDown((Buttons)key);
        }
        public override bool JustPressed(Keys key)
        {
            return _previousGamePadState.IsButtonUp((Buttons)key) && _currentGamePadState.IsButtonDown((Buttons)key);
        }
        public override bool JustReleased(Keys key)
        {
            return _previousGamePadState.IsButtonDown((Buttons)key) && _currentGamePadState.IsButtonUp((Buttons)key);
        }
    }
}