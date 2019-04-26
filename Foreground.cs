namespace NapoleonsButtons
{
    public struct Color
    {
        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
    }

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
            return new Render(
                $"\x1b[38;2;{Color.Red};{Color.Green};{Color.Blue}m{childRender.Text}\x1b[39m",
                childRender.Size);
        }

        public override Size Measure(LayoutParameters parameters)
        {
            return Child.Measure(parameters);
        }
    }
}