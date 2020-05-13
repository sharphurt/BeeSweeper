using System;

namespace BeeSweeper.Architecture
{
    public static class GameSettings
    {
        public const string GameName = "BeeSweeper";
        public const int CellSide = 30;
        public static readonly int CellWidth = (int) Math.Sqrt(2 * CellSide * CellSide - 2 * CellSide * CellSide * -0.5);
        public static readonly int CellHeight = (int) (CellSide + 2 * Math.Sqrt(CellSide * CellSide - Math.Pow(CellWidth * 0.5, 2)));
        public const int ImageSize = (int) (CellSide * 1.3);
        public const int TicksPerSecond = 60;
    }
}