using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Input;

namespace TileEngine.IGUI
{
    [Flags]
    public enum IDCMPFlags
    {
        None = 0x0,
        SizeVerify = 0x1,
        NewSize = 0x2,
        RefreshWindow = 0x4,
        MouseButtons = 0x8,
        MouseMove = 0x10,
        GadgetDown = 0x20,
        GadgetUp = 0x40,
        ReqSet = 0x80,
        MenuPick = 0x100,
        CloseWindow = 0x200,
        RawKey = 0x400,
        ReqVerify = 0x800,
        ReqClear = 0x1000,
        MenuVerify = 0x2000,
        NewPrefs = 0x4000,
        DiskInserted = 0x8000,
        DiskRemoved = 0x10000,
        WBenchMessage = 0x20000,
        ActiveWindow = 0x40000,
        InactiveWindow = 0x80000,
        DeltaMove = 0x100000,
        VanillaKey = 0x200000,
        IntuiTicks = 0x400000,
        IDCMPUpdate = 0x800000,
        MenuHelp = 0x1000000,
        ChangeWindow = 0x2000000,
        GadgetHelp = 0x4000000,
        LonelyMessage = unchecked((int)0x80000000)
    }

    public enum InputCode
    {
        None,
        Pressed,
        Released,
        Repeat
    }

    public class InputEvent : EventArgs
    {
        private IDCMPFlags _class;
        private Window window;
        private Gadget gadget;
        private InputCode code;
        private int x;
        private int y;
        private MouseButton button;

        public InputEvent(IDCMPFlags _class, Window window, Gadget gadget, int x, int y, InputCode code, MouseButton button)
        {
            this._class = _class;
            this.window = window;
            this.gadget = gadget;
            this.x = x;
            this.y = y;
            this.code = code;
            this.button = button;
        }
        public IDCMPFlags Class
        {
            get { return _class; }
            set { _class = value; }
        }

        public Window Window
        {
            get { return window; }
        }

        public Gadget Gadget
        {
            get { return gadget; }
        }

        public InputCode Code
        {
            get { return code; }
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public MouseButton Button
        {
            get { return button; }
        }
    }
}
