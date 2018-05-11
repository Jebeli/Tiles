using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.XGUI
{
    public class Label : Widget
    {
        string text;
        private Icons icon;
        public Label(Widget parent, string text = "", Icons icon = Icons.NONE)
            : base(parent)
        {
            this.text = text;
            this.icon = icon;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Icons Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            int iw = icon == Icons.NONE ? 0 : 25;
            if (string.IsNullOrEmpty(text)) return new Vector2(iw, iw);
            if (iw == 0) iw = 16;
            
            if (FixedWidth > 0)
            {
                return new Vector2(FixedWidth, iw);
            }
            else
            {
                return new Vector2(gfx.MeasureTextWidth(Font, text) + iw, iw);
            }
        }

        public override void Render(IGraphics gfx)
        {
            Theme.RenderLabel(gfx, this);
            base.Render(gfx);
        }
    }
}
