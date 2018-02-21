using System;
using System.Diagnostics;

namespace CSharpCurses.Controls
{
    public class ProgressBar : Widget
    {
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
        private float _progress;

        public int Width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    RequireDraw = true;
                }
            }
        }
        private int _width; 

        public ProgressBar(int x, int y, int w)
            : base(x, y)
        {
            Debug.Assert(w > 3);

            Width = w;
        }

        protected override void OnDraw(int x, int y)
        {
            Console.SetCursorPosition(x, y);

            Console.Write("[");

            // Width - Border - Text
            int progressWidth = Width - 2 - 5;
            int progressLength = (int)(progressWidth * _progress);

            Console.Write(new string('=', progressLength) + new string(' ', progressWidth - progressLength));
            Console.Write("] " + string.Format("{0:P0}", _progress).PadLeft(4)); // "100%"
        }
    }
}
