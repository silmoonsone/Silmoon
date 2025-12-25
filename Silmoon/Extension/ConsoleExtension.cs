using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class ConsoleExtension
    {
        public static class Foreground
        {
            public const string Black = "\x1b[30m";
            public const string Red = "\x1b[31m";
            public const string Green = "\x1b[32m";
            public const string Yellow = "\x1b[33m";
            public const string Blue = "\x1b[34m";
            public const string Magenta = "\x1b[35m";
            public const string Cyan = "\x1b[36m";
            public const string Gray = "\x1b[37m";

            public const string BrightBlack = "\x1b[90m";   // 常见也叫 DarkGray
            public const string BrightRed = "\x1b[91m";
            public const string BrightGreen = "\x1b[92m";
            public const string BrightYellow = "\x1b[93m";
            public const string BrightBlue = "\x1b[94m";
            public const string BrightMagenta = "\x1b[95m";
            public const string BrightCyan = "\x1b[96m";
            public const string White = "\x1b[97m";
        }
        public static class Background
        {
            public const string Black = "\x1b[40m";
            public const string Red = "\x1b[41m";
            public const string Green = "\x1b[42m";
            public const string Yellow = "\x1b[43m";
            public const string Blue = "\x1b[44m";
            public const string Magenta = "\x1b[45m";
            public const string Cyan = "\x1b[46m";
            public const string Gray = "\x1b[47m";

            public const string BrightBlack = "\x1b[100m";
            public const string BrightRed = "\x1b[101m";
            public const string BrightGreen = "\x1b[102m";
            public const string BrightYellow = "\x1b[103m";
            public const string BrightBlue = "\x1b[104m";
            public const string BrightMagenta = "\x1b[105m";
            public const string BrightCyan = "\x1b[106m";
            public const string White = "\x1b[107m";
        }


        // Reset / Clear
        public const string Reset = "\x1b[0m";

        // 只清前景/背景（很常用：避免误伤其它样式）
        public const string ResetForeground = "\x1b[39m";
        public const string ResetBackground = "\x1b[49m";

        // Mapping ConsoleColor to ANSI Codes
        private static readonly Dictionary<ConsoleColor, string> ForegroundColorMap = new Dictionary<ConsoleColor, string>()
        {
            { ConsoleColor.Black, Foreground.Black },

            { ConsoleColor.DarkBlue, Foreground.Blue },
            { ConsoleColor.DarkGreen, Foreground.Green },
            { ConsoleColor.DarkCyan, Foreground.Cyan },
            { ConsoleColor.DarkRed, Foreground.Red },
            { ConsoleColor.DarkMagenta, Foreground.Magenta },
            { ConsoleColor.DarkYellow, Foreground.Yellow },
            { ConsoleColor.Gray, Foreground.Gray },

            { ConsoleColor.DarkGray, Foreground.BrightBlack },
            { ConsoleColor.Blue, Foreground.BrightBlue },
            { ConsoleColor.Green, Foreground.BrightGreen },
            { ConsoleColor.Cyan, Foreground.BrightCyan },
            { ConsoleColor.Red, Foreground.BrightRed },
            { ConsoleColor.Magenta, Foreground.BrightMagenta },
            { ConsoleColor.Yellow, Foreground.BrightYellow },
            { ConsoleColor.White, Foreground.White }
        };

        private static readonly Dictionary<ConsoleColor, string> BackgroundColorMap = new Dictionary<ConsoleColor, string>()
        {
            { ConsoleColor.Black, Background.Black },

            { ConsoleColor.DarkBlue, Background.Blue },
            { ConsoleColor.DarkGreen, Background.Green },
            { ConsoleColor.DarkCyan, Background.Cyan },
            { ConsoleColor.DarkRed, Background.Red },
            { ConsoleColor.DarkMagenta, Background.Magenta },
            { ConsoleColor.DarkYellow, Background.Yellow },
            { ConsoleColor.Gray, Background.Gray },

            { ConsoleColor.DarkGray, Background.BrightBlack },
            { ConsoleColor.Blue, Background.BrightBlue },
            { ConsoleColor.Green, Background.BrightGreen },
            { ConsoleColor.Cyan, Background.BrightCyan },
            { ConsoleColor.Red, Background.BrightRed },
            { ConsoleColor.Magenta, Background.BrightMagenta },
            { ConsoleColor.Yellow, Background.BrightYellow },
            { ConsoleColor.White, Background.White }
        };


        // 256-color / TrueColor
        public static string Foreground256(int index) => $"\x1b[38;5;{index}m";
        public static string Background256(int index) => $"\x1b[48;5;{index}m";

        public static string ForegroundRgb(byte r, byte g, byte b) => $"\x1b[38;2;{r};{g};{b}m";
        public static string BackgroundRgb(byte r, byte g, byte b) => $"\x1b[48;2;{r};{g};{b}m";


#if NET10_0_OR_GREATER
        extension(Console)
        {
            public static string GetAnsiForegroundColorString(ConsoleColor color) => ForegroundColorMap.TryGetValue(color, out var code) ? code : ResetForeground;
            public static string GetAnsiBackgroundColorString(ConsoleColor color) => BackgroundColorMap.TryGetValue(color, out var code) ? code : ResetBackground;
            public static string GetAnsiColorString(ConsoleColor foregroundColor, ConsoleColor backgroundColor) => string.Concat(GetAnsiForegroundColorString(foregroundColor), GetAnsiBackgroundColorString(backgroundColor));

            public static void ResetAnsiColor() => Console.Write(Reset);
            public static void WriteWithColor(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null) => Console.Write(WarpStringANSIColor(text, foregroundColor, backgroundColor));
            public static void WriteLineWithColor(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null) => Console.WriteLine(WarpStringANSIColor(text, foregroundColor, backgroundColor));
            public static string WarpStringANSIColor(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null) => string.Concat(GetAnsiColorString(foregroundColor ?? Console.ForegroundColor, backgroundColor ?? Console.BackgroundColor), text, ResetForeground, ResetBackground);
        }
#else
        public static string GetAnsiForegroundColorString(ConsoleColor color) => ForegroundColorMap.TryGetValue(color, out var code) ? code : ResetForeground;
        public static string GetAnsiBackgroundColorString(ConsoleColor color) => BackgroundColorMap.TryGetValue(color, out var code) ? code : ResetBackground;
        public static string GetAnsiColorString(ConsoleColor foregroundColor, ConsoleColor backgroundColor) => string.Concat(GetAnsiForegroundColorString(foregroundColor), GetAnsiBackgroundColorString(backgroundColor));

        public static void ResetAnsiColor() => Console.Write(Reset);
        public static void WriteWithColor(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null) => Console.Write(WarpStringANSIColor(text, foregroundColor, backgroundColor));
        public static void WriteLineWithColor(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null) => Console.WriteLine(WarpStringANSIColor(text, foregroundColor, backgroundColor));
        public static string WarpStringANSIColor(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null) => string.Concat(GetAnsiColorString(foregroundColor ?? Console.ForegroundColor, backgroundColor ?? Console.BackgroundColor), text, ResetForeground, ResetBackground);
#endif

        /// <summary>
        /// Text styles
        /// </summary>
        public static class Style
        {
            public const string Bold = "\x1b[1m";
            public const string Dim = "\x1b[2m";
            public const string Italic = "\x1b[3m";
            public const string Underline = "\x1b[4m";
            public const string Blink = "\x1b[5m";
            public const string Reverse = "\x1b[7m";
            public const string Hidden = "\x1b[8m";
            public const string Strike = "\x1b[9m";

            // partial reset
            public const string NormalIntensity = "\x1b[22m"; // not bold/dim
            public const string NoItalic = "\x1b[23m";
            public const string NoUnderline = "\x1b[24m";
            public const string NoReverse = "\x1b[27m";
            public const string NoHidden = "\x1b[28m";
            public const string NoStrike = "\x1b[29m";
        }

        /// <summary>
        /// Cursor / screen (CLI 很实用)
        /// </summary>
        public static class Cursor
        {
            public const string Home = "\x1b[H";
            public const string ClearScreen = "\x1b[2J";
            public const string ClearLine = "\x1b[2K";
            public const string Hide = "\x1b[?25l";
            public const string Show = "\x1b[?25h";

            public static string Up(int n) => $"\x1b[{n}A";
            public static string Down(int n) => $"\x1b[{n}B";
            public static string Right(int n) => $"\x1b[{n}C";
            public static string Left(int n) => $"\x1b[{n}D";
            public static string MoveTo(int row1Based, int col1Based) => $"\x1b[{row1Based};{col1Based}H";
        }
    }
}
