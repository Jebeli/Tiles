using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Input;
using TileEngine.Logging;

namespace TileEngine.IGUI
{
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
    public class PropGadget : Gadget
    {
        public const int PROP_SIZE = 20;
        public const int KNOBHMIN = 6;
        public const int KNOBVMIN = 4;
        public const int MAXBODY = 0xFFFF;
        public const int MAXPOT = 0xFFFF;
        protected int total;
        protected int visible;
        protected int top;
        protected int overlap;
        private int horizBody;
        private int vertBody;
        private int horizPot;
        private int vertPot;
        private int hpotRes;
        private int vpotRes;
        private PropFlags flags;
        private int cWidth;
        private int cHeight;
        private int leftBorder;
        private int topBorder;
        private Rect knob;
        private int knobStartX;
        private int knobStartY;
        private int skipTicks;

        public PropGadget()
          : this(TagItems.Empty)
        {
        }

        public PropGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.PropGadget;
            flags = PropFlags.AutoKnob | PropFlags.FreeHoriz;
            overlap = 1;
            New(tags);
            ChangeValues();
        }

        public PropFlags PropFlags
        {
            get { return flags; }
            set { flags = value; }
        }

        public int HorizPot
        {
            get { return horizPot; }
            set { horizPot = value; }
        }
        public int VertPot
        {
            get { return vertPot; }
            set { vertPot = value; }
        }
        public int HorizBody
        {
            get { return horizBody; }
            set { horizBody = value; }
        }
        public int VertBody
        {
            get { return vertBody; }
            set { vertBody = value; }
        }

        public int HPotRes
        {
            get { return hpotRes; }
            set { hpotRes = value; }
        }
        public int VPotRes
        {
            get { return vpotRes; }
            set { vpotRes = value; }
        }

        public int FreeTop
        {
            get { return top; }
            set
            {
                if (top != value)
                {
                    top = value;
                    ChangeValues();
                }
            }
        }

