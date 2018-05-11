using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI.SysGadgets
{
    internal class CloseGadget : SysGadget
    {
        internal CloseGadget(Window window) : base(window)
        {
            Width = SYSGADWIDTH;
            Height = SYSGADHEIGHT;
        }

        public override void HandleInput(InputEvent inputEvent)
        {
            base.HandleInput(inputEvent);
            if (inputEvent.InputClass == InputClass.GadgetUp)
            {
                GUISystem.OnInput(new InputEvent(inputEvent)
                {
                    InputClass = InputClass.WindowClose,
                    Window = Window
                });
            }
        }

        public override void RenderBack(IGraphics graphics)
        {
            base.RenderBack(graphics);
            IBox box = new Box(WinBox);
            if (box.IsEmpty) return;
            var di = DrawInfo;
            int h_spacing = box.Width * 4 / 10;
            int v_spacing = box.Height * 3 / 10;
            box = box.Shrink(h_spacing, v_spacing);
            RenderBox(graphics, box, di.DarkEdgePen);
            ClearBox(graphics, box.Shrink(1, 1), Color.White);
        }
    }
}
