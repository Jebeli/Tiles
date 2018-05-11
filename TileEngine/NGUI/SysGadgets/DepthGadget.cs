using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI.SysGadgets
{
    internal class DepthGadget : SysGadget
    {
        internal DepthGadget(Window window) : base(window)
        {
            Width = SYSGADWIDTH;
            Height = SYSGADHEIGHT;
            Left = -SYSGADWIDTH + 1;
            DimFlags = DimFlags.RelRight;
        }

        public override void HandleInput(InputEvent inputEvent)
        {
            base.HandleInput(inputEvent);
            if (inputEvent.InputClass == InputClass.GadgetUp)
            {
                Window.ToggleDepth();
            }
        }

        public override void RenderBack(IGraphics graphics)
        {
            base.RenderBack(graphics);
            var box = WinBox;
            if (box.IsEmpty) return;
            var di = DrawInfo;
            int h_spacing = box.Width / 6;
            int v_spacing = box.Height / 6;
            box = box.Shrink(h_spacing, v_spacing);
            RenderRect(graphics, box.Left, box.Top, box.Right - box.Width / 3, box.Bottom - box.Height / 3, di.DarkEdgePen);
            ClearRect(graphics, box.Left + 1, box.Top + 1, box.Right - box.Width / 3 - 1, box.Bottom - box.Height / 3 - 1, Color.Gray);
            RenderRect(graphics, box.Left + box.Width / 3, box.Top + box.Height / 3, box.Right, box.Bottom, di.DarkEdgePen);
            ClearRect(graphics, box.Left + box.Width / 3 + 1, box.Top + box.Height / 3 + 1, box.Right - 1, box.Bottom - 1, Color.White);
        }
    }
}
