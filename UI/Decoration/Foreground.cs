using NapoleonsButtons.UI.Util;

namespace NapoleonsButtons.UI.Decoration
{
    public class Foreground : Element, IHasChild
    {
        public Foreground(Element child, Color color)
        {
            Child = child;
            Color = color;
        }

        public Color Color { get; set; }

        public Element Child { get; set; }

        public override Render Render(RenderParameters parameters)
        {
            var childRender = Child.Render(parameters);

            if (childRender.Buffer.Length > 0)
            {
                var esc = Escape.Foreground(Color);
                childRender.Buffer[0] = esc + childRender.Buffer[0];

                // replace reset colour with setting colour back to our color :-)
                for (var i = 0; i < childRender.Buffer.Length; i++)
                    childRender.Buffer[i] = childRender.Buffer[i].Replace("\x1b[39m", esc);

                childRender.Buffer[^1] = childRender.Buffer[^1] + "\x1b[39m";
            }

            return childRender;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Child.Measure(availableSize);
            return Child.DesiredSize;
        }

        protected override void ArrangeOverride(Size actualSize)
        {
            Child.Arrange(actualSize);
        }
    }
}