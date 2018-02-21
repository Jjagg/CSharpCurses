using System;
using System.Collections.Generic;

namespace CSharpCurses.Controls
{
    public sealed class Screen
    {
       private readonly List<Widget> _widgets = new List<Widget>();

        private int _lastWidth;
        private int _lastHeight;
        private bool _hasBorder;

        public event EventHandler<ResizedEventArgs> Resized;

        public bool HasBorder
        {
            get => _hasBorder;
            set
            {
                if (_hasBorder == value)
                    return;
                Redraw();
                _hasBorder = value;
            }
        }

        internal Screen()
        {
        }

        internal void StartRun()
        {
            SetLastSize();
        }

        private void SetLastSize()
        {
            _lastWidth = BufferManager.Width;
            _lastHeight = BufferManager.Height;
        }

        public void Update()
        {
            if (_lastWidth != BufferManager.Width || _lastHeight != BufferManager.Height)
            {
                BufferManager.Logger.Log(Logger.Level.Info, $"Resized from ({_lastWidth}, {_lastHeight}) to ({BufferManager.Width}, {BufferManager.Height}).");
                BufferManager.UpdateSize();
                OnResized();
                SetLastSize();
                Redraw();
            }
            foreach (var item in _widgets)
                item.Update();
        }

        public void Redraw()
        {
            BufferManager.Clear();
            foreach (var item in _widgets)
                item.Draw(0, 0, true);
            if (HasBorder)
                DrawBorder();
        }

        public void Draw()
        {
            foreach (var item in _widgets)
                item.Draw(0, 0);
            if (HasBorder)
                DrawBorder();
        }

        public void Add(Widget item)
        {
            _widgets.Add(item);
        }

        public void Remove(Widget item)
        {
            _widgets.Remove(item);
        }

        private void DrawBorder()
        {
            BufferManager.DrawBorder(Borders.ThinBorder);
        }

        private void OnResized()
        {
            var e = new ResizedEventArgs(_lastWidth, _lastHeight, BufferManager.Width, BufferManager.Height);
            Resized?.Invoke(this, e);
        }
    }
}
