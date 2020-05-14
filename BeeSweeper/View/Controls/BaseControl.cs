using System;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;

namespace BeeSweeper.View.Controls
{
    public abstract class BaseControl : UserControl
    {
        public Size Size;

        public BaseControl()
        {
            Dock = DockStyle.Fill;
            DoubleBuffered = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Focus();
        }
    }
}