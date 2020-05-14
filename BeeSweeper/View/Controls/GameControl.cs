using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;

namespace BeeSweeper.View.Controls
{
    public class GameControl : BaseControl
    {
        private readonly Fonts _fonts = new Fonts();

        private const int HeaderHeight = GameSettings.CellRadius * 2;
        private Button _resetButton;
        private MenuStrip _gameMenu;
        private Panel _infoPanel;
        private Label _scoreLabel;
        public Label StopwatchLabel;
        private FieldControl _fieldControl;

        private Point? _cellUnderCursorLocation;


        public GameControl(GameModel gameModel) : base(gameModel)
        {
            BackColor = Palette.Colors.FormBackground;
            var formWidth = Cell.CalculateVertices(new Point(gameModel.Level.Size.Width - 1, 1))[1].X;
            var formHeight = Cell.CalculateVertices(new Point(0, gameModel.Level.Size.Height - 1))[0].Y;
            formHeight += HeaderHeight + 27;
            Size = new Size(formWidth, formHeight);
            InitializeControls();
            gameModel.ScoreChanged += OnScoreChange;
        }

        private void InitializeControls()
        {
            _fieldControl = new FieldControl(gameModel);

            _gameMenu = new MenuStrip
            {
                Location = new Point(0, 0),
                Size = new Size(Size.Width, 10),
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                ForeColor = Palette.Colors.TextColor,
                Renderer = new ToolStripProfessionalRenderer(),
                Dock = DockStyle.Top
            };

            var propertiesButton = new ToolStripButton("Properties");

            // propertiesButton.Click += (sender, args) =>
            // {
            //     form.Enabled = false;
            //     var propertiesForm = new PropertiesForm();
            //     propertiesForm.Show(form);
            // };

            var aboutButton = new ToolStripButton("About");

            // aboutButton.Click += (sender, args) =>
            // {
            //     form.Enabled = false;
            //     var propertiesForm = new PropertiesForm();
            //     propertiesForm.Show(form);
            // };

            _gameMenu.Items.Add(propertiesButton);
            _gameMenu.Items.Add(aboutButton);

            _infoPanel = new Panel
            {
                Location = new Point(0, _gameMenu.Height),
                Size = new Size(Size.Width, HeaderHeight),
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                ForeColor = Palette.Colors.TextColor,
                Dock = DockStyle.Top
            };

            var buttonSize = new Size((int) (_infoPanel.Height * 0.7), (int) (_infoPanel.Height * 0.7));
            _resetButton = new Button
            {
                Size = buttonSize,
                Location = new Point(_infoPanel.Width / 2 - buttonSize.Width / 2,
                    _infoPanel.Height / 2 - buttonSize.Height / 2),
                BackColor = Palette.Colors.TextColor,
                FlatStyle = FlatStyle.Flat,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            _infoPanel.Controls.Add(_resetButton);

            StopwatchLabel = new Label
            {
                Size = new Size(Size.Width / 2, buttonSize.Height),
                Location = new Point(5, (HeaderHeight - _fonts.TimerFont.Height) / 2),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                ForeColor = Palette.Colors.TextColor,
                Font = _fonts.TimerFont,
                Text = "00:00"
            };

            _scoreLabel = new Label
            {
                Size = new Size(Size.Width / 2, buttonSize.Height),
                Location = new Point(Size.Width / 2, (HeaderHeight - _fonts.TimerFont.Height) / 2),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Palette.Colors.InterfaceBackgroundColor,
                ForeColor = Palette.Colors.TextColor,
                Font = _fonts.TimerFont,
                Text = "0"
            };

            _infoPanel.Controls.Add(_scoreLabel);
            _infoPanel.Controls.Add(StopwatchLabel);


            Controls.Add(_fieldControl);
            Controls.Add(_infoPanel);
            Controls.Add(_gameMenu);
            _resetButton.FlatAppearance.BorderColor = Palette.Colors.UnrevealedColor;
            _resetButton.Click += (sender, args) => gameModel.StartGame();
        }

        private void OnScoreChange()
        {
            _scoreLabel.Text = gameModel.Score.ToString();
        }
    }
}