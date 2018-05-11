using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.NGUI
{
    [Flags]
    public enum DimFlags
    {
        None = 0x0,
        RelRight = 0x1,
        RelBottom = 0x2,
        RelWidth = 0x4,
        RelHeight = 0x8,
        RelCenterX = 0x10,
        RelCenterY = 0x20
    }

    public enum InputClass
    {
        None,
        MouseMove,
        MouseDown,
        MouseUp,
        KeyDown,
        KeyUp,
        GadgetDown,
        GadgetUp,
        WindowClose,
        WindowActive,
        WindowInactive,
        ReqSet,
        ReqClear,
        ReqAuto,
        Timer
    }

    [Flags]
    public enum StateFlags
    {
        None = 0x0,
        Hover = 0x1,
        Selected = 0x2,
        Active = 0x4,
        Checked = 0x8
    }

    [Flags]
    public enum WindowFlags
    {
        None = 0x0,
        CloseGadget = 0x1,
        DepthGadget = 0x2,
        ZoomGadget = 0x4,
        DragGadget = 0x8,
        SizeGadget = 0x10,
        Backdrop = 0x20,
        Borderless = 0x40,
        Activate = 0x80,
        SizeBRight = 0x100,
        SizeBBottom = 0x200
    }

    [Flags]
    public enum GadgetFlags
    {
        None = 0x0,
        EndGadget = 0x1,
        SysGadget = 0x2,
        RelVerify = 0x4
    }

    [Flags]
    public enum ReqFlags
    {
        None = 0x0,
        SysReq = 0x1
    }

    [Flags]
    public enum EasyReqFlags
    {
        None = 0x0,
        CenterWindow = 0x1,
        CenterScreen = 0x2,
        Blocking = 0x4
    }

    [Flags]
    public enum PropFlags
    {
        None = 0x0,
        AutoKnob = 0x1,
        FreeVert = 0x2,
        FreeHoriz = 0x4,
        Borderless = 0x8,
        KnobHit = 0x10
    }

    [Flags]
    public enum StringFlags
    {
        None = 0x0,
        LongInt = 0x1,
        StringLeft = 0x0,
        StringRight = 0x2,
        StringCenter = 0x4
    }

    public enum Orientation
    {
        Vertical,
        Horizontal
    }

    public enum WhichDomain
    {
        Minimum,
        Nominal,
        Maximum
    }

    public enum LayoutAlignment
    {
        Minimum,
        Middle,
        Maximum,
        Fill
    }

    public enum LayoutKind
    {
        Box,
        Group,
        Grid
    }
}
