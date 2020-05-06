using System;
using System.Drawing;

namespace BeeSweeper.model
{
    public class Cell
    {
        public CellType CellType { get; set; }
        public CellAttr CellAttr { get; set; }
        public int BeesAround { get; set; }
        
        
        
        public override string ToString()
        {
            return BeesAround.ToString();
        }
    }
}