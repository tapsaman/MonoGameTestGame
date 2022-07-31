using System;

namespace MonoGameTestGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ZeldaAdventure666())
                game.Run();
        }
    }
}
