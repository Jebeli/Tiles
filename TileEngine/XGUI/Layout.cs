using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.XGUI
{
    public enum Alignment
    {
        Minimum = 0,
        Middle,
        Maximum,
        Fill
    };

    public enum Orientation
    {
        Horizontal = 0,
        Vertical
    };

    public enum BorderPlace
    {
        Center,
        North,
        South,
        West,
        East
    }
    public abstract class Layout
    {
        public abstract void PerformLayout(IGraphics gfx, Widget widget);
        public abstract Vector2 GetPreferredSize(IGraphics gfx, Widget widget);

    }

    public class BoxLayout : Layout
    {
        protected Orientation orientation;
        protected Alignment alignment;
        protected int margin;
        protected int spacing;

        public BoxLayout(Orientation orientation, Alignment alignment = Alignment.Middle, int margin = 0, int spacing = 0)
        {
            this.orientation = orientation;
            this.alignment = alignment;
            this.margin = margin;
            this.spacing = spacing;
        }

        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public Alignment Alignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        public int Margin
        {
            get { return margin; }
            set { margin = value; }
        }

        public int Spacing
        {
            get { return spacing; }
            set { spacing = value; }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx, Widget widget)
        {
            Vector2 size = new Vector2(2 * margin, 2 * margin);
            int xOffset = widget.BorderLeft + widget.BorderRight;
            int yOffset = widget.BorderTop + widget.BorderBottom;
            bool first = true;
            int axis1 = (int)orientation;
            int axis2 = (((int)orientation) + 1) % 2;
            foreach (var w in widget.Children)
            {
                if (!w.Visible) continue;
                if (first)
                    first = false;
                else
                    size[axis1] += spacing;

                Vector2 ps = w.GetPreferredSize(gfx);
                Vector2 fs = w.FixedSize;
                Vector2 targetSize = Widget.GetValidSize(ps, fs);// new Vector2(fs.X != 0 ? fs.X : ps.X, fs.Y != 0 ? fs.Y : ps.Y);
                size[axis1] += targetSize[axis1];
                size[axis2] = Math.Max(size[axis2], targetSize[axis2] + 2 * margin);
                first = false;
            }
            return size + new Vector2(xOffset, yOffset);
        }

        public override void PerformLayout(IGraphics gfx, Widget widget)
        {
            Vector2 fs_w = widget.FixedSize;
            Vector2 s_w = widget.Size;
            Vector2 containerSize = Widget.GetValidSize(s_w, fs_w);// new Vector2(fs_w.X != 0 ? fs_w.X : widget.Width, fs_w.Y != 0 ? fs_w.Y : widget.Height);
            containerSize.X -= widget.BorderLeft;
            containerSize.X -= widget.BorderRight;
            containerSize.Y -= widget.BorderTop;
            containerSize.Y -= widget.BorderBottom;
            int axis1 = (int)orientation;
            int axis2 = (((int)orientation) + 1) % 2;
            int position = margin;
            if (orientation == Orientation.Vertical)
            {
                position += widget.BorderTop;
            }
            else
            {
                position += widget.BorderLeft;
            }
            int xOffset = widget.BorderLeft;
            int yOffset = widget.BorderTop;
            bool first = true;
            foreach (var w in widget.Children)
            {
                if (!w.Visible) continue;
                if (first)
                    first = false;
                else
                    position += spacing;
                Vector2 ps = w.GetPreferredSize(gfx);
                Vector2 fs = w.FixedSize;
                Vector2 targetSize = Widget.GetValidSize(ps, fs);// new Vector2(fs.X != 0 ? fs.X : ps.X, fs.Y != 0 ? fs.Y : ps.Y);
                Vector2 pos = new Vector2(xOffset, yOffset);
                pos[axis1] = position;
                switch (alignment)
                {
                    case Alignment.Minimum:
                        pos[axis2] += margin;
                        break;
                    case Alignment.Middle:
                        pos[axis2] += (containerSize[axis2] - targetSize[axis2]) / 2;
                        break;
                    case Alignment.Maximum:
                        pos[axis2] += containerSize[axis2] - targetSize[axis2] - margin * 2;
                        break;
                    case Alignment.Fill:
                        pos[axis2] += margin;
                        targetSize[axis2] = fs[axis2] != 0 ? fs[axis2] : (containerSize[axis2] - margin * 2);
                        break;
                }
                w.Position = pos;
                w.Size = targetSize;
                w.PerformLayout(gfx);
                position += targetSize[axis1];
            }
        }
    }

    public class GroupLayout : Layout
    {
        protected int margin;
        protected int spacing;
        protected int groupSpacing;
        protected int groupIndent;

        public GroupLayout(int margin = 15, int spacing = 6, int groupSpacing = 14, int groupIndent = 20)
        {
            this.margin = margin;
            this.spacing = spacing;
            this.groupSpacing = groupSpacing;
            this.groupIndent = groupIndent;
        }

        public int Margin
        {
            get { return margin; }
            set { margin = value; }
        }

        public int Spacing
        {
            get { return spacing; }
            set { spacing = value; }
        }

        public int GroupSpacing
        {
            get { return groupSpacing; }
            set { groupSpacing = value; }
        }

        public int GroupIndent
        {
            get { return groupIndent; }
            set { groupIndent = value; }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx, Widget widget)
        {
            int height = margin + widget.BorderTop;
            int width = margin * 2 + widget.BorderLeft + widget.BorderRight;
            bool first = true;
            bool indent = false;
            foreach (var c in widget.Children)
            {
                if (!c.Visible) continue;
                Label label = c as Label;
                if (!first)
                    height += (label == null) ? spacing : groupSpacing;
                first = false;
                Vector2 ps = c.GetPreferredSize(gfx);
                Vector2 fs = c.FixedSize;
                Vector2 targetSize = Widget.GetValidSize(ps, fs);// new Vector2(fs.X != 0 ? fs.X : ps.X, fs.Y != 0 ? fs.Y : ps.Y);
                bool indentCur = indent & label == null;
                height += targetSize.Y;
                width = Math.Max(width, targetSize.X + 2 * margin + (indentCur ? groupIndent : 0));
                if (label != null)
                    indent = !string.IsNullOrEmpty(label.Text);

            }
            height += margin;
            height += widget.BorderBottom;
            return new Vector2(width, height);
        }

        public override void PerformLayout(IGraphics gfx, Widget widget)
        {
            int height = margin + widget.BorderTop;
            int availableWidth = (widget.FixedWidth != 0 ? widget.FixedWidth : widget.Width) - 2 * margin;
            availableWidth -= (widget.BorderLeft + widget.BorderRight);
            bool first = true;
            bool indent = false;
            foreach (var c in widget.Children)
            {
                if (!c.Visible) continue;
                Label label = c as Label;
                if (!first)
                    height += (label == null) ? spacing : groupSpacing;
                first = false;
                bool indentCur = indent & label == null;
                Vector2 ps = new Vector2(availableWidth - (indentCur ? groupIndent : 0), c.GetPreferredSize(gfx).Y);
                Vector2 fs = c.FixedSize;
                Vector2 targetSize = new Vector2(fs.X != 0 ? fs.X : ps.X, fs.Y != 0 ? fs.Y : ps.Y);
                c.Position = new Vector2(margin + (indentCur ? groupIndent : 0), height);
                c.Size = targetSize;
                c.PerformLayout(gfx);
                height += targetSize.Y;
                if (label != null)
                    indent = !string.IsNullOrEmpty(label.Text);
            }
        }
    }

    public class BorderLayout : Layout
    {
        private Widget centerWidget;
        private Widget northWidget;
        private Widget southWidget;
        private Widget westWidget;
        private Widget eastWidget;

        public void SetBorderPlace(Widget widget, BorderPlace place)
        {
            switch (place)
            {
                case BorderPlace.Center:
                    centerWidget = widget;
                    break;
                case BorderPlace.North:
                    northWidget = widget;
                    break;
                case BorderPlace.South:
                    southWidget = widget;
                    break;
                case BorderPlace.West:
                    westWidget = widget;
                    break;
                case BorderPlace.East:
                    eastWidget = widget;
                    break;
            }
        }
        public override Vector2 GetPreferredSize(IGraphics gfx, Widget widget)
        {
            Vector2 total = new Vector2();
            Vector2 north = new Vector2();
            Vector2 south = new Vector2();
            Vector2 west = new Vector2();
            Vector2 east = new Vector2();
            Vector2 center = new Vector2();
            if (northWidget != null) north = northWidget.GetPreferredSize(gfx);
            if (southWidget != null) south = southWidget.GetPreferredSize(gfx);
            if (westWidget != null) west = westWidget.GetPreferredSize(gfx);
            if (eastWidget != null) east = eastWidget.GetPreferredSize(gfx);
            if (centerWidget != null) center = centerWidget.GetPreferredSize(gfx);
            total += center;
            total.Y += north.Y + south.Y;
            total.X += west.X + east.X;

            return total;
        }

        public override void PerformLayout(IGraphics gfx, Widget widget)
        {

        }
    }
}
