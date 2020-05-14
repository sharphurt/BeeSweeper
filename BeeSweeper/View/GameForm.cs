using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using Timer = System.Timers.Timer;

namespace BeeSweeper.View
{
    public class GameForm : Form
    {
        private readonly GameModel _model = new GameModel(Levels.LevelsByName["Easy"]);
        private readonly GameScene _scene;
        private readonly Stopwatch _stopwatch;

        private Timer _updater = new Timer(1000 / GameSettings.TicksPerSecond);

        public GameForm()
        {
            InitializeForm();
            _scene = new GameScene(_model);  
            ClientSize = new Size(_scene.Size.Width, _scene.Size.Height);
            SetScene(_scene);


            _model.GameFinished += OnGameFinished;
            _model.GameStarted += OnGameStarted;
            _updater.Elapsed += OnUpdateForm;
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

        private void SetScene(BaseScene newScene)
        {
            Controls.Add(newScene);
        }

        private void OnUpdateForm(object e, EventArgs args)
        {
            _scene.StopwatchLabel.Text = $@"{_stopwatch.Elapsed.Minutes}:" + $@"{_stopwatch.Elapsed.Seconds:d2}";
            Invalidate();
        }
    }
}