using System;

namespace CSharpCurses
{
    public static class Extensions
    {
        /// <summary>
        /// "Hello World " => "ello World H"
        /// </summary>
        public static string MoveRight(this string str, int count = 1)
        {
            var first = str.Substring(0, count);
            return str.Remove(0, count) + first;
        }

        /// <summary>
        /// "Hello World " => " Hello World"
        /// </summary>
        public static string MoveLeft(this string str, int count = 1)
        {
            var start = str.Length - count;
            var first = str.Substring(start, count);
            return str.Remove(start, count).Insert(0, first);
        }

        public static T[] Resize<T>(this T[] ts, int size, T def)
        {
            return Resize(ts, size, () => def);
        }

        public static T[] Resize<T>(this T[] ts, int size, Func<T> def)
        {
            var newBuffer = new T[size];
            int i;
            for (i = 0; i < Math.Min(ts.Length, size); i++)
                newBuffer[i] = ts[i];
            for (; i < newBuffer.Length; i++)
                newBuffer[i] = def();
            return newBuffer;
        }
    }
}
