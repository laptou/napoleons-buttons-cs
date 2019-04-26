using System;
using System.Collections.Generic;

namespace NapoleonsButtons
{
    public abstract class Element
    {
        #region Hierarchy

        public Element? Parent { get; internal set; }

        #endregion

        #region Rendering

        public abstract Render Render(RenderParameters parameters);

        #endregion

        #region Layout

        public abstract Size Measure(LayoutParameters parameters);

        public virtual Size ActualSize { get; private set; }

        // bubbles up the hierarchy
        public virtual void InvalidateSize()
        {
            Parent?.InvalidateSize();
        }

        // tunnels down the hierarchy
        public virtual void UpdateSize(Size newSize)
        {
            ActualSize = newSize;
        }

        #endregion
    }

    public struct LayoutParameters
    {
        public LayoutParameters(Size availableSize)
        {
            AvailableSize = availableSize;
        }

        public Size AvailableSize { get; }
    }

    public interface IHasChildren
    {
        IEnumerable<Element> Children { get; }
    }

    public interface IHasChild
    {
        Element Child { get; }
    }

    public struct RenderParameters
    {
        public Size TargetSize { get; }
    }

    public struct Render
    {
        public string Text { get; }
        public Size Size { get; }

        public Render(string text)
        {
            Text = text;
            Size = new Size(text.Length, 1);
        }

        public Render(string text, Size size)
        {
            Text = text;
            Size = size;
        }
    }

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
    }
}