using System.Drawing;

namespace BeeSweeper
{
    public class Level
    {
        public Size Size { get; }
        public int Percent { get; }

        public Level(Size size, int percent)
        {
            Size = size;
            Percent = percent;
        }
    }
}