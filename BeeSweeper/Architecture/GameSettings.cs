using System;

namespace BeeSweeper.Architecture
{
    public static class GameSettings
    {
        public const int CellSide = 15;
        public static readonly int Width = (int) Math.Sqrt(2 * CellSide * CellSide - 2 * CellSide * CellSide * -0.5);
        public static readonly int Height = (int) (CellSide + 2 * Math.Sqrt(CellSide * CellSide - Math.Pow(Width * 0.5, 2)));
        public const int ImageSize = (int) (CellSide * 1.3);
    }
}