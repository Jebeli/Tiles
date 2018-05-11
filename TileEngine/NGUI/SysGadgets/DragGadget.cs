using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI.SysGadgets
{
    internal class DragGadget : SysGadget
    {
        internal DragGadget(Window window) : base(window)
        {
            if (window.WindowFlags.HasFlag(WindowFlags.CloseGadget)) { Left = SYSGADWIDTH; Width -= SYSGADWIDTH; }
            Height = SYSGADHEIGHT;
            if (window.WindowFlags.HasFlag(WindowFlags.DepthGadget)) { Width -= SYSGADWIDTH; }
            if (window.WindowFlags.HasFlag(WindowFlags.ZoomGadget)) { Width -= SYSGADWIDTH; }
            DimFlags = DimFlags.RelWidth;
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
                        Window.Move(inputEvent.DeltaX, inputEvent.DeltaY);
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
        }
    }
}
