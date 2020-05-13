﻿using System.Drawing;
 using BeeSweeper.Architecture;

namespace BeeSweeperGame
{
    public static class CellByLocation
    {
        public static Point GetCellLocationByCursorPosition(Point cursor, Field field)
        {
            for (var x = 0; x < field.Width; x++)
            for (var y = 0; y < field.Height; y++)
            {
                var location = new Point(x, y);
                if (IsPointOnCell(cursor, location))
                    return location;
            }

            return Point.Empty;
        }

        private static bool IsPointOnCell(Point cursor, Point cellPos)
        {
            var vertices = Cell.CalculateVertices(cellPos);
            for (var i = 0; i < 6; i++)
                if (!IsPointLeftToSegment(vertices[i], vertices[(i + 1) % 6], cursor))
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