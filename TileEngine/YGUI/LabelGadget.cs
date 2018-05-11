using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.YGUI
{
    public class LabelGadget : Gadget
    {
        private bool frame;

        public LabelGadget(Gadget parent, string label)
            : base(parent, label)
        {
            TransparentBackground = true;
        }

        public bool Frame
        {
            get { return frame; }
            set { frame = value; }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            int textWidth = gfx.MeasureTextWidth(Font, Label);
            return new Vector2(textWidth + 4, 24);
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

        public override string ToString()
        {
            return $"Label {Label}";
        }
    }
}
