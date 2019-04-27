using System;

namespace NapoleonsButtons.UI.Layout
{
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right
    }

    public class HorizontalAlign : Element, IHasChild
    {
        private Element _child;

        public HorizontalAlign(Element child, HorizontalAlignment alignment)
        {
            _child = child;
            Alignment = alignment;
        }

        public HorizontalAlignment Alignment { get; set; }

        public override Render Render(RenderParameters parameters)
        {
            int space = GivenSize.Width - _child.GivenSize.Width, left;

            switch (Alignment)
            {
                case HorizontalAlignment.Left:
                    left = 0;
                    break;
                case HorizontalAlignment.Center:
                    left = space / 2;
                    break;
                case HorizontalAlignment.Right:
                    left = GivenSize.Width - _child.GivenSize.Width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var buffer = new string[GivenSize.Height];
            var childRender = _child.Render(parameters);

            for (var i = 0; i < buffer.Length; i++)
                buffer[i] = new string(' ', left) + childRender.Buffer[i] + new string(' ', space - left);

            return new Render(buffer, GivenSize);
        }

        public override Size Measure(LayoutParameters parameters)
        {
            var childSize = _child.Measure(parameters);
            return new Size(parameters.AvailableSize.Width, childSize.Height);
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