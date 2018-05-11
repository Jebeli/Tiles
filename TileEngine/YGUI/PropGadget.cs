using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;
using TileEngine.Logging;

namespace TileEngine.YGUI
{
    public class PropGadget : Gadget
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
        private bool freeHoriz;
        private bool freeVert;
        private int cWidth;
        private int cHeight;
        private int leftBorder;
        private int topBorder;
        private Rect knob;
        private int knobStartX;
        private int knobStartY;
        private bool knobHit;
        private int timerDelay;

        public PropGadget(Gadget parent)
            : base(parent)
        {
            freeHoriz = true;
            RelVerify = false;
        }

        public event EventHandler<EventArgs> PropChanged;
        public EventHandler<EventArgs> PropChangedEvent
        {
            set { PropChanged += value; }
        }

        protected virtual void OnPropChanged()
        {
            PropChanged?.Invoke(this, EventArgs.Empty);
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

        public bool FreeHoriz
        {
            get { return freeHoriz; }
            set { freeHoriz = value; }
        }

        public bool FreeVert
        {
            get { return freeVert; }
            set { freeVert = value; }
        }

        public Rect Knob
        {
            get { return knob; }
        }

        public bool KnobHit
        {
            get { return knobHit; }
        }

        protected void CalcKnobSize()
        {
            Rect rect = Bounds;
            if (!Borderless)
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
            if (freeHoriz)
            {
                knob.Width = cWidth * horizBody / MAXBODY;
                if (knob.Width < KNOBHMIN) knob.Width = KNOBHMIN;
                knob.X = knob.X + (cWidth - knob.Width) * HorizPot / MAXPOT;
                if (horizBody > 0)
                {
                    if (horizBody < MAXBODY / 2)
                    {
                        hpotRes = MAXPOT * 32768 / ((MAXBODY * 32768 / horizBody) - 32768);
                    }
                    else
                    {
                        hpotRes = MAXPOT;
                    }
                }
                else
                {
                    hpotRes = 1;
                }
            }
            if (freeVert)
            {
                knob.Height = cHeight * vertBody / MAXBODY;
                if (knob.Height < KNOBVMIN) knob.Height = KNOBVMIN;
                knob.Y = knob.Y + (cHeight - knob.Height) * VertPot / MAXPOT;
                if (vertBody > 0)
                {
                    if (vertBody < MAXBODY / 2)
                    {
                        vpotRes = MAXPOT * 32768 / ((MAXBODY * 32768 / vertBody) - 32768);
                    }
                    else
                    {
                        vpotRes = MAXPOT;
                    }
                }
                else
                {
                    vpotRes = 1;
                }
            }
        }

        private void HandleContainerHit(Rect knob, int mx, int my, int dx, int dy)
        {
            if (freeHoriz)
            {
                if (mx < knob.Left)
                {
                    if (dx > hpotRes) { dx -= hpotRes; }
                    else { dx = 0; }
                }
                else if (mx > knob.Right)
                {
                    if (dx < MAXPOT - hpotRes) { dx += hpotRes; }
                    else { dx = MAXPOT; }
                }
            }
            if (freeVert)
            {
                if (my < knob.Top)
                {
                    if (dy > vpotRes) { dy -= vpotRes; }
                    else { dy = 0; }
                }
                else if (my > knob.Bottom)
                {
                    if (dy < MAXPOT - vpotRes) { dy += vpotRes; }
                    else { dy = MAXPOT; }
                }
            }
            ModifyProp(freeHoriz, freeVert, dx, dy, horizBody, vertBody);
        }

        public void ModifyProp(bool freeHoriz, bool freeVert, int horizPot, int vertPot, int horizBody, int vertBody)
        {
            this.freeHoriz = freeHoriz;
            this.freeVert = freeVert;
            this.horizPot = horizPot;
            this.vertPot = vertPot;
            this.horizBody = horizBody;
            this.vertBody = vertBody;
            CalcKnobSize();
            OnPropChanged();
        }


        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            Vector2 result = new Vector2(PROP_SIZE, PROP_SIZE);
            if (freeHoriz) result.X += 75;
            if (freeVert) result.Y += 75;
            return result;
        }

