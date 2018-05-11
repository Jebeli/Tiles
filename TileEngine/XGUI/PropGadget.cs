using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;
using TileEngine.Logging;

namespace TileEngine.XGUI
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
    public class PropGadget : Widget
    {
        public const int PROP_SIZE = 20;
        public const int KNOBHMIN = 6;
        public const int KNOBVMIN = 4;
        public const int MAXBODY = 0xFFFF;
        public const int MAXPOT = 0xFFFF;
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

        public PropGadget(Widget parent)
            : base(parent)
        {
            flags = PropFlags.AutoKnob | PropFlags.FreeHoriz;
        }

        public event EventHandler<EventArgs> PropChanged;
        public EventHandler<EventArgs> PropChangedEvent
        {
            set { PropChanged += value; }
        }

        public PropFlags Flags
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

        public Rect Knob
        {
            get { return knob; }
        }

        protected void CalcKnobSize()
        {
            Rect rect = Bounds;
            if (!flags.HasFlag(PropFlags.Borderless))
            {
                rect.Inflate(-2, -2);
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
            PropChanged?.Invoke(this, EventArgs.Empty);
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            Vector2 result = new Vector2(PROP_SIZE, PROP_SIZE);
            if (flags.HasFlag(PropFlags.FreeHoriz)) result.X += 75;
            if (flags.HasFlag(PropFlags.FreeVert)) result.Y += 75;
            return result;
        }

        public override void PerformLayout(IGraphics gfx)
        {
            base.PerformLayout(gfx);
            CalcKnobSize();
        }

        public override void Render(IGraphics gfx)
        {
            Theme.RenderProp(gfx, this);
            base.Render(gfx);
        }

        public override bool MouseButtonDownEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            if (Enabled && button == MouseButton.Left)
            {
                CalcKnobSize();
                if (knob.Contains(p))
                {
                    p -= Position;
                    knobStartX = p.X - (knob.Left - Left);
                    knobStartY = p.Y - (knob.Top - Top);
                    Logger.Detail("GUI", $"Knob Hit Mouse: {p.X}/{p.Y} KS: {knobStartX}/{knobStartY}");
                    flags |= PropFlags.KnobHit;
                }
                else
                {
                    flags &= ~PropFlags.KnobHit;
                    HandleContainerHit(knob, p.X, p.Y, horizPot, vertPot);
                }
                widget = this;
                return true;
            }
            return false;
        }

        public override bool MouseButtonUpEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            if (base.MouseButtonUpEvent(p, button, ref widget))
            {
                flags &= ~PropFlags.KnobHit;
                return true;
            }
            return false;
        }

        public override bool MouseMoveEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            widget = this;
            if (Enabled && Selected)
            {
                if (flags.HasFlag(PropFlags.KnobHit))
                {
                    p -= Position;

                    int dx = p.X - knobStartX;
                    int dy = p.Y - knobStartY;
                    Logger.Detail("GUI", $"Knob Move Mouse: {p.X}/{p.Y} KS: {knobStartX}/{knobStartY}");
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
            }
            return true;
        }

        public override bool TimerEvent(Vector2 p, MouseButton button)
        {
            if (Enabled && Selected && !flags.HasFlag(PropFlags.KnobHit))
            {
                Vector2 d = p - Parent.AbsolutePosition;
                CalcKnobSize();
                if (!knob.Contains(d))
                {
                    HandleContainerHit(knob, d.X, d.Y, horizPot, vertPot);
                    return true;
                }
            }
            return false;
        }
    }
}
