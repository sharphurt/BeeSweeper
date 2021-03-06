﻿using System;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using BeeSweeper.Forms;
using BeeSweeper.model;

namespace BeeSweeper.View.Controls
{
    public class FieldControl : BaseControl
    {
        private readonly Fonts _fonts = new Fonts();
        private readonly Images _images = new Images();
        private readonly GameModel _model;
        private Point? _cellUnderCursorLocation;

        public FieldControl(GameModel gameModel)
        {
            _model = gameModel;
            BackColor = Palette.Colors.FormBackground;
            var formWidth = Cell.CalculateVertices(new Point(gameModel.Level.Size.Width - 1, 1))[1].X;
            var formHeight = Cell.CalculateVertices(new Point(0, gameModel.Level.Size.Height - 1))[0].Y;
            Size = new Size(formWidth, formHeight);
            _images.Load();
            gameModel.PrepareField();
        }

        public new event Action MouseDown;
        public new event Action MouseUp;

        private void DrawGrid(Graphics graphics)
        {
            var field = _model.Field;

            for (var x = 0; x < field.Width; x++)
            for (var y = 0; y < field.Height; y++)
            {
                var cell = _model.Field[x, y];
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
            var cell = _model.Field[pos].BeesAround;
            var textPos = Cell.CalculateTextPosition(pos);
            graphics.DrawString(cell.ToString(), new Font(_fonts.Font, FontStyle.Bold),
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
            MouseDown?.Invoke();
            if (!_model.GameOver)
                _cellUnderCursorLocation = CellByLocation.GetCellLocationByCursorPosition(e.Location, _model.Field);
            else
                _cellUnderCursorLocation = null;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None && _cellUnderCursorLocation.HasValue && !_model.GameOver)
                _cellUnderCursorLocation = CellByLocation.GetCellLocationByCursorPosition(e.Location, _model.Field);
            else
                _cellUnderCursorLocation = null;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseUp?.Invoke();
            _cellUnderCursorLocation = null;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (_model.GameOver)
                return;
            if (e.Button == MouseButtons.Left && _cellUnderCursorLocation.HasValue)
                _model.OpenCell(_cellUnderCursorLocation.Value);
            else if (e.Button == MouseButtons.Right && _cellUnderCursorLocation.HasValue)
                _model.ChangeAttr(_cellUnderCursorLocation.Value);
            Invalidate();
        }
    }
}