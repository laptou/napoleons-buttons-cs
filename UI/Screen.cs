using System;

namespace NapoleonsButtons.UI
{
    public class Screen : ContainerElement
    {
        public Screen(Element child) : base(child)
        {
            MeasureInvalidated += OnMeasureInvalidated;
            RenderInvalidated += OnRenderInvalidated;
        }

        private void OnMeasureInvalidated(object sender, EventArgs e)
        {
            var size = Measure(new Size(Console.WindowWidth, Console.WindowHeight));
            Arrange(size);
        }

        private void OnRenderInvalidated(object sender, EventArgs eventArgs)
        {
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
            return Child.Render(parameters);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Child.Measure(availableSize);
            return availableSize;
        }

        protected override void ArrangeOverride(Size actualSize)
        {
            Child.Arrange(actualSize);
        }
    }
}