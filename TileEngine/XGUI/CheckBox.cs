using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.XGUI
{
    public class CheckBox : Button
    {
        public CheckBox(Widget parent, string text = "")
            : base(parent, text)
        {
            Flags = ButtonFlags.ToggleButton;
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            int tw = gfx.MeasureTextWidth(Font, Text);
            return new Vector2(25 + tw, 20);
        }
        public override void Render(IGraphics gfx)
        {
            Theme.RenderCheckBox(gfx, this);            
        }


    }
}
