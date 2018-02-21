using System;

namespace CSharpCurses.Controls
{
    public class Canvas : Widget
    {
        public int Width { get; }
        public int Height { get; }

        public Canvas(int x, int y, int width, int height)
            : base(x, y)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public void DrawChar(int x, int y, char c)
        {
            var cl = Console.CursorLeft;
            var ct = Console.CursorTop;

            Console.SetCursorPosition(X + x, Y + y);
            Console.Write(c);

            Console.SetCursorPosition(cl, ct);
        }

        public void DrawChar(int x, int y, char c, ConsoleColor fg, ConsoleColor bg)
        {
            var cl = Console.CursorLeft;
            var ct = Console.CursorTop;
            var cf = Console.ForegroundColor;
            var cb = Console.BackgroundColor;

            Console.SetCursorPosition(X + x, Y + y);
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.Write(c);

            Console.SetCursorPosition(cl, ct);
            Console.ForegroundColor = cf;
            Console.BackgroundColor = cb;
        }
        
        protected override void OnDraw(int x, int y)
        {
        }

        public void Clear()
        {
            var previousFg = Console.ForegroundColor;
            var previousBg = Console.BackgroundColor;
            if (Foreground.HasValue) Console.ForegroundColor = Foreground.Value; 
            if (Background.HasValue) Console.BackgroundColor = Background.Value; 

            var blank = new string(' ', Width);
            for (var dy = 0; dy < Height; dy++)
                BufferManager.Write(X, Y + dy, blank);

            Console.ForegroundColor = previousFg;
            Console.BackgroundColor = previousBg;
        }
    }
}