using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;

namespace BeeSweeper.View
{
    public class MenuScene : BaseScene
    {
        public MenuScene(GameModel gameModel) : base(gameModel)
        {
            var tutorialImage = new PictureBox();
            tutorialImage.Image = Image.FromFile("Assets/Textures/tutorial.png");
            tutorialImage.Dock = DockStyle.Fill;
            tutorialImage.SizeMode = PictureBoxSizeMode.StretchImage;

            var backButton = new PictureBox();
            backButton.Image = Image.FromFile("Assets/Textures/back_button.png");
            backButton.SizeMode = PictureBoxSizeMode.StretchImage;
            backButton.Width = 148;
            backButton.Height = 70;
            backButton.Left = 64;
         //   backButton.Click += (sender, args) => gameModel.GameState = GameState.Menu;

            Controls.Add(backButton);
            Controls.Add(tutorialImage);
            SizeChanged += (sender, args) => { backButton.Top = ClientSize.Height - 100; };
        }
    }
}