using System;

namespace CSharpCurses
{
    public class CharBuffer
    {
        public int Size { get; private set; }
        public char[] Buffer { get; private set; }

        public CharBuffer(int size, char c = ' ')
        {
            Size = size;
            Buffer = new char[size];
            for (var i = 0; i < size; i++)
                Buffer[i] = c;
        }

        public char this[int index]
        {
            get => Buffer[index];
            set => Buffer[index] = value;
        }

        public void Resize(int size, char c = ' ')
        {
            // create a new, larger buffer
            var newBuffer = new char[size];
            int i;
            for (i = 0; i < Math.Min(Buffer.Length, size); i++)
                newBuffer[i] = Buffer[i];
            for (; i < newBuffer.Length; i++)
                newBuffer[i] = c;
            Buffer = newBuffer;
            Size = size;
        }

        public void Clear(char c = ' ')
        {
            for (var i = 0; i < Size; i++)
                Buffer[i] = c;
        }
    }
}