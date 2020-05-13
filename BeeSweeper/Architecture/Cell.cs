using System;
using System.Collections.Generic;
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
            var vertices = new Point[6];
            var normal =
                (int) Math.Sqrt(Math.Pow(GameSettings.CellRadius, 2) - Math.Pow(GameSettings.CellRadius * 0.5, 2));
            var centre = new Point(
                normal + pos.X * normal * 2 + pos.Y % 2 * normal,
                GameSettings.CellRadius + pos.Y * (GameSettings.CellRadius * 3 / 2)
            );

            for (var i = 0; i < 6; i++)
            {
                var vertex = new Point(
                    (int) (centre.X + GameSettings.CellRadius * Math.Sin(i * Math.PI / 3)),
                    (int) (centre.Y + GameSettings.CellRadius * Math.Cos(i * Math.PI / 3))
                );
                vertices[i] = vertex;
            }

            return vertices;
        }

        public static Point CalculateImagePosition(Point pos)
        {
            var vertices = CalculateVertices(pos);
            return new Point(vertices[4].X, vertices[4].Y) + new Size(GameSettings.CellRadius / 3, 0);
        }
        
        public static Point CalculateTextPosition(Point pos)
        {
            var vertices = CalculateVertices(pos);
            return new Point(vertices[4].X, vertices[4].Y) + new Size(GameSettings.CellRadius / 2, 0);
        }
    }
}