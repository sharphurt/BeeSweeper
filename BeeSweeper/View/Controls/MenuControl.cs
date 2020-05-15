using System;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;

namespace BeeSweeper.View.Controls
{
    public class MenuControl : BaseControl
    {
        private readonly Fonts _fonts = new Fonts();
        public MenuControl()
        {
            BackColor = Color.White;
            Size = new Size(400, 300);
            var buttonSize = new Size(200, 50);
            var startButton = new Button
            {
                Size = buttonSize,
                Location = new Point(
                    (Size.Width - buttonSize.Width) / 2,
                    (Size.Height - buttonSize.Height) / 2 - buttonSize.Height + 20),
                Font = _fonts.ButtonFont,
                Text = "New game",
                FlatStyle = FlatStyle.Flat
            };

            var settingsButton = new Button
            {
                Size = buttonSize,
                Location = new Point(
                    (Size.Width - buttonSize.Width) / 2,
                    (Size.Height - buttonSize.Height) / 2 + 30),
                Font = _fonts.ButtonFont,
                Text = "Settings",
                FlatStyle = FlatStyle.Flat
            };

            var aboutButton = new Button
            {
                Size = buttonSize,
                Location = new Point(
                    (Size.Width - buttonSize.Width) / 2,
                    (Size.Height - buttonSize.Height) / 2 + buttonSize.Height + 40),
                Font = _fonts.ButtonFont,
                Text = "About",
                FlatStyle = FlatStyle.Flat
            };

            var nameLabel = new Label
            {
                Size = buttonSize,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(_fonts.Font.FontFamily, 30f, FontStyle.Bold),
                Text = GameSettings.GameName,
                ForeColor = Palette.Colors.TextColor
            };
            Controls.Add(startButton);
            Controls.Add(settingsButton);
            Controls.Add(aboutButton);
            Controls.Add(nameLabel);

            startButton.Click += (sender, args) => { GameStartButtonClick?.Invoke(); };
            settingsButton.Click += (sender, args) => { SettingsButtonClick?.Invoke(); };
            aboutButton.Click += (sender, args) => { AboutButtonClick?.Invoke(); };
        }

        public static event Action GameStartButtonClick;
        public static event Action SettingsButtonClick;
        public static event Action AboutButtonClick;
    }
}