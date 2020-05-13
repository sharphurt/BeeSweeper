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
            Text = GameSettings.GameName;
            var formHeight = _gameModel.Level.Size.Height * GameSettings.CellHeight -
                             (_gameModel.Level.Size.Height - 1) * (int) Math.Floor(GameSettings.CellSide * 0.5);
            var formWidth = GameSettings.CellWidth * _gameModel.Level.Size.Width + GameSettings.CellWidth / 2;
            Size = new Size(formWidth, formHeight);
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