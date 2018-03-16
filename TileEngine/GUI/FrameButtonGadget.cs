using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI
{
    public class FrameButtonGadget : ButtonGadget
    {
        public FrameButtonGadget()
        {
            GadgetImage = Intuition.NewObject(Intuition.FRAMEICLASS,
                (Tags.IA_EdgesOnly, false),
                (Tags.IA_FrameType, FrameType.Button)) as Image;
        }
        protected override void RenderBase(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw, IBox container)
        {
            RenderBaseFrame(gadgetInfo, graphics, redraw, container);
        }


    }
}