        public override void PerformLayout(IGraphics gfx)
        {
            base.PerformLayout(gfx);
            CalcKnobSize();
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

        protected override void HandleSelectDown(Vector2 p)
        {
            CalcKnobSize();
            p += Position;
            if (knob.Contains(p))
            {
                knobStartX = p.X - (knob.Left - LeftEdge);
                knobStartY = p.Y - (knob.Top - TopEdge);
                Logger.Info("GUI", $"Knob Hit: {p.X}/{p.Y} KS: {knobStartX}/{knobStartY}");
                knobHit = true;
            }
            else
            {
                Logger.Info("GUI", $"Knob Miss: {p.X}/{p.Y}");
                knobHit = false;
                HandleContainerHit(knob, p.X, p.Y, horizPot, vertPot);
                timerDelay = 2;
            }
            base.HandleSelectDown(p);
        }

        protected override void HandleSelectUp(Vector2 p)
        {
            knobHit = false;
            base.HandleSelectUp(p);
        }

        protected override void HandleSelectMove(Vector2 p)
        {
            if (knobHit)
            {
                HandleKnobMove(p);
            }
            base.HandleSelectMove(p);
        }

        private void HandleKnobMove(Vector2 p)
        {
            p += Position;

            int dx = p.X - knobStartX;
            int dy = p.Y - knobStartY;
            Logger.Detail("GUI", $"Knob Move Mouse: {p.X}/{p.Y} KS: {knobStartX}/{knobStartY}");
            if (freeHoriz && (cWidth != knob.Width))
            {
                dx = (dx * MAXPOT) / (cWidth - knob.Width);
                if (dx < 0) dx = 0;
                if (dx > MAXPOT) dx = MAXPOT;
            }
            if (freeVert && (cHeight != knob.Height))
            {
                dy = (dy * MAXPOT) / (cHeight - knob.Height);
                if (dy < 0) dy = 0;
                if (dy > MAXPOT) dy = MAXPOT;
            }
            if ((freeHoriz && (dx != horizPot)) ||
                (freeVert && (dy != vertPot)))
            {
                ModifyProp(freeHoriz, freeVert, dx, dy, horizBody, vertBody);
            }
        }

        public override bool HandleMouseDrag(Vector2 p, Vector2 rel, MouseButton button)
        {
            if (Enabled && knobHit)
            {
                HandleKnobMove(p);
                return true;
            }
            return false;
        }

        public override void HandleTimer(Vector2 p, MouseButton button)
        {
            timerDelay--;
            if (timerDelay <= 0)
            {
                timerDelay = 0;
                if (Enabled && MouseSelected && !knobHit)
                {
                    p += Position;
                    CalcKnobSize();
                    if (!knob.Contains(p))
                    {
                        HandleContainerHit(knob, p.X, p.Y, horizPot, vertPot);
                    }
                    else
                    {
                        knobStartX = p.X - (knob.Left - LeftEdge);
                        knobStartY = p.Y - (knob.Top - TopEdge);
                        knobHit = true;
                    }
                }
            }
        }

        internal static void FindScrollerValues(int total, int visible, int top, int overlap, out int body, out int pot)
        {
            int hidden = total > visible ? total - visible : 0;
            if (top > hidden) top = hidden;
            body = (hidden > 0) ? ((visible - overlap) * MAXBODY) / (total - overlap) : MAXBODY;
            pot = (hidden > 0) ? (top * MAXPOT) / hidden : 0;
        }

        internal static int FindScrollerTop(int total, int visible, int pot)
        {
            int hidden = total > visible ? total - visible : 0;
            return ((hidden * pot) + (MAXPOT / 2)) / MAXPOT;
        }


        public override string ToString()
        {
            if (freeHoriz)
            {
                return $"PropGadget {horizPot}/{horizBody}";
            }
            else if (freeVert)
            {
                return $"PropGadget {vertPot}/{vertBody}";
            }
            return base.ToString();
        }
    }
}
