using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using BeeSweeper.Architecture;

namespace BeeSweeper.View
{
    public class Fonts
    {
        private readonly PrivateFontCollection fontCollection = new PrivateFontCollection();
        public Font ButtonFont;
        public Font Font;

        public Fonts()
        {
            Load();
        }

        public void Load()
        {
            Font = LoadFontFromFile("ProximaNova-Regular.ttf", (int) (GameSettings.CellRadius * 0.8));
            ButtonFont = LoadFontFromFile("ProximaNova-Regular.ttf", 15);
        }

        private Font LoadFontFromFile(string fileName, int fontSize)
        {
            fontCollection.AddFontFile("Assets/Fonts/" + fileName);
            return new Font(fontCollection.Families.Last(), fontSize, GraphicsUnit.Pixel);
        }
    }
}