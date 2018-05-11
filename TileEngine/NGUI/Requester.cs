using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI
{
    public class Requester : GUIElement
    {        
        public Requester()
        {
        }

        public ReqFlags ReqFlags { get; set; }
        public Color BackFill { get; set; }
        public Window Owner { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            var tx = TextElement;
            if (tx != null)
            {
                tx.Top = Height / 3;
                tx.DimFlags = DimFlags.RelWidth;
            }
        }

        public override void HandleInput(InputEvent inputEvent)
        {

        }

        public override void RenderBack(IGraphics graphics)
        {
            IBox box = new Box(WinBox);
            if (box.IsEmpty) return;
            var di = DrawInfo;
            RenderBox3D(graphics, box, di.ShinePen, di.ShadowPen);
            RenderBox3D(graphics, box.Shrink(1, 1), di.ShadowPen, di.ShinePen);
            ClearBox(graphics, box.Shrink(2, 2), BackFill);
        }
        public override void RenderFront(IGraphics graphics)
        {

        }

    }
}
