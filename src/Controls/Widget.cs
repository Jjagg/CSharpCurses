using System;

namespace CSharpCurses.Controls
{

    public abstract class Widget
    {
        public int X { get; set; }
        public int Y { get; set; }

        public virtual ConsoleColor? Foreground
        {
            get => _foreground;
            set
            {
                if (_foreground != value)
                {
                    _foreground = value;
                    RequireDraw = true;
                }
            }
        }
        private ConsoleColor? _foreground;

        public virtual ConsoleColor? Background
        {
            get => _background;
            set
            {
                if (_background != value)
                {
                    _background = value;
                    RequireDraw = true;
                }
            }
        }
        private ConsoleColor? _background;

        protected bool RequireDraw = true;

        public Widget(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Draw(int x, int y, bool force = false)
        {
            if (RequireDraw || force)
            {
                ConsoleApp.PushState();

                if (_foreground.HasValue) Console.ForegroundColor = _foreground.Value;
                if (_background.HasValue) Console.BackgroundColor = _background.Value;

                OnDraw(X + x, Y + y);

                ConsoleApp.PopState();

                RequireDraw = false;
            }
        }

        public virtual void Update()
        {
        }

        protected abstract void OnDraw(int x, int y);
    }
}