        public int FreeVisible
        {
            get { return visible; }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    ChangeValues();
                }
            }
        }

        public int FreeTotal
        {
            get { return total; }
            set
            {
                if (total != value)
                {
                    total = value;
                    ChangeValues();
                }
            }
        }

        public Rect Knob
        {
            get { return knob; }
        }

        public Orientation Orientation
        {
            get { return flags.HasFlag(PropFlags.FreeHoriz) ? Orientation.Horizontal : Orientation.Vertical; }
            set
            {
                if (value == Orientation.Horizontal)
                {
                    flags &= ~PropFlags.FreeVert;
                    flags |= PropFlags.FreeHoriz;
                }
                else
                {
                    flags &= ~PropFlags.FreeHoriz;
                    flags |= PropFlags.FreeVert;
                }
            }
        }

        public int PotRes
        {
            get { return (Orientation == Orientation.Horizontal) ? hpotRes : vpotRes; }
        }

        public override void Layout(bool initial)
        {
            base.Layout(initial);
            CalcKnobSize();
        }

        private void HandleContainerHit(Rect knob, int mx, int my, int dx, int dy)
        {
            if (flags.HasFlag(PropFlags.FreeHoriz))
            {
                if (mx < knob.Left)
                {
                    if (dx > HPotRes) { dx -= HPotRes; }
                    else { dx = 0; }
                }
                else if (mx > knob.Right)
                {
                    if (dx < MAXPOT - HPotRes) { dx += HPotRes; }
                    else { dx = MAXPOT; }
                }
            }
            if (flags.HasFlag(PropFlags.FreeVert))
            {
                if (my < knob.Top)
                {
                    if (dy > VPotRes) { dy -= VPotRes; }
                    else { dy = 0; }
                }
                else if (my > knob.Bottom)
                {
                    if (dy < MAXPOT - VPotRes) { dy += VPotRes; }
                    else { dy = MAXPOT; }
                }
            }
            ModifyProp(flags, dx, dy, horizBody, vertBody);
        }

        public override GoActiveResult HandleInput(IDCMPFlags idcmp, int x, int y, MouseButton button, InputCode code)
        {
            var res = base.HandleInput(idcmp, x, y, button, code);
            if (res == GoActiveResult.MeActive)
            {
                switch (idcmp)
                {
                    case IDCMPFlags.MouseButtons:
                        if (code == InputCode.Pressed)
                        {
                            skipTicks = 2;
                            CalcKnobSize();
                            x += leftBorder;
                            y += topBorder;
                            if (knob.Contains(x, y))
                            {
                                knobStartX = x - (knob.Left - leftBorder);
                                knobStartY = y - (knob.Top - topBorder);
                                Logger.Detail("GUI", $"Knob Hit Mouse: {x}/{y} KS: {knobStartX}/{knobStartY} PR: {PotRes} K: {knob}");
                                flags |= PropFlags.KnobHit;
                            }
                            else
                            {
                                Logger.Info("GUI", $"Knob Miss Mouse: {x}/{y} KS: {knobStartX}/{knobStartY} PR: {PotRes} K: {knob}");
                                flags &= ~PropFlags.KnobHit;
                                HandleContainerHit(knob, x, y, horizPot, vertPot);
                            }
                        }
                        else if (code == InputCode.Released)
                        {
                            flags &= ~PropFlags.KnobHit;
                        }
                        break;
                    case IDCMPFlags.MouseMove:
                        if (flags.HasFlag(PropFlags.KnobHit))
                        {
                            x += leftBorder;
                            y += topBorder;
                            int dx = x - knobStartX;
                            int dy = y - knobStartY;
                            Logger.Detail("GUI", $"Knob Move Mouse: {x}/{y} KS: {knobStartX}/{knobStartY} PR: {PotRes} K: {knob}");
                            if (flags.HasFlag(PropFlags.FreeHoriz) && (cWidth != knob.Width))
                            {
                                dx = (dx * MAXPOT) / (cWidth - knob.Width);
                                if (dx < 0) dx = 0;
                                if (dx > MAXPOT) dx = MAXPOT;
                            }
                            if (flags.HasFlag(PropFlags.FreeVert) && (cHeight != knob.Height))
                            {
                                dy = (dy * MAXPOT) / (cHeight - knob.Height);
                                if (dy < 0) dy = 0;
                                if (dy > MAXPOT) dy = MAXPOT;
                            }
                            if ((flags.HasFlag(PropFlags.FreeHoriz) && (dx != horizPot)) ||
                                (flags.HasFlag(PropFlags.FreeVert) && (dy != vertPot)))
                            {
                                ModifyProp(flags, dx, dy, horizBody, vertBody);
                            }
                        }
                        break;
                    case IDCMPFlags.IntuiTicks:
                        if (Selected && Active && !flags.HasFlag(PropFlags.KnobHit))
                        {
                            skipTicks--;
                            if (skipTicks <= 0)
                            {
                                skipTicks = 0;
                                CalcKnobSize();
                                x += leftBorder;
                                y += topBorder;
                                if (!knob.Contains(x, y))
                                {
                                    HandleContainerHit(knob, x, y, horizPot, vertPot);
                                }
                            }
                        }
                        break;
                }
            }
            return res;
        }

        public void ModifyProp(PropFlags flags, int horizPot, int vertPot, int horizBody, int vertBody)
        {
            this.flags = flags;
            this.horizPot = horizPot;
            this.vertPot = vertPot;
            this.horizBody = horizBody;
            this.vertBody = vertBody;
            CalcKnobSize();
            UpdateValues();
        }

        protected virtual void UpdateValues()
        {
            int pot = Orientation == Orientation.Horizontal ? horizPot : vertPot;
            int lev = FindScrollerTop(total, visible, pot);
            if (lev != top)
            {
                top = lev;
            }
        }

        protected void ChangeValues()
        {
            FindScrollerValues(total, visible, top, overlap, out int body, out int pot);
            if (Orientation == Orientation.Horizontal)
            {
                ModifyProp(flags, pot, vertPot, body, vertBody);
            }
            else
            {
                ModifyProp(flags, horizPot, pot, horizBody, body);
            }
        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.PGA_Borderless:
                    if (tag.GetTagData(false))
                    {
                        flags |= PropFlags.Borderless;
                    }
                    else
                    {
                        flags &= ~PropFlags.Borderless;
                    }
                    return 1;
                case Tags.PGA_Freedom:
                    if (tag.GetTagData(PropFlags.FreeHoriz) == PropFlags.FreeHoriz)
                    {
                        flags &= ~PropFlags.FreeVert;
                        flags |= PropFlags.FreeHoriz;
                    }
                    else
                    {
                        flags &= ~PropFlags.FreeHoriz;
                        flags |= PropFlags.FreeVert;
                    }
                    return 1;
                case Tags.PGA_Top:
                    top = tag.GetTagData(0);
                    return 1;
                case Tags.PGA_Total:
                    total = tag.GetTagData(0);
                    return 1;
                case Tags.PGA_Visible:
                    visible = tag.GetTagData(0);
                    return 1;
            }
            return base.SetTag(set, update, tag);
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

        protected void CalcKnobSize()
        {
            Rect rect = Bounds;
            if (!flags.HasFlag(PropFlags.Borderless))
            {
                rect.Inflate(-1, -1);
            }
            CalcKnobSize(rect, ref knob);
        }

        private void CalcKnobSize(Rect container, ref Rect knob)
        {
            knob.X = container.Left;
            knob.Y = container.Top;
            knob.Width = container.Width;
            knob.Height = container.Height;
            cWidth = knob.Width;
            cHeight = knob.Height;
            leftBorder = container.Left;
            topBorder = container.Top;
            if (flags.HasFlag(PropFlags.FreeHoriz))
            {
                if (flags.HasFlag(PropFlags.AutoKnob))
                {
                    knob.Width = cWidth * HorizBody / MAXBODY;
                    if (knob.Width < KNOBHMIN) knob.Width = KNOBHMIN;
                }
                knob.X = knob.X + (cWidth - knob.Width) * HorizPot / MAXPOT;
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

            if (flags.HasFlag(PropFlags.FreeVert))
            {
                if (flags.HasFlag(PropFlags.AutoKnob))
                {
                    knob.Height = cHeight * VertBody / MAXBODY;
                    if (knob.Height < KNOBVMIN) knob.Height = KNOBVMIN;
                }
                knob.Y = knob.Y + (cHeight - knob.Height) * VertPot / MAXPOT;
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
    }
}
