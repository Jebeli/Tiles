using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI.SysGadgets
{
    internal class SysGadget : Gadget
    {
        internal const int SYSGADHEIGHT = 20;
        internal const int SYSGADWIDTH = 20;
        private Window window;
        internal SysGadget(Window window)
        {
            this.window = window;
            GadgetFlags |= GadgetFlags.SysGadget;
        }

        public override Window Window
        {
            get { return window; }
        }

        protected override Color GetBgColor()
        {
            bool active = Window.State.HasFlag(StateFlags.Active);
            bool hover = State.HasFlag(StateFlags.Hover);
            if (active)
            {
                return hover ? DrawInfo.HoverActiveBorderPen : DrawInfo.ActiveBorderPen;
            }
            return base.GetBgColor();
        }

        public override void RenderBack(IGraphics graphics)
        {
            IBox box = new Box(WinBox);
            if (box.IsEmpty) return;
            RenderBox3D(graphics, box, GetShineColor(), GetShadowColor());
            ClearBox(graphics, box.Shrink(1, 1), GetBgColor());
        }

    }
}
