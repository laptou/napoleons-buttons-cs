using System;

namespace NapoleonsButtons.UI.Layout
{
    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom
    }

    public class VerticalAlign : ContainerElement, IHasChild
    {
        public VerticalAlign(Element child, VerticalAlignment alignment) : base(child)
        {
            Alignment = alignment;
        }

        public VerticalAlignment Alignment { get; set; }

        public override Render Render(RenderParameters parameters)
        {
            int margin;

            switch (Alignment)
            {
                case VerticalAlignment.Top:
                    margin = 0;
                    break;
                case VerticalAlignment.Center:
                    margin = (GivenSize.Height - Child.GivenSize.Height) / 2;
                    break;
                case VerticalAlignment.Bottom:
                    margin = GivenSize.Height - Child.GivenSize.Height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var buffer = new string[GivenSize.Height];
            var childRender = Child.Render(parameters);

            Array.Copy(
                childRender.Buffer,
                0,
                buffer,
                margin,
                childRender.Buffer.Length);

            return new Render(buffer, GivenSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Child.Measure(availableSize);
            return new Size(Child.DesiredSize.Width, availableSize.Height);
        }

        protected override void ArrangeOverride(Size actualSize)
        {
            Child.Arrange(new Size(actualSize.Width, Math.Min(actualSize.Height, Child.DesiredSize.Height)));
        }
    }
}