using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NapoleonsButtons.UI
{
    public abstract class Element
    {
        #region Hierarchy

        public Element? Parent { get; internal set; }

        #endregion

        #region Rendering

        public virtual void InvalidateRender()
        {
            Parent?.InvalidateRender();
            MeasureInvalidated?.Invoke(this, null);
        }

        public virtual void OnRenderInvalidated()
        {
            RenderInvalidated?.Invoke(this, null);
        }

        public abstract Render Render(RenderParameters parameters);

        #endregion

        #region Layout

        public event EventHandler MeasureInvalidated;
        public event EventHandler RenderInvalidated;

        public virtual Size DesiredSize { get; protected set; }

        public virtual Size GivenSize { get; private set; }

        public virtual void Measure(Size availableSize)
        {
            DesiredSize = MeasureOverride(availableSize);
        }

        protected abstract Size MeasureOverride(Size availableSize);

        public virtual void InvalidateMeasure()
        {
            Parent?.InvalidateMeasure();
        }

        #endregion
    }

    public abstract class WrapperBase : Element, IHasChild
    {
        private Element _child;

        public Element Child
        {
            get => _child;
            set
            {
                _child = value;
                InvalidateMeasure();
            }
        }
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
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        IEnumerable<Element> Children { get; }

        void AddChild(Element element);
        void RemoveChild(Element element);
    }

    public interface IHasChild
    {
        Element Child { get; set; }
    }

    public struct RenderParameters
    {
        public RenderParameters(Size targetSize, Size screenSize)
        {
            TargetSize = targetSize;
            ScreenSize = screenSize;
        }

        public Size TargetSize { get; }
        public Size ScreenSize { get; }
    }

    [DebuggerDisplay("< width: {Width}, height: {Height} >")]
    public struct Render
    {
        public string[] Buffer { get; }
        public Size Size { get; }

        public Render(string buffer)
        {
            // buffer should not contain \n

            Buffer = new string[] {buffer};
            Size = new Size(buffer.Length, 1);
        }

        public Render(string[] buffer, Size size)
        {
            Buffer = buffer;
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

        public void Deconstruct(out int width, out int height)
        {
            width = Width;
            height = Height;
        }
    }
}