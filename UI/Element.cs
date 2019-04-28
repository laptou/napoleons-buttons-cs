using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NapoleonsButtons.UI
{
    public abstract class Element
    {
        #region Hierarchy

        public Element? Parent { get; internal set; }

        #endregion

        #region Rendering

        public void InvalidateRender()
        {
            Parent?.InvalidateRender();
            RenderInvalidated?.Invoke(this, null);
        }

        public abstract Render Render(RenderParameters parameters);

        public event EventHandler RenderInvalidated;

        #endregion

        #region Layout

        public event EventHandler MeasureInvalidated;

        public virtual Size DesiredSize { get; private set; }

        public virtual Size GivenSize { get; private set; }

        public virtual Size Measure(Size availableSize)
        {
            DesiredSize = MeasureOverride(availableSize);
            return DesiredSize;
        }

        protected abstract Size MeasureOverride(Size availableSize);

        public virtual void Arrange(Size actualSize)
        {
            GivenSize = actualSize;
            ArrangeOverride(actualSize);
        }

        protected abstract void ArrangeOverride(Size actualSize);

        public virtual void InvalidateMeasure()
        {
            Parent?.InvalidateMeasure();
            MeasureInvalidated?.Invoke(this, null);
        }

        #endregion
    }

    public abstract class ContainerElement : Element, IHasChild
    {
        private Element _child;

        protected ContainerElement(Element child)
        {
            _child = child;
        }

        public Element Child
        {
            get => _child;
            set
            {
                var oldChild = _child;
                _child = value;
                OnChildChanged(_child, oldChild);
            }
        }

        protected virtual void OnChildChanged(Element newChild, Element oldChild)
        {
            InvalidateMeasure();
        }
    }

    public abstract class MultiContainerElement : Element, IHasChildren
    {
        private readonly ObservableCollection<Element> _children = new ObservableCollection<Element>();

        protected MultiContainerElement(params Element[] children)
        {
            foreach (var child in children)
                _children.Add(child);
        }

        public IList<Element> Children => _children;

        IEnumerable<Element> IHasChildren.Children => _children;

        public void AddChild(Element element)
        {
            _children.Add(element);
            element.Parent = this;
        }

        public void RemoveChild(Element element)
        {
            _children.Remove(element);
            element.Parent = null;
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

    public struct Render
    {
        public string[] Buffer { get; }
        public Size Size { get; }

        public Render(string buffer)
        {
            // buffer should not contain \n

            Buffer = new[] {buffer};
            Size = new Size(buffer.Length, 1);
        }

        public Render(string[] buffer, Size size)
        {
            Buffer = buffer;
            Size = size;
        }
    }

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