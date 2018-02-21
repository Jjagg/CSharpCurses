using System;
using System.Diagnostics;

namespace CSharpCurses.Controls
{
    public class BlockProgressBar : Widget
    {
        private readonly bool _vertical;
        private int _size; 
        private float _progress;

        public int Size
        {
            get => _size;
            set
            { if (_size != value)
                {
                    _size = value;
                    RequireDraw = true;
                }
            }
        }

        public float Progress
        {
            get => _progress;
            set
            {
                var newValue = Math.Min(Math.Max(value, 0.0f), 1.0f);
                _progress = newValue;
                RequireDraw = true;
            }
        }

        private static readonly char[][] _blocks =
        {
            new []{' ', '\u2581', '\u2582', '\u2583', '\u2584', '\u2585', '\u2586', '\u2587'}, // vertical
            new []{' ', '\u258F', '\u258E', '\u258D', '\u258C', '\u258B', '\u258A', '\u2589'}, // horizontal
        };

        public BlockProgressBar(int x, int y, int w, bool vertical = false)
            : base(x, y)
        {
            Debug.Assert(w > 3);
            Size = w;
            _vertical = vertical;
        }

        protected override void OnDraw(int x, int y)
        {
            BufferManager.CursorX = x;
            BufferManager.CursorY = y;

            // Width - Border - Text
            var fullBlocks = (int) (Size * _progress);

            if (_vertical)
            {
                var lastBlockIndex = (int) (Size * Progress * 8) % 8;
                BufferManager.LineVer(' ', Math.Max(Size - fullBlocks - 1, 0));
                BufferManager.WriteVertical(_blocks[0][lastBlockIndex]);
                BufferManager.WriteVertical(new string(BlockElements.Full, fullBlocks));
            }
            else
            {
                var lastBlockIndex = (int) ((Size - 2) * Progress * 8) % 8;
                Console.Write(new string(BlockElements.Full, fullBlocks));
                Console.Write(_blocks[1][lastBlockIndex]);
                Console.Write(new string(' ', (Size - 2) - fullBlocks - 1));
            }
        }
    }
}
