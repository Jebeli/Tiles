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

namespace GDITiles
{
    using System;
    using TileEngine.Files;
    using TileEngine.Graphics;
    using TileEngine.Logging;

    public class GDIGraphics : AbstractGraphics
    {
        private System.Drawing.Imaging.PixelFormat pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
        private System.Drawing.Bitmap view;
        private System.Drawing.Graphics gfx;
        private System.Drawing.Pen gridPen;
        private System.Drawing.Pen selectPen;


        public GDIGraphics(int width, int height)
            : base(width, height)
        {
            InitPens();
        }

        public void RenderTo(System.Drawing.Graphics graphics, System.Drawing.Rectangle dst)
        {
            System.Drawing.Rectangle src = new System.Drawing.Rectangle(0, 0, view.Width, view.Height);
            graphics.DrawImage(view, dst, src, System.Drawing.GraphicsUnit.Pixel);
        }

        public override void ClearScreen()
        {
            if (CheckInFrame())
            {
                gfx.Clear(System.Drawing.Color.Black);
            }
        }

        public override void DrawTileGrid(int x, int y, int width, int height)
        {
            DrawTile(x, y, width, height, gridPen);
        }

        public override void DrawTileSelected(int x, int y, int width, int height)
        {
            DrawTile(x, y, width, height, selectPen);
        }
        public override Texture CreateTexture(string textureId, int width, int height)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height, pixelFormat);
            return new GDITexture(textureId, bmp);
        }

        public override Texture GetTexture(string textureId, IFileResolver fileResolver)
        {
            string fileName = fileResolver.Resolve(textureId);
            return LoadTexture(textureId, fileName);
        }

        private GDITexture LoadTexture(string textureId, string fileName)
        {
            if (fileName != null)
            {
                System.Drawing.Bitmap bmp = System.Drawing.Image.FromFile(fileName) as System.Drawing.Bitmap;
                if (bmp != null)
                {
                    return new GDITexture(textureId, bmp);
                }
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && gfx != null)
            {
                gfx.Dispose();
            }
            if (disposing && view != null)
            {
                view.Dispose();
            }
            if (disposing)
            {
                gridPen.Dispose();
                selectPen.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override bool NeedsInitGraphics
        {
            get { return gfx == null || view == null || view.Width != viewWidth || view.Height != viewHeight; }
        }
        protected override void InitGraphics()
        {
            Logger.Info("Gfx", $"Init Graphics {viewWidth} x {viewHeight} scale = {viewScale}");
            gfx?.Dispose();
            view?.Dispose();
            view = new System.Drawing.Bitmap(viewWidth, viewHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            gfx = System.Drawing.Graphics.FromImage(view);
            InitGraphics(gfx);
        }

        private static void InitGraphics(System.Drawing.Graphics g)
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
        }

        private void InitPens()
        {
            gridPen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(96, System.Drawing.Color.Wheat));
            selectPen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(96, System.Drawing.Color.Gold));
        }
        private void DrawTile(int x, int y, int width, int height, System.Drawing.Pen pen)
        {
            float x1 = x;
            float x2 = x + width / 2;
            float x3 = x + width;
            float y1 = y;
            float y2 = y + height / 2;
            float y3 = y + height;
            System.Drawing.PointF[] poly = new System.Drawing.PointF[5];
            poly[0].X = x1;
            poly[0].Y = y2;
            poly[1].X = x2;
            poly[1].Y = y1;
            poly[2].X = x3;
            poly[2].Y = y2;
            poly[3].X = x2;
            poly[3].Y = y3;
            poly[4].X = x1;
            poly[4].Y = y2;
            gfx.DrawPolygon(pen, poly);
        }
    }
}
