using System.Collections.Generic;
using System.Drawing;

namespace BeeSweeper.Architecture
{
    public static class Levels
    {
        public static readonly Dictionary<string, Level> LevelsByName = new Dictionary<string, Level>
        {
            {"Easy", new Level("Easy", new Size(10, 10), 10)},
            {"Medium", new Level("Medium", new Size(10, 10), 20)},
            {"Hard", new Level("Hard", new Size(10, 10), 30)},
            {"Custom", new Level("Custom", new Size(0, 0), 0)}
        };

        public static Dictionary<string, int> CustomLevelParametersByName = new Dictionary<string, int>
        {
            {"Width", 0},
            {"Height", 0},
            {"Percent", 0}
        };

        public static Level SelectedLevel = LevelsByName["Easy"];
    }
}