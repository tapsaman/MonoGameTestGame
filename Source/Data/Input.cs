using Microsoft.Xna.Framework.Input;
using TapsasEngine.Utilities;

namespace ZA6
{
    public static class Input
    {
        /* Wether unused controller is enabled by pulling 
        data each update to check if any key is pressed */
        public static bool AutoEnableController = true;
        public static bool GamePadEnabled;

        public static InputController P1 { get; private set; } = new KeyboardController();
        private static InputController _otherController;

        public static void Update()
        {
            P1.Update();

            if (AutoEnableController && _otherController != null)
            {
                _otherController.Update();
                
                if (_otherController.IsAnyKeyPressed())
                    SwitchControllers();
            }
        }

        public static void DetectGamePadController()
        {
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
            {
                if (GamePad.GetCapabilities(i).IsConnected)
                {
                    _otherController = new GamePadController(i);
                    return;
                }
            }
        }

        public static void EnableGamePadController()
        {
            if (GamePadEnabled)
                return;
            
            DetectGamePadController();
            
            if (_otherController == null)
                throw new System.Exception("No supported game pads found!");
            
            SwitchControllers();
        }

        public static void DisableGamePadController()
        {
            if (!GamePadEnabled)
                return;
            
            SwitchControllers();
        }

        private static void SwitchControllers()
        {
            var otherController = _otherController;
            _otherController = P1;
            
            if (P1 is GamePadController)
            {
                GamePadEnabled = true;
                Static.DevUtils.SetMessage("Gamepad enabled");
            }
            else
            {
                GamePadEnabled = false;
                Static.DevUtils.SetMessage("Gamepad disabled");
            }
        }
    }
}