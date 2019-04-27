using System;

namespace NapoleonsButtons.UI
{
    public class Screen : Element
    {
        private Element _child;

        public Screen(Element child)
        {
            _child = child;
        }

        public Element Child
        {
            get => _child;
            set
            {
                _child = value;
                InvalidateMeasure();
            }
        }

        public override void OnMeasureInvalidated(Size newSize)
        {
            base.OnMeasureInvalidated(newSize);
            _child.Measure(new LayoutParameters(newSize));

            InvalidateRender();
        }

        public override void OnRenderInvalidated()
        {
            base.OnRenderInvalidated();
            Render();
        }

        public void Render()
        {
            var render = Render(new RenderParameters(GivenSize, GivenSize));

            Console.Clear();
            Console.Write(string.Join(Environment.NewLine, render.Buffer));
        }

        public override Render Render(RenderParameters parameters)
        {
            return _child.Render(parameters);
        }

        public override void Measure(LayoutParameters parameters)
        {
            DesiredSize = new Size(Console.WindowWidth, Console.WindowHeight);
        }
    }
}