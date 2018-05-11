using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI
{
    public class Gadget : GUIElement
    {

        public Gadget()
        {
            GadgetFlags = GadgetFlags.RelVerify;
        }

        public virtual Window Window
        {
            get { return GUISystem.GetWindow(this); }
        }
        public Requester Requester
        {
            get { return GUISystem.GetRequester(this); }
        }
        public GadgetFlags GadgetFlags { get; set; }

        public override bool HitTest(int x, int y)
        {
            var win = Window;
            if (win != null)
            {
                if (win.InRequest)
                {                    
                    if (GadgetFlags.HasFlag(GadgetFlags.SysGadget) || Requester != null)
                    {
                        return base.HitTest(x, y);
                    }
                    return false;
                }
            }
            return base.HitTest(x, y);
        }

        public override void HandleInput(InputEvent inputEvent)
        {

        }

        public override void RenderBack(IGraphics graphics)
        {
            SimpleGadgetRender(graphics);

        }

        public override void RenderFront(IGraphics graphics)
        {
            SimpleDisableRender(graphics);
        }

    }
}
