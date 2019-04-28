using System;
using System.Diagnostics;

namespace NapoleonsButtons.UI
{
    [DebuggerDisplay("< width: {Width}, height: {Height} >")]
    public struct Size
    {
        public int Width { get; }
        public int Height { get; }

        public static readonly Size Zero = new Size(0, 0);

        public Size(int width, int height)
        {
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 0) throw new ArgumentOutOfRangeException(nameof(height));

            Width = width;
            Height = height;
        }

        public void Deconstruct(out int width, out int height)
        {
            width = Width;
            height = Height;
        }
    }
}