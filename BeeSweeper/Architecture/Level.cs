using System.Drawing;

namespace BeeSweeper
{
    public class Level
    {
        public Size Size { get; }
        public int Percent { get; }
        
        public string Name { get; }

        public Level(string name, Size size, int percent)
        {
            Name = name;
            Size = size;
            Percent = percent;
        }
    }
}