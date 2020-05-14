using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using BeeSweeper.Architecture;

namespace BeeSweeper.View
{
    public class Fonts
    {
        public Font InformerFont;
        public Font TimerFont;

        private readonly PrivateFontCollection fontCollection = new PrivateFontCollection();

        public Fonts()
        {
            Load();
        }

        public void Load()
        {
            InformerFont = LoadFontFromFile("ProximaNova-Regular.ttf", (int) (GameSettings.CellRadius * 0.8));
            TimerFont = LoadFontFromFile("timer_font.ttf", GameSettings.CellRadius);
        }

        private Font LoadFontFromFile(string fileName, int fontSize)
        {
            fontCollection.AddFontFile("Assets/Fonts/" + fileName);
            return new Font(fontCollection.Families.Last(), fontSize, GraphicsUnit.Pixel);
        }
    }
}