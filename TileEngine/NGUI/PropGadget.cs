using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI
{
    public class PropGadget : Gadget
    {
        public const int KNOBHMIN = 6;
        public const int KNOBVMIN = 4;
        public const int MAXBODY = 0xFFFF;
        public const int MAXPOT = 0xFFFF;
        private int horizTop;
        private int vertTop;
        private int horizPot;
        private int vertPot;
        public PropGadget()
        {
            HorizOverlap = 1;
            VertOverlap = 1;
            GadgetFlags = GadgetFlags.None;
            PropFlags = PropFlags.FreeVert | PropFlags.AutoKnob;
        }

        public PropFlags PropFlags { get; set; }
        public int HorizPot
        {
            get { return horizPot; }
            set
            {
                if (horizPot != value)
                {
                    horizPot = value;
                    //Notify();
                }
            }
        }
        public int VertPot
        {
            get { return vertPot; }
            set
            {
                if (vertPot != value)
                {
                    vertPot = value;
                    //Notify();
                }
            }
        }
        public int HorizBody { get; set; }
        public int VertBody { get; set; }
        public int CWidth { get; set; }
        public int CHeight { get; set; }
        public int LeftBorder { get; set; }
        public int TopBorder { get; set; }
        public int HPotRes { get; set; }
        public int VPotRes { get; set; }
        public int VertTotal { get; set; }
        public int VertVisible { get; set; }
        public int VertTop
        {
            get { return vertTop; }
            set
            {
                if (vertTop != value)
                {
                    vertTop = value;
                    //Notify();
                }
            }
        }
        public int VertOverlap { get; set; }
        public int HorizTotal { get; set; }
        public int HorizVisible { get; set; }
        public int HorizTop
        {
            get { return horizTop; }
            set
            {
                if (horizTop != value)
                {
                    horizTop = value;
                    //Notify();
                }
            }
        }
        public int HorizOverlap { get; set; }

        public int FreeTotal
        {
            get { return FreeVert ? VertTotal : HorizTotal; }
            set
            {
                if (FreeTotal != value)
                {
                    if (FreeVert)
                    {
                        VertTotal = value;
                    }
                    else
                    {
                        HorizTotal = value;
                    }
                    UpdateScrollerValues();
                }
            }
        }
        public int FreeVisible
        {
            get { return FreeVert ? VertVisible : HorizVisible; }
            set
            {
                if (FreeVisible != value)
                {
                    if (FreeVert)
                    {
                        VertVisible = value;
                    }
                    else
                    {
                        HorizVisible = value;
                    }
                    UpdateScrollerValues();
                }
            }
        }

        public int FreeTop
        {
            get { return FreeVert ? VertTop : HorizTop; }
            set
            {
                if (FreeTop != value)
                {
                    if (FreeVert)
                    {
                        VertTop = value;
                    }
                    else
                    {
                        HorizTop = value;
                    }
                    UpdateScrollerValues();
                }
            }
        }

        private bool FreeVert { get { return PropFlags.HasFlag(PropFlags.FreeVert); } }
        private bool FreeHoriz { get { return PropFlags.HasFlag(PropFlags.FreeHoriz); } }

        private int propStartX;
        private int propStartY;

        private void UpdateScrollerValues()
        {
            FindScrollerValues(VertTotal, VertVisible, VertTop, VertOverlap, out int body, out int pot);
            VertBody = body;
            VertPot = pot;
            FindScrollerValues(HorizTotal, HorizVisible, HorizTop, HorizOverlap, out body, out pot);
            HorizBody = body;
            HorizPot = pot;
        }

        public override void Layout()
        {
            base.Layout();
            UpdateScrollerValues();
        }

        public override void RenderBack(IGraphics graphics)
        {
            IBox box = new Box(WinBox);
            if (box.IsEmpty) return;
            var di = DrawInfo;
            if (!PropFlags.HasFlag(PropFlags.Borderless))
            {
                RenderBox(graphics, box, di.DarkEdgePen);
                box = box.Shrink(1, 1);
            }
            IBox knob = null;
            CalcKnobSize(box, ref knob);
            RefreshPropGadgetKnob(graphics, di, this, box, knob);
        }

        private void HandleContainerHit(IBox knob, int mx, int my, int dx, int dy)
        {
            if (PropFlags.HasFlag(PropFlags.FreeHoriz))
            {
                if (mx < knob.Left)
                {
                    if (dx > HPotRes)
                    {
                        dx -= HPotRes;
                    }
                    else
                    {
                        dx = 0;
                    }
                }
                else if (mx > knob.Right)
                {
                    if (dx < MAXPOT - HPotRes)
                    {
                        dx += HPotRes;
                    }
                    else
                    {
                        dx = MAXPOT;
                    }
                }
            }
            if (PropFlags.HasFlag(PropFlags.FreeVert))
            {
                if (my < knob.Top)
                {
                    if (dy > VPotRes)
                    {
                        dy -= VPotRes;
                    }
                    else
                    {
                        dy = 0;
                    }
                }
                else if (my > knob.Bottom)
                {
                    if (dy < MAXPOT - VPotRes)
                    {
                        dy += VPotRes;
                    }
                    else
                    {
                        dy = MAXPOT;
                    }
                }
            }
            ModifyProp(PropFlags, dx, dy, HorizBody, VertBody, true);
        }

        public override void HandleInput(InputEvent inputEvent)
        {
            base.HandleInput(inputEvent);
            IBox box = new Box(ScrBox);
            if (!PropFlags.HasFlag(PropFlags.Borderless))
            {
                box = box.Shrink(1, 1);
            }
            IBox knob = null;
            int mx = inputEvent.MouseX;
            int my = inputEvent.MouseY;
            int dx = HorizPot;
            int dy = VertPot;
            switch (inputEvent.InputClass)
            {
                case InputClass.Timer:
                    if (Selected && !PropFlags.HasFlag(PropFlags.KnobHit))
                    {
                        CalcKnobSize(box, ref knob);
                        HandleContainerHit(knob, mx, my, dx, dy);
                    }
                    break;
                case InputClass.MouseDown:
                    CalcKnobSize(box, ref knob);
                    if (knob.ContainsPoint(mx, my))
                    {
                        propStartX = mx - knob.Left;
                        propStartY = my - knob.Top;
                        PropFlags |= PropFlags.KnobHit;
                    }
                    else
                    {
                        PropFlags &= ~PropFlags.KnobHit;
                        HandleContainerHit(knob, mx, my, dx, dy);
                    }
                    break;
                case InputClass.MouseMove:
                    CalcKnobSize(box, ref knob);
                    if (PropFlags.HasFlag(PropFlags.KnobHit))
                    {
                        mx = mx - LeftBorder;
                        my = my - TopBorder;
                        dx = mx - propStartX;
                        dy = my - propStartY;
                        if (PropFlags.HasFlag(PropFlags.FreeHoriz) && (CWidth != knob.Width))
                        {
                            dx = (dx * MAXPOT) / (CWidth - knob.Width);
                            if (dx < 0) dx = 0;
                            if (dx > MAXPOT) dx = MAXPOT;
                        }
                        if (PropFlags.HasFlag(PropFlags.FreeVert) && (CHeight != knob.Height))
                        {
                            dy = (dy * MAXPOT) / (CHeight - knob.Height);
                            if (dy < 0) dy = 0;
                            if (dy > MAXPOT) dy = MAXPOT;
                        }
                        if ((PropFlags.HasFlag(PropFlags.FreeHoriz) && (dx != HorizPot)) ||
                            (PropFlags.HasFlag(PropFlags.FreeVert) && (dy != VertPot)))
                        {
                            ModifyProp(PropFlags, dx, dy, HorizBody, VertBody, true);
                        }
                    }
                    break;
                case InputClass.MouseUp:
                    ModifyProp(PropFlags & ~PropFlags.KnobHit, dx, dy, HorizBody, VertBody, true);
                    break;
            }
        }

        public void ModifyProp(PropFlags flags, int horizPot, int vertPot, int horizBody, int vertBody, bool notify)
        {
            PropFlags = flags;
            this.horizPot = horizPot;
            this.vertPot = vertPot;
            HorizBody = horizBody;
            VertBody = vertBody;
            horizTop = FindScrollerTop(HorizTotal, HorizVisible, HorizPot);
            vertTop = FindScrollerTop(VertTotal, VertVisible, VertPot);
            if (notify) Notify();
        }

        private static void RefreshPropGadgetKnob(IGraphics graphics, DrawInfo drawInfo, Gadget gadget, IBox clear, IBox knob)
        {
            bool selected = gadget.Selected;
            bool disabled = !gadget.Enabled;
            bool hover = gadget.Hover;
            Color shine = Color.White;
            Color shadow = Color.Black;
            Color fillColor = Color.Black;
            Color backColor = Color.Black;
            if (drawInfo != null)
            {
                shine = drawInfo.ShinePen;
                shadow = drawInfo.ShadowPen;
                backColor = drawInfo.BackPen;
                fillColor = drawInfo.BackPen;
                if (hover)
                {
                    backColor = drawInfo.HoverBackPen;
                }
            }
            if (selected)
            {
                Color temp = shine;
                shine = shadow;
                shadow = temp;
            }
            ClearBox(graphics, clear, fillColor);
            RenderBox3D(graphics, knob, shine, shadow);
            ClearBox(graphics, knob.Shrink(1, 1), backColor);
        }


        private void CalcKnobSize(IBox container, ref IBox knob)
        {
            if (knob == null) knob = new Box();
            knob.Left = container.Left;
            knob.Top = container.Top;
            knob.Width = container.Width;
            knob.Height = container.Height;
            CWidth = knob.Width;
            CHeight = knob.Height;
            LeftBorder = container.Left;
            TopBorder = container.Top;
            if (PropFlags.HasFlag(PropFlags.FreeHoriz))
            {
                if (PropFlags.HasFlag(PropFlags.AutoKnob))
                {
                    knob.Width = CWidth * HorizBody / MAXBODY;
                    if (knob.Width < KNOBHMIN) knob.Width = KNOBHMIN;
                }
                knob.Left = knob.Left + (CWidth - knob.Width) * HorizPot / MAXPOT;
                if (HorizBody > 0)
                {
                    if (HorizBody < MAXBODY / 2)
                    {
                        HPotRes = MAXPOT * 32768 / ((MAXBODY * 32768 / HorizBody) - 32768);
                    }
                    else
                    {
                        HPotRes = MAXPOT;
                    }
                }
                else
                {
                    HPotRes = 1;
                }
            }
            if (PropFlags.HasFlag(PropFlags.FreeVert))
            {
                if (PropFlags.HasFlag(PropFlags.AutoKnob))
                {
                    knob.Height = CHeight * VertBody / MAXBODY;
                    if (knob.Height < KNOBVMIN) knob.Height = KNOBVMIN;
                }
                knob.Top = knob.Top + (CHeight - knob.Height) * VertPot / MAXPOT;
                if (VertBody > 0)
                {
                    if (VertBody < MAXBODY / 2)
                    {
                        VPotRes = MAXPOT * 32768 / ((MAXBODY * 32768 / VertBody) - 32768);
                    }
                    else
                    {
                        VPotRes = MAXPOT;
                    }
                }
                else
                {
                    VPotRes = 1;
                }
            }
        }

        private static void FindScrollerValues(int total, int visible, int top, int overlap, out int body, out int pot)
        {
            int hidden = total > visible ? total - visible : 0;
            if (top > hidden) top = hidden;
            body = (hidden > 0) ? ((visible - overlap) * MAXBODY) / (total - overlap) : MAXBODY;
            pot = (hidden > 0) ? (top * MAXPOT) / hidden : 0;
        }

        private static int FindScrollerTop(int total, int visible, int pot)
        {
            int hidden = total > visible ? total - visible : 0;
            return ((hidden * pot) + (MAXPOT / 2)) / MAXPOT;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetType().Name);
            sb.Append(": ");
            sb.Append(Text);
            if (FreeVert)
            {
                sb.Append(" V:");
                sb.Append(VertTop);
                sb.Append("/");
                sb.Append(VertTotal);
            }
            if (FreeHoriz)
            {
                sb.Append(" H:");
                sb.Append(HorizTop);
                sb.Append("/");
                sb.Append(HorizTotal);
            }
            return sb.ToString();
        }
    }
}
