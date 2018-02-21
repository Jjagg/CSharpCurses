using System;

namespace CSharpCurses
{
    public sealed class ScreenBuffer
    {
        private struct Range
        {
            public int Start;
            public int Size;

            public Range(int start, int size)
            {
                Start = start;
                Size = size;
            }

            public int End => Start + Size;
            public bool IsEmpty => Size == 0;
            
            public static Range Empty => new Range();

            public static Range Join(Range r1, Range r2)
            {
                if (r1.IsEmpty)
                    return r2;
                if (r2.IsEmpty)
                    return r1;
                var start = Math.Min(r1.Start, r2.Start);
                return new Range
                {
                    Start = start,
                    Size = Math.Max(r1.End, r2.End) - start
                };
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        private CharBuffer[] _charBuffers;
        private Range[] _invalidated;

        public ScreenBuffer(int width, int height)
        {
            Width = width;
            Height = height;
            _charBuffers = new CharBuffer[height];
            _invalidated = new Range[height];
            for (var i = 0; i < _charBuffers.Length; i++)
                _charBuffers[i] = new CharBuffer(width);
        }

        public void Set(int x, int y, char c)
        {
            if (x < 0 || y < 0 || y >= Height || x >= Width)
                return;
            if (_charBuffers[y][x] == c)
                return;
            _charBuffers[y][x] = c;
            Invalidate(y, x, 1);
        }

        public void SetWidth(int width)
        {
            foreach (var b in _charBuffers)
                b.Resize(width);
            Width = width;
        }

        public void SetHeight(int height)
        {
            _charBuffers = _charBuffers.Resize(height, () => new CharBuffer(Width));
            _invalidated = _invalidated.Resize(height, new Range { Start = 0, Size = Width });
            Height = height;
        }

        public void Clear()
        {
            foreach (var b in _charBuffers)
                b.Clear();
        }

        public void Invalidate()
        {
            Invalidate(0, Height, 0, Width);
        }

        private void Invalidate(int y, int start, int size)
        {
            _invalidated[y] = Range.Join(_invalidated[y], new Range(start, size));
        }

        private void Invalidate(int y, int height, int start, int len)
        {
            for (var i = y; i < y + height && i < Height; i++)
                _invalidated[i] = Range.Join(_invalidated[i], new Range(start, len));
        }

        public void Flush()
        {
            for (var y = 0; y < Height; y++)
            {
                var invRange = _invalidated[y];
                if (!invRange.IsEmpty)
                {
                    Console.SetCursorPosition(invRange.Start, y);
                    Console.Write(_charBuffers[y].Buffer, invRange.Start, invRange.Size);
                    _invalidated[y] = Range.Empty;
                }
            }
        }
    }
}
