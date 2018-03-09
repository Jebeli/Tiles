/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
*/

namespace TileEngine.GUI
{
    using System;
    public enum WATags
    {
        WA_Left,
        WA_Top,
        WA_Width,
        WA_Height,
        WA_IDCMP,
        WA_Flags,
        WA_Gadgets,
        WA_Checkmark,
        WA_Title,
        WA_ScreenTitle,
        WA_MinWidth,
        WA_MinHeight,
        WA_MaxWidth,
        WA_MaxHeight,
        WA_InnerWidth,
        WA_InnerHeight,
        WA_Screen,
        WA_Zoom,
        WA_SizeGadget,
        WA_DragBar,
        WA_DepthGadget,
        WA_CloseGadget,
        WA_Backdrop,
        WA_ReportMouse,
        WA_NoCareRefresh,
        WA_Borderless,
        WA_Activate,
        WA_RMBTrap,
        WA_WBenchWindow,
        WA_SimpleRefresh,
        WA_SmartRefresh,
        WA_SizeBRight,
        WA_SizeBBottom,
        WA_AutoAdjust,
        WA_GimmeZeroZero,
        WA_MenuHelp,
        WA_NewLookMenus,
        WA_AmigaKey,
        WA_NotifyDepth,
        WA_Pointer,
        WA_BusyPointer,
        WA_PointerDelay,
        WA_TabletMessages,
        WA_HelpGroup,
        WA_HelpGroupWindow,
        WA_Hidden,
        WA_ToolBox,
        WA_ShapeRegion,
        WA_ShapeHook,
        WA_InFrontOf,
        WA_Opacity,
        WA_BackgroundColor,
        WA_ForegroundColor
    }

    [Flags]
    public enum WindowFlags
    {
        None = 0,
        WFLG_SIZEGADGET = 0x1,
        WFLG_DRAGBAR = 0x2,
        WFLG_DEPTHGADGET = 0x4,
        WFLG_CLOSEGADGET = 0x8,
        WFLG_SIZEBRIGHT = 0x10,
        WFLG_SIZEBBOTTOM = 0x20,
        WFLG_SMART_REFRESH = 0x0,
        WFLG_SIMPLE_REFRESH = 0x40,
        WFLG_SUPER_BITMAP = 0x80,
        WFLG_OTHER_REFRESH = WFLG_SIMPLE_REFRESH | WFLG_SUPER_BITMAP,
        WFLG_REFRESHBITS = WFLG_OTHER_REFRESH,
        WFLG_BACKDROP = 0x100,
        WFLG_REPORTMOUSE = 0x200,
        WFLG_GIMMEZEROZERO = 0x400,
        WFLG_BORDERLESS = 0x800,
        WFLG_ACTIVATE = 0x1000,
        WFLG_WINDOWACTIVE = 0x2000,
        WFLG_INREQUEST = 0x4000,
        WFLG_MENUSTATE = 0x8000,
        WFLG_RMBTRAP = 0x10000,
        WFLG_NOCAREREFRESH = 0x20000,
        WFLG_NW_EXTENDED = 0x40000, // 18
        WFLG_NEWLOOKMENUS = 0x200000, // 21
        WFLG_WINDOWREFRESH = 0x1000000, // 24
        WFLG_WBENCHWINDOW = 0x2000000, // 25
        WFLG_WINDOWTICKED = 0x4000000, // 26
        WFLG_VISITOR = 0x8000000, // 27
        WFLG_ZOOMED = 0x10000000, // 28
        WFLG_HASZOOM = 0x20000000, // 29
        WFLG_TOOLBOX = 0x40000000, // 30
        WFLG_PRIVATEFLAGS = WFLG_WINDOWREFRESH | WFLG_WINDOWTICKED | WFLG_VISITOR | WFLG_ZOOMED | WFLG_WINDOWACTIVE
    }

    [Flags]
    public enum IDCMPFlags
    {
        SIZEVERIFY = 0x01,
        NEWSIZE = 0x02,
        REFRESHWINDOW = 0x04,
        MOUSEBUTTONS = 0x08,
        MOUSEMOVE = 0x10,
        GADGETDOWN = 0x20,
        GADGETUP = 0x40,
        REQSET = 0x80,
        MENUPICK = 0x100,
        CLOSEWINDOW = 0x200,
        RAWKEY = 0x400,
        REQVERIFY = 0x800,
        REQCLEAR = 0x1000,
        MENUVERIFY = 0x2000,
        NEWPREFS = 0x4000,
        DISKINSERTED = 0x8000,
        DISKREMOVED = 0x10000,
        WBENCHMESSAGE = 0x20000,
        ACTIVEWINDOW = 0x40000,
        INACTIVEWINDOW = 0x80000,
        DELTAMOVE = 0x100000,
        VANILLAKEY = 0x200000,
        INTUITICKS = 0x400000,
        AUTOREQUEST = 0x800000,
        LONELYMESSAGE = 0x8000000
    }

    [Flags]
    public enum GadgetType
    {
        BOOLGADGET = 0x0001,
        GADGET0002 = 0x0002,
        PROPGADGET = 0x0003,
        STRGADGET = 0x0004,
        SIZING = 0x0010,
        WDRAGGING = 0x0020,
        WDEPTH = 0x040,
        WZOOM = 0x060,
        CLOSE = 0x080,
        REQGADGET = 0x1000,
        GZZGADGET = 0x2000,
        SCRGADGET = 0x4000,
        SYSGADGET = 0x8000,
        GADGETTYPE = 0xFC00
    }

    [Flags]
    public enum GadgetActivation
    {
        RELVERIFY = 0x0001,
        GADGIMMEDIATE = 0x0002,
        ENDGADGET = 0x0004,
        FOLLOWMOUSE = 0x0008,
        RIGHTBORDER = 0x0010,
        LEFTBORDER = 0x0020,
        TOPBORDER = 0x0040,
        BOTTOMBORDER = 0x0080,
        TOGGLESELECT = 0x0100
    }

    [Flags]
    public enum GadgetFlags
    {
        GADGHIGHBITS = 0x0003,
        GADGHCOMP = 0x0000,
        GADGHBOX = 0x0001,
        GADGHIMAGE = 0x0002,
        GADGHNONE = 0x0003,
        GADGIMAGE = 0x0004,
        GRELBOTTOM = 0x0008,
        GRELRIGHT = 0x0010,
        GRELWIDTH = 0x0020,
        GRELHEIGHT = 0x0040,
        SELECTED = 0x0080,
        GADGDISABLED = 0x0100
    }

    [Flags]
    public enum PropFlags
    {
        AUTOKNOB = 0x01,
        FREEHORIZ = 0x02,
        FREEVERT = 0x04,
        PROPBORDERLESS = 0x08,
        KNOBHIT = 0x100
    }

    public enum AutoRequestPositionMode
    {
        TopLeft,
        CenterWindow,
        CenterScreen
    }
}
