using Microsoft.Xna.Framework.Input;

namespace MonoGameTestGame
{
    public static class Input
    {
        public static InputController P1 { get; private set; } = new KeyboardController();

        public static void EnableGamePadController()
        {
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
            {
                if (GamePad.GetCapabilities(i).IsConnected)
                {
                    P1 = new GamePadController(i);
                    Static.GamePadEnabled = true;
                    return;
                }
            }
            throw new System.Exception("No supported game pads found!");
        }

        public static void DisableGamePadController()
        {
            P1 = new KeyboardController();
            P1.Update();
            Static.GamePadEnabled = false;
        }
    }
}