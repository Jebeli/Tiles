using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Fonts;
using TileEngine.Graphics;

namespace TileEngine.IGUI
{

    [Flags]
    public enum WindowFlags
    {
        None = 0,
        SizeGadget = 0x1,
        DragBar = 0x2,
        DepthGadget = 0x4,
        CloseGadget = 0x8,
        SizeBRight = 0x10,
        SizeBBottom = 0x20,
        SmartRefresh = 0x0,
        SimpleRefresh = 0x40,
        SuperBitmap = 0x80,
        Backdrop = 0x100,
        ReportMouse = 0x200,
        GimmerZeroZero = 0x400,
        Borderless = 0x800,
        Activate = 0x1000,
        WindowActive = 0x00002000,
        InRequest = 0x00004000,
        MenuState = 0x00008000,
        RMBTrap = 0x00010000,
        NoCareRefresh = 0x00020000,
        WindowRefresh = 0x01000000,
        WindowTicked = 0x04000000,
        Zoomed = 0x10000000,
        HasZoom = 0x20000000,
        ToolWindow = 0x40000000
    }

    public class Window : Root
    {
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private int minWidth;
        private int minHeight;
        private int maxWidth;
        private int maxHeight;
        private int borderLeft;
        private int borderTop;
        private int borderRight;
        private int borderBottom;
        private WindowFlags flags;
        private string title;
        private List<Gadget> gadgets;
        private Screen screen;
        private ITheme theme;
        private Font font;
        private IDCMPFlags idcmpFlags;
        private Window parent;
        private bool mouseHover;
        private PopupMenu popup;

        internal Window(Screen screen, params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            this.screen = screen;
            theme = screen.Theme;
            font = screen.Font;
            flags = WindowFlags.GimmerZeroZero;
            idcmpFlags = IDCMPFlags.ActiveWindow | IDCMPFlags.ChangeWindow | IDCMPFlags.CloseWindow | IDCMPFlags.GadgetDown | IDCMPFlags.GadgetUp | IDCMPFlags.InactiveWindow | IDCMPFlags.IntuiTicks;
            leftEdge = 0;
            topEdge = 0;
            width = 200;
            height = 200;
            gadgets = new List<Gadget>();
            New(tags);
            AdjustBorder();
            InitSysGadgets();
            Layout(true);
        }

        public Window Parent
        {
            get { return parent; }
            protected set { parent = value; }
        }

        public PopupMenu OpenPopupMenu(params (Tags, object)[] tags)
        {
            popup = Screen.OpenPopupMenu(this, tags);
            return popup;
        }


        protected void AdjustBorder()
        {
            if (flags.HasFlag(WindowFlags.Borderless))
            {
                borderLeft = 0;
                borderTop = 0;
                borderBottom = 0;
                borderRight = 0;
            }
            else if (flags.HasFlag(WindowFlags.ToolWindow))
            {
                borderLeft = 2;
                borderTop = 2;
                borderBottom = 2;
                borderRight = 2;
            }
            else
            {
                borderLeft = screen.WBorLeft;
                borderRight = screen.WBorRight;
                borderBottom = screen.WBorBottom;
                borderTop = screen.WBorTop;
                if (!string.IsNullOrEmpty(title))
                {
                    if (borderTop < 20) borderTop = 20;
                }
                if (flags.HasFlag(WindowFlags.CloseGadget) || flags.HasFlag(WindowFlags.DepthGadget) || flags.HasFlag(WindowFlags.HasZoom))
                {
                    if (borderTop < 20) borderTop = 20;
                }
                if (flags.HasFlag(WindowFlags.SizeGadget))
                {
                    if (!flags.HasFlag(WindowFlags.SizeBRight) && !flags.HasFlag(WindowFlags.SizeBBottom))
                    {
                        flags |= WindowFlags.SizeBRight;
                    }
                    if (flags.HasFlag(WindowFlags.SizeBRight))
                    {
                        if (borderRight < 20) borderRight = 20;
                    }
                    if (flags.HasFlag(WindowFlags.SizeBBottom))
                    {
                        if (borderBottom < 20) borderBottom = 20;
                    }
                }
                else
                {
                    flags &= ~WindowFlags.SizeBBottom;
                    flags &= ~WindowFlags.SizeBRight;
                }
            }
        }

        public WindowFlags Flags
        {
            get { return flags; }
            internal set { flags = value; }
        }

        public IDCMPFlags IDCMPFlags
        {
            get { return idcmpFlags; }
            internal set { idcmpFlags = value; }
        }

        public int LeftEdge
        {
            get { return leftEdge; }
            internal set { leftEdge = value; }
        }

        public int TopEdge
        {
            get { return topEdge; }
            internal set { topEdge = value; }
        }

        public int Width
        {
            get { return width; }
            internal set { width = value; }
        }

        public int Height
        {
            get { return height; }
            internal set { height = value; }
        }

        public int BorderLeft
        {
            get { return borderLeft; }
        }

