using System;
using System.Threading;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using BeeSweeper.Forms;
using BeeSweeper.View.Controls;

namespace BeeSweeper.View
{
    public class GameForm : Form
    {
        private GameModel _model;
        private IControl _currentControl;
        private Images _images = new Images();

        public GameForm()
        {
            InitializeForm();
            SetScene(new MenuControl());

            MenuControl.GameStartButtonClick += OnGameStartButtonClick;
            MenuControl.SettingsButtonClick += OnSettingsButtonClick;
            MenuControl.AboutButtonClick += OnAboutButtonClick;

            SettingsControl.ApplyButtonClick += OnCloseSettings;
            SettingsControl.CancelButtonClick += OnCloseSettings;

            GameControl.MenuButton.Click += OnMenuButtonClick;
            AboutControl.BackButtonClick += OnBackButtonClick;

            var thread = new Thread(o => ShowMessagesIfExists());
            thread.Start();
        }

        public void InitializeForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint,
                true);
            DoubleBuffered = true;
            Text = GameSettings.GameName;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = _images.FormIcon;
        }


        private void SetScene(BaseControl newControl)
        {
            ClientSize = newControl.Size;
            Controls.Clear();
            Controls.Add(newControl);
            _currentControl = newControl;
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
            SetScene(new SettingsControl());
        }

        private void OnCloseSettings()
        {
            SetScene(new MenuControl());
        }

        private void OnAboutButtonClick()
        {
            SetScene(new AboutControl());
        }

        private void OnBackButtonClick()
        {
            SetScene(new MenuControl());
        }

        private void ShowMessagesIfExists()
        {
            while (true)
            {
                lock (_currentControl.Messages)
                {
                    if (_currentControl.Messages.Count > 0)
                        _currentControl.Messages.Pop().Show();
                }

                Thread.Sleep(100);
            }
        }
    }
}