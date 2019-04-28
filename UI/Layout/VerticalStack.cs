using System;
using static System.Math;

namespace NapoleonsButtons.UI.Layout
{
    public class VerticalStack : MultiContainerElement<Element>
    {
        public VerticalStack(params Element[] children) : base(children)
        {
        }

        public override Render Render(RenderParameters parameters)
        {
            var buffer = new string[GivenSize.Height];
            var offset = 0;

            foreach (var child in Children)
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

            foreach (var child in Children)
            {
                child.Measure(childAvailableSize);

                var (childWidth, childHeight) = child.DesiredSize;
                width = Max(childWidth, width);
                height += childHeight;
            }

            return new Size(width, height);
        }

        protected override void ArrangeOverride(Size actualSize)
        {
            foreach (var child in Children)
                child.Arrange(new Size(
                    actualSize.Width,
                    Min(actualSize.Height, child.DesiredSize.Height)));
        }
    }
}