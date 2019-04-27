using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static System.Math;

namespace NapoleonsButtons.UI.Layout
{
    public class VerticalStack : Element, IHasChildren
    {
        private readonly ObservableCollection<Element> _children = new ObservableCollection<Element>();

        public VerticalStack(params Element[] children)
        {
            foreach (var child in children)
                _children.Add(child);
        }

        public override Render Render(RenderParameters parameters)
        {
            var buffer = new string[GivenSize.Height];
            var offset = 0;

            foreach (var child in _children)
            {
                var render = child.Render(parameters);
                Array.Copy(
                    render.Buffer,
                    0,
                    buffer,
                    offset,
                    render.Buffer.Length);

                offset += render.Size.Height;
            }

            return new Render(buffer, GivenSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            int width = 0, height = 0;

            var childAvailableSize = new Size(availableSize.Width, 0);

            foreach (var child in _children)
            {
                child.Measure(childAvailableSize);

                var (childWidth, childHeight) = child.DesiredSize;
                width = Max(childWidth, width);
                height += childHeight;
            }

            return new Size(width, height);
        }

        public IEnumerable<Element> Children => _children;

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
}