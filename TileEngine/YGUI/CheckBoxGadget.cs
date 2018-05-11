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
    public class CheckBoxGadget : Gadget
    {
        private bool _checked;
        public CheckBoxGadget(Gadget parent, string label)
            : base(parent, label)
        {
            Size = new Vector2(64, 24);
        }

        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            int textWidth = gfx.MeasureTextWidth(Font, Label);
            return new Vector2(textWidth + 24 + 2 * 4, 24);            
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

        protected override void HandleSelectUp(Vector2 p)
        {
            _checked = !_checked;
            base.HandleSelectUp(p);
        }

        public override string ToString()
        {
            return $"CheckBox {Label}";
        }
    }
}
