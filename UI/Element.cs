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

}