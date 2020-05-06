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

        public Winner Winner { get; set; }

        public bool GameOver { get; private set; }

        public void OpenCell(Point pos)
        {
            Field.OpenEmptyArea(pos);
            GameOver = CheckForGameOver(pos);
        }

        private bool CheckForGameOver(Point pos)
        {
            if (Field[pos].CellType == CellType.Bee)
            {
                Winner = Winner.Computer;
                return true;
            }

            if (Field.Map.Cast<Cell>().Count(c => c.CellType != CellType.Bee && c.CellAttr != CellAttr.None) ==
                Field.Map.Length - Field.TotalBeesCount)
            {
                Winner = Winner.Player;
                return true;
            }

            return false;
        }
    }
}