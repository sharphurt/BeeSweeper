using System;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;
using BlindMan.View.Controls;

namespace BeeSweeper.Forms
{
    public class GameForm : Form
    {
        private readonly GameModel _gameModel = new GameModel(Levels.LevelsByName["Easy"]);

        public GameForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint,
                true);
            DoubleBuffered = true;
            Text = GameSettings.GameName;
            var formWidth = Cell.CalculateVertices(new Point(_gameModel.Level.Size.Width - 1, 1))[1].X;
            var formHeight = Cell.CalculateVertices(new Point(0, _gameModel.Level.Size.Height - 1))[0].Y;
            ClientSize = new Size(formWidth, formHeight);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;    
            SetControl(new GameControl(_gameModel));

        }

        private void SetControl(BaseControl newControl)
        {
            foreach (Control control in Controls)
                control.Dispose();
            Controls.Clear();
            Controls.Add(newControl);
        }

    }
}