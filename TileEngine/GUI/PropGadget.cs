using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI
{
    public class PropGadget : Gadget
    {
        private int newTop;
        private bool setFlag;
        public PropGadget()
        {
            GadgetType &= ~GadgetType.GTYPEMASK;
            GadgetType |= GadgetType.PROPGADGET;
            Flags = GadgetFlags.BOOPSIGADGET;
            PropInfo = new PropInfo();
        }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.PGA_Top:
                    newTop = tag.GetTagData(Top);
                    setFlag = true;
                    return 1;
                case Tags.PGA_Visible:
                    Visible = tag.GetTagData(Visible);
                    setFlag = true;
                    return 1;
                case Tags.PGA_Total:
                    Total = tag.GetTagData(Total);
                    setFlag = true;
                    return 1;
                case Tags.PGA_Freedom:
                    PropInfo.Flags &= ~(PropFlags.FREEHORIZ | PropFlags.FREEVERT);
                    PropInfo.Flags |= tag.GetTagData(PropFlags.FREEVERT);
                    return 1;
                case Tags.PGA_Boderless:
                    if (tag.GetTagData(false))
                    {
                        PropInfo.Flags |= PropFlags.PROPBORDERLESS;
                    }
                    else
                    {
                        PropInfo.Flags &= ~PropFlags.PROPBORDERLESS;
                    }
                    return 1;
                case Tags.PGA_HorizPot:
                    PropInfo.HorizPot = tag.GetTagData(0);
                    return 1;
                case Tags.PGA_HorizBody:
                    PropInfo.HorizBody = tag.GetTagData(PropInfo.MAXBODY);
                    return 1;
                case Tags.PGA_VertPot:
                    PropInfo.VertPot = tag.GetTagData(0);
                    return 1;
                case Tags.PGA_VertBody:
                    PropInfo.VertBody = tag.GetTagData(PropInfo.MAXBODY);
                    return 1;
                default:
                    return base.SetTag(gadgetInfo, set, update, tag);
            }
        }

        protected override int BeforeSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update)
        {
            newTop = Top;
            setFlag = false;
            return 0;
        }

        protected override int AfterSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, int returnValue)
        {
            if (setFlag || set == SetFlags.New)
            {
                if (Total > Visible)
                {
                    if (newTop > (Total - Visible))
                        newTop = Total - Visible;
                }
                else
                {
                    newTop = 0;
                }
                if (newTop < 0)
                {
                    newTop = 0;
                }
                if (Top != newTop)
                {
                    Top = newTop;
                    NotifyTop(gadgetInfo, true);
                }
                FindScrollerValues(Total, Visible, Top, 0, out int body, out int pot);
                if ((PropInfo.Flags & PropFlags.FREEVERT) == PropFlags.FREEVERT)
                {
                    PropInfo.VertBody = body;
                    PropInfo.VertPot = pot;
                }
                if ((PropInfo.Flags & PropFlags.FREEHORIZ) == PropFlags.FREEHORIZ)
                {
                    PropInfo.HorizBody = body;
                    PropInfo.HorizPot = pot;
                }
            }
            if (set == SetFlags.New)
            {
                if (GadgetImage == null)
                {
                    PropInfo.Flags |= PropFlags.AUTOKNOB;
                }
            }
            return base.AfterSetTags(gadgetInfo, set, update, returnValue);
        }

        public override GadgetActive HandleInput(GadgetInfo gadgetInfo, InputEvent inputEvent, ref int termination, int mouseX, int mouseY)
        {
            GadgetActive ga = base.HandleInput(gadgetInfo, inputEvent, ref termination, mouseX, mouseY);
            switch (inputEvent.InputClass)
            {
                case InputClass.MOUSEMOVE:
                    if ((PropInfo.Flags & PropFlags.KNOBHIT) == PropFlags.KNOBHIT)
                    {
                        UpdateTop(gadgetInfo, false);
                    }
                    break;
                case InputClass.MOUSEUP:
                    UpdateTop(gadgetInfo, true);
                    break;
            }
            return ga;
        }

        private void NotifyTop(GadgetInfo gadgetInfo, bool final)
        {
            UpdateFlags flags = final ? UpdateFlags.Final : UpdateFlags.Interim;
            Notify(gadgetInfo, flags, (Tags.PGA_Top, Top), (Tags.GA_ID, GadgetId));
        }

        private void UpdateTop(GadgetInfo gadgetInfo, bool final)
        {
            int pot = (PropInfo.Flags & PropFlags.FREEVERT) == PropFlags.FREEVERT ? PropInfo.VertPot : PropInfo.HorizPot;
            int top = FindScrollerTop(Total, Visible, pot);
            if (top != Top || final)
            {
                Top = top;
                NotifyTop(gadgetInfo, final);
            }
        }

        public override void Render(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw)
        {
            Render(this, gadgetInfo, graphics, redraw);
        }

        private static void Render(Gadget gadget, GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw)
        {
            if (gadget != null)
            {
                IBox container = null;
                gadget.GetGadgetIBox(gadgetInfo, ref container);
                if ((container.Width <= 1) || (container.Height <= 1))
                    return;
                PropInfo pi = gadget.PropInfo;
                if (pi != null)
                {
                    if ((pi.Flags & PropFlags.PROPBORDERLESS) != PropFlags.PROPBORDERLESS)
                    {
                        graphics.DrawRect(container.LeftEdge, container.TopEdge, container.RightEdge, container.BottomEdge, gadgetInfo.DrawInfo.ShadowPen);
                        container.LeftEdge++;
                        container.TopEdge++;
                        container.Width -= 2;
                        container.Height -= 2;
                    }
                    IBox knob = null;
                    CalcKnobSize(gadget, container, ref knob);
                    RefreshPropGadgetKnob(graphics, gadgetInfo.DrawInfo, gadget, container, knob, gadgetInfo.Window, gadgetInfo.Requester);
                }
                Intuition.DrawIntuiText(graphics, gadget.GadgetText, container.LeftEdge, container.TopEdge);
            }
        }

        private static void RefreshPropGadgetKnob(IGraphics rport, DrawInfo drawInfo, Gadget gadget, IBox clear, IBox knob, Window window, Requester req)
        {
            bool selected = (gadget.Flags & GadgetFlags.SELECTED) == GadgetFlags.SELECTED;
            bool disabled = (gadget.Flags & GadgetFlags.GADGDISABLED) == GadgetFlags.GADGDISABLED;
            bool hover = (gadget.Flags & GadgetFlags.HOVER) == GadgetFlags.HOVER;
            bool inactive = true;
            if ((gadget.Activation & (GadgetActivation.TOPBORDER | GadgetActivation.BOTTOMBORDER | GadgetActivation.RIGHTBORDER | GadgetActivation.LEFTBORDER)) != GadgetActivation.NONE)
            {
                inactive = (window.Flags & WindowFlags.WFLG_WINDOWACTIVE) != WindowFlags.WFLG_WINDOWACTIVE;
            }
            Color shine = Color.White;
            Color shadow = Color.Black;
            Color fillColor = Color.Black;
            Color backColor = Color.Black;
            if (drawInfo != null)
            {
                fillColor = drawInfo.PropClearPen;
                shine = drawInfo.ShinePen;
                shadow = drawInfo.ShadowPen;
                backColor = drawInfo.BackgoundPen;
                if (!inactive)
                {
                    backColor = drawInfo.FillPen;
                }
                if (hover && !disabled)
                {
                    shine = drawInfo.HoverShinePen;
                    shadow = drawInfo.HoverShadowPen;
                    backColor = drawInfo.InactiveHoverBackgroundPen;
                    if (!inactive)
                    {
                        backColor = drawInfo.HoverBackgroundPen;
                    }
                }
            }
            if (selected)
            {
                Color temp = shine;
                shine = shadow;
                shadow = temp;
            }
            rport.RectFill(clear, fillColor);
            FrameImage.InternalDrawFrame(rport, shine, shadow, knob.LeftEdge, knob.TopEdge, knob.Width, knob.Height, false);
            rport.RectFill(knob.LeftEdge + 1, knob.TopEdge + 1, knob.RightEdge - 1, knob.BottomEdge - 1, backColor);
        }

        public static void CalcKnobSize(Gadget gadget, IBox container, ref IBox knob)
        {
            if (knob == null) knob = new Box();
            knob.LeftEdge = container.LeftEdge;
            knob.TopEdge = container.TopEdge;
            knob.Width = container.Width;
            knob.Height = container.Height;
            PropInfo pi = gadget.PropInfo;
            if (pi != null)
            {
                pi.CWidth = knob.Width;
                pi.CHeight = knob.Height;
                pi.LeftBorder = container.LeftEdge;
                pi.TopBorder = container.TopEdge;
                if (((pi.Flags & PropFlags.AUTOKNOB) != PropFlags.AUTOKNOB) && gadget.GadgetImage != null)
                {
                    knob.Width = gadget.GadgetImage.Width;
                    knob.Height = gadget.GadgetImage.Height;
                }
                if ((pi.Flags & PropFlags.FREEHORIZ) == PropFlags.FREEHORIZ)
                {
                    if ((pi.Flags & PropFlags.AUTOKNOB) == PropFlags.AUTOKNOB)
                    {
                        knob.Width = pi.CWidth * pi.HorizBody / PropInfo.MAXBODY;
                        if (knob.Width < PropInfo.KNOBHMIN) knob.Width = PropInfo.KNOBHMIN;
                    }
                    knob.LeftEdge = knob.LeftEdge + (pi.CWidth - knob.Width) * pi.HorizPot / PropInfo.MAXPOT;
                    if (pi.HorizBody > 0)
                    {
                        if (pi.HorizBody < PropInfo.MAXBODY / 2)
                        {
                            pi.HPotRes = PropInfo.MAXPOT * 32768 / ((PropInfo.MAXBODY * 32768 / pi.HorizBody) - 32768);
                        }
                        else
                        {
                            pi.HPotRes = PropInfo.MAXPOT;
                        }
                    }
                    else
                    {
                        pi.HPotRes = 1;
                    }
                }
                if ((pi.Flags & PropFlags.FREEVERT) == PropFlags.FREEVERT)
                {
                    if ((pi.Flags & PropFlags.AUTOKNOB) == PropFlags.AUTOKNOB)
                    {
                        knob.Height = pi.CHeight * pi.VertBody / PropInfo.MAXBODY;
                        if (knob.Height < PropInfo.KNOBVMIN) knob.Height = PropInfo.KNOBVMIN;
                    }
                    knob.TopEdge = knob.TopEdge + (pi.CHeight - knob.Height) * pi.VertPot / PropInfo.MAXPOT;
                    if (pi.VertBody > 0)
                    {
                        if (pi.VertBody < PropInfo.MAXBODY / 2)
                        {
                            pi.VPotRes = PropInfo.MAXPOT * 32768 / ((PropInfo.MAXBODY * 32768 / pi.VertBody) - 32768);
                        }
                        else
                        {
                            pi.VPotRes = PropInfo.MAXPOT;
                        }
                    }
                    else
                    {
                        pi.VPotRes = 1;
                    }
                }
            }
        }

        public static void RefreshPropGadget(Gadget gadget, Window window, Requester req, IGraphics graphics)
        {
            GadgetInfo gi = Intuition.SetupGInfo(window, req, gadget, graphics);
            Render(gadget, gi, graphics, GadgetRedraw.Redraw);
        }


        public int Top { get; set; }
        public int Visible { get; set; }
        public int Total { get; set; }

        private static void FindScrollerValues(int total, int visible, int top, int overlap, out int body, out int pot)
        {
            int hidden = total > visible ? total - visible : 0;
            if (top > hidden) top = hidden;
            body = (hidden > 0) ? ((visible - overlap) * PropInfo.MAXBODY) / (total - overlap) : PropInfo.MAXBODY;
            pot = (hidden > 0) ? (top * PropInfo.MAXPOT) / hidden : 0;
        }

        private static int FindScrollerTop(int total, int visible, int pot)
        {
            int hidden = total > visible ? total - visible : 0;
            return ((hidden * pot) + (PropInfo.MAXPOT / 2)) / PropInfo.MAXPOT;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            sb.Append(" ");
            sb.Append(PropInfo);
            return sb.ToString();
        }
    }
}
