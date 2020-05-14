using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using BeeSweeper.Forms;
using NUnit.Framework.Internal.Execution;

namespace BeeSweeper.View.Controls
{
    public class AboutControl : BaseControl
    {
        public static event Action BackButtonClick;

        private readonly Images _images = new Images();
        private readonly Fonts _fonts = new Fonts();

        public AboutControl()
        {
            Size = new Size(400, 300);
            BackgroundImage = _images.About;
            BackgroundImageLayout = ImageLayout.Zoom;
            var buttonSize = new Size(200, 50);

            var nameLabel = new Label
            {
                Size = buttonSize,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(_fonts.Font.FontFamily, 30f, FontStyle.Bold),
                Text = GameSettings.GameName,
                ForeColor = Palette.Colors.TextColor,
                BackColor = Palette.Colors.FormBackground
            };
            
            var backButton = new Button
            {
                Size = buttonSize,
                Location = new Point(
                    (Size.Width - buttonSize.Width) / 2,
                    (Size.Height - buttonSize.Height) / 2 + buttonSize.Height + 40),
                Font = _fonts.ButtonFont,
                Text = "Back",
                FlatStyle = FlatStyle.Flat
            };

            var vkLink = new PictureBox
            {
                Size = new Size(24, 24),
                Location = new Point(165, 160),
                Image = _images.VkLogo,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            var githubLink = new PictureBox
            {
                Size = new Size(24, 24),
                Location = new Point(195, 160),
                Image = _images.GithubLogo,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            Controls.Add(githubLink);
            Controls.Add(vkLink);
            Controls.Add(backButton);
            Controls.Add(nameLabel);

            vkLink.Click += (sender, args) => { System.Diagnostics.Process.Start(GameSettings.VkLink); };
            githubLink.Click += (sender, args) => { System.Diagnostics.Process.Start(GameSettings.GithubLink); };
            backButton.Click += (sender, args) => { BackButtonClick?.Invoke(); };
        }
    }
}