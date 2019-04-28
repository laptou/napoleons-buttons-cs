using System;

namespace NapoleonsButtons.UI.Layout
{
    public class Padding : ContainerElement<Element>
    {
        private int _left, _top, _right, _bottom;

        public Padding(Element child) : base(child)
        {
        }

        public int Bottom
        {
            get => _bottom;
            set
            {
                _bottom = value;
                InvalidateMeasure();
            }
        }

        public int Right
        {
            get => _right;
            set
            {
                _right = value;
                InvalidateMeasure();
            }
        }

        public int Top
        {
            get => _top;
            set
            {
                _top = value;
                InvalidateMeasure();
            }
        }

        public int Left
        {
            get => _left;
            set
            {
                _left = value;
                InvalidateMeasure();
            }
        }


        public override Render Render(RenderParameters parameters)
        {
            var render = Child.Render(parameters);
            var buffer = new string[GivenSize.Height];

            for (var y = 0; y < buffer.Length; y++)
            {
                if (y < Top || y > buffer.Length - Bottom || y - Top >= render.Buffer.Length)
                {
                    buffer[y] = new string(' ', GivenSize.Width);
                    continue;
                }

                buffer[y] = new string(' ', Left) + render.Buffer[y - Top] + new string(' ', Right);
            }

            return new Render(buffer, GivenSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var childSize = Child.Measure(new Size(
                Math.Max(availableSize.Width - Left - Right, 0),
                Math.Max(availableSize.Height - Top - Bottom, 0)));

            return new Size(childSize.Width + Left + Right, childSize.Height + Top + Bottom);
        }

        protected override void ArrangeOverride(Size actualSize)
        {
            Child.Arrange(new Size(
                Math.Max(actualSize.Width - Left - Right, 0),
                Math.Max(actualSize.Height - Top - Bottom, 0)));
        }
    }
}