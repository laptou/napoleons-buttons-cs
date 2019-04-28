using System;

namespace NapoleonsButtons.UI.Layout
{
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right
    }

    public class HorizontalAlign : ContainerElement
    {
        public HorizontalAlign(Element child, HorizontalAlignment alignment) : base(child)
        {
            Alignment = alignment;
        }

        public HorizontalAlignment Alignment { get; set; }

        public override Render Render(RenderParameters parameters)
        {
            int space = GivenSize.Width - Child.GivenSize.Width, left;

            switch (Alignment)
            {
                case HorizontalAlignment.Left:
                    left = 0;
                    break;
                case HorizontalAlignment.Center:
                    left = space / 2;
                    break;
                case HorizontalAlignment.Right:
                    left = GivenSize.Width - Child.GivenSize.Width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var buffer = new string[GivenSize.Height];
            var childRender = Child.Render(parameters);

            for (var i = 0; i < buffer.Length; i++)
                buffer[i] = new string(' ', left) + childRender.Buffer[i] + new string(' ', space - left);

            return new Render(buffer, GivenSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var childSize = Child.Measure(availableSize);
            return new Size(availableSize.Width, childSize.Height);
        }

        protected override void ArrangeOverride(Size actualSize)
        {
            Child.Arrange(new Size(Math.Min(actualSize.Width, Child.DesiredSize.Width), actualSize.Height));
        }
    }
}