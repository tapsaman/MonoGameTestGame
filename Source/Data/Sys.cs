using System;

namespace MonoGameTestGame
{
    public static class Sys
    {
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
        public static void LogError(string message)
        {
            Console.WriteLine("*ERROR* " + message);
        }
    }
}