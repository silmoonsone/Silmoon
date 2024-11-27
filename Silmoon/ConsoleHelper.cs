using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon
{
    public static class ConsoleHelper
    {
        public static class Foreground
        {
            public const string Black = "\x1b[30m";
            public const string DarkGray = "\x1b[90m";
            public const string Red = "\x1b[31m";
            public const string DarkRed = "\x1b[91m";
            public const string Green = "\x1b[32m";
            public const string DarkGreen = "\x1b[92m";
            public const string Yellow = "\x1b[33m";
            public const string DarkYellow = "\x1b[93m";
            public const string Blue = "\x1b[34m";
            public const string DarkBlue = "\x1b[94m";
            public const string Magenta = "\x1b[35m";
            public const string DarkMagenta = "\x1b[95m";
            public const string Cyan = "\x1b[36m";
            public const string DarkCyan = "\x1b[96m";
            public const string Gray = "\x1b[37m";
            public const string White = "\x1b[97m";
        }

        public static class Background
        {
            public const string Black = "\x1b[40m";
            public const string DarkGray = "\x1b[100m";
            public const string Red = "\x1b[41m";
            public const string DarkRed = "\x1b[101m";
            public const string Green = "\x1b[42m";
            public const string DarkGreen = "\x1b[102m";
            public const string Yellow = "\x1b[43m";
            public const string DarkYellow = "\x1b[103m";
            public const string Blue = "\x1b[44m";
            public const string DarkBlue = "\x1b[104m";
            public const string Magenta = "\x1b[45m";
            public const string DarkMagenta = "\x1b[105m";
            public const string Cyan = "\x1b[46m";
            public const string DarkCyan = "\x1b[106m";
            public const string Gray = "\x1b[47m";
            public const string White = "\x1b[107m";
        }

        // ANSI Reset Code
        public const string Reset = "\x1b[0m";

        // Mapping ConsoleColor to ANSI Codes
        private static readonly Dictionary<ConsoleColor, string> ForegroundColorMap = new Dictionary<ConsoleColor, string>()
        {
            { ConsoleColor.Black, Foreground.Black },
            { ConsoleColor.DarkGray, Foreground.DarkGray },
            { ConsoleColor.Red, Foreground.Red },
            { ConsoleColor.DarkRed, Foreground.DarkRed },
            { ConsoleColor.Green, Foreground.Green },
            { ConsoleColor.DarkGreen, Foreground.DarkGreen },
            { ConsoleColor.Yellow, Foreground.Yellow },
            { ConsoleColor.DarkYellow, Foreground.DarkYellow },
            { ConsoleColor.Blue, Foreground.Blue },
            { ConsoleColor.DarkBlue, Foreground.DarkBlue },
            { ConsoleColor.Magenta, Foreground.Magenta },
            { ConsoleColor.DarkMagenta, Foreground.DarkMagenta },
            { ConsoleColor.Cyan, Foreground.Cyan },
            { ConsoleColor.DarkCyan, Foreground.DarkCyan },
            { ConsoleColor.Gray, Foreground.Gray },
            { ConsoleColor.White, Foreground.White }
        };

        private static readonly Dictionary<ConsoleColor, string> BackgroundColorMap = new Dictionary<ConsoleColor, string>()
        {
            { ConsoleColor.Black, Background.Black },
            { ConsoleColor.DarkGray, Background.DarkGray },
            { ConsoleColor.Red, Background.Red },
            { ConsoleColor.DarkRed, Background.DarkRed },
            { ConsoleColor.Green, Background.Green },
            { ConsoleColor.DarkGreen, Background.DarkGreen },
            { ConsoleColor.Yellow, Background.Yellow },
            { ConsoleColor.DarkYellow, Background.DarkYellow },
            { ConsoleColor.Blue, Background.Blue },
            { ConsoleColor.DarkBlue, Background.DarkBlue },
            { ConsoleColor.Magenta, Background.Magenta },
            { ConsoleColor.DarkMagenta, Background.DarkMagenta },
            { ConsoleColor.Cyan, Background.Cyan },
            { ConsoleColor.DarkCyan, Background.DarkCyan },
            { ConsoleColor.Gray, Background.Gray },
            { ConsoleColor.White, Background.White }
        };

        public static string GetAnsiForegroundColorString(ConsoleColor color) => ForegroundColorMap.TryGetValue(color, out var code) ? code : Reset;
        public static string GetAnsiBackgroundColorString(ConsoleColor color) => BackgroundColorMap.TryGetValue(color, out var code) ? code : Reset;
        public static string GetAnsiColorString(ConsoleColor foregroundColor, ConsoleColor backgroundColor) => GetAnsiForegroundColorString(foregroundColor) + GetAnsiBackgroundColorString(backgroundColor);

        public static void ResetAnsiColor() => Console.Write(Reset);
        public static void WriteWithColor(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            Console.Write(WarpStringANSIColor(text, foregroundColor, backgroundColor));
        }
        public static void WriteLineWithColor(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            Console.WriteLine(WarpStringANSIColor(text, foregroundColor, backgroundColor));
        }
        public static string WarpStringANSIColor(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            return GetAnsiColorString(foregroundColor ?? Console.ForegroundColor, backgroundColor ?? Console.BackgroundColor) + text + Reset;
        }
    }
}
