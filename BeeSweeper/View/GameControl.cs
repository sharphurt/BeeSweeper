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
            new Font(new FontFamily("Arial"), GameSettings.CellRadius * 0.6f, FontStyle.Bold);

        private static Pen _outlinePen = new Pen(Color.FromArgb(127, 127, 127));

        private const int TileSize = 40;

        public GameControl(GameModel gameModel) : base(gameModel)
        {
            images.Load();
            gameModel.StartGame();
            BackColor = Color.Gray;
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
                            DrawImageCell(new Point(x, y), images.Flag, graphics);
                            break;
                        case CellAttr.Questioned:
                            DrawImageCell(new Point(x, y), images.Question, graphics);
                            break;
                        case CellAttr.Opened:
                            DrawOpenedCell(new Point(x, y), cell.CellType, graphics);
                            break;
                        case CellAttr.None:
                            DrawEmpty(new Point(x, y), _unrevealedColor, graphics);
                            break;
                    }
                }
            }
        }

        private void DrawEmpty(Point pos, Color color, Graphics graphics)
        {
            var vertices = Cell.CalculateVertices(pos);
            graphics.FillPolygon(new SolidBrush(color), vertices);
            graphics.DrawPolygon(_outlinePen, vertices);
        }

        private void DrawAim(Graphics graphics)
        {
            if (_cellUnderCursorLocation != Point.Empty)
                graphics.DrawPolygon(new Pen(Color.Aqua, 2f), Cell.CalculateVertices(_cellUnderCursorLocation));
        }

        private void DrawImageCell(Point pos, Image image, Graphics graphics)
        {
            DrawEmpty(pos, _revealedColor, graphics);
            var imagePos = Cell.CalculateImagePosition(pos);
            var destRectangle = new Rectangle(imagePos.X, imagePos.Y, GameSettings.ImageSize, GameSettings.ImageSize);
            graphics.DrawImage(image, destRectangle, 0f, 0f, images.Flag.Width, images.Flag.Height, GraphicsUnit.Pixel);
        }

        private void DrawInformer(Point pos, Graphics graphics)
        {
            DrawEmpty(pos, _revealedColor, graphics);
            var cell = gameModel.Field[pos].BeesAround;
            var textPos = Cell.CalculateTextPosition(pos);
            graphics.DrawString(cell.ToString(), _labelFont, new SolidBrush(Color.Black), textPos.X, textPos.Y);
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
                case CellType.Empty:
                    DrawEmpty(pos, _revealedColor, graphics);
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
            _cellUnderCursorLocation = GetCellLocationByCursorPosition(e.Location, gameModel.Field);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None && _cellUnderCursorLocation != Point.Empty)
                _cellUnderCursorLocation = GetCellLocationByCursorPosition(e.Location, gameModel.Field);
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
            gameModel.OpenCell(_cellUnderCursorLocation);
        }

        public Point GetCellLocationByCursorPosition(Point cursor, Field field)
        {
            for (var x = 0; x < field.Width; x++)
            for (var y = 0; y < field.Height; y++)
            {
                var location = new Point(x, y);
                if (IsPointOnCell(cursor, location))
                    return location;
            }

            return Point.Empty;
        }

        private bool IsPointOnCell(Point cursor, Point cellPos)
        {
            var vertices = Cell.CalculateVertices(cellPos);
            for (var i = 0; i < 6; i++)
                if (IsPointLeftToSegment(vertices[i], vertices[(i + 1) % 6], cursor))
                    return false;
            return true;
        }

        private bool IsPointLeftToSegment(Point segmentBegin, Point segmentEnd, Point point)
        {
            return (segmentEnd.X - segmentBegin.X) * (point.Y - segmentBegin.Y)
                - (segmentEnd.Y - segmentBegin.Y) * (point.X - segmentBegin.X) >= 0;
        }
    }
}