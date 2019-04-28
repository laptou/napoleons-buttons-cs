namespace NapoleonsButtons.UI
{
    public struct RenderParameters
    {
        public RenderParameters(Size targetSize, Size screenSize)
        {
            TargetSize = targetSize;
            ScreenSize = screenSize;
        }

        public Size TargetSize { get; }
        public Size ScreenSize { get; }
    }
    
    public struct Render
    {
        public string[] Buffer { get; }
        public Size Size { get; }

        public Render(string buffer)
        {
            // buffer should not contain \n

            Buffer = new[] {buffer};
            Size = new Size(buffer.Length, 1);
        }

        public Render(string[] buffer, Size size)
        {
            Buffer = buffer;
            Size = size;
        }
    }
}