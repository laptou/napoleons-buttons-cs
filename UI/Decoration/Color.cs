namespace NapoleonsButtons.UI.Decoration
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
}