        public int BorderTop
        {
            get { return borderTop; }
        }

        public int BorderRight
        {
            get { return borderRight; }
        }

        public int BorderBottom
        {
            get { return borderBottom; }
        }

        public Rect Bounds
        {
            get { return new Rect(leftEdge, topEdge, width, height); }
        }

        public Rect GZZBounds
        {
            get { return new Rect(leftEdge + borderLeft, topEdge + borderTop, width - borderLeft - borderRight, height - borderTop - borderBottom); }
        }

        public string Title
        {
            get { return title; }
            internal set { title = value; }
        }

        public Screen Screen
        {
            get { return screen; }
            internal set { screen = value; }
        }

        public ITheme Theme
        {
            get { return theme; }
            set
            {
                if (theme != value)
                {
                    theme = value;
                }
            }
        }

        public Font Font
        {
            get { return font; }
        }

        public bool MouseHover
        {
            get { return mouseHover; }
            internal set { mouseHover = value; }
        }

        public bool Active
        {
            get { return flags.HasFlag(WindowFlags.WindowActive); }
            internal set { SetFlag(WindowFlags.WindowActive, value); }
        }

        public void Close()
        {
            Screen?.CloseWindow(this);
        }

        public void Layout(bool initial = false)
        {
            foreach (var g in gadgets)
            {
                g.Layout(initial);
            }
        }

        public void Render(IGraphics gfx)
        {
            gfx.SaveState();
            gfx.Translate(leftEdge, topEdge);
            gfx.SetClip(0, 0, width, height);
            Theme.RenderWindow(gfx, this);
            foreach (var g in gadgets.Where(x => x.Type.HasFlag(GadgetType.GzzGadget)))
            {
                g.Render(gfx);
            }
            gfx.SetClip(borderLeft, borderTop, width - 1 - borderLeft - borderRight, height - 1 - borderTop - borderBottom);
            foreach (var g in gadgets.Where(x => !x.Type.HasFlag(GadgetType.GzzGadget)))
            {
                g.Render(gfx);
            }
            gfx.ClearClip();

            gfx.RestoreState();
        }

        public Gadget FindGadget(int x, int y)
        {
            Gadget gad = null;
            int gzzx = x - leftEdge;
            int gzzy = y - topEdge;
            foreach (var g in gadgets)
            {
                if (g.Bounds.Contains(gzzx, gzzy))
                {
                    int gx = gzzx - g.Bounds.Left;
                    int gy = gzzy - g.Bounds.Top;
                    if (g.HitTest(gx, gy) == HitTestResult.GadgetHit)
                    {
                        gad = g;
                    }
                }
            }
            return gad;
        }

        private void InsertGadget(Gadget gadget, int pos)
        {
            gadget.Window = this;
            gadgets.Insert(pos, gadget);
        }

        private void AddGadget(Gadget gadget)
        {
            gadget.Window = this;
            gadgets.Add(gadget);
            foreach (Gadget g in gadget.Members)
            {
                AddGadget(g);
            }
        }

        public void AddGList(IEnumerable<Gadget> glist)
        {
            foreach (var g in glist)
            {
                AddGadget(g);
            }
        }

        private void InitSysGadgets()
        {
            int left = 0;
            int right = 0;
            if (flags.HasFlag(WindowFlags.CloseGadget))
            {
                var cg = new Gadget(
                    (Tags.GA_Left, 0),
                    (Tags.GA_Top, 0),
                    (Tags.GA_Width, 20),
                    (Tags.GA_Height, borderTop),
                    (Tags.GA_SysGadget, true),
                    (Tags.GA_GZZGadget, true),
                    (Tags.GA_TopBorder, true),
                    (Tags.GA_SysGType, GadgetType.Close)
                    );
                InsertGadget(cg, 0);
                left += 20;
            }
            if (flags.HasFlag(WindowFlags.SizeGadget))
            {
                var cg = new Gadget(
                    (Tags.GA_RelRight, -20),
                    (Tags.GA_RelBottom, -20),
                    (Tags.GA_Width, 20),
                    (Tags.GA_Height, 20),
                    (Tags.GA_SysGadget, true),
                    (Tags.GA_GZZGadget, true),
                    (Tags.GA_BottomBorder, true),
                    (Tags.GA_SysGType, GadgetType.Sizing)
                    );
                InsertGadget(cg, 0);
                SetFlag(WindowFlags.HasZoom, true);
            }
            if (flags.HasFlag(WindowFlags.DepthGadget))
            {
                var cg = new Gadget(
                    (Tags.GA_RelRight, -20),
                    (Tags.GA_Top, 0),
                    (Tags.GA_Width, 20),
                    (Tags.GA_Height, borderTop),
                    (Tags.GA_SysGadget, true),
                    (Tags.GA_GZZGadget, true),
                    (Tags.GA_TopBorder, true),
                    (Tags.GA_SysGType, GadgetType.WDepth)
                    );
                InsertGadget(cg, 0);
                right += 20;
            }
            if (flags.HasFlag(WindowFlags.HasZoom))
            {
                var cg = new Gadget(
                    (Tags.GA_RelRight, -(20 + right)),
                    (Tags.GA_Top, 0),
                    (Tags.GA_Width, 20),
                    (Tags.GA_Height, borderTop),
                    (Tags.GA_SysGadget, true),
                    (Tags.GA_GZZGadget, true),
                    (Tags.GA_TopBorder, true),
                    (Tags.GA_SysGType, GadgetType.WZoom)
                    );
                InsertGadget(cg, 0);
                right += 20;
            }
            if (flags.HasFlag(WindowFlags.DragBar))
            {
                var cg = new Gadget(
                    (Tags.GA_Left, left),
                    (Tags.GA_Top, 0),
                    (Tags.GA_RelWidth, -(right + left)),
                    (Tags.GA_Height, borderTop),
                    (Tags.GA_SysGadget, true),
                    (Tags.GA_GZZGadget, true),
                    (Tags.GA_TopBorder, true),
                    (Tags.GA_SysGType, GadgetType.WDragging)
                    );
                InsertGadget(cg, 0);
            }
        }

