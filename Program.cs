using System;
using NapoleonsButtons.UI;
using NapoleonsButtons.UI.Content;
using NapoleonsButtons.UI.Decoration;
using NapoleonsButtons.UI.Layout;

namespace NapoleonsButtons
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var screen = new Screen(
                new VerticalAlign(
                    new VerticalStack(
                        new HorizontalAlign(
                            new Foreground(
                                new Text("NAPOLEON'S BUTTONS"),
                                new Color(200, 128, 255)),
                            HorizontalAlignment.Center),
                        new HorizontalAlign(
                            new Foreground(
                                new Text($"SIZE: {Console.WindowWidth}x{Console.WindowHeight}"),
                                new Color(255, 255, 255)),
                            HorizontalAlignment.Center)
                    ),
                    VerticalAlignment.Center
                )
            );
            screen.InvalidateMeasure();
            screen.Render();

            Console.ReadLine();
        }
    }
}