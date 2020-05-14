using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using BeeSweeper.Forms;
using BeeSweeper.model;
using BeeSweeper.View.Controls;

namespace BeeSweeper.View
{
    public class FieldControl : BaseControl
    {
        private readonly Images _images = new Images();
        private Fonts _fonts = new Fonts();

        private Point? _cellUnderCursorLocation;


        public FieldControl(GameModel gameModel) : base(gameModel)
        {
            BackColor = Palette.Colors.FormBackground;

            var formWidth = Cell.CalculateVertices(new Point(gameModel.Level.Size.Width - 1, 1))[1].X;
            var formHeight = Cell.CalculateVertices(new Point(0, gameModel.Level.Size.Height - 1))[0].Y;
            Size = new Size(formWidth, formHeight);
            _images.Load();
            gameModel.PrepareField();
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
            if (_cellUnderCursorLocation.HasValue)
                graphics.DrawPolygon(new Pen(Color.DimGray, 3f),
                    Cell.CalculateVertices(_cellUnderCursorLocation.Value));
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
                _cellUnderCursorLocation = null;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None && _cellUnderCursorLocation.HasValue && !gameModel.GameOver)
                _cellUnderCursorLocation = CellByLocation.GetCellLocationByCursorPosition(e.Location, gameModel.Field);
            else
                _cellUnderCursorLocation = null;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _cellUnderCursorLocation = null;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (gameModel.GameOver)
                return;
            if (e.Button == MouseButtons.Left && _cellUnderCursorLocation.HasValue)
                gameModel.OpenCell(_cellUnderCursorLocation.Value);
            else if (e.Button == MouseButtons.Right && _cellUnderCursorLocation.HasValue)
                gameModel.ChangeAttr(_cellUnderCursorLocation.Value);
        }
    }
}