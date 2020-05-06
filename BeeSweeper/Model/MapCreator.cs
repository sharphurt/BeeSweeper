using System;
using System.Collections.Generic;
using System.Drawing;
using BeeSweeper.model;

namespace BeeSweeper.Model
{
    public static class MapCreator
    {
        public static Field CreateMap(Level level)
        {
            var field = new Field(level.Size, level.Percent);
            var minesCount = level.Size.Width * level.Size.Height * level.Percent / 100;
            var r = new Random();
            for (var i = 0; i < minesCount; i++)
            {
                var x = r.Next(level.Size.Width);
                var y = r.Next(level.Size.Height);
                if (field[x, y].CellType == CellType.Bee)
                    continue;
                field[x, y].CellType = CellType.Bee;
            }

            CountNeighbours(field);
            return field;
        }

        public static void CountNeighbours(Field field)
        {
            for (var x = 0; x < field.Width; x++)
            for (var y = 0; y < field.Height; y++)
            {
                var position = new Point(x, y);
                var beesCount = field.CountNeighbouringBees(position);
                field[position].BeesAround = beesCount;

                if (beesCount > 0)
                    field[position].CellType = CellType.Informer;
                else if (beesCount == 0)
                    field[position].CellType = CellType.Empty;
                else
                    field[position].CellType = CellType.Bee;
            }
        }
    }
}