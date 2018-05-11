using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.Nano
{

    public enum PopupSide
    {
        Left = 0,
        Right
    }

    public class Popup : Window
    {
        protected Window parentWindow;
        protected Vector2 anchorPos;
        protected int anchorHeight;
        protected PopupSide side;
        public Popup(Widget parent, Window parentWindow)
            : base(parent)
        {
            this.parentWindow = parentWindow;
            anchorHeight = 30;
            side = PopupSide.Right;
        }

        public Vector2 AnchorPos
        {
            get { return anchorPos; }
            set { anchorPos = value; }
        }

        public int AnchorHeight
        {
            get { return anchorHeight; }
            set { anchorHeight = value; }
        }

        public PopupSide Side
        {
            get { return side; }
            set { side = value; }
        }

        public Window ParentWindow
        {
            get { return parentWindow; }
            set { parentWindow = value; }
        }

        public override void PerformLayout(IGraphics gfx)
        {
            if (layout != null || children.Count != 1)
            {
                base.PerformLayout(gfx);
            }
            else
            {
                children[0].Position = new Vector2(0, 0);
                children[0].Size = size;
                children[0].PerformLayout(gfx);
            }
            if (side == PopupSide.Left)
                anchorPos[0] -= size[0];
        }

        internal protected override void RefreshRelativePlacement()
        {
            parentWindow.RefreshRelativePlacement();
            visible &= parentWindow.VisibleRecursive;
            pos = parentWindow.Position + anchorPos - new Vector2(0, anchorHeight);
        }

        public override void Draw(IGraphics gfx)
        {
            RefreshRelativePlacement();
            if (!visible) return;
            Vector2 bv = pos + new Vector2(0, anchorHeight);
            int sign = -1;
            if (side == PopupSide.Left)
            {
                bv.X += size.X;
                sign = 1;
            }
            
            for (int i = 15; i >= 0; i--)
            {
                gfx.FillRectangle(bv.X, bv.Y - i, 1, 2*i, theme.WindowPopup);
                bv.X += sign;
            }
            //gfx.DrawLine(bv.X + 15 * sign, bv.Y, bv.X - 1 * sign, bv.Y - 15, theme.WindowPopup);
            //gfx.DrawLine(bv.X - 1 * sign, bv.Y - 15, bv.X - 1 * sign, bv.Y + 15, theme.WindowPopup);
            //gfx.DrawLine(bv.X - 1 * sign, bv.Y + 15, bv.X + 15 * sign, bv.Y, theme.WindowPopup);
            base.Draw(gfx);
        }
    }
}
