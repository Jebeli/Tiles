using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.XGUI
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
            ThinBorder = true;
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
            if (Layout != null || VisibleChildCount != 1)
            {
                base.PerformLayout(gfx);
            }
            else
            {
                var c = VisibleChildAt(0);
                c.Position = new Vector2(BorderLeft, BorderTop);
                c.Size = ClientSize;
                c.PerformLayout(gfx);
            }
            if (side == PopupSide.Left)
                anchorPos[0] -= Size[0];
        }

        internal protected override void RefreshRelativePlacement()
        {
            parentWindow.RefreshRelativePlacement();
            Visible &= parentWindow.VisibleRecursive;
            Position = parentWindow.Position + anchorPos - new Vector2(0, anchorHeight);
        }

        public override void Render(IGraphics gfx)
        {
            RefreshRelativePlacement();
            if (!Visible) return;
            Vector2 bv = Position + new Vector2(0, anchorHeight);
            int sign = -1;
            if (side == PopupSide.Left)
            {
                bv.X += Size.X;
                sign = 1;
            }

            for (int i = 15; i >= 0; i--)
            {
                gfx.FillRectangle(bv.X, bv.Y - i, 1, 2 * i, Theme.BackPen);
                bv.X += sign;
            }
            base.Render(gfx);
        }
    }
}
