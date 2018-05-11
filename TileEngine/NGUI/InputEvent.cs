using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Input;

namespace TileEngine.NGUI
{
    public class InputEvent : EventArgs
    {
        public InputEvent()
        {

        }

        public InputEvent(InputEvent other)
        {
            InputClass = other.InputClass;
            MouseX = other.MouseX;
            MouseY = other.MouseY;
            DeltaX = other.DeltaX;
            DeltaY = other.DeltaY;
            MouseButton = other.MouseButton;
            Key = other.Key;
            GUIElement = other.GUIElement;
            Window = other.Window;
            Gadget = other.Gadget;
            Requester = other.Requester;
        }

        public InputClass InputClass { get; set; }
        public int MouseX { get; set; }
        public int MouseY { get; set; }
        public int DeltaX { get; set; }
        public int DeltaY { get; set; }
        public MouseButton MouseButton { get; set; }
        public Key Key { get; set; }
        public IGUIElement GUIElement { get; set; }
        public Window Window { get; set; }
        public Gadget Gadget { get; set; }
        public Requester Requester { get; set; }
    }
}
