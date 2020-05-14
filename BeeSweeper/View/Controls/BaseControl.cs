using System;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;

namespace BeeSweeper.View.Controls
{
    public abstract class BaseControl : UserControl
    {
        protected GameModel gameModel;
        public Size Size;

        public BaseControl(GameModel gameModel)
        {
            Dock = DockStyle.Fill;
            DoubleBuffered = true;
            this.gameModel = gameModel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Focus();
        }
    }
}