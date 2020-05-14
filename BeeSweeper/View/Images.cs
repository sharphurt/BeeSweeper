using System.Drawing;

namespace BeeSweeper.Forms
{
    public class Images
    {
        public Images()
        {
            Load();
        }

        public Image Bee { get; private set; }
        public Image Flag { get; private set; }
        public Image Question { get; private set; }
        public Image Lose { get; private set; }
        public Image Luck { get; private set; }
        public Image Please { get; private set; }
        public Image Win { get; private set; }
        public Image About { get; private set; }
        public Image VkLogo { get; private set; }
        public Image GithubLogo { get; private set; }

        private Image LoadImageFromAssets(string fileName)
        {
            return Image.FromFile("Assets/Textures/" + fileName);
        }

        public void Load()
        {
            Bee = LoadImageFromAssets("bee.png");
            Flag = LoadImageFromAssets("flag.png");
            Question = LoadImageFromAssets("question.png");
            Lose = LoadImageFromAssets("gameover.png");
            Luck = LoadImageFromAssets("luck.png");
            Please = LoadImageFromAssets("please.png");
            Win = LoadImageFromAssets("win.png");
            About = LoadImageFromAssets("about.png");
            VkLogo = LoadImageFromAssets("vk_logo.png");
            GithubLogo = LoadImageFromAssets("github_logo.png");
        }
    }
}