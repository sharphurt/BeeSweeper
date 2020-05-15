using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using BeeSweeper.Forms;
using Timer = System.Timers.Timer;

namespace BeeSweeper.View.Controls
{
    public class GameControl : BaseControl
    {
        private const int HeaderHeight = GameSettings.CellRadius * 2;

        public static readonly ToolStripButton MenuButton = new ToolStripButton("Menu");
        private static Label _scoreLabel = new Label();
        private readonly Fonts _fonts = new Fonts();
        private readonly Images _images = new Images();

        private readonly GameModel _model;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Timer _updater = new Timer(1000 / GameSettings.TicksPerSecond);

        private Point? _cellUnderCursorLocation;
        private FieldControl _fieldControl;
        private Button _resetButton;
        private Label _stopwatchLabel;

        public GameControl(GameModel gameModel)
        {
            _model = gameModel;

            BackColor = Palette.Colors.FormBackground;
            var formWidth = Cell.CalculateVertices(new Point(gameModel.Level.Size.Width - 1, 1))[1].X;
            var formHeight = Cell.CalculateVertices(new Point(0, gameModel.Level.Size.Height - 1))[0].Y;
            formHeight += HeaderHeight + 27;
            Size = new Size(formWidth, formHeight);

            InitializeControls();

            _model.GameFinished += OnGameFinished;
            _model.GameStarted += OnGameStarted;
            gameModel.ScoreChanged += OnScoreChange;
        }

        private void InitializeControls()
        {
            _fieldControl = new FieldControl(_model);

            var gameMenu = new MenuStrip
            {
                Location = new Point(0, 0),
                Size = new Size(Size.Width, 10),
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                ForeColor = Palette.Colors.TextColor,
                Renderer = new ToolStripProfessionalRenderer(),
                Dock = DockStyle.Top
            };

            gameMenu.Items.Add(MenuButton);

            var infoPanel = new Panel
            {
                Location = new Point(0, gameMenu.Height),
                Size = new Size(Size.Width, HeaderHeight),
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                ForeColor = Palette.Colors.TextColor,
                Dock = DockStyle.Top
            };

            var buttonSize = new Size((int) (infoPanel.Height * 0.7), (int) (infoPanel.Height * 0.7));

            _resetButton = new Button
            {
                Size = buttonSize,
                Location = new Point(infoPanel.Width / 2 - buttonSize.Width / 2,
                    infoPanel.Height / 2 - buttonSize.Height / 2),
                FlatStyle = FlatStyle.Flat,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackgroundImage = _images.Luck
            };


            infoPanel.Controls.Add(_resetButton);

            _stopwatchLabel = new Label
            {
                Size = new Size(Size.Width / 2, buttonSize.Height),
                Location = new Point(5, (HeaderHeight - _fonts.Font.Height) / 2),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                ForeColor = Palette.Colors.TextColor,
                Font = _fonts.Font,
                Text = "00:00"
            };

            _scoreLabel = new Label
            {
                Size = new Size(Size.Width / 2, buttonSize.Height),
                Location = new Point(Size.Width / 2, (HeaderHeight - _fonts.Font.Height) / 2),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                ForeColor = Palette.Colors.TextColor,
                Font = _fonts.Font,
                Text = "0"
            };

            infoPanel.Controls.Add(_scoreLabel);
            infoPanel.Controls.Add(_stopwatchLabel);


            Controls.Add(_fieldControl);
            Controls.Add(infoPanel);
            Controls.Add(gameMenu);
            _resetButton.Click += (sender, args) => OnResetButtonClick();
            _updater.Elapsed += OnUpdateControl;
            _fieldControl.MouseDown += OnGameFieldMouseDown;
            _fieldControl.MouseUp += OnGameFieldMouseUp;
            _updater.Start();
        }

        public void OnResetButtonClick()
        {
            _resetButton.BackgroundImage = _images.Luck;
            _stopwatch.Restart();
            _model.StartGame();
        }

        private void OnScoreChange()
        {
            _scoreLabel.Text = _model.Score.ToString();
        }

        private void OnUpdateControl(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _stopwatchLabel.Text = $@"{_stopwatch.Elapsed.Minutes:d2}:" + $@"{_stopwatch.Elapsed.Seconds:d2}";
        }

        private void OnGameFinished(Winner winner)
        {
            _stopwatch.Stop();
            SetButtonGameOverIcon(winner);
            lock (Messages)
                if (winner == Winner.Player)
                    Messages.Push(new GameMessage("Congratulations! You won with score: " + _model.Score,
                            "Game over!", MessageBoxButtons.OK, MessageBoxIcon.None));
                else if (winner == Winner.Computer)
                    Messages.Push(new GameMessage("Oops! You lose ;( Collected score: " + _model.Score,
                        "Game over!", MessageBoxButtons.OK, MessageBoxIcon.None));
        }

        private void OnGameStarted()
        {
            _stopwatch.Restart();
        }

        private void SetButtonGameOverIcon(Winner winner)
        {
            if (winner == Winner.Player)
                _resetButton.BackgroundImage = _images.Win;
            else if (winner == Winner.Computer)
                _resetButton.BackgroundImage = _images.Lose;
        }

        private void OnGameFieldMouseDown()
        {
            if (_model.GameOver)
                SetButtonGameOverIcon(_model.Winner);
            else
                _resetButton.BackgroundImage = _images.Please;
        }

        private void OnGameFieldMouseUp()
        {
            if (_model.GameOver)
                SetButtonGameOverIcon(_model.Winner);
            else
                _resetButton.BackgroundImage = _images.Luck;
        }
    }
}