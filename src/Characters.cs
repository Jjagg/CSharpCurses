using BE = CSharpCurses.BlockElements;

namespace CSharpCurses
{
    internal static class Border
    {
        public static readonly int Empty = 0;
        public static readonly int TopLeft = 1;
        public static readonly int Top = 2;
        public static readonly int TopRight = 3;
        public static readonly int Right = 4;
        public static readonly int BotRight = 5;
        public static readonly int Bot = 6;
        public static readonly int BotLeft = 7;
        public static readonly int Left = 8;
    }

    public static class Borders
    {
        public static readonly char[] ThickBorder =
        {
            ' ' , BE.Full , BE.UpperHalf , BE.Full ,
            BE.Full, BE.Full , BE.LowerHalf ,
            BE.Full, BE.Full
        };
        public static readonly char[] ThinBorder =
        {
            ' ' , BE.UpperLeft , BE.UpperOneEight , BE.UpperRight ,
            BE.RightHalf, BE.LowerRight , BE.LowerOneEight ,
            BE.LowerLeft, BE.LeftHalf
        };
    }

    internal static class BlockElements
    {
        public static readonly char Full = '\u2588';
        public static readonly char UpperHalf = '\u2580';
        public static readonly char LowerHalf = '\u2584';
        public static readonly char LeftHalf = '\u258C';
        public static readonly char RightHalf = '\u2590';
        public static readonly char UpperLeft = '\u259B';
        public static readonly char UpperRight = '\u259C';
        public static readonly char LowerRight = '\u259F';
        public static readonly char LowerLeft = '\u2599';
        
        public static readonly char LeftOneEight = '\u258F';
        public static readonly char RightOneEight = '\u2595';
        public static readonly char UpperOneEight = '\u2594';
        public static readonly char LowerOneEight = '\u2581';
    }
}