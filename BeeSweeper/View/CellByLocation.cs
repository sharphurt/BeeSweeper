﻿using System.Drawing;
 using BeeSweeper.Architecture;

namespace BeeSweeperGame
{
    public static class CellByLocation
    {
        public static Cell GetCellByLocation(Point location, Field field)
        {
            for (var x = 0; x < field.Width; x++)
            for (var y = 0; y < field.Height; y++)
            {
                var cell = field[x, y];
                if (IsPointOnCell(location, new Point(x, y)))
                    return cell;
            }

            return null;
        }

        private static bool IsPointOnCell(Point pos, Point cell)
        {
            var vertices = Cell.CalculateVertices(pos);
            for (var i = 0; i < 6; i++)
                if (!IsPointLeftToSegment(vertices[i], vertices[(i + 1) % 6], pos))
                    return false;
            return true;
        }

        private static bool IsPointLeftToSegment(Point segmentBegin, Point segmentEnd, Point point)
        {
            return (segmentEnd.X - segmentBegin.X) * (point.Y - segmentBegin.Y)
                   - (segmentEnd.Y - segmentBegin.Y) * (point.X - segmentBegin.X) >= 0;
        }
    }
}