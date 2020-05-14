using System;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;

namespace BeeSweeper.View
{
    public abstract class BaseScene : UserControl
    {
        protected GameModel gameModel;
        public Size Size;

        public BaseScene(GameModel gameModel)
        {
            Dock = DockStyle.Fill;
            DoubleBuffered = true;
            this.gameModel = gameModel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BackColor = Palette.Colors.FormBackground;
            Focus();
        }
    }
}