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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Fonts;
    using TileEngine.Graphics;
    using TileEngine.Screens;

    public class Gadget : Root, IBox
    {
        private Window window;
        private Requester requester;
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private GadgetFlags flags;
        private GadgetActivation activation;
        private GadgetType gadgetType;
        private IntuiText gadgetText;
        private LabelPlace labelPlace;
        private Border gadgetRender;
        private Border selectRender;
        private Image gadgetImage;
        private Image selectImage;
        private PropInfo propInfo;
        private StringInfo stringInfo;
        private int gadgetId;
        private Font font;

        private IList<Gadget> glist = null;
        private GroupGadget group = null;

        public Gadget()
        {
            Activation = GadgetActivation.GADGIMMEDIATE | GadgetActivation.RELVERIFY;
        }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.GA_Left:
                    LeftEdge = tag.GetTagData(0);
                    return 1;
                case Tags.GA_Top:
                    TopEdge = tag.GetTagData(0);
                    return 1;
                case Tags.GA_Width:
                    Width = tag.GetTagData(0);
                    return 1;
                case Tags.GA_Height:
                    Height = tag.GetTagData(0);
                    return 1;
                case Tags.GA_RelRight:
                    LeftEdge = tag.GetTagData(0);
                    Flags |= GadgetFlags.GRELRIGHT;
                    return 1;
                case Tags.GA_RelBottom:
                    TopEdge = tag.GetTagData(0);
                    Flags |= GadgetFlags.GRELBOTTOM;
                    return 1;
                case Tags.GA_RelWidth:
                    Width = tag.GetTagData(0);
                    Flags |= GadgetFlags.GRELWIDTH;
                    return 1;
                case Tags.GA_RelHeight:
                    Height = tag.GetTagData(0);
                    Flags |= GadgetFlags.GRELHEIGHT;
                    return 1;
                case Tags.GA_RelSpecial:
                    if (tag.GetTagData(false)) { Flags |= GadgetFlags.RELSPECIAL; }
                    else { Flags &= ~GadgetFlags.RELSPECIAL; }
                    return 1;
                case Tags.GA_Bounds:
                    // TODO: bounds
                    return 1;
                case Tags.GA_GadgetHelp:
                    if (tag.GetTagData(false)) { Flags |= GadgetFlags.GADGETHELP; }
                    else { Flags &= ~GadgetFlags.GADGETHELP; }
                    return 0;
                case Tags.GA_List:
                    glist = tag.GetTagData<IList<Gadget>>();
                    return 0;
                case Tags.GA_Group:
                    group = tag.GetTagData<GroupGadget>();
                    return 0;
                case Tags.GA_LabelPlace:
                    LabelPlace = tag.GetTagData(LabelPlace.PlaceTextIn);
                    return 1;
                case Tags.GA_Text:
                    GadgetText = new IntuiText(tag.GetTagData(""));
                    if (GadgetText != null)
                    {
                        Flags &= ~GadgetFlags.LABELMASK;
                        Flags |= GadgetFlags.LABELSTRING;
                    }
                    return 1;
                case Tags.GA_IntuiText:
                    GadgetText = tag.GetTagData<IntuiText>();
                    if (GadgetText != null)
                    {
                        Flags &= ~GadgetFlags.LABELMASK;
                        Flags |= GadgetFlags.LABELITEXT;
                    }
                    return 1;
                case Tags.GA_LabelImage:
                    GadgetText = tag.GetTagData<IntuiText>();
                    if (GadgetText != null)
                    {
                        Flags &= ~GadgetFlags.LABELMASK;
                        Flags |= GadgetFlags.LABELIMAGE;
                    }
                    return 1;
                case Tags.GA_Image:
                    GadgetImage = tag.GetTagData<Image>();
                    if (GadgetImage != null)
                    {
                        Flags |= GadgetFlags.GADGIMAGE;
                    }
                    return 1;
                case Tags.GA_Border:
                    GadgetRender = tag.GetTagData<Border>();
                    if (GadgetRender != null)
                    {
                        Flags &= ~GadgetFlags.GADGIMAGE;
                    }
                    return 1;
                case Tags.GA_SelectRender:
                    SelectRender = tag.GetTagData<Border>();
                    if (SelectRender != null)
                    {
                        Flags &= ~GadgetFlags.GADGIMAGE;
                        Flags &= ~GadgetFlags.GADGHIGHBITS;
                        Flags |= GadgetFlags.GADGHIMAGE;
                    }
                    return 1;
                case Tags.GA_GZZGadget:
                    if (tag.GetTagData(false)) { GadgetType |= GadgetType.GZZGADGET; }
                    else { GadgetType &= ~GadgetType.GZZGADGET; }
                    return 0;
                case Tags.GA_SysGadget:
                    if (tag.GetTagData(false)) { GadgetType |= GadgetType.SYSGADGET; }
                    else { GadgetType &= ~GadgetType.SYSGADGET; }
                    return 0;
                case Tags.GA_Selected:
                    if (tag.GetTagData(false)) { Flags |= GadgetFlags.SELECTED; }
                    else { Flags &= ~GadgetFlags.SELECTED; }
                    return 1;
                case Tags.GA_Disabled:
                    if (tag.GetTagData(false)) { Flags |= GadgetFlags.GADGDISABLED; }
                    else { Flags &= ~GadgetFlags.GADGDISABLED; }
                    return 1;
                case Tags.GA_EndGadget:
                    if (tag.GetTagData(false)) { Activation |= GadgetActivation.ENDGADGET; }
                    else { Activation &= ~GadgetActivation.ENDGADGET; }
                    return 0;
                case Tags.GA_Immediate:
                    if (tag.GetTagData(false)) { Activation |= GadgetActivation.GADGIMMEDIATE; }
                    else { Activation &= ~GadgetActivation.GADGIMMEDIATE; }
                    return 0;
                case Tags.GA_RelVerify:
                    if (tag.GetTagData(false)) { Activation |= GadgetActivation.RELVERIFY; }
                    else { Activation &= ~GadgetActivation.RELVERIFY; }
                    return 0;
                case Tags.GA_FollowMouse:
                    if (tag.GetTagData(false)) { Activation |= GadgetActivation.FOLLOWMOUSE; }
                    else { Activation &= ~GadgetActivation.FOLLOWMOUSE; }
                    return 0;
                case Tags.GA_RightBorder:
                    if (tag.GetTagData(false)) { Activation |= GadgetActivation.RIGHTBORDER; }
                    else { Activation &= ~GadgetActivation.RIGHTBORDER; }
                    return 0;
                case Tags.GA_LeftBorder:
                    if (tag.GetTagData(false)) { Activation |= GadgetActivation.LEFTBORDER; }
                    else { Activation &= ~GadgetActivation.LEFTBORDER; }
                    return 0;
                case Tags.GA_TopBorder:
                    if (tag.GetTagData(false)) { Activation |= GadgetActivation.TOPBORDER; }
                    else { Activation &= ~GadgetActivation.TOPBORDER; }
                    return 0;
                case Tags.GA_BottomBorder:
                    if (tag.GetTagData(false)) { Activation |= GadgetActivation.BOTTOMBORDER; }
                    else { Activation &= ~GadgetActivation.BOTTOMBORDER; }
                    return 0;
                case Tags.GA_ToggleSelect:
                    if (tag.GetTagData(false)) { Activation |= GadgetActivation.TOGGLESELECT; }
                    else { Activation &= ~GadgetActivation.TOGGLESELECT; }
                    return 0;
                case Tags.GA_TabCycle:
                    if (tag.GetTagData(false)) { Flags |= GadgetFlags.TABCYCLE; }
                    else { Flags &= ~GadgetFlags.TABCYCLE; }
                    return 0;
                case Tags.GA_SysGType:
                    GadgetType &= ~GadgetType.SYSTYPEMASK;
                    GadgetType |= (tag.GetTagData(GadgetType.NONE) & GadgetType.SYSTYPEMASK);
                    return 0;
                case Tags.GA_ID:
                    if (set != SetFlags.Update)
                    {
                        GadgetId = tag.GetTagData(0);
                    }
                    return 0;
                case Tags.GA_UserData:
                    UserData = tag.Item2;
                    return 0;
                default:
                    return 0;
            }
        }

        protected override int BeforeSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update)
        {
            glist = null;
            group = null;
            return 0;
        }

        protected override int AfterSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, int returnValue)
        {
            if (set == SetFlags.New)
            {
                if (glist != null) glist.Add(this);
            }
            if (group != null) group.AddMember(this);
            return returnValue;
        }

        public override void Get((Tags, object) attr)
        {
            switch (attr.Item1)
            {
                case Tags.GA_Left:
                case Tags.GA_RelRight:
                    attr.Item2 = LeftEdge;
                    break;
                case Tags.GA_Top:
                case Tags.GA_RelBottom:
                    attr.Item2 = TopEdge;
                    break;
                case Tags.GA_Width:
                case Tags.GA_RelWidth:
                    attr.Item2 = Width;
                    break;
                case Tags.GA_Height:
                case Tags.GA_RelHeight:
                    attr.Item2 = Height;
                    break;
                case Tags.GA_Selected:
                    attr.Item2 = (Flags & GadgetFlags.SELECTED) == GadgetFlags.SELECTED;
                    break;
                case Tags.GA_Disabled:
                    attr.Item2 = (Flags & GadgetFlags.GADGDISABLED) == GadgetFlags.GADGDISABLED;
                    break;
                case Tags.GA_ID:
                    attr.Item2 = GadgetId;
                    break;
                case Tags.GA_UserData:
                    attr.Item2 = UserData;
                    break;
                case Tags.GA_RelSpecial:
                    attr.Item2 = (Flags & GadgetFlags.RELSPECIAL) == GadgetFlags.RELSPECIAL;
                    break;
                case Tags.GA_GadgetHelp:
                    attr.Item2 = (Flags & GadgetFlags.GADGETHELP) == GadgetFlags.GADGETHELP;
                    break;
                case Tags.GA_Text:
                    attr.Item2 = GadgetText?.IText;
                    break;
                case Tags.GA_IntuiText:
                    attr.Item2 = GadgetText;
                    break;
                case Tags.GA_GZZGadget:
                    attr.Item2 = (GadgetType & GadgetType.GZZGADGET) == GadgetType.GZZGADGET;
                    break;
                case Tags.GA_SysGadget:
                    attr.Item2 = (GadgetType & GadgetType.SYSGADGET) == GadgetType.SYSGADGET;
                    break;
                case Tags.GA_EndGadget:
                    attr.Item2 = (Activation & GadgetActivation.ENDGADGET) == GadgetActivation.ENDGADGET;
                    break;
                case Tags.GA_Immediate:
                    attr.Item2 = (Activation & GadgetActivation.GADGIMMEDIATE) == GadgetActivation.GADGIMMEDIATE;
                    break;
                case Tags.GA_RelVerify:
                    attr.Item2 = (Activation & GadgetActivation.RELVERIFY) == GadgetActivation.RELVERIFY;
                    break;
                case Tags.GA_FollowMouse:
                    attr.Item2 = (Activation & GadgetActivation.FOLLOWMOUSE) == GadgetActivation.FOLLOWMOUSE;
                    break;
                case Tags.GA_RightBorder:
                    attr.Item2 = (Activation & GadgetActivation.RIGHTBORDER) == GadgetActivation.RIGHTBORDER;
                    break;
                case Tags.GA_LeftBorder:
                    attr.Item2 = (Activation & GadgetActivation.LEFTBORDER) == GadgetActivation.LEFTBORDER;
                    break;
                case Tags.GA_TopBorder:
                    attr.Item2 = (Activation & GadgetActivation.TOPBORDER) == GadgetActivation.TOPBORDER;
                    break;
                case Tags.GA_BottomBorder:
                    attr.Item2 = (Activation & GadgetActivation.BOTTOMBORDER) == GadgetActivation.BOTTOMBORDER;
                    break;
                case Tags.GA_ToggleSelect:
                    attr.Item2 = (Activation & GadgetActivation.TOGGLESELECT) == GadgetActivation.TOGGLESELECT;
                    break;
                case Tags.GA_TabCycle:
                    attr.Item2 = (Flags & GadgetFlags.TABCYCLE) == GadgetFlags.TABCYCLE;
                    break;
                case Tags.GA_SysGType:
                    attr.Item2 = GadgetType & GadgetType.SYSTYPEMASK;
                    break;
                case Tags.GA_Bounds:
                    // TODO: Bounds
                    break;
                default:
                    base.Get(attr);
                    break;
            }
        }

        public ImageState ImageState
        {
            get
            {
                ImageState state = ImageState.None;
                bool selected = ((Flags & GadgetFlags.SELECTED) == GadgetFlags.SELECTED);
                bool disabled = ((Flags & GadgetFlags.GADGDISABLED) == GadgetFlags.GADGDISABLED);
                bool hover = ((Flags & GadgetFlags.HOVER) == GadgetFlags.HOVER);
                bool @checked = ((Flags & GadgetFlags.CHECKED) == GadgetFlags.CHECKED);
                bool active = (window.Flags & WindowFlags.WFLG_WINDOWACTIVE) == WindowFlags.WFLG_WINDOWACTIVE;
                bool border = (Activation & (GadgetActivation.BOTTOMBORDER | GadgetActivation.TOPBORDER | GadgetActivation.LEFTBORDER | GadgetActivation.RIGHTBORDER)) != GadgetActivation.NONE;
                if (!border) active = false;
                if (selected) state |= ImageState.Selected;
                if (disabled) state |= ImageState.Disabled;
                if (hover) state |= ImageState.Hover;
                if (!active) state |= ImageState.Inactive;
                if (@checked) state |= ImageState.Checked;
                return state;
            }
        }

        public virtual HitTestResult HitTest(GadgetInfo gadgetInfo, int mouseX, int mouseY)
        {
            return HitTestResult.GadgetHit;
        }

        public virtual void Render(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw)
        {

        }

        internal protected void RenderBaseFrame(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw, IBox container)
        {
            if (GadgetImage != null)
            {
                if ((GadgetImage.Width < container.Width) || ((Flags & GadgetFlags.GRELWIDTH) == GadgetFlags.GRELWIDTH))
                {
                    GadgetImage.Width = container.Width;
                }
                if ((GadgetImage.Height < container.Height) || ((Flags & GadgetFlags.GRELWIDTH) == GadgetFlags.GRELWIDTH))
                {
                    GadgetImage.Height = container.Height;
                }
                int x = container.LeftEdge + (container.Width / 2) - (GadgetImage.Width / 2);
                int y = container.TopEdge + (container.Height / 2) - (GadgetImage.Height / 2);
                Intuition.DrawImageState(graphics, GadgetImage, x, y, ImageState, gadgetInfo.DrawInfo);
            }
        }

        public virtual GadgetActive GoActive(GadgetInfo gadgetInfo, InputEvent inputEvent, ref int termination, int mouseX, int mouseY)
        {
            return GadgetActive.MeActive;
        }

        public virtual void GoInactive(GadgetInfo gadgetInfo, int abort = 1)
        {

        }

        public virtual GadgetActive HandleInput(GadgetInfo gadgetInfo, InputEvent inputEvent, ref int termination, int mouseX, int mouseY)
        {
            return GadgetActive.MeActive;
        }

        public IBox GetGadgetIBox()
        {
            IBox box = new Box()
            {
                LeftEdge = LeftEdge,
                TopEdge = TopEdge,
                Width = Width,
                Height = Height
            };
            return box;
        }

        public IBox GetWinGadgetIBox()
        {
            IBox box = null;
            GetWinGadgetIBox(window?.Screen, window, requester, ref box);
            return box;
        }

        public void GetWinGadgetIBox(IScreen screen, Window win, Requester req, ref IBox box)
        {
            GetWinGadgetIBox(this, screen, win, req, ref box);
        }

        public static void GetWinGadgetIBox(Gadget gad, IScreen screen, Window win, Requester req, ref IBox box)
        {
            if (box == null) box = new Box();
            if (gad != null)
            {
                IBox domain = new Box();
                Intuition.GetGadgetDomain(gad, screen, win, req, ref domain);
                box.LeftEdge = gad.LeftEdge;
                box.TopEdge = gad.TopEdge;
                box.Width = gad.Width;
                box.Height = gad.Height;
                if ((gad.Flags & GadgetFlags.GRELRIGHT) == GadgetFlags.GRELRIGHT)
                    box.LeftEdge += domain.Width - 1;

                if ((gad.Flags & GadgetFlags.GRELBOTTOM) == GadgetFlags.GRELBOTTOM)
                    box.TopEdge += domain.Height - 1;

                if ((gad.Flags & GadgetFlags.GRELWIDTH) == GadgetFlags.GRELWIDTH)
                    box.Width += domain.Width;

                if ((gad.Flags & GadgetFlags.GRELHEIGHT) == GadgetFlags.GRELHEIGHT)
                    box.Height += domain.Height;

                box.LeftEdge += domain.LeftEdge;
                box.TopEdge += domain.TopEdge;
            }
        }

        public IBox GetScrGadgetIBox()
        {
            IBox box = null;
            GetScrGadgetIBox(window?.Screen, window, requester, ref box);
            return box;
        }

        public void GetScrGadgetIBox(IScreen screen, Window win, Requester req, ref IBox box)
        {
            GetScrGadgetIBox(this, screen, win, req, ref box);
        }

        public static void GetScrGadgetIBox(Gadget gad, IScreen screen, Window win, Requester req, ref IBox box)
        {
            GetWinGadgetIBox(gad, screen, win, req, ref box);
            if (win != null)
            {
                box.LeftEdge += win.LeftEdge;
                box.TopEdge += win.TopEdge;
            }
        }

        public void GetDomGadgetIBox(IScreen screen, Window win, Requester req, ref IBox box)
        {
            GetDomGadgetIBox(this, screen, win, req, ref box);
        }
        public static void GetDomGadgetIBox(Gadget gad, IScreen screen, Window win, Requester req, ref IBox box)
        {
            IBox domain = new Box();
            GetWinGadgetIBox(gad, screen, win, req, ref box);
            Intuition.GetGadgetDomain(gad, screen, win, req, ref domain);
            box.LeftEdge -= domain.LeftEdge;
            box.TopEdge -= domain.TopEdge;
        }

        public void GetGadgetIBox(GadgetInfo gadgetInfo, ref IBox ibox)
        {
            GetGadgetIBox(this, gadgetInfo, ref ibox);
        }
        public static void GetGadgetIBox(Gadget gadget, GadgetInfo gadgetInfo, ref IBox ibox)
        {
            if (ibox == null) ibox = new Box();
            if (gadget != null)
            {
                gadget.GetWinGadgetIBox(gadgetInfo.Screen, gadgetInfo.Window, gadgetInfo.Requester, ref ibox);
            }
        }

        protected virtual void PrintGadgetLabel(GadgetInfo gadgetInfo, ImageState state, IGraphics rPort)
        {
            if (GadgetText == null)
                return;
            IBox container = null;
            GetGadgetIBox(gadgetInfo, ref container);
            if ((container.Width <= 1) || (container.Height <= 1))
                return;
            bool disabled = (state & ImageState.Disabled) == ImageState.Disabled;
            if (disabled)
            {
                GadgetText.FrontPen = gadgetInfo.DrawInfo.DisabledTextPen;
            }
            else
            {
                GadgetText.FrontPen = gadgetInfo.DrawInfo.TextPen;
            }
            switch (Flags & GadgetFlags.LABELMASK)
            {
                case GadgetFlags.LABELITEXT:
                    Intuition.DrawIntuiText(rPort, GadgetText, container.LeftEdge, container.TopEdge);
                    break;
                case GadgetFlags.LABELSTRING:
                    Intuition.DrawIntuiText(rPort, GadgetText, container.LeftEdge, container.TopEdge);
                    break;
                case GadgetFlags.LABELIMAGE:
                    Intuition.DrawIntuiText(rPort, GadgetText, container.LeftEdge, container.TopEdge);
                    break;
            }
        }

        public static Gadget MakeBoolGadget(string text, int width, int height, GadgetFlags flags = 0)
        {
            Gadget gadget = Intuition.NewObject(Intuition.FRBUTTONCLASS,
                width <= 0 ? (Tags.GA_RelWidth, width) : (Tags.GA_Width, width),
                height <= 0 ? (Tags.GA_RelHeight, height) : (Tags.GA_Height, height),
                (Tags.GA_Text, text)) as Gadget;
            gadget.Flags |= flags;
            return gadget;
        }

        public static Gadget MakeVertPropGadget(int total, int visible, int top, int width, int height)
        {
            Gadget gadget = Intuition.NewObject(Intuition.PROPGCLASS,
                width <= 0 ? (Tags.GA_RelWidth, width) : (Tags.GA_Width, width),
                height <= 0 ? (Tags.GA_RelHeight, height) : (Tags.GA_Height, height),
                (Tags.PGA_Freedom, PropFlags.FREEVERT),
                (Tags.PGA_Total, total),
                (Tags.PGA_Visible, visible),
                (Tags.PGA_Top, top)
                ) as Gadget;
            return gadget;
        }

        public static Gadget MakeHorizPropGadget(int total, int visible, int top, int width, int height)
        {
            Gadget gadget = Intuition.NewObject(Intuition.PROPGCLASS,
                width <= 0 ? (Tags.GA_RelWidth, width) : (Tags.GA_Width, width),
                height <= 0 ? (Tags.GA_RelHeight, height) : (Tags.GA_Height, height),
                (Tags.PGA_Freedom, PropFlags.FREEHORIZ),
                (Tags.PGA_Total, total),
                (Tags.PGA_Visible, visible),
                (Tags.PGA_Top, top)
                ) as Gadget;
            return gadget;
        }

        public static Gadget MakeStringGadget(string text, int width, int height)
        {
            Gadget gadget = Intuition.NewObject(Intuition.STRGCLASS,
                width <= 0 ? (Tags.GA_RelWidth, width) : (Tags.GA_Width, width),
                height <= 0 ? (Tags.GA_RelHeight, height) : (Tags.GA_Height, height),
                (Tags.STRINGA_TextVal, text)) as Gadget;
            return gadget;
        }

        public void SetPosition(int x, int y)
        {
            if ((leftEdge != x) || (topEdge != y))
            {
                leftEdge = x;
                topEdge = y;
                if (window != null) window.Invalidate();
            }
        }

        public int GadgetId
        {
            get { return gadgetId; }
            set { gadgetId = value; }
        }

        public object UserData { get; set; }

        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        public Window Window
        {
            get { return window; }
            set { window = value; }
        }

        public Requester Requester
        {
            get { return requester; }
            set
            {
                requester = value;
                if (requester == null)
                {
                    gadgetType &= ~GadgetType.REQGADGET;
                }
                else
                {
                    gadgetType |= GadgetType.REQGADGET;
                }
            }
        }

        public int LeftEdge
        {
            get { return leftEdge; }
            set { leftEdge = value; }
        }

        public int TopEdge
        {
            get { return topEdge; }
            set { topEdge = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int RightEdge { get { return LeftEdge + Width - 1; } }
        public int BottomEdge { get { return TopEdge + Height - 1; } }
        public int CenterX { get { return LeftEdge + Width / 2; } }
        public int CenterY { get { return TopEdge + Height / 2; } }
        public bool ContainsPoint(int x, int y)
        {
            return ((LeftEdge <= x) && (TopEdge <= y) && (RightEdge >= x) && (BottomEdge >= y));
        }

        public GadgetFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public GadgetActivation Activation
        {
            get { return activation; }
            set { activation = value; }
        }

        public GadgetType GadgetType
        {
            get { return gadgetType; }
            set { gadgetType = value; }
        }

        public LabelPlace LabelPlace
        {
            get { return labelPlace; }
            set
            {
                if (labelPlace != value)
                {
                    labelPlace = value;
                    AdjustLabel();
                }
            }
        }

        public IntuiText GadgetText
        {
            get { return gadgetText; }
            set
            {
                gadgetText = value;
                AdjustLabel();
            }
        }

        public Border GadgetRender
        {
            get { return gadgetRender; }
            set { gadgetRender = value; }
        }
        public Border SelectRender
        {
            get { return selectRender; }
            set { selectRender = value; }
        }

        public bool Disabled
        {
            get { return (flags & GadgetFlags.GADGDISABLED) == GadgetFlags.GADGDISABLED; }
            set
            {
                if (value)
                {
                    flags |= GadgetFlags.GADGDISABLED;
                }
                else
                {
                    flags &= ~GadgetFlags.GADGDISABLED;
                }
            }
        }
        public bool Selected
        {
            get { return (flags & GadgetFlags.SELECTED) == GadgetFlags.SELECTED; }
            set
            {
                if (value)
                {
                    flags |= GadgetFlags.SELECTED;
                }
                else
                {
                    flags &= ~GadgetFlags.SELECTED;
                }
            }
        }
        public bool Checked
        {
            get { return (flags & GadgetFlags.CHECKED) == GadgetFlags.CHECKED; }
            set
            {
                if (value)
                {
                    flags |= GadgetFlags.CHECKED;
                }
                else
                {
                    flags &= ~GadgetFlags.CHECKED;
                }
            }
        }

        public Border Border
        {
            get
            {
                if ((Flags & GadgetFlags.GADGHNONE) == GadgetFlags.GADGHNONE) return gadgetRender;
                return Selected ? selectRender : gadgetRender;
            }
        }

        public Image GadgetImage
        {
            get { return gadgetImage; }
            set { gadgetImage = value; }
        }

        public Image SelectImage
        {
            get { return selectImage; }
            set { selectImage = value; }
        }

        public PropInfo PropInfo
        {
            get { return propInfo; }
            set { propInfo = value; }
        }

        public StringInfo StringInfo
        {
            get { return stringInfo; }
            set { stringInfo = value; }
        }

        public string Text
        {
            get
            {
                if (gadgetText != null) return gadgetText.IText;
                return gadgetId.ToString();
            }
        }

        private void AdjustLabel()
        {
            if (gadgetText != null)
            {
                switch (labelPlace)
                {
                    case LabelPlace.PlaceTextIn:
                        gadgetText.LeftEdge = Width / 2;
                        gadgetText.TopEdge = Height / 2;
                        gadgetText.VerticalTextAlign = VerticalTextAlign.Center;
                        gadgetText.HorizontalTextAlign = HorizontalTextAlign.Center;
                        break;
                    case LabelPlace.PlaceTextAbove:
                        gadgetText.LeftEdge = Width / 2;
                        gadgetText.TopEdge = -1;
                        gadgetText.VerticalTextAlign = VerticalTextAlign.Bottom;
                        gadgetText.HorizontalTextAlign = HorizontalTextAlign.Center;
                        break;
                    case LabelPlace.PlaceTextBelow:
                        gadgetText.LeftEdge = Width / 2;
                        gadgetText.TopEdge = Height;
                        gadgetText.VerticalTextAlign = VerticalTextAlign.Top;
                        gadgetText.HorizontalTextAlign = HorizontalTextAlign.Center;
                        break;
                    case LabelPlace.PlaceTextLeft:
                        gadgetText.LeftEdge = -6;
                        gadgetText.TopEdge = Height / 2;
                        gadgetText.VerticalTextAlign = VerticalTextAlign.Center;
                        gadgetText.HorizontalTextAlign = HorizontalTextAlign.Right;
                        break;
                    case LabelPlace.PlaceTextRight:
                        gadgetText.LeftEdge = Width + 6;
                        gadgetText.TopEdge = Height / 2;
                        gadgetText.VerticalTextAlign = VerticalTextAlign.Center;
                        gadgetText.HorizontalTextAlign = HorizontalTextAlign.Left;
                        break;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(gadgetType.ToString());
            sb.Append(" \"");
            sb.Append(Text);
            sb.Append("\" Box: ");
            sb.Append(GetGadgetIBox());
            sb.Append(" Win: ");
            sb.Append(GetWinGadgetIBox());
            if (Checked)
            {
                sb.Append(" Checked");
            }
            return sb.ToString();
        }
    }
}
