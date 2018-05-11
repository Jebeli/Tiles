using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI.SysGadgets
{
    internal class SizeGadget : SysGadget
    {
        internal SizeGadget(Window window) : base(window)
        {
            Width = SYSGADWIDTH;
            Height = SYSGADHEIGHT;
            Left = -SYSGADWIDTH + 1;
            Top = -SYSGADHEIGHT + 1;
            DimFlags = DimFlags.RelRight | DimFlags.RelBottom;
            GadgetFlags &= ~GadgetFlags.RelVerify;
        }

        public override void HandleInput(InputEvent inputEvent)
        {
            base.HandleInput(inputEvent);
            switch (inputEvent.InputClass)
            {
                case InputClass.MouseMove:
                    if (Active)
                    {
                        Window.Size(inputEvent.DeltaX, inputEvent.DeltaY);
                    }
                    break;
                case InputClass.GadgetUp:
                    Active = false;
                    break;
            }
        }

        public override void RenderBack(IGraphics graphics)
        {
            base.RenderBack(graphics);
            IBox box = new Box(WinBox);
            if (box.IsEmpty) return;
            var di = DrawInfo;
            int h_spacing = box.Width / 5;
            int v_spacing = box.Height / 5;
            box = box.Shrink(h_spacing, v_spacing);
            for (int y = box.Top; y <= box.Bottom; y++)
            {
                int x = box.Left + (box.Bottom - y) * box.Width / box.Height;
                ClearRect(graphics, x, y, box.Right, y + 1, Color.White);
            }
            graphics.DrawLine(box.Left, box.Bottom, box.Right, box.Top, di.DarkEdgePen);
            graphics.DrawLine(box.Right, box.Top, box.Right, box.Bottom, di.DarkEdgePen);
            graphics.DrawLine(box.Right, box.Bottom, box.Left, box.Bottom, di.DarkEdgePen);
        }
    }
}
