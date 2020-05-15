using System.Collections.Generic;

namespace BeeSweeper.View.Controls
{
    public interface IControl
    {
        Stack<GameMessage> Messages { get; }
    }
}