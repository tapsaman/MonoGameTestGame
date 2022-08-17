/*using System;
using System.Diagnostics;

namespace ZA6
{
    public static class Sys
    {
        public static void Log(object message)
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
        public static void TimeExcecution(int times, Action function)
        {
            long added = 0;

            for (int i = 0; i < times; i++)
            {
                long start = Stopwatch.GetTimestamp();
                function.Invoke();
                long end = Stopwatch.GetTimestamp();

                added += end - start;
            }

            long average = added / times;
		
			TimeSpan addedTs = new TimeSpan(added);
            TimeSpan averageTs = new TimeSpan(average);

            Console.WriteLine(
                "Ran method " + function + " " + times +
                " times, run time " + addedTs.TotalMilliseconds +
				"ms, average " + averageTs.TotalMilliseconds + "ms"
            );
        }
    }
}*/