using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI
{
    public class Image : GUIElement
    {

        public Image()
        {
        }
        public override void HandleInput(InputEvent inputEvent)
        {

        }

        public virtual void RenderState(IGraphics graphics, StateFlags state, IBox box)
        {

        }

        public override void RenderBack(IGraphics graphics)
        {
            SimpleImageRender(graphics);

        }
        public override void RenderFront(IGraphics graphics)
        {

        }

    }
}
