using System.Drawing;

namespace BeeSweeper.Forms
{
    public class Images
    {
        public Image Bee { get; private set; }
        public Image Flag { get; private set; }
        public Image Question { get; private set; }
        public Image GameOver { get; private set; }
        public Image Luck { get; private set; }
        public Image Please { get; private set; }
        public Image Win { get; private set; }


        private Image LoadImageFromAssets(string fileName) =>
            Image.FromFile("Assets/Textures/" + fileName);

        public void Load()
        {
            Bee = LoadImageFromAssets("bee.png");
            Flag = LoadImageFromAssets("flag.png");
            Question = LoadImageFromAssets("question.png");
            GameOver = LoadImageFromAssets("gameover.png");
            Luck = LoadImageFromAssets("luck.png");
            Please = LoadImageFromAssets("please.png");
            Win = LoadImageFromAssets("win.png");

        }
    }
}