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
                childRender.Buffer[0] = $"\x1b[38;2;{Color.Red};{Color.Green};{Color.Blue}m" + childRender.Buffer[0];
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