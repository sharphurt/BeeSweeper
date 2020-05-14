using System.Collections.Generic;
using System.Drawing;

namespace BeeSweeper.Architecture
{
    public static class Levels
    {
        public static readonly Dictionary<string, Level> LevelsByName = new Dictionary<string, Level>
        {
            {"Easy", new Level(new Size(7, 7), 10)},
            {"Middle", new Level(new Size(10, 10), 20)},
            {"Hard", new Level(new Size(10, 10), 30)}
        };
    }
}