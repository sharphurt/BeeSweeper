﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BeeSweeper.model;

namespace BeeSweeper.Architecture
{
    public class Field
    {
        private readonly int _fillPercent;

        public Field(Size size, int percent)
        {
            _fillPercent = percent;
            Map = new Cell[size.Width, size.Height];
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                Map[x, y] = new Cell();
        }

        public Cell[,] Map { get; set; }

        public int TotalBees => Map.Length * _fillPercent / 100;
        public int Width => Map.GetLength(0);
        public int Height => Map.GetLength(1);

        public Cell this[int x, int y] => Map[x, y];
        public Cell this[Point pos] => Map[pos.X, pos.Y];

        public int CountNeighbouringBees(Point pos)
        {
            if (Map[pos.X, pos.Y].CellType == CellType.Bee)
                return -1;
            return GetNeighboursAndDirections(pos).Count(neighbour => neighbour.Key.CellType == CellType.Bee);
        }

        private Dictionary<Cell, Direction> GetNeighboursAndDirections(Point pos)
        {
            var neighbours = new Dictionary<Cell, Direction>();
            foreach (Direction direction in typeof(Direction).GetEnumValues())
            {
                var neighbour = GetNeighbour(pos, direction);
                if (neighbour != null)
                    neighbours.Add(neighbour, direction);
            }

            return neighbours;
        }

        private Cell GetNeighbour(Point pos, Direction direction)
        {
            var neighbourLocation = Util.DirToNeighbourPos(pos, direction);
            return Util.IsLocationValid(neighbourLocation, new Size(Width, Height))
                ? Map[neighbourLocation.X, neighbourLocation.Y]
                : null;
        }

        private void OpenNonEmptyNeighbours(Point pos)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var neighbour = GetNeighbour(pos, direction);
                if (neighbour != null && neighbour.CellType != CellType.Bee && neighbour.CellAttr == CellAttr.None)
                    neighbour.CellAttr = CellAttr.Opened;
            }
        }

        private bool AreEmptyNeighboursExists(Point pos)
        {
            return (from Direction direction in Enum.GetValues(typeof(Direction))
                    select GetNeighbour(pos, direction))
                .Any(neighbour => neighbour != null && neighbour.CellType == CellType.Empty);
        }

        public void OpenEmptyArea(Point pos, out int collectedScore)
        {
            var visited = new HashSet<Cell>();
            if (Map[pos.X, pos.Y].CellAttr == CellAttr.Opened)
            {
                collectedScore = 0;
                return;
            }

            Map[pos.X, pos.Y].CellAttr = CellAttr.Opened;
            ExploreEmptyArea(pos, Direction.UpLeft, ref visited);
            ExploreEmptyArea(pos, Direction.UpRight, ref visited);
            collectedScore = visited.Where(cell => cell.CellType == CellType.Informer).Sum(cell => cell.BeesAround);
        }

        private void ExploreEmptyArea(Point pos, Direction dir, ref HashSet<Cell> visited)
        {
            while (Util.IsLocationValid(pos, new Size(Width, Height)))
            {
                var cell = Map[pos.X, pos.Y];
                visited.Add(cell);

                if (cell.CellAttr == CellAttr.None)
                    cell.CellAttr = CellAttr.Opened;

                if (cell.CellType == CellType.Informer
                    || cell.CellAttr == CellAttr.Flagged
                    || cell.CellAttr == CellAttr.Questioned)
                    break;

                OpenNonEmptyNeighbours(pos);

                if (AreEmptyNeighboursExists(pos))
                {
                    var neighboursAndDirections = GetNeighboursAndDirections(pos);
                    foreach (var e in neighboursAndDirections)
                    {
                        if (visited.Contains(e.Key) || e.Value == Direction.Left || e.Value == Direction.Right)
                            continue;
                        ExploreEmptyArea(Util.DirToNeighbourPos(pos, e.Value), e.Value, ref visited);
                    }
                }

                pos.X += dir == Direction.UpLeft || dir == Direction.DownLeft ? -1 : 1;
            }
        }
    }
}