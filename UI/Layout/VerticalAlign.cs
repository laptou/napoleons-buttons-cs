using System;

namespace NapoleonsButtons.UI.Layout
{
    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom
    }

    public class VerticalAlign : WrapperBase, IHasChild
    {
        public VerticalAlign(Element child, VerticalAlignment alignment)
        {
            Child = child;
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
            var childSize = Child.MeasureOverride(availableSize);
            DesiredSize = new Size(childSize.Width, availableSize.Height);
        }

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
}