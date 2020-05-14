using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BeeSweeper.Architecture;

namespace BeeSweeper.View
{
    public class GameForm : Form
    {
        private readonly GameModel _model = new GameModel(Levels.LevelsByName["Easy"]);
        private const int HeaderHeight = GameSettings.CellRadius * 2;

        private readonly Fonts _fonts = new Fonts();
        private readonly Timer SpentTime;
        public Button ResetButton;
        public MenuStrip GameMenu;
        public Panel InfoPanel;
        public Label Timer;

        public GameForm()
        {
            InitializeForm();
            SetScene(new GameScene(_model));
            InitializeControls();
            _model.GameStateChanged += OnGameStateChanged;
        }

        public void InitializeForm()
        {
            var formWidth = Cell.CalculateVertices(new Point(_model.Level.Size.Width - 1, 1))[1].X;
            var formHeight = Cell.CalculateVertices(new Point(0, _model.Level.Size.Height - 1))[0].Y
                             + HeaderHeight + 30;

            ClientSize = new Size(formWidth, formHeight);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint,
                true);
            DoubleBuffered = true;
            Text = GameSettings.GameName;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        public void InitializeControls()
        {
            GameMenu = new MenuStrip
            {
                Location = new Point(0, 0),
                Size = new Size(Width, 10),
                BackColor = Palette.Colors.UnrevealedColor,
                ForeColor = Palette.Colors.RevealedColor,
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

            GameMenu.Items.Add(propertiesButton);
            GameMenu.Items.Add(aboutButton);

            InfoPanel = new Panel
            {
                Location = new Point(0, GameMenu.Height),
                Size = new Size(Width, HeaderHeight),
                BackColor = Palette.Colors.UnrevealedColor,
                ForeColor = Palette.Colors.RevealedColor,
                Dock = DockStyle.Top
            };

            var buttonSize = new Size((int) (InfoPanel.Height * 0.7), (int) (InfoPanel.Height * 0.7));
            ResetButton = new Button
            {
                Size = buttonSize,
                Location = new Point(InfoPanel.Width / 2 - buttonSize.Width / 2,
                    InfoPanel.Height / 2 - buttonSize.Height / 2),
                BackColor = Palette.Colors.RevealedColor,
                FlatStyle = FlatStyle.Flat,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            InfoPanel.Controls.Add(ResetButton);

            var timerFont = _fonts.TimerFont;
            Timer = new Label
            {
                Size = new Size(400, buttonSize.Height),
                Location = new Point(5, ResetButton.Location.Y),
                BackColor = Palette.Colors.UnrevealedColor,
                ForeColor = Palette.Colors.RevealedColor,
                Font = _fonts.TimerFont,
                Text = "00:00"
            };
            InfoPanel.Controls.Add(Timer);

            Controls.Add(InfoPanel);
            Controls.Add(GameMenu);
            ResetButton.FlatAppearance.BorderColor = Palette.Colors.UnrevealedColor;
            ResetButton.Click += (sender, args) => _model.StartGame();
        }

        private void OnGameStateChanged(Winner winner)
        {
            MessageBox.Show("Game over. Winner: " + winner, "Game over", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void SetScene(BaseScene newScene)
        {
            Controls.Add(newScene);
        }
    }
}