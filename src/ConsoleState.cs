using System;

namespace CSharpCurses
{
    public struct ConsoleState
    {
        public int CursorX { get; }
        public int CursorY { get; }
        public ConsoleColor Foreground { get; }
        public ConsoleColor Background { get; }

        public ConsoleState(int cursorX, int cursorY, ConsoleColor foreground, ConsoleColor background)
        {
            CursorX = cursorX;
            CursorY = cursorY;
            Foreground = foreground;
            Background = background;
        }

        public static ConsoleState Create()
        {
            return new ConsoleState(Console.CursorLeft, Console.CursorTop,
                Console.ForegroundColor, Console.BackgroundColor);
        }
    }
}