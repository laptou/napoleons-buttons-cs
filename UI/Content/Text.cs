using System;
using System.Linq;

namespace NapoleonsButtons.UI.Content
{
    public class Text : Element
    {
        private string[] _content;
        private bool     _wrap;

        private string[]? _wrappedContent;

        public Text(params string[] content)
        {
            _content = content;
        }

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

        public override Render Render(RenderParameters parameters)
        {
            if (Wrap)
                return new Render(_wrappedContent!, GivenSize);

            return new Render(_content, GivenSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Wrap)
            {
                var numLines = (int) Math.Ceiling((float) _content.Length / availableSize.Height);
                return new Size(availableSize.Width, numLines);
            }

            return new Size(Content.Max(l => l.Length), Content.Length);
        }

        protected override void ArrangeOverride(Size actualSize)
        {
            if (Wrap)
            {
                var (width, height) = GivenSize;

                _wrappedContent = new string[height];

                for (int wrappedLine = 0, currentLine = 0, offset = 0;
                     wrappedLine < GivenSize.Height;
                     wrappedLine++)
                {
                    var text = _content[currentLine];
                    var remaining = text.Length - offset;

                    if (remaining >= width)
                    {
                        _wrappedContent[wrappedLine] = text[offset..offset + width];
                        offset += GivenSize.Width;
                    }
                    else
                    {
                        _wrappedContent[wrappedLine] = text[offset..] + new string(' ', remaining);
                        offset = 0;
                        currentLine++;
                    }
                }
            }
        }
    }
}