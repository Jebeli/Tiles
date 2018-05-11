using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;
using TileEngine.Logging;

namespace TileEngine.Nano
{
    public class ImagePanel : Widget
    {
        protected int thumbSize;
        protected int spacing;
        protected int marging;
        protected int mouseIndex;
        protected List<TextureRegion> images;

        public ImagePanel(Widget parent)
            : base(parent)
        {
            thumbSize = 64;
            spacing = 10;
            marging = 10;
            mouseIndex = -1;
            images = new List<TextureRegion>();
        }

        public int ThumbSize
        {
            get { return thumbSize; }
            set { thumbSize = value; }
        }

        public int Spacing
        {
            get { return spacing; }
            set { spacing = value; }
        }

        public int Margin
        {
            get { return marging; }
            set { marging = value; }
        }

        public int MouseIndex
        {
            get { return mouseIndex; }
        }

        public IList<TextureRegion> Images
        {
            get { return images; }
            set
            {
                images.Clear();
                if (value != null)
                {
                    images.AddRange(value);
                }
            }
        }

        protected Vector2 GridSize
        {
            get
            {
                int cols = 1 + Math.Max(0, (int)((size.X - 2 * marging - thumbSize) / (float)(thumbSize + spacing)));
                int rows = (images.Count + cols - 1) / cols;
                return new Vector2(cols, rows);
            }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            Vector2 grid = GridSize;
            return new Vector2(grid.X * thumbSize + (grid.X - 1) * spacing + 2 * marging,
                grid.Y * thumbSize + (grid.Y - 1) * spacing + 2 * marging);
        }

        public override void Draw(IGraphics gfx)
        {
            Vector2 grid = GridSize;
            for (int i = 0; i < images.Count; i++)
            {
                Vector2 p = pos + new Vector2(marging, marging) +
                    new Vector2(i % grid.X, i / grid.X) * (thumbSize + spacing);
                var img = images[i];
                int imgw = img.Width;
                int imgh = img.Height;
                int iw = Math.Min(thumbSize, imgw);
                int ih = Math.Min(thumbSize, imgh);
                int ix = (thumbSize / 2 - iw / 2);
                int iy = (thumbSize / 2 - ih / 2);
                gfx.Render(img, p.X + ix - img.OffsetX, p.Y + iy - img.OffsetY, iw, ih);
                if (i == mouseIndex)
                {
                    gfx.DrawRectangle(p.X, p.Y, thumbSize, thumbSize, theme.BorderLight);
                }
                else
                {
                    gfx.DrawRectangle(p.X, p.Y, thumbSize, thumbSize, theme.BorderDark);
                }

            }
        }

        public override bool MouseMotionEvent(Vector2 p, Vector2 rel, MouseButton button)
        {
            mouseIndex = IndexForPosition(p);
            return true;
        }

        protected int IndexForPosition(Vector2 p)
        {
            float ppX = p.X;
            float ppY = p.Y;
            ppX -= pos.X;
            ppY -= pos.Y;
            ppX -= marging;
            ppY -= marging;
            ppX /= (thumbSize + spacing);
            ppY /= (thumbSize + spacing);
            float iconRegion = thumbSize / (float)(thumbSize + spacing);
            bool overImage = ((ppX - Math.Floor(ppX)) < iconRegion) && ((ppY - Math.Floor(ppY)) < iconRegion);
            if (overImage)
            {
                Vector2 gridPos = new Vector2((int)ppX, (int)ppY);
                Vector2 grid = GridSize;
                overImage &= ((gridPos.X >= 0) && (gridPos.Y >= 0) && (gridPos.X < grid.X) && (gridPos.Y < grid.Y));
                return overImage ? gridPos.X + gridPos.Y * grid.X : -1;
            }
            else
            {
                return -1;
            }
        }

    }
}
