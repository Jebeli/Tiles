using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Fonts;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.IGUI
{
    public enum HitTestResult
    {
        None = 0,
        GadgetHit = 4
    }

    [Flags]
    public enum GoActiveResult
    {
        MeActive = 0x0,
        NoReuse = 0x1,
        Reuse = 0x2,
        NextActive = 0x3,
        PrevActive = 0x4,
        Verify = 0x8000,
    }

    [Flags]
    public enum GadgetFlags
    {
        None = 0,
        RelBottom = 0x8,
        RelRight = 0x10,
        RelWidth = 0x20,
        RelHeight = 0x40,
        Selected = 0x80,
        Disabled = 0x100,
        TabCycle = 0x200,
        Hidden = 0x400,
        RelSpecial = 0x4000
    }

    [Flags]
    public enum ActivationFlags
    {
        None = 0,
        RelVerify = 1,
        Immediate = 2,
        EndGadget = 4,
        FollowMouse = 8,
        RightBorder = 0x10,
        LeftBorder = 0x20,
        TopBorder = 0x40,
        BottomBorder = 0x80,
        ToggleSelect = 0x100,
        ActiveGadget = 0x4000
    }

    [Flags]
    public enum GadgetType
    {
        None = 0x0,
        GzzGadget = 0x2000,
        ReqGadget = 0x1000,
        SysGadget = 0x8000,
        Sizing = 0x10,
        WDragging = 0x20,
        WDepth = 0x40,
        WZoom = 0x60,
        Close = 0x80,
        SysTypeMask = 0xF0,
        TypeMask = 0x7,
        BoolGadget = 0x1,
        Gadget02 = 0x2,
        PropGadget = 0x3,
        StrGadget = 0x4,
        CustomGadget = 0x5
    }

    public enum DomainType
    {
        Minimum,
        Nominal,
        Maximum
    }

    public enum TextPlace
    {
        TopCenter,
        TopLeft,
        TopRight,
        InCenter,
        InLeft,
        InRight,
        BottomCenter,
        BottomLeft,
        BottomRight,
        LeftCenter,
        RightCenter

    }
    public class Gadget : Root
    {
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        protected Rect bounds;
        private string text;
        private Icons icon;
        private TextPlace textPlace;
        private Window window;
        private Font font;

        private GadgetFlags flags;
        private ActivationFlags activation;
        private GadgetType type;
        private bool mouseHover;
        private bool togSelect;
        private int id;
        private object userData;
        private int weightedWidth = 100;
        private int weightedHeight = 100;
        private int preferredWidth = 64;
        private int preferredHeight = 24;
        private string hintInfo;

        public Gadget()
            : this(TagItems.Empty)
        {
        }

        public Gadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            flags = GadgetFlags.None;
            activation = ActivationFlags.Immediate | ActivationFlags.RelVerify;
            type = GadgetType.BoolGadget;
            textPlace = TextPlace.InCenter;
            New(tags);
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

        public int WeightedWidth
        {
            get { return weightedWidth; }
            set { weightedWidth = value; }
        }

        public int WeightedHeight
        {
            get { return weightedHeight; }
            set { weightedHeight = value; }
        }

        public int PreferredWidth
        {
            get { return preferredWidth; }
            set { preferredWidth = value; }
        }

        public int PreferredHeight
        {
            get { return preferredHeight; }
            set { preferredHeight = value; }
        }

        public Rect Bounds
        {
            get { return bounds; }
        }

        public string Text
        {
            get { return text; }
            internal set { text = value; }
        }

        public string HintInfo
        {
            get { return hintInfo; }
            internal set { hintInfo = value; }
        }

        public Icons Icon
        {
            get { return icon; }
            set { icon = value; }
        }
        public TextPlace TextPlace
        {
            get { return textPlace; }
            internal set { textPlace = value; }
        }

        public ImageState State
        {
            get
            {
                ImageState state = ImageState.None;
                if (Selected) state |= ImageState.Selected;
                if (togSelect) state |= ImageState.Selected;
                if (Active) state |= ImageState.Active;
                if (Disabled) state |= ImageState.Disabled;
                if (MouseHover) state |= ImageState.Hover;
                return state;
            }
        }

        public Window Window
        {
            get { return window; }
            internal set { window = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public object UserData
        {
            get { return userData; }
            set { userData = value; }
        }

        public ITheme Theme
        {
            get { return window?.Theme; }
        }

        public Font Font
        {
            get
            {
                if (font != null) return font;
                if (window != null) return window.Font;
                return null;
            }
            internal set { font = value; }
        }

        public GadgetFlags Flags
        {
            get { return flags; }
            internal set { flags = value; }
        }

        public ActivationFlags Activation
        {
            get { return activation; }
            internal set { activation = value; }
        }

        public GadgetType Type
        {
            get { return type; }
            internal set { type = value; }
        }

        public bool MouseHover
        {
            get { return mouseHover; }
            internal set { mouseHover = value; }
        }

        public bool Active
        {
            get { return activation.HasFlag(ActivationFlags.ActiveGadget); }
            internal set { SetFlag(ActivationFlags.ActiveGadget, value); }
        }

        public bool Selected
        {
            get { return flags.HasFlag(GadgetFlags.Selected); }
            internal set { SetFlag(GadgetFlags.Selected, value); }
        }

        public bool Disabled
        {
            get { return flags.HasFlag(GadgetFlags.Disabled); }
            internal set { SetFlag(GadgetFlags.Disabled, value); }
        }

        public bool Hidden
        {
            get { return flags.HasFlag(GadgetFlags.Hidden); }
            internal set {
                if (value != Hidden)
                {
                    SetFlag(GadgetFlags.Hidden, value);
                    foreach(var mem in Members)
                    {
                        if (mem is Gadget gad)
                        {
                            gad.Hidden = value;
                        }
                    }
                }
            }
        }

        public bool TogSelect
        {
            get { return togSelect; }
            set { togSelect = value; }
        }

        public virtual HitTestResult HitTest(int x, int y)
        {
            if (!Hidden && x >= 0 && y >= 0 && x < bounds.Width && y < bounds.Height)
            {
                return HitTestResult.GadgetHit;
            }
            return HitTestResult.None;
        }

        public virtual GoActiveResult GoActive(int x, int y, ref int termination)
        {
            if (!Disabled && !Hidden)
            {
                return GoActiveResult.MeActive;
            }
            else return GoActiveResult.NoReuse;
        }

        public virtual void GoInactive(bool abort)
        {

        }

        private GoActiveResult HandleInput(InputEvent ie)
        {
            return HandleInput(ie.Class, ie.X, ie.Y, ie.Button, ie.Code);
        }

        public virtual GoActiveResult HandleInput(IDCMPFlags idcmp, int x, int y, MouseButton button, InputCode code)
        {
            if (!Disabled)
            {
                //Notify(UpdateFlags.Final, (Tags.IDCMP_Update, new InputEvent(idcmp, Window, this, x, y, code, button)));
                switch (idcmp)
                {
                    case IDCMPFlags.GadgetUp:
                        NotifyClick();
                        break;
                }
                return GoActiveResult.MeActive;
            }
            else return GoActiveResult.NoReuse;
        }

        public virtual void Layout(bool initial)
        {
            var rect = type.HasFlag(GadgetType.GzzGadget) ? window.Bounds : window.GZZBounds;
            rect.Offset(-window.LeftEdge, -window.TopEdge);
            bounds.X = flags.HasFlag(GadgetFlags.RelRight) ? rect.Width + leftEdge : rect.Left + leftEdge;
            bounds.Y = flags.HasFlag(GadgetFlags.RelBottom) ? rect.Height + topEdge : rect.Top + topEdge;
            bounds.Width = flags.HasFlag(GadgetFlags.RelWidth) ? rect.Width + width : width;
            bounds.Height = flags.HasFlag(GadgetFlags.RelHeight) ? rect.Height + height : height;
        }

        public virtual void GetPreferredSize(ref int width, ref int height)
        {
            width = preferredWidth;
            height = preferredHeight;
        }

        public virtual void Render(IGraphics gfx)
        {
            if (!Hidden)
            {
                Theme.RenderGadget(gfx, this);
            }
        }

        private void NotifyClick()
        {
            Notify(UpdateFlags.Final, (Tags.GA_ID, id));
        }

        internal GadgetType SysGType
        {
            get { return type & GadgetType.SysTypeMask; }
            set
            {
                type &= ~GadgetType.SysTypeMask;
                type |= value & GadgetType.SysTypeMask;
            }
        }

        internal GadgetType GType
        {
            get { return type & GadgetType.TypeMask; }
            set
            {
                type &= ~GadgetType.TypeMask;
                type |= value & GadgetType.TypeMask;
            }
        }

        private void SetFlag(GadgetFlags flag, object value)
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

        private void SetFlag(ActivationFlags flag, object value)
        {
            if ((bool)value)
            {
                activation |= flag;
            }
            else
            {
                activation &= ~flag;
            }
        }

        private void SetFlag(GadgetType flag, object value)
        {
            if ((bool)value)
            {
                type |= flag;
            }
            else
            {
                type &= ~flag;
            }
        }

        protected virtual void OnNotifyClick(int id)
        {

        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.IDCMP_Update:
                    HandleInput(tag.GetTagData<InputEvent>());
                    return 0;
                case Tags.GA_ID:
                    if (set != SetFlags.Update)
                    {
                        id = tag.GetTagData(0);
                    }
                    else
                    {
                        OnNotifyClick(tag.GetTagData(0));
                    }
                    return 0;
                case Tags.GA_UserData:
                    userData = tag.Item2;
                    return 0;
                case Tags.GA_Left:
                    leftEdge = tag.GetTagData(0);
                    SetFlag(GadgetFlags.RelRight, false);
                    return 1;
                case Tags.GA_RelRight:
                    leftEdge = tag.GetTagData(0);
                    SetFlag(GadgetFlags.RelRight, true);
                    return 1;
                case Tags.GA_Top:
                    topEdge = tag.GetTagData(0);
                    SetFlag(GadgetFlags.RelBottom, false);
                    return 1;
                case Tags.GA_RelBottom:
                    topEdge = tag.GetTagData(0);
                    SetFlag(GadgetFlags.RelBottom, true);
                    return 1;
                case Tags.GA_Width:
                    width = tag.GetTagData(0);
                    SetFlag(GadgetFlags.RelWidth, false);
                    return 1;
                case Tags.GA_RelWidth:
                    width = tag.GetTagData(0);
                    SetFlag(GadgetFlags.RelWidth, true);
                    return 1;
                case Tags.GA_Height:
                    height = tag.GetTagData(0);
                    SetFlag(GadgetFlags.RelHeight, false);
                    return 1;
                case Tags.GA_RelHeight:
                    height = tag.GetTagData(0);
                    SetFlag(GadgetFlags.RelHeight, true);
                    return 1;
                case Tags.GA_WidthWeight:
                    weightedWidth = tag.GetTagData(0);
                    return 1;
                case Tags.GA_HeightWeight:
                    weightedHeight = tag.GetTagData(0);
                    return 1;
                case Tags.GA_Selected:
                    togSelect = tag.GetTagData(false);
                    SetFlag(GadgetFlags.Selected, tag.GetTagData(false));
                    return 1;
                case Tags.GA_Disabled:
                    SetFlag(GadgetFlags.Disabled, tag.GetTagData(false));
                    return 1;
                case Tags.GA_EndGadget:
                    SetFlag(ActivationFlags.EndGadget, tag.GetTagData(false));
                    return 1;
                case Tags.GA_FollowMouse:
                    SetFlag(ActivationFlags.FollowMouse, tag.GetTagData(false));
                    return 1;
                case Tags.GA_BottomBorder:
                    SetFlag(ActivationFlags.BottomBorder, tag.GetTagData(false));
                    return 1;
                case Tags.GA_Immediate:
                    SetFlag(ActivationFlags.Immediate, tag.GetTagData(false));
                    return 1;
                case Tags.GA_RelVerify:
                    SetFlag(ActivationFlags.RelVerify, tag.GetTagData(false));
                    return 1;
                case Tags.GA_LeftBorder:
                    SetFlag(ActivationFlags.LeftBorder, tag.GetTagData(false));
                    return 1;
                case Tags.GA_TopBorder:
                    SetFlag(ActivationFlags.TopBorder, tag.GetTagData(false));
                    return 1;
                case Tags.GA_RightBorder:
                    SetFlag(ActivationFlags.RightBorder, tag.GetTagData(false));
                    return 1;
                case Tags.GA_ToggleSelect:
                    SetFlag(ActivationFlags.ToggleSelect, tag.GetTagData(false));
                    return 1;
                case Tags.GA_SysGadget:
                    SetFlag(GadgetType.SysGadget, tag.GetTagData(false));
                    return 0;
                case Tags.GA_GZZGadget:
                    SetFlag(GadgetType.GzzGadget, tag.GetTagData(false));
                    return 0;
                case Tags.GA_SysGType:
                    SysGType = tag.GetTagData(GadgetType.None);
                    return 0;
                case Tags.GA_Text:
                    text = tag.GetTagData("");
                    return 1;
                case Tags.GA_Icon:
                    icon = tag.GetTagData(Icons.NONE);
                    return 1;
                case Tags.GA_HintInfo:
                    hintInfo = tag.GetTagData("");
                    return 1;
                case Tags.LAYOUT_Parent:
                    //window = ((Gadget)value).window;
                    return 1;
                default:
                    return base.SetTag(set, update, tag);
            }
        }
    }
}
