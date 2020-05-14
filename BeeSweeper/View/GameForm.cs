using System;
using System.Diagnostics;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using BeeSweeper.View.Controls;

namespace BeeSweeper.View
{
    public class GameForm : Form
    {
        private GameModel _model;

        public GameForm()
        {
            InitializeForm();
            SetScene(new MenuControl());

            MenuControl.GameStartButtonClick += OnGameStartButtonClick;
            MenuControl.SettingsButtonClick += OnSettingsButtonClick;
            SettingsControl.ApplyButtonClick += OnCloseSettings;
            SettingsControl.CancelButtonClick += OnCloseSettings;
            GameControl.MenuButton.Click += OnMenuButtonClick;
        }

        public void InitializeForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint,
                true);
            DoubleBuffered = true;
            Text = GameSettings.GameName;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }


        private void SetScene(BaseControl newControl)
        {
            ClientSize = newControl.Size;
            // foreach (Control control in Controls)
            //     control.Dispose();
            Controls.Clear();
            Controls.Add(newControl);
        }

        private void OnMenuButtonClick(object sender, EventArgs eventArgs)
        {
            SetScene(new MenuControl());
        }

        private void OnGameStartButtonClick()
        {
            _model = new GameModel(Levels.SelectedLevel);
            var gameControl = new GameControl(_model);
            SetScene(gameControl);
            gameControl.OnResetButtonClick();
        }

        private void OnSettingsButtonClick()
        {
            var gameControl = new SettingsControl();
            SetScene(gameControl);
        }

        private void OnCloseSettings()
        {
            var menuControl = new MenuControl();
            SetScene(menuControl);
        }
    }
}