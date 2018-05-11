using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.Nano
{
    public class PopupButton : Button
    {
        protected Icons chevronIcon;
        protected Popup popup;
        public PopupButton(Widget parent, string caption = "", Icons buttonIcon = Icons.NONE)
            : base(parent, caption, buttonIcon)
        {
            chevronIcon = theme.PopupChevronRightIcon;
            Flags = ButtonFlags.PopupButton | ButtonFlags.ToggleButton;
            Window parentWindow = Window;
            popup = new Popup(parentWindow.Parent, Window);
            popup.Size = new Vector2(320, 250);
            popup.Visible = false;
        }

        public Icons ChevronIcon
        {
            get { return chevronIcon; }
            set { chevronIcon = value; }
        }

        public PopupSide Side
        {
            get { return popup.Side; }
            set
            {
                popup.Side = value;
                if (popup.Side == PopupSide.Right)
                {
                    chevronIcon = theme.PopupChevronRightIcon;
                }
                else
                {
                    chevronIcon = theme.PopupChevronLeftIcon;
                }
            }
        }

        public Popup Popup
        {
            get { return popup; }
            set { popup = value; }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            return base.GetPreferredSize(gfx) + new Vector2(15, 0);
        }

        public override void Draw(IGraphics gfx)
        {
            if (!enabled && pushed) pushed = false;
            popup.Visible = pushed;
            base.Draw(gfx);
            if (chevronIcon != Icons.NONE)
            {
                int iw = 18;
                Vector2 iconPos = new Vector2(0, pos.Y + size.Y / 2 - 1 - 8);
                if (popup.Side == PopupSide.Right)
                {
                    iconPos[0] = pos.X + size.X - iw - 8;
                }
                else
                {
                    iconPos[0] = pos.X + 8;
                }
                gfx.RenderIcon((int)chevronIcon, iconPos.X, iconPos.Y);
            }
        }

        public override void PerformLayout(IGraphics gfx)
        {
            base.PerformLayout(gfx);
            Window parentWindow = Window;
            int posY = AbsolutePosition.Y - parentWindow.Position.Y + size.Y / 2;
            if (popup.Side == PopupSide.Right)
            {
                popup.AnchorPos = new Vector2(parentWindow.Width + 15, posY);
            }
            else
            {
                popup.AnchorPos = new Vector2(0 - 15, posY);
            }
        }
    }
}
