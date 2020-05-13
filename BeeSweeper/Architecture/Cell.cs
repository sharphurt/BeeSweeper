using System;
using System.Drawing;
using BeeSweeper.model;

namespace BeeSweeper.Architecture
{
    public class Cell
    {
        public CellType CellType { get; set; }
        public CellAttr CellAttr { get; set; }
        public int BeesAround { get; set; }


        public override string ToString()
        {
            return BeesAround.ToString();
        }

        public static Point[] CalculateVertices(Point pos)
        {
            var xCoordinate = pos.X * GameSettings.Width;
            xCoordinate += pos.Y % 2 == 1 ? GameSettings.Width / 2 : 0;
            var yCoordinate = (int) Math.Floor(GameSettings.CellSide * 0.5);

            Point[] vertices =
            {
                new Point(xCoordinate, yCoordinate),
                new Point(
                    xCoordinate + GameSettings.Width / 2 + (pos.Y % 2 == 0 ? 0 : 1),
                    yCoordinate - (int) Math.Floor(GameSettings.CellSide * 0.5) - 1
                    ),
                new Point(xCoordinate + GameSettings.Width, yCoordinate),
                new Point(xCoordinate + GameSettings.Width, yCoordinate + GameSettings.CellSide + 2),
                new Point(
                    xCoordinate + GameSettings.Width / 2,
                    yCoordinate - (int) Math.Floor(GameSettings.CellSide * 0.5) + GameSettings.Height
                    ),
                new Point(xCoordinate, yCoordinate + GameSettings.CellSide + 1)
            };
            return vertices;
        }

        public static Point CalculateImagePosition(Point pos)
        {
            var vertices = CalculateVertices(pos);
            return new Point(
                vertices[0].X + GameSettings.Width / 2 - GameSettings.ImageSize / 2,
                vertices[0].Y + GameSettings.Height / 4 - GameSettings.ImageSize / 2
                );
        }
    }
}