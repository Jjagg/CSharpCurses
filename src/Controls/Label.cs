using System;

namespace CSharpCurses.Controls
{
    public class Label : Widget
    {
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value.PadRight(_text.Length);
                    RequireDraw = true;
                }
            }
        }
        private string _text = string.Empty;

        public Label(int x, int y, string text)
            : base(x, y)
        {
            Text = text;
        }

        protected override void OnDraw(int x, int y)
        {
            BufferManager.Logger.Log(Logger.Level.Info, $"Drew label '{Text}'.");
            BufferManager.Write(x, y, Text);
        }
    }
}
