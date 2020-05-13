using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BeeSweeper.model;

namespace BeeSweeper.Architecture
{
    public class GameModel
    {
        public event Action<Winner> GameStateChanged;

        public GameModel(Level level)
        {
            Level = level;
        }

        public readonly Level Level;
        
        public Field Field;

        public int Score { get; private set; }

        public Winner Winner { get; private set; }

        public bool GameOver { get; private set; }

        public void OpenCell(Point pos)
        {
            if (GameOver) return;
                Field.OpenEmptyArea(pos, out var collectedScore);
            Score += collectedScore;
            GameOver = CheckForGameOver(pos);
            if (GameOver)
            {
                GameStateChanged?.Invoke(Winner);
                foreach (var cell in Field.Map.Cast<Cell>().Where(c => c.CellType == CellType.Bee))
                    cell.CellAttr = CellAttr.Opened;
            }
        }

        public void StartGame()
        {
            Field = MapCreator.CreateMap(Level);
            Winner = Winner.Nobody;
            Score = 0;
            GameOver = false;
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

            if (Field.Map.Cast<Cell>().Count(c => c.CellType != CellType.Bee && c.CellAttr == CellAttr.Opened) ==
                Field.Map.Length - Field.TotalBeesCount)
            {
                Winner = Winner.Player;
                return true;
            }

            return false;
        }
    }
}