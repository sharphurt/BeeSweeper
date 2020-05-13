using System;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using BeeSweeper.Forms;
using BeeSweeper.model;
using BeeSweeperGame;

namespace BlindMan.View.Controls
{
    public class GameControl : BaseControl
    {
        Images images = new Images();
        private Timer updateTimer;

        private static Color _emptyCellBrush = Color.SandyBrown;
        private static Color _unrevealedColor = Color.FromArgb(80, 80, 80);
        private static Color _revealedColor = Color.FromArgb(190, 190, 190);
        private Point _cellUnderCursorLocation = Point.Empty;

        private static Font _labelFont =
            new Font(new FontFamily("Courier New"), GameSettings.CellSide * 0.8f, FontStyle.Bold);

        private static Pen _outlinePen = new Pen(Color.FromArgb(127, 127, 127));

        private const int TileSize = 40;

        public GameControl(GameModel gameModel) : base(gameModel)
        {
            images.Load();
            gameModel.StartGame();
            BackColor = Color.Black;
        }

        private void Draw(Graphics graphics)
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
                            DrawImageCell(new Point(x, y), images.Flag, graphics);
                            break;
                        case CellAttr.Questioned:
                            DrawImageCell(new Point(x, y), images.Question, graphics);
                            break;
                        case CellAttr.Opened:
                            DrawOpenedCell(new Point(x, y), cell.CellType, graphics);
                            break;
                        default:
                            DrawEmpty(new Point(x, y), graphics);
                            break;
                    }
                }
            }
        }

        private void DrawEmpty(Point pos, Graphics graphics)
        {
            var vertices = Cell.CalculateVertices(pos);
            graphics.FillPolygon(new SolidBrush(_unrevealedColor), vertices);
            graphics.DrawPolygon(_outlinePen, vertices);
        }

        // private static void DrawAim(Graphics graphics)
        // {
        //     if (MouseHandler.CellUnderCursor != null && MouseHandler.MousePressed)
        //         graphics.DrawPolygon(new Pen(GameProperties.CellPathColor, 2f), MouseHandler.CellUnderCursor.Vertices);
        // }

        private void DrawImageCell(Point pos, Image image, Graphics graphics)
        {
            DrawEmpty(pos, graphics);
            var imagePos = Cell.CalculateImagePosition(pos);
            var destRectangle = new Rectangle(imagePos.X, imagePos.Y, GameSettings.ImageSize, GameSettings.ImageSize);
            graphics.DrawImage(image, destRectangle, 0f, 0f, images.Flag.Width, images.Flag.Height, GraphicsUnit.Pixel);
        }

        private void DrawInformer(Point pos, Graphics graphics)
        {
            DrawEmpty(pos, graphics);
            var cell = gameModel.Field[pos].BeesAround;
            var imagePos = Cell.CalculateImagePosition(pos);
            graphics.DrawString(cell.ToString(), _labelFont, new SolidBrush(Color.Black), imagePos.X, imagePos.Y);
        }

        private void DrawOpenedCell(Point pos, CellType cellType, Graphics graphics)
        {
            switch (cellType)
            {
                case CellType.Informer:
                    DrawInformer(pos, graphics);
                    break;
                case CellType.Bee:
                    DrawImageCell(pos, images.Bee, graphics);
                    break;
                default:
                    DrawEmpty(pos, graphics);
                    break;
            }
        }

        private void StartGameUpdaterTimer(GameModel model)
        {
            updateTimer = new Timer();
            updateTimer.Interval = 1000 / GameSettings.TicksPerSecond;
            updateTimer.Tick += (sender, args) => Invalidate();
            updateTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _cellUnderCursorLocation = CellByLocation.GetCellLocationByCursorPosition(e.Location, gameModel.Field);
            if (gameModel.GameOver || _cellUnderCursorLocation == Point.Empty) return;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None && _cellUnderCursorLocation != Point.Empty)
                _cellUnderCursorLocation = CellByLocation.GetCellLocationByCursorPosition(e.Location, gameModel.Field);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_cellUnderCursorLocation == Point.Empty) return;
            if (gameModel.GameOver) return;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            gameModel.OpenCell(_cellUnderCursorLocation);
        }
    }
}