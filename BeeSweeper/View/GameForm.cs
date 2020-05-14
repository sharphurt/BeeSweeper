using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using BeeSweeper.View.Controls;
using Timer = System.Timers.Timer;

namespace BeeSweeper.View
{
    public class GameForm : Form
    {
        private readonly GameModel _model = new GameModel(Levels.LevelsByName["Easy"]);
        private readonly GameControl _gameControl;
        private readonly MenuControl _menuControl;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private Timer _updater = new Timer(1000 / GameSettings.TicksPerSecond);

        public GameForm()
        {
            InitializeForm();
            _gameControl = new GameControl(_model);   
            _menuControl = new MenuControl(_model);

            SetScene(_menuControl);
            
            _model.GameFinished += OnGameFinished;
            _model.GameStarted += OnGameStarted;
            _updater.Elapsed += OnUpdateForm;
            _menuControl.GameStartButtonClick += OnGameStartButtonClick;
            _updater.Start();
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


        private void OnGameFinished(Winner winner)
        {
            _stopwatch.Stop();
            Invalidate();
            MessageBox.Show("Game over. Winner: " + winner, "Game over", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void OnGameStarted()
        {
            _stopwatch.Restart();
        }

        private void SetScene(BaseControl newControl)
        {
            ClientSize = newControl.Size;
            foreach (Control control in Controls)
                control.Dispose();
            Controls.Clear();
            Controls.Add(newControl);
        }

        private void OnUpdateForm(object e, EventArgs args)
        {
            _gameControl.StopwatchLabel.Text = $@"{_stopwatch.Elapsed.Minutes}:" + $@"{_stopwatch.Elapsed.Seconds:d2}";
            Invalidate();
        }

        private void OnGameStartButtonClick()
        {
            SetScene(_gameControl);
        }
    }
}