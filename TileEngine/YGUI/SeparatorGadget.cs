using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.YGUI
{
    public class SeparatorGadget : Gadget
    {
        private Orientation orientation;
        private int padding;

        public SeparatorGadget(Gadget parent, Orientation orientation = Orientation.Vertical)
            : base(parent)
        {
            padding = 8;
            this.orientation = orientation;
        }

        public int Padding
        {
            get { return padding; }
            set { padding = value; }
        }

        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            switch (orientation)
            {
                case Orientation.Horizontal:
                    return new Vector2(1, padding);
                default:
                    return new Vector2(padding, 1);
            }
        }


        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }
    }
}
