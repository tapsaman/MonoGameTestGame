using System;

namespace ZA6
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
        public static void Debug(string message)
        {
            Console.WriteLine("*DEBUG* " + message);
        }
    }
}