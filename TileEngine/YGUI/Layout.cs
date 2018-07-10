/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
 */

namespace TileEngine.YGUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;
    using TileEngine.Graphics;

    public enum Orientation
    {
        Horizontal = 0,
        Vertical
    };

    public enum Alignment
    {
        Minimum = 0,
        Middle,
        Maximum,
        Fill
    };

    public abstract class Layout
    {
        public abstract void PerformLayout(IGraphics gfx, Gadget gadget);
        public abstract Point GetPreferredSize(IGraphics gfx, Gadget gadget);

        protected internal static Point GetValidSize(Point pref, Point fix)
        {
            return new Point(fix.X != 0 ? fix.X : pref.X, fix.Y != 0 ? fix.Y : pref.Y);
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

        public override Point GetPreferredSize(IGraphics gfx, Gadget gadget)
        {
            Point size = new Point(2 * margin, 2 * margin);
            int xOffset = gadget.BorderLeft + gadget.BorderRight;
            int yOffset = gadget.BorderTop + gadget.BorderBottom;
            bool first = true;
            int axis1 = (int)orientation;
            int axis2 = (((int)orientation) + 1) % 2;
            foreach (var w in gadget.Children)
            {
                if (!w.Visible) continue;
                if (first)
                    first = false;
                else
                    size[axis1] += spacing;

                Point ps = w.GetPreferredSize(gfx);
                Point fs = w.FixedSize;
                Point targetSize = GetValidSize(ps, fs);// new Vector2(fs.X != 0 ? fs.X : ps.X, fs.Y != 0 ? fs.Y : ps.Y);
                size[axis1] += targetSize[axis1];
                size[axis2] = Math.Max(size[axis2], targetSize[axis2] + 2 * margin);
                first = false;
            }
            return size + new Point(xOffset, yOffset);
        }

        public override void PerformLayout(IGraphics gfx, Gadget gadget)
        {
            Point fs_w = gadget.FixedSize;
            Point s_w = gadget.Size;
            Point containerSize = GetValidSize(s_w, fs_w);// new Vector2(fs_w.X != 0 ? fs_w.X : widget.Width, fs_w.Y != 0 ? fs_w.Y : widget.Height);
            containerSize.X -= gadget.BorderLeft;
            containerSize.X -= gadget.BorderRight;
            containerSize.Y -= gadget.BorderTop;
            containerSize.Y -= gadget.BorderBottom;
            int axis1 = (int)orientation;
            int axis2 = (((int)orientation) + 1) % 2;
            int position = margin;
            if (orientation == Orientation.Vertical)
            {
                position += gadget.BorderTop;
            }
            else
            {
                position += gadget.BorderLeft;
            }
            int xOffset = gadget.BorderLeft;
            int yOffset = gadget.BorderTop;
            bool first = true;
            foreach (var w in gadget.Children)
            {
                if (!w.Visible) continue;
                if (first)
                    first = false;
                else
                    position += spacing;
                Point ps = w.GetPreferredSize(gfx);
                Point fs = w.FixedSize;
                Point targetSize = GetValidSize(ps, fs);// new Vector2(fs.X != 0 ? fs.X : ps.X, fs.Y != 0 ? fs.Y : ps.Y);
                Point pos = new Point(xOffset, yOffset);
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
}
