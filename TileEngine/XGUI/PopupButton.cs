using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.XGUI
{
    public class PopupButton : Button
    {
        private Popup popup;
        private Icons chevronIcon;
        public PopupButton(Widget parent, string text = "", Icons icon = Icons.NONE)
            : base(parent, text, icon)
        {
            Flags = ButtonFlags.PopupButton | ButtonFlags.ToggleButton;
            Window parentWindow = Window;
            popup = new Popup(parentWindow.Parent, Window);
            popup.Size = new Vector2(320, 250);
            popup.Visible = false;
        }

        public PopupSide Side
        {
            get { return popup.Side; }
            set
            {
                popup.Side = value;
                if (popup.Side == PopupSide.Right)
                {
                    chevronIcon = Theme.PopupChevronRightIcon;
                }
                else
                {
                    chevronIcon = Theme.PopupChevronLeftIcon;
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

        public override void Render(IGraphics gfx)
        {
            if (!Enabled && Checked) Checked = false;
            popup.Visible = Checked;
            base.Render(gfx);
        }

        public override void PerformLayout(IGraphics gfx)
        {
            base.PerformLayout(gfx);
            Window parentWindow = Window;
            int posY = AbsolutePosition.Y - parentWindow.Position.Y + Size.Y / 2;
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
