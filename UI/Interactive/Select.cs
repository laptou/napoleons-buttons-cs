using System;
using System.Collections.Specialized;
using System.Linq;
using NapoleonsButtons.UI.Content;
using NapoleonsButtons.UI.Decoration;
using NapoleonsButtons.UI.Util;

namespace NapoleonsButtons.UI.Interactive
{
    public class Select : MultiContainerElement<Option>
    {
        private int _selectedIndex;

        public Select(params Option[] children) : base(children)
        {
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value < 0 || value > Children.Count)
                    throw new ArgumentOutOfRangeException();

                _selectedIndex = value;
                InvalidateRender();
            }
        }

        public Option SelectedOption
        {
            get => Children[SelectedIndex];
            set => SelectedIndex = Children.IndexOf(value);
        }

        public override Render Render(RenderParameters parameters)
        {
            // how much space surrounds elements?
            var childrenWidth = Children.Select(c => c.GivenSize.Width).Sum();
            var totalSpace = GivenSize.Width - childrenWidth;
            var space = totalSpace / (Children.Count - 1);

            var buffer = new string[GivenSize.Height];

            for (var i = 0; i < Children.Count; i++)
            {
                var render = Children[i].Render(parameters);

                for (var y = 0; y < buffer.Length && y < render.Buffer.Length; y++)
                {
                    buffer[y] += render.Buffer[y];

                    if (i < Children.Count - 1)
                        buffer[y] += new string(' ', space);
                }
            }

            return new Render(buffer, GivenSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var width = 0;
            var height = 0;

            foreach (var option in Children)
            {
                var (optionWidth, optionHeight) = option.Measure(new Size(0, availableSize.Height));
                width += optionWidth;
                height = Math.Max(height, optionHeight);
            }

            return new Size(width, height);
        }

        protected override void ArrangeOverride(Size actualSize)
        {
            // TODO: account for when children want more space than is available
            foreach (var option in Children)
                option.Arrange(option.DesiredSize);
        }

        protected override void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnChildrenChanged(sender, e);

            for (var i = 0; i < Children.Count; i++)
                Children[i].Selected = i == SelectedIndex;
        }
    }

    public class Option : Text
    {
        private Color? _foreground, _background;
        private bool   _selected;
        private Color  _selectedForeground, _selectedBackground;

        public Option(params string[] content) : base(content)
        {
        }

        public Color? Foreground
        {
            get => _foreground;
            set
            {
                _foreground = value;
                InvalidateRender();
            }
        }

        public Color? Background
        {
            get => _background;
            set
            {
                _background = value;
                InvalidateRender();
            }
        }

        public Color SelectedForeground
        {
            get => _selectedForeground;
            set
            {
                _selectedForeground = value;
                InvalidateRender();
            }
        }

        public Color SelectedBackground
        {
            get => _selectedBackground;
            set
            {
                _selectedBackground = value;
                InvalidateRender();
            }
        }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                InvalidateRender();
            }
        }

        public override Render Render(RenderParameters parameters)
        {
            var render = base.Render(parameters);

            if (render.Buffer.Length == 0) return render;

            string fgEscape, bgEscape;

            if (Selected)
            {
                fgEscape = Escape.Foreground(SelectedForeground);
                bgEscape = Escape.Background(SelectedBackground);
            }
            else
            {
                fgEscape = Foreground != null ? Escape.Foreground(Foreground.Value) : "";
                bgEscape = Background != null ? Escape.Background(Background.Value) : "";
            }

            render.Buffer[0] = fgEscape + bgEscape + render.Buffer[0];
            render.Buffer[^1] = render.Buffer[^1] + Escape.ForegroundReset() + Escape.BackgroundReset();

            return render;
        }
    }
}