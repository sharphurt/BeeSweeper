using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BeeSweeper.Model;

namespace BeeSweeper.model
{
    public class GameModel
    {
        private static readonly Dictionary<string, Level> Levels = new Dictionary<string, Level>
        {
            {"Easy", new Level(new Size(10, 10), 15)},
            {"Middle", new Level(new Size(10, 10), 20)},
            {"Hard", new Level(new Size(10, 10), 30)}
        };

        public GameModel(Level level)
        {
            Field = MapCreator.CreateMap(level);
        }

        public readonly Field Field;

        public bool GameOver { get; set; }

        public void OpenCellsAround(Point pos)
        {
            Field.OpenEmptyArea(pos);
            GameOver = CheckForGameOver();
        }

        private bool CheckForGameOver()
        {
            var openedCells = Field.Map.Cast<Cell>()
                .Count(c => c.CellType != CellType.Bee && c.CellAttr != CellAttr.None);
            return Field.Map.Cast<Cell>().Any(c => c.CellType == CellType.Bee && c.CellAttr == CellAttr.Opened) ||
                   openedCells == Field.Map.Length - Field.TotalBeesCount;
        }
    }
}