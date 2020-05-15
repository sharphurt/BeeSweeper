using System;
using System.Collections.Generic;
using System.Drawing;
using BeeSweeper.model;

namespace BeeSweeper.Model
{
    public class CoordinatesConverter
    {
        Dictionary<Direction, Func<bool, Size>> Converters = new Dictionary<Direction, Func<bool, Size>>
        {
            
        }
        
        
        
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
    }
}