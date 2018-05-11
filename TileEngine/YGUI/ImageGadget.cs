using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.YGUI
{
    public class ImageGadget : Gadget
    {
        private bool frame;
        private TextureRegion image;

        public ImageGadget(Gadget parent)
            : base(parent)
        {
            frame = true;
        }

        public bool Frame
        {
            get { return frame; }
            set { frame = value; }
        }


        public TextureRegion Image
        {
            get { return image; }
            set { image = value; }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            Vector2 res = new Vector2();
            if (image != null)
            {
                res = new Vector2(image.Width, image.Height);
            }
            if (!frame)
            {
                res.X += 2;
                res.Y += 2;
            }
            return res;            
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

    }
}
