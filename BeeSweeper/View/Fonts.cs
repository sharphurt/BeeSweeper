using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using BeeSweeper.Architecture;

namespace BeeSweeper.View
{
    public class Fonts
    {
        public Font Font;
        public Font ButtonFont;

        private readonly PrivateFontCollection fontCollection = new PrivateFontCollection();

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