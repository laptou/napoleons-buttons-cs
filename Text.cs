using System.Text;
using static System.Math;

namespace NapoleonsButtons
{
    public class Text : Element
    {
        private string _content = "";
        private bool _wrap;

        private string? _wrappedContent;

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                InvalidateSize();
            }
        }

        public bool Wrap
        {
            get => _wrap;
            set
            {
                _wrap = value;
                InvalidateSize();
            }
        }

        public override void UpdateSize(Size newSize)
        {
            base.UpdateSize(newSize);

            if (Wrap)
            {
                var builder = new StringBuilder();

                for (var i = 0; i < ActualSize.Height; i++)
                    builder.AppendLine(_content.Substring(i * newSize.Width, newSize.Width));

                _wrappedContent = builder.ToString();
            }
        }

        public override Render Render(RenderParameters parameters)
        {
            if (Wrap)
                return new Render(_wrappedContent!, ActualSize);

            return new Render(Content.Substring(ActualSize.Width), ActualSize);
        }

        public override Size Measure(LayoutParameters parameters)
        {
            if (Wrap)
            {
                var numLines = (int) Ceiling((float) _content.Length / parameters.AvailableSize.Height);
                return new Size(parameters.AvailableSize.Width, numLines);
            }

            return new Size(Content.Length, 1);
        }
    }
}