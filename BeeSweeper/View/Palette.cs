using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using BeeSweeper.Architecture;

namespace BeeSweeper.View
{
    public static class Palette
    {
        public static class Colors
        {
            public static Color UnrevealedColor = Color.FromArgb(80, 80, 80);
            public static Color RevealedColor = Color.FromArgb(190, 190, 190);
            public static Color FormBackground = Color.FromArgb(50, 50, 50);

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
            public static Pen OutlinePen = new Pen(Color.FromArgb(127, 127, 127));
        }
    }
}