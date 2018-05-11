using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.YGUI
{
    public class ItemGadget : Gadget
    {
        private List<ItemGadget> items;

        public ItemGadget(Gadget parent, string label)
            : base(parent, label)
        {
            items = new List<ItemGadget>();
        }

        public List<ItemGadget> Items
        {
            get { return items; }
            set { items = value; }
        }

        protected override void HandleSelectMove(Vector2 p)
        {
            foreach (var oi in items)
            {
                oi.Selected = oi == this;
            }
            base.HandleSelectMove(p);
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

    }
}
