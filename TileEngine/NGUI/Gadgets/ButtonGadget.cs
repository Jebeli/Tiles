using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI.Gadgets
{
    public class ButtonGadget : Gadget
    {
        private Image renderImage;
        private Image selectImage;
        private Image disabledImage;

        public Image RenderImage
        {
            get { return renderImage; }
            set
            {
                if (renderImage != value)
                {
                    renderImage = value;
                }
            }
        }

        public Image SelectImage
        {
            get { return selectImage; }
            set
            {
                if (selectImage != value)
                {
                    selectImage = value;
                }
            }
        }

        public Image DisabledImage
        {
            get { return disabledImage; }
            set
            {
                if (disabledImage != value)
                {
                    disabledImage = value;
                }
            }
        }

        public override void RenderBack(IGraphics graphics)
        {
            if (!Enabled && disabledImage != null)
            {
                disabledImage.RenderState(graphics, State, WinBox);
            }
            else if (Selected && selectImage != null)
            {
                selectImage.RenderState(graphics, State, WinBox);
            }
            else if (renderImage != null)
            {
                renderImage.RenderState(graphics, State, WinBox);
            }
            else
            {
                base.RenderBack(graphics);
            }
        }

        public override void RenderFront(IGraphics graphics)
        {
            if (!Enabled && disabledImage == null)
            {
                base.RenderFront(graphics);
            }
        }

    }
}
