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
    public enum Tags
    {
        TAG_None,
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
        WA_HoverOpacity,
        WA_BackgroundColor,
        WA_ForegroundColor,
        ICA_MAP,
        ICA_TARGET,
        GA_List,
        GA_Group,
        GA_Left,
        GA_RelRight,
        GA_Top,
        GA_RelBottom,
        GA_Width,
        GA_RelWidth,
        GA_Height,
        GA_RelHeight,
        GA_Text,
        GA_Image,
        GA_Border,
        GA_SelectRender,
        GA_Highlight,
        GA_Disabled,
        GA_GZZGadget,
        GA_ID,
        GA_UserData,
        GA_Selected,
        GA_EndGadget,
        GA_Immediate,
        GA_RelVerify,
        GA_FollowMouse,
        GA_RightBorder,
        GA_LeftBorder,
        GA_TopBorder,
        GA_BottomBorder,
        GA_ToggleSelect,
        GA_SysGadget,
        GA_SysGType,
        GA_DrawInfo,
        GA_IntuiText,
        GA_LabelImage,
        GA_TabCycle,
        GA_GadgetHelp,
        GA_Bounds,
        GA_RelSpecial,
        GA_TextAttr,
        GA_ReadOnly,
        GA_Underscore,
        GA_ActivateKey,
        GA_BackFill,
        GA_GadgetHelpText,
        GA_UserInput,
        GA_LabelPlace,
        IA_Left,
        IA_Top,
        IA_Width,
        IA_Height,
        IA_FGPen,
        IA_BGPen,
        IA_Data,
        IA_LineWidth,
        IA_Pens,
        IA_Resolution,
        IA_Recessed,
        IA_EdgesOnly,
        IA_FrameType,
        IA_ReadOnly,
        SYSIA_DrawInfo,
        SYSIA_Which,
        SYSIA_Size,
        SYSIA_Flags,
        SYSIA_WithBorder,
        GT_VisualInfo,
        GT_Flags,
        GT_Underscore,
        GTA_GadgetKind,
        GTA_ArrowScroller,
        GTA_ScrollerArrow1,
        GTA_ScrollerArrow2,
        GTA_Arrow_Type,
        GTA_ScrollerDec,
        GTA_ScrollerInc,
        GTCB_Checked,
        GTCY_Labels,
        GTCY_Active,
        GTTX_Text,
        GTTX_CopyText,
        GTTX_Border,
        GTNM_Number,
        GTNM_Border = GTTX_Border,
        GTMX_Labels,
        GTMX_Active,
        GTMX_Spacing,
        GTST_String,
        GTST_MaxChars,
        GTIN_Number,
        GTIN_MaxChars = GTST_MaxChars,
        GTSC_Top,
        GTSC_Total,
        GTSC_Visible,
        GTSC_Arrows,
        PGA_Top,
        PGA_Visible,
        PGA_Total,
        PGA_Freedom,
        PGA_HorizPot,
        PGA_HorizBody,
        PGA_VertPot,
        PGA_VertBody,
        PGA_Boderless,
        STRINGA_LongVal,
        STRINGA_TextVal,
        STRINGA_MaxChars,
        GTSL_Min,
        GTSL_Max,
        GTSL_Level,
        GTSL_MaxLevelLen,
        GTSL_LevelFormat,
        GTSL_LevelPlace,
        GTLV_Top,
        GTLV_Labels,
        GTLV_ReadOnly,
        GTLV_ScrollWidth,
        GTLV_ShowSelected,
        GTLV_Selected

    }

    [Flags]
    public enum ClassFlags
    {
        None = 0x00,
        InList = 0x01
    }

    [Flags]
    public enum SysImageFlags
    {
        None = 0,
        GadTools = 1,
        NoBorder = 2
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
    public enum MoreWindowFlags
    {
        WFLG_NONE = 0,
        WFLG_HOVER = 1
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
        NONE = 0x0000,
        BOOLGADGET = 0x0001,
        GADGET0002 = 0x0002,
        PROPGADGET = 0x0003,
        STRGADGET = 0x0004,
        CUSTOMGADGET = 0x0005,
        GTYPEMASK = 0x0007,
        SIZING = 0x0010,
        WDRAGGING = 0x0020,
        SDRAGGING = 0x0030,
        WDEPTH = 0x0040,
        SDEPTH = 0x0050,
        WZOOM = 0x0060,
        SUNUSED = 0x070,
        CLOSE = 0x080,
        GADTOOLS = 0x100,
        REQGADGET = 0x1000,
        GZZGADGET = 0x2000,
        SCRGADGET = 0x4000,
        SYSGADGET = 0x8000,
        GADGETTYPE = 0xFF00,
        SYSTYPEMASK = 0x00F0
    }

    [Flags]
    public enum GadgetActivation
    {
        NONE = 0x0000,
        RELVERIFY = 0x0001,
        GADGIMMEDIATE = 0x0002,
        ENDGADGET = 0x0004,
        FOLLOWMOUSE = 0x0008,
        RIGHTBORDER = 0x0010,
        LEFTBORDER = 0x0020,
        TOPBORDER = 0x0040,
        BOTTOMBORDER = 0x0080,
        TOGGLESELECT = 0x0100,
        STRINGCENTER = 0x0200,
        STRINGRIGHT = 0x0400,
        STRINGLEFT = 0x0,
        LONGINT = 0x0800,
        ALTKEYMAP = 0x1000,
        STRINGEXTEND = 0x2000,
        BOOLEXTEND = 0x4000,
        ACTIVEGADGET = 0x8000,
        BORDERSNIFF = 0x10000
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
        GADGDISABLED = 0x0100,
        TABCYCLE = 0x0200,
        STRINGEXTEND = 0x0400,
        IMAGEDISABLE = 0x0800,
        LABELITEXT = 0,
        LABELSTRING = 0x1000,
        LABELIMAGE = 0x2000,
        LABELMASK = 0x3000,
        RELSPECIAL = 0x4000,
        CHECKED = 0x8000,
        BOUNDS = 0x10000,
        GADGETHELP = 0x20000,
        SCROLLRASTER = 0x40000,
        BOOPSIGADGET = 0x80000,
        HOVER = 0x100000
    }

    public enum LabelPlace
    {
        PlaceTextIn = 0x0,
        PlaceTextLeft = 0x01,
        PlaceTextRight = 0x02,
        PlaceTextAbove = 0x04,
        PlaceTextBelow = 0x08,
    }

    [Flags]
    public enum NewGadgetFlags
    {
        None,
        PlaceTextLeft = 0x01,
        PlaceTextRight = 0x02,
        PlaceTextAbove = 0x04,
        PlaceTextBelow = 0x08,
        PlaceTextIn = 0x10,
        PlaceText = PlaceTextLeft | PlaceTextRight | PlaceTextAbove | PlaceTextBelow | PlaceTextIn,
        HighLabel = 0x20
    }

    [Flags]
    public enum UpdateFlags
    {
        Final = 0x00,
        Interim = 0x01
    }

    public enum SetFlags
    {
        New = 0x00,
        Set = 0x01,
        Update = 0x02
    }

    [Flags]
    public enum HitTestResult
    {
        NoHit = 0x00,
        ImageHit = 0x01,
        GadgetHit = 0x04
    }

    public enum GadgetRedraw
    {
        Toggle = 0,
        Redraw = 1,
        Update = 2
    }

    [Flags]
    public enum GadgetActive
    {
        MeActive = 0,
        NoReuse = 1,
        Reuse = 2,
        Verify = 4,
        NextActive = 8,
        PrevActive = 16
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

    public enum GadKind
    {
        None,
        Generic,
        Button,
        Checkbox,
        Integer,
        ListView,
        Mx,
        Number,
        Cycle,
        Palette,
        Scroller,
        Slider,
        String,
        Text
    }

    internal enum GadToolsKind
    {
        None,
        ArrowUp,
        ArrowDown,
        ArrowLeft,
        ArrowRight
    }

    public enum InputClass
    {
        NULL = 0,
        RAWKEY = 1,
        RAWMOUSE = 2,
        MOUSEDOWN = 3,
        MOUSEUP = 4,
        MOUSEMOVE = 5,
        KEYDOWN = 6,
        KEYUP = 7,
        GADGETDOWN = 8,
        GADGETUP = 9,
        TIMER
    }

    [Flags]
    public enum ImageState
    {
        None = 0x0,
        Normal = 0x0,
        Selected = 0x1,
        Disabled = 0x2,
        Hover = 0x4,
        Inactive = 0x8,
        Busy = 0x10,
        Indeterminate = 0x20,
        Checked = 0x40
    }

    [Flags]
    public enum FrameFlags
    {
        None = 0,
        Specify = 1
    }

    public enum FrameType
    {
        Default,
        Button,
        Ridge,
        IconDropBox
    }

    public enum SysImageType
    {
        Depth,
        Zoom,
        Size,
        Close,
        SDepth,
        Left,
        Up,
        Right,
        Down,
        Check,
        Mx,
        Drag
    }
}
