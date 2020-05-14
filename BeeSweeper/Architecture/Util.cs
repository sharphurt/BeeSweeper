using System.Drawing;
using BeeSweeper.model;

namespace BeeSweeper.Architecture
{
    public static class Util
    {
        public static Size DirToOffsets(Direction direction, bool isYEven)
        {
            switch (direction)
            {
                case Direction.UpLeft:
                    return isYEven ? new Size(-1, -1) : new Size(0, -1);
                case Direction.UpRight:
                    return isYEven ? new Size(0, -1) : new Size(1, -1);
                case Direction.Right:
                    return new Size(1, 0);
                case Direction.DownRight:
                    return isYEven ? new Size(0, 1) : new Size(1, 1);
                case Direction.DownLeft:
                    return isYEven ? new Size(-1, 1) : new Size(0, 1);
                case Direction.Left:
                    return new Size(-1, 0);
                default: return Size.Empty;
            }
        }

        public static bool IsLocationValid(Point pos, Size bounds)
        {
            return pos.X >= 0 && pos.X < bounds.Width && pos.Y >= 0 && pos.Y < bounds.Height;
        }

        public static Point DirToNeighbourPos(Point pos, Direction direction)
        {
            return pos + DirToOffsets(direction, pos.Y % 2 == 0);
        }
    }
}