using System;

namespace BeeSweeper.Architecture
{
    public static class GameSettings
    {
        public const string GameName = "BeeSweeper";
        public const int CellRadius = 45;
        public static readonly int CellWidth = (int) Math.Sqrt(3 * CellRadius * CellRadius);
        public static readonly int CellHeight = (int) (CellRadius + 2 * Math.Sqrt(CellRadius * CellRadius - Math.Pow(CellWidth * 0.5, 2)));
        public const int ImageSize = CellRadius;
        public const int TicksPerSecond = 60;
    }
}