        private void SetFlag(WindowFlags flag, object value)
        {
            if ((bool)value)
            {
                flags |= flag;
            }
            else
            {
                flags &= ~flag;
            }
        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.WA_Left:
                    leftEdge = tag.GetTagData(0);
                    return 1;
                case Tags.WA_Top:
                    topEdge = tag.GetTagData(0);
                    return 1;
                case Tags.WA_Width:
                    width = tag.GetTagData(0);
                    return 1;
                case Tags.WA_Height:
                    height = tag.GetTagData(0);
                    return 1;
                case Tags.WA_Flags:
                    flags = tag.GetTagData(WindowFlags.None);
                    return 1;
                case Tags.WA_Gadget:
                    AddGadget(tag.GetTagData<Gadget>());
                    if (set != SetFlags.New) Layout(false);
                    return 1;
                case Tags.WA_Gadgets:
                    foreach (var g in tag.GetTagData<IEnumerable<Gadget>>())
                    {
                        AddGadget(g);
                    }
                    if (set != SetFlags.New) Layout(false);
                    return 1;
                case Tags.WA_Title:
                    title = tag.GetTagData("");
                    return 1;
                case Tags.WA_MinWidth:
                    minWidth = tag.GetTagData(0);
                    return 0;
                case Tags.WA_MinHeight:
                    minHeight = tag.GetTagData(0);
                    return 0;
                case Tags.WA_MaxWidth:
                    maxWidth = tag.GetTagData(0);
                    return 0;
                case Tags.WA_MaxHeight:
                    maxHeight = tag.GetTagData(0);
                    return 0;
                case Tags.WA_InnerWidth:
                    width = tag.GetTagData(0)+ borderLeft + borderRight;
                    return 1;
                case Tags.WA_InnerHeight:
                    height = tag.GetTagData(0) + borderTop + borderBottom;
                    return 1;
                case Tags.WA_Zoom:
                    return 0;
                case Tags.WA_SizeGadget:
                    SetFlag(WindowFlags.SizeGadget, tag.GetTagData(false));
                    return 1;
                case Tags.WA_DragBar:
                    SetFlag(WindowFlags.DragBar, tag.GetTagData(false));
                    return 1;
                case Tags.WA_DepthGadget:
                    SetFlag(WindowFlags.DepthGadget, tag.GetTagData(false));
                    return 1;
                case Tags.WA_CloseGadget:
                    SetFlag(WindowFlags.CloseGadget, tag.GetTagData(false));
                    return 1;
                case Tags.WA_Backdrop:
                    SetFlag(WindowFlags.Backdrop, tag.GetTagData(false));
                    return 1;
                case Tags.WA_Activate:
                    SetFlag(WindowFlags.Activate, tag.GetTagData(false));
                    return 1;
                case Tags.WA_RMBTrap:
                    SetFlag(WindowFlags.RMBTrap, tag.GetTagData(false));
                    return 1;
                case Tags.WA_SimpleRefresh:
                    SetFlag(WindowFlags.SimpleRefresh, tag.GetTagData(false));
                    return 1;
                case Tags.WA_SmartRefresh:
                    SetFlag(WindowFlags.SmartRefresh, tag.GetTagData(false));
                    return 1;
                case Tags.WA_SizeBRight:
                    SetFlag(WindowFlags.SizeBRight, tag.GetTagData(false));
                    return 1;
                case Tags.WA_SizeBBottom:
                    SetFlag(WindowFlags.SizeBBottom, tag.GetTagData(false));
                    return 1;
                case Tags.WA_AutoAdjust:

                    return 1;
                case Tags.WA_GimmeZeroZero:
                    SetFlag(WindowFlags.GimmerZeroZero, tag.GetTagData(false));
                    return 1;
                default:
                    return base.SetTag(set, update, tag);
            }

        }
    }
}
