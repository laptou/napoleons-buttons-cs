using System;
using NapoleonsButtons.UI;
using NapoleonsButtons.UI.Content;
using NapoleonsButtons.UI.Decoration;
using NapoleonsButtons.UI.Interactive;
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
                            HorizontalAlignment.Center),
                        new Padding(
                            new Select(
                                new Option("START GAME")
                                {
                                    Foreground = new Color(240, 60, 60),
                                    SelectedForeground = new Color(255, 255, 255),
                                    SelectedBackground = new Color(240, 60, 60)
                                },
                                new Option("BIBLIOGRAPHY") {Foreground = new Color(60, 240, 60)},
                                new Option("EXIT") {Foreground = new Color(60, 60, 240)})
                        ) {Top = 2, Left = 10, Right = 10, Bottom = 2}
                    ),
                    VerticalAlignment.Center
                )
            );

            screen.InvalidateMeasure();
            screen.InvalidateRender();

            Console.ReadLine();
        }
    }
}