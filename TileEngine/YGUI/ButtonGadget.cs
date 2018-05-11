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
    public class ButtonGadget : Gadget
    {
        private bool repeat;
        private int timerDelay;

        public ButtonGadget(Gadget parent, Icons icon = Icons.NONE)
            : base(parent, "", icon)
        {
            Size = new Vector2(24, 24);
        }

        public ButtonGadget(Gadget parent, string label, Icons icon = Icons.NONE)
            : base(parent, label, icon)
        {
            Size = new Vector2(64, 24);
        }

        public bool Repeat
        {
            get { return repeat; }
            set { repeat = value; }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            Vector2 res = new Vector2(24, 24);
            res.X = Math.Max(gfx.MeasureTextWidth(Font, Label) + 8, res.X);
            return res;
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

        protected override void HandleSelectDown(Vector2 p)
        {
            timerDelay = 2;
            base.HandleSelectDown(p);
        }

        protected override void HandleSelectUp(Vector2 p)
        {
            if (Sticky)
            {
                Selected = !Selected;
            }
            base.HandleSelectUp(p);
        }

        public override void HandleTimer(Vector2 p, MouseButton button)
        {
            timerDelay--;
            if (timerDelay <= 0)
            {
                timerDelay = 0;
                if (repeat && Enabled && button == MouseButton.Left && MouseSelected)
                {
                    OnGadgetUp();
                }
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Label))
            {
                return $"Button {Label}";
            }
            if (Icon != Icons.NONE)
            {
                return $"Button {Icon}";
            }
            return "Button";
        }

    }
}
