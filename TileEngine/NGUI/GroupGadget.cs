using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI
{
    public class GroupGadget : Gadget
    {
        private bool frame;

        public bool Frame
        {
            get { return frame; }
            set
            {
                if (frame != value)
                {
                    frame = value;
                }
            }
        }


        public override void RenderBack(IGraphics graphics)
        {
            if (frame)
            {
                var di = DrawInfo;
                var box = new Box(WinBox);
                var pbox = GetPreferredSize();
                if (pbox.Width > box.Width) box.Width = pbox.Width;
                if (pbox.Height > box.Width) box.Height = pbox.Height;
                RenderBox3D(graphics, box, di.ShadowPen, di.ShinePen);
                RenderBox3D(graphics, box.Shrink(1, 1), di.ShinePen, di.ShadowPen);
            }
        }

    }
}
