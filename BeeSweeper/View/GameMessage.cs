using System.Windows.Forms;

namespace BeeSweeper.View
{
    public class GameMessage
    {
        private string Message { get; }

        private string Caption { get; }
        private MessageBoxButtons Buttons { get; }
        private MessageBoxIcon Icon { get; }

        public GameMessage(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Message = message;
            Caption = caption;
            Buttons = buttons;
            Icon = icon;
        }

        public void Show()
        {
            MessageBox.Show(Message, Caption, Buttons, Icon);
        }
    }
}