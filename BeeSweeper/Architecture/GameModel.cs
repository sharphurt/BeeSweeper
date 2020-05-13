using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BeeSweeper.model;

namespace BeeSweeper.Architecture
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

        public int Score { get; private set; }

        public Winner Winner { get; private set; }

        public bool GameOver { get; private set; }

        public void OpenCell(Point pos)
        {
            Field.OpenEmptyArea(pos, out var collectedScore);
            Score += collectedScore;
            GameOver = CheckForGameOver(pos);
        }

        public void ChangeAttr(Point pos)
        {
            var cell = Field[pos];
            if (cell.CellAttr == CellAttr.Opened)
                return;
            switch (cell.CellAttr)
            {
                case CellAttr.None:
                    cell.CellAttr = CellAttr.Flagged;
                    break;
                case CellAttr.Flagged:
                    cell.CellAttr = CellAttr.Questioned;
                    break;
                case CellAttr.Questioned:
                    cell.CellAttr = CellAttr.None;
                    break;
            }
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