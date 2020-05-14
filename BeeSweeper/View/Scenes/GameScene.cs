using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using BeeSweeper.Forms;
using BeeSweeper.model;

namespace BeeSweeper.View
{
    public class GameScene : BaseScene
    {
        private readonly Images _images = new Images();
        private Fonts _fonts = new Fonts();
        
        private const int HeaderHeight = GameSettings.CellRadius * 2;
        public Button ResetButton;
        public MenuStrip GameMenu;
        public Panel InfoPanel;
        public Label ScoreLabel;
        public Label StopwatchLabel;
        public FieldControl FieldControl;

        private Point _cellUnderCursorLocation = Point.Empty;


        public GameScene(GameModel gameModel) : base(gameModel)
        {
            var formWidth = Cell.CalculateVertices(new Point(gameModel.Level.Size.Width - 1, 1))[1].X;
            var formHeight = Cell.CalculateVertices(new Point(0, gameModel.Level.Size.Height - 1))[0].Y
                             + HeaderHeight +
                             27;
            Size = new Size(formWidth, formHeight);
            InitializeControls();
            gameModel.ScoreChanged += OnScoreChange;
        }

        public void InitializeControls()
        {
            FieldControl = new FieldControl(gameModel);

            GameMenu = new MenuStrip
            {
                Location = new Point(0, 0),
                Size = new Size(Size.Width, 10),
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
                Size = new Size(Size.Width, HeaderHeight),
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

            StopwatchLabel = new Label
            {
                Size = new Size(Size.Width / 2, buttonSize.Height),
                Location = new Point(5, (HeaderHeight - _fonts.TimerFont.Height) / 2),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Palette.Colors.UnrevealedColor,
                ForeColor = Palette.Colors.RevealedColor,
                Font = _fonts.TimerFont,
                Text = "00:00"
            };

            ScoreLabel = new Label
            {
                Size = new Size(Size.Width / 2, buttonSize.Height),
                Location = new Point(Size.Width / 2, (HeaderHeight - _fonts.TimerFont.Height) / 2),
                TextAlign  = ContentAlignment.MiddleRight,
                BackColor = Palette.Colors.UnrevealedColor,
                ForeColor = Palette.Colors.RevealedColor,
                Font = _fonts.TimerFont,
                Text = "0"
            };

            InfoPanel.Controls.Add(ScoreLabel);
            InfoPanel.Controls.Add(StopwatchLabel);


            Controls.Add(FieldControl);
            Controls.Add(InfoPanel);
            Controls.Add(GameMenu);
            ResetButton.FlatAppearance.BorderColor = Palette.Colors.UnrevealedColor;
            ResetButton.Click += (sender, args) => gameModel.StartGame();
        }


        private void DrawGrid(Graphics graphics)
        {
            var field = gameModel.Field;

            for (var x = 0; x < field.Width; x++)
            {
                for (var y = 0; y < field.Height; y++)
                {
                    var cell = gameModel.Field[x, y];
                    switch (cell.CellAttr)
                    {
                        case CellAttr.Flagged:
                            DrawImage(new Point(x, y), _images.Flag, Palette.Colors.UnrevealedColor, graphics);
                            break;
                        case CellAttr.Questioned:
                            DrawImage(new Point(x, y), _images.Question, Palette.Colors.UnrevealedColor, graphics);
                            break;
                        case CellAttr.Opened:
                            DrawOpenedCell(new Point(x, y), cell.CellType, graphics);
                            break;
                        case CellAttr.None:
                            DrawEmpty(new Point(x, y), Palette.Colors.UnrevealedColor, graphics);
                            break;
                    }
                }
            }
        }

        private void DrawEmpty(Point pos, Color color, Graphics graphics)
        {
            var vertices = Cell.CalculateVertices(pos);
            graphics.FillPolygon(new SolidBrush(color), vertices);
            graphics.DrawPolygon(Palette.Pens.OutlinePen, vertices);
        }

        private void DrawAim(Graphics graphics)
        {
            if (_cellUnderCursorLocation != Point.Empty)
                graphics.DrawPolygon(new Pen(Color.DimGray, 3f), Cell.CalculateVertices(_cellUnderCursorLocation));
        }

        private void DrawImage(Point pos, Image image, Color backColor, Graphics graphics)
        {
            DrawEmpty(pos, backColor, graphics);
            var imagePos = Cell.CalculateImagePosition(pos);
            var destRectangle = new Rectangle(imagePos.X, imagePos.Y, GameSettings.CellRadius, GameSettings.CellRadius);
            graphics.DrawImage(image, destRectangle, 0f, 0f, _images.Flag.Width, _images.Flag.Height,
                GraphicsUnit.Pixel);
        }

        private void DrawInformer(Point pos, Graphics graphics)
        {
            DrawEmpty(pos, Palette.Colors.RevealedColor, graphics);
            var cell = gameModel.Field[pos].BeesAround;
            var textPos = Cell.CalculateTextPosition(pos);
            graphics.DrawString(cell.ToString(), _fonts.InformerFont,
                new SolidBrush(Palette.Colors.ColorsByNeighbouringBees[cell]), textPos.X, textPos.Y);
        }

        private void DrawOpenedCell(Point pos, CellType cellType, Graphics graphics)
        {
            switch (cellType)
            {
                case CellType.Informer:
                    DrawInformer(pos, graphics);
                    break;
                case CellType.Bee:
                    DrawImage(pos, _images.Bee, Palette.Colors.RevealedColor, graphics);
                    break;
                case CellType.Empty:
                    DrawEmpty(pos, Palette.Colors.RevealedColor, graphics);
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawGrid(e.Graphics);
            DrawAim(e.Graphics);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!gameModel.GameOver)
                _cellUnderCursorLocation = CellByLocation.GetCellLocationByCursorPosition(e.Location, gameModel.Field);
            else
                _cellUnderCursorLocation = Point.Empty;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None && _cellUnderCursorLocation != Point.Empty && !gameModel.GameOver)
                _cellUnderCursorLocation = CellByLocation.GetCellLocationByCursorPosition(e.Location, gameModel.Field);
            else
                _cellUnderCursorLocation = Point.Empty;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _cellUnderCursorLocation = Point.Empty;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (gameModel.GameOver)
                return;
            if (e.Button == MouseButtons.Left)
                gameModel.OpenCell(_cellUnderCursorLocation);
            else if (e.Button == MouseButtons.Right)
                gameModel.ChangeAttr(_cellUnderCursorLocation);
        }

        private void OnScoreChange()
        {
            ScoreLabel.Text = gameModel.Score.ToString();
        }
    }
}