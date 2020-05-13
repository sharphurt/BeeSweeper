using System;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using BeeSweeper.Forms;
using BeeSweeper.model;
using BeeSweeper.View;

namespace BlindMan.View.Controls
{
    public class GameControl : BaseControl
    {
        Images images = new Images();
        private Timer updateTimer;
        private Point _cellUnderCursorLocation = Point.Empty;


        public GameControl(GameModel gameModel) : base(gameModel)
        {
            images.Load();
            gameModel.StartGame();
            BackColor = Palette.Colors.FormBackground;
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
                            DrawImage(new Point(x, y), images.Flag, Palette.Colors.UnrevealedColor, graphics);
                            break;
                        case CellAttr.Questioned:
                            DrawImage(new Point(x, y), images.Question, Palette.Colors.UnrevealedColor, graphics);
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
            graphics.DrawImage(image, destRectangle, 0f, 0f, images.Flag.Width, images.Flag.Height, GraphicsUnit.Pixel);
        }

        private void DrawInformer(Point pos, Graphics graphics)
        {
            DrawEmpty(pos, Palette.Colors.RevealedColor, graphics);
            var cell = gameModel.Field[pos].BeesAround;
            var textPos = Cell.CalculateTextPosition(pos);
            graphics.DrawString(cell.ToString(), Palette.Fonts.LabelFont,
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
                    DrawImage(pos, images.Bee, Palette.Colors.RevealedColor, graphics);
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

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
        }
    }
}