using System;

namespace AntBlazor.Docs.Build.Utils
{
    public static class ConsoleUtils
    {
        public static void WriteLine(string message, ConsoleColor foregroundColor)
        {
            var currentForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(message);
            Console.ForegroundColor = currentForegroundColor;
        }

        public static void WriteWelcome()
        {
        }
    }
}