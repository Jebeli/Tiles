using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.Nano
{
    public class CheckBox : Widget
    {
        protected string caption;
        protected bool isChecked;
        protected bool pushed;

        public CheckBox(Widget parent, string caption = "Untitled")
            : base(parent)
        {
            this.caption = caption;
        }

        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public bool Checked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public bool Pushed
        {
            get { return pushed; }
            set { pushed = value; }
        }

        public override bool MouseButtonEvent(Vector2 p, MouseButton button, bool down)
        {
            base.MouseButtonEvent(p, button, down);
            if (!enabled) return false;
            if (button == MouseButton.Left)
            {
                if (down)
                {
                    pushed = true;
                }
                else if (pushed)
                {
                    if (Contains(p))
                    {
                        isChecked = !isChecked;
                    }
                    pushed = false;
                }
                return true;
            }
            return false;
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            if (!fixedSize.IsEmpty) return fixedSize;
            int fs = HasFontSize ? FontSize : theme.ButtonFontSize;
            int tw = gfx.MeasureTextWidth(Font, caption);
            return new Vector2(tw + (int)(1.8f * fs), (int)(1.3f * fs));
        }

        public override void Draw(IGraphics gfx)
        {
            base.Draw(gfx);
            Color grad = pushed ? new Color(0, 0, 0, 100) : new Color(0, 0, 0, 32);
            Color gradBot = new Color(0, 0, 0, 32);

            gfx.FillRectangle(pos.X + 1, pos.Y + 1, size.Y - 2, size.Y - 2, grad, gradBot);

            if (isChecked)
            {
                Icons icon = theme.CheckBoxIcon;
                int iy = pos.Y + size.Y / 2 - 9;
                int ix = pos.X + size.Y / 2 - 9;
                gfx.RenderIcon((int)icon, ix, iy);
            }

            Color tc = theme.TextColor;
            if (!enabled) tc = theme.DisabledTextColor;
            gfx.RenderText(Font, caption, (int)(pos.X + size.Y + size.Y * 0.5f), (int)(pos.Y + size.Y * 0.5f), tc, HorizontalTextAlign.Left);

        }
    }
}
