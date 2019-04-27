using System;

namespace NapoleonsButtons.UI.Content
{
    public class Text : Element
    {
        public Text(params string[] content)
        {
            _content = content;
        }

        private string[] _content = new string[0];
        private bool _wrap;

        private string[]? _wrappedContent;

        public string[] Content
        {
            get => _content;
            set
            {
                _content = value;
                InvalidateMeasure();
            }
        }

        public bool Wrap
        {
            get => _wrap;
            set
            {
                _wrap = value;
                InvalidateMeasure();
            }
        }

        public override void OnMeasureInvalidated(Size newSize)
        {
            base.OnMeasureInvalidated(newSize);

            if (Wrap)
            {
                var (width, height) = GivenSize;

                _wrappedContent = new string[height];

                for (int wrappedLine = 0, currentLine = 0, offset = 0;
                    wrappedLine < GivenSize.Height;
                    wrappedLine++)
                {
                    var text = _content[currentLine];

                    if (text.Length - offset <= GivenSize.Width)
                    {
                        _wrappedContent[wrappedLine] = text[offset..offset + width];
                        offset += GivenSize.Width;
                    }
                    else
                    {
                        _wrappedContent[wrappedLine] = text[offset..];
                        offset = 0;
                        currentLine++;
                    }
                }
            }
        }

        public override Render Render(RenderParameters parameters)
        {
            if (Wrap)
                return new Render(_wrappedContent!, GivenSize);

            return new Render(_content, GivenSize);
        }

        public override Size Measure(LayoutParameters parameters)
        {
            if (Wrap)
            {
                var numLines = (int) Math.Ceiling((float) _content.Length / parameters.AvailableSize.Height);
                return new Size(parameters.AvailableSize.Width, numLines);
            }

            return new Size(Content.Length, 1);
        }
    }
}