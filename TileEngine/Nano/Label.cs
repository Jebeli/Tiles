using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.Nano
{
    public class Label : Widget
    {
        protected string caption;
        protected Color color;
        protected Icons icon;

        public Label(Widget parent, string caption, Icons icon = Icons.NONE, int fontSize = -1)
            : base(parent)
        {
            this.caption = caption;
            this.icon = icon;
            if (theme != null)
            {
                this.fontSize = theme.StandardFontSize;
                color = theme.TextColor;
            }
            if (fontSize >= 0) this.fontSize = fontSize;
        }

        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public Icons Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public override Theme Theme
        {
            get { return base.Theme; }
            set
            {
                base.Theme = value;
                if (value != null)
                {
                    fontSize = value.StandardFontSize;
                    color = value.TextColor;
                }
            }
        }


        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            if (string.IsNullOrEmpty(caption)) return new Vector2(0, 0);
            if (fixedSize.X > 0)
            {
                return new Vector2(fixedSize.X, fontSize);
            }
            else
            {
                return new Vector2(gfx.MeasureTextWidth(Font, caption), fontSize);
            }
        }

        public override void Draw(IGraphics gfx)
        {
            base.Draw(gfx);
            if (fixedSize.X > 0)
            {
                gfx.RenderText(Font, caption, pos.X + size.X / 2, pos.Y + size.Y / 2, color);
            }
            else
            {
                if (icon != Icons.NONE)
                {
                    gfx.RenderIcon((int)icon, pos.X + size.X / 2 - 9, pos.Y + size.Y / 2 - 9);
                }
                if (!string.IsNullOrEmpty(caption))
                {
                    gfx.RenderText(Font, caption, pos.X + size.X / 2, pos.Y + size.Y / 2, color);
                }
            }
        }
    }
}
