using NapoleonsButtons.UI.Decoration;

namespace NapoleonsButtons.UI.Util
{
    public static class Escape
    {
        public static string Foreground(Color color)
        {
            return $"\x1b[38;2;{color.Red};{color.Green};{color.Blue}m";
        }

        public static string ForegroundReset()
        {
            return "\x1b[39m";
        }

        public static string Background(Color color)
        {
            return $"\x1b[48;2;{color.Red};{color.Green};{color.Blue}m";
        }

        public static string BackgroundReset()
        {
            return "\x1b[49m";
        }
    }
}