using System;

namespace CSharpCurses
{
    public static class BufferManager
    {
        public static int CursorX { get; set; }
        public static int CursorY { get; set; }

        private static ScreenBuffer _screenBuffer;

        public static int Width => Console.WindowWidth;
        public static int Height => Console.WindowHeight;

        public static readonly Logger Logger;

        static BufferManager()
        {
            Logger = new Logger();
        }

        public static void Initialize()
        {
            if (_screenBuffer == null)
                _screenBuffer = new ScreenBuffer(Width, Height);
            else
                UpdateSize();
        }

        public static void Invalidate()
        {
            _screenBuffer.Invalidate();
        }

        public static void Redraw()
        {
            Invalidate();
            Flush();
        }

        public static void Clear()
        {
            _screenBuffer.Clear();
        }
        
        public static void Flush()
        {
            _screenBuffer.Flush();
        }

        public static void UpdateSize()
        {
            _screenBuffer.SetWidth(Width);
            _screenBuffer.SetHeight(Height);
            _screenBuffer.Invalidate();
        }
        
        #region Write Methods

        public static void Write(char c)
        {
            _screenBuffer.Set(CursorX, CursorY, c);
            CursorX++;
        }

        public static void Write(string str)
        {
            foreach (var c in str)
                Write(c);
        }

        public static void Write(int x, int y, char c)
        {
            CursorX = x;
            CursorY = y;
            Write(c);
        }

        public static void Write(int x, int y, string str)
        {
            CursorX = x;
            CursorY = y;
            Write(str);
        }

        public static void WriteVertical(char c)
        {
            _screenBuffer.Set(CursorX, CursorY, c);
            CursorY++;
        }

        public static void WriteVertical(string str)
        {
            foreach (var c in str)
                WriteVertical(c);
        }

        public static void WriteVertical(int x, int y, string str)
        {
            CursorX = x;
            CursorY = y;
            WriteVertical(str);
        }

        #endregion

        public static void LineHor(char c, int width)
        {
            Write(new string(c, width));
        }

        public static void LineHor(char c, int x, int y, int width)
        {
            CursorX = x;
            CursorY = y;
            LineHor(c, width);
        }

        public static void LineVer(char c, int height)
        {
            WriteVertical(new string(c, height));
        }

        public static void LineVer(char c, int x, int y, int height)
        {
            CursorX = x;
            CursorY = y;
            LineVer(c, height);
        }

        public static void DrawBorder(char[] border)
        {
            DrawBorder(border, 0, 0, Width, Height);
            Logger.Log(Logger.Level.Info, "Drew border.");
        }

        public static void DrawBorder(char[] border, int x, int y, int width, int height)
        {
            var strUp = new string(border[Border.Top], width - 2);
            var strLow = new string(border[Border.Bot], width - 2);
            var strLeft = new string(border[Border.Left], height - 2);
            var strRight = new string(border[Border.Right], height - 2);
            Write(x + 1, y, strUp);
            Write(x + 1, y + height - 1, strLow);
            WriteVertical(x, y + 1, strLeft);
            WriteVertical(x + width - 1, y + 1, strRight);
            Write(x, y, border[Border.TopLeft]);
            Write(x + width - 1, y, border[Border.TopRight]);
            Write(x + width - 1, y + height - 1, border[Border.BotRight]);
            Write(x, y + height - 1, border[Border.BotLeft]);
        }
    }
}
