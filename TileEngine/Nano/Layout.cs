using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.Nano
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

    public abstract class Layout
    {
        public abstract void PerformLayout(IGraphics gfx, Widget widget);
        public abstract Vector2 GetPreferredSize(IGraphics gfx, Widget widget);
        protected int GetWindowHeaderHeight(Widget widget)
        {
            Window window = widget as Window;
            if (window != null) return window.HeaderHeight;
            return 0;
        }
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
            int yOffset = 0;
            int hh = GetWindowHeaderHeight(widget);
            if (hh > 0)
            {
                if (orientation == Orientation.Vertical)
                    size[1] += hh - margin / 2;
                else
                    yOffset = hh;
            }
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
                Vector2 targetSize = new Vector2(fs.X != 0 ? fs.X : ps.X, fs.Y != 0 ? fs.Y : ps.Y);
                size[axis1] += targetSize[axis1];
                size[axis2] = Math.Max(size[axis2], targetSize[axis2] + 2 * margin);
                first = false;
            }
            return size + new Vector2(0, yOffset);
        }

        public override void PerformLayout(IGraphics gfx, Widget widget)
        {
            Vector2 fs_w = widget.FixedSize;
            Vector2 containerSize = new Vector2(fs_w.X != 0 ? fs_w.X : widget.Width, fs_w.Y != 0 ? fs_w.Y : widget.Height);
            int axis1 = (int)orientation;
            int axis2 = (((int)orientation) + 1) % 2;
            int position = margin;
            int yOffset = 0;
            int hh = GetWindowHeaderHeight(widget);
            if (hh > 0)
            {
                if (orientation == Orientation.Vertical)
                    position += hh - margin / 2;
                else
                {
                    yOffset = hh;
                    containerSize[1] -= yOffset;
                }
            }

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
                Vector2 targetSize = new Vector2(fs.X != 0 ? fs.X : ps.X, fs.Y != 0 ? fs.Y : ps.Y);
                Vector2 pos = new Vector2(0, yOffset);
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
            int height = margin;
            int width = margin * 2;
            int hh = GetWindowHeaderHeight(widget);
            if (hh > 0) height += hh - margin / 2;
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
                Vector2 targetSize = new Vector2(fs.X != 0 ? fs.X : ps.X, fs.Y != 0 ? fs.Y : ps.Y);
                bool indentCur = indent & label == null;
                height += targetSize.Y;
                width = Math.Max(width, targetSize.X + 2 * margin + (indentCur ? groupIndent : 0));
                if (label != null)
                    indent = !string.IsNullOrEmpty(label.Caption);

            }
            height += margin;
            return new Vector2(width, height);
        }

        public override void PerformLayout(IGraphics gfx, Widget widget)
        {
            int height = margin;
            int availableWidth = (widget.FixedWidth != 0 ? widget.FixedWidth : widget.Width) - 2 * margin;
            int hh = GetWindowHeaderHeight(widget);
            if (hh > 0) height += hh - margin / 2;
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
                    indent = !string.IsNullOrEmpty(label.Caption);

            }

        }
    }
}
