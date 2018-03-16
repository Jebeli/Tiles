using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Input;

namespace TileEngine.GUI
{
    public class InputEvent
    {
        public InputEvent()
        {

        }

        public InputEvent(InputEvent ie)
        {
            InputClass = ie.InputClass;
            Key = ie.Key;
            MouseButton = ie.MouseButton;
            X = ie.X;
            Y = ie.Y;
            TimeStamp = ie.TimeStamp;
        }

        public InputClass InputClass { get; set; }
        public Key Key { get; set; }
        public MouseButton MouseButton { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public TimeSpan TimeStamp { get; set; }
    }
}
