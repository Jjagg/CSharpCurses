using System;

namespace CSharpCurses.Controls
{
    public class ResizedEventArgs : EventArgs
    {
        public readonly int OldWidth;
        public readonly int OldHeight;
        public readonly int NewWidth;
        public readonly int NewHeight;

        public ResizedEventArgs(int oldWidth, int oldHeight, int newWidth, int newHeight)
        {
            OldWidth = oldWidth;
            OldHeight = oldHeight;
            NewWidth = newWidth;
            NewHeight = newHeight;
        }
    }
}