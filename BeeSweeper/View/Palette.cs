using System.Collections.Generic;
using System.Drawing;

namespace BeeSweeper.View
{
    public static class Palette
    {
        public static class Colors
        {
            public static Color UnrevealedColor = Color.FromArgb(200, 200, 200);
            public static Color RevealedColor = Color.FromArgb(255, 255, 255);
            public static Color InterfaceBackgroundColor = Color.White;
            public static Color FormBackground = Color.White;
            public static Color TextColor = Color.Black;
            public static Color OutlineColor = Color.Gray;

            public static readonly Dictionary<int, Color> ColorsByNeighbouringBees = new Dictionary<int, Color>
            {
                {1, Color.DarkBlue},
                {2, Color.Crimson},
                {3, Color.ForestGreen},
                {4, Color.Chocolate},
                {5, Color.DarkMagenta},
                {6, Color.DarkOrange}
            };
        }

        public static class Pens
        {
            public static Pen OutlinePen = new Pen(Colors.OutlineColor, 2);
        }
    }
}