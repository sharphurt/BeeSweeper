using System;
using System.Drawing;
using System.Linq;
using BeeSweeper.model;

namespace BeeSweeper.Architecture
{
    public class GameModel
    {
        public readonly Level Level;

        private bool _isFirstClick = true;
        private int _score;

        public Field Field;

        public bool GameOver = true;

        public GameModel(Level level)
        {
            Level = level;
        }

        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                ScoreChanged?.Invoke();
            }
        }

        public Winner Winner { get; private set; }

        public event Action<Winner> GameFinished;
        public event Action GameStarted;
        public event Action ScoreChanged;

        public void OpenCell(Point pos)
        {
            RegenerateFieldIfNecessary(pos);
            Field.OpenEmptyArea(pos, out var collectedScore);
            Score += collectedScore;
            CheckForGameOver(pos);
        }

        private void RegenerateFieldIfNecessary(Point pos)
        {
            if (_isFirstClick && Field[pos].CellType == CellType.Bee)
                while (Field[pos].CellType == CellType.Bee)
                    PrepareField();
            _isFirstClick = false;
        }

        public void PrepareField()
        {
            Field = MapCreator.CreateMap(Level);
        }

        public void StartGame()
        {
            PrepareField();
            Winner = Winner.Nobody;
            Score = 0;
            GameOver = false;
            GameStarted?.Invoke();
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

        private void CheckForGameOver(Point pos)
        {
            GameOver = GetWinner(pos) != Winner.Nobody;
            if (!GameOver) return;
            Winner = GetWinner(pos);
            foreach (var cell in Field.Map)
                cell.CellAttr = CellAttr.Opened;
            GameFinished?.Invoke(Winner);
        }

        private Winner GetWinner(Point pos)
        {
            if (Field[pos].CellType == CellType.Bee)
                return Winner.Computer;
            var map = Field.Map.Cast<Cell>().ToList();
            if (map.Where(c=> c.CellType == CellType.Bee).All(c=> c.CellAttr == CellAttr.Flagged))
                return Winner.Player;
            return Winner.Nobody;
        }
    }
}