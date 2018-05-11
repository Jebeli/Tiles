using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI.SysGadgets
{
    internal class ZoomGadget : SysGadget
    {
        internal ZoomGadget(Window window) : base(window)
        {
            Width = SYSGADWIDTH;
            Height = SYSGADHEIGHT;
            Left = -SYSGADWIDTH + 1;
            if (window.WindowFlags.HasFlag(WindowFlags.DepthGadget)) { Left = -2 * SYSGADWIDTH + 1; }
            DimFlags = DimFlags.RelRight;
        }

        public override void HandleInput(InputEvent inputEvent)
        {
            base.HandleInput(inputEvent);
            if (inputEvent.InputClass == InputClass.GadgetUp)
            {
                Window.Zip();
            }
        }

        public override void RenderBack(IGraphics graphics)
        {
            base.RenderBack(graphics);
            IBox box = new Box(WinBox);            
            if (box.IsEmpty) return;
            var di = DrawInfo;
            int h_spacing = box.Width / 6;
            int v_spacing = box.Height / 6;
            box = box.Shrink(h_spacing, v_spacing);
            RenderBox(graphics, box, di.DarkEdgePen);
            ClearBox(graphics, box.Shrink(1, 1), Color.Gray);
            box.Width = box.Width / 2;
            box.Height = box.Height / 2;
            RenderBox(graphics, box, di.DarkEdgePen);
            ClearBox(graphics, box.Shrink(1, 1), Color.White);
        }
    }
}
