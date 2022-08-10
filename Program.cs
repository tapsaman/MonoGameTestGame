using System;

namespace ZA6
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
