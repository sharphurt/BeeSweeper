using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BeeSweeper.Architecture;

namespace BeeSweeper.View.Controls
{
    public abstract class BaseControl : UserControl, IControl
    {
        public Stack<GameMessage> Messages { get; }

        protected BaseControl()
        {
            Messages = new Stack<GameMessage>();
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