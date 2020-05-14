using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using NUnit.Framework.Internal.Execution;

namespace BeeSweeper.View.Controls
{
    public class MenuControl : BaseControl
    {
        public event Action GameStartButtonClick;
        private readonly Fonts _fonts = new Fonts();


        public MenuControl(GameModel gameModel) : base(gameModel)
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
                Text = "New game",
                FlatStyle = FlatStyle.Flat
            };
            
            var settingsButton = new Button
            {
                Size = buttonSize,
                Location = new Point(
                    (Size.Width - buttonSize.Width) / 2, 
                    (Size.Height - buttonSize.Height) / 2 + 30),
                Text = "Settings",
                FlatStyle = FlatStyle.Flat
            };
            
            var exitButton = new Button
            {
                Size = buttonSize,
                Location = new Point(
                    (Size.Width - buttonSize.Width) / 2, 
                    (Size.Height - buttonSize.Height) / 2 + buttonSize.Height + 40),
                Text = "Exit",
                FlatStyle = FlatStyle.Flat
            };

            var nameLabel = new Label
            {
                Size = buttonSize,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(_fonts.InformerFont.FontFamily, 30f, FontStyle.Bold),
                Text = GameSettings.GameName,
                ForeColor = Color.Black
            };
            Controls.Add(startButton);
            Controls.Add(settingsButton);
            Controls.Add(exitButton);
            Controls.Add(nameLabel);

            startButton.Click += (sender, args) => { GameStartButtonClick?.Invoke(); };
        }
    }
}