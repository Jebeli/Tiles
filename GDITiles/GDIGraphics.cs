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
    using TileEngine.Files;
    using TileEngine.Graphics;
    using TileEngine.Logging;

    public class GDIGraphics : AbstractGraphics
    {
        private System.Drawing.Imaging.PixelFormat pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
        private System.Drawing.Bitmap view;
        private System.Drawing.Graphics gfx;

        public GDIGraphics(int width, int height)
            : base(width, height)
        {

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
    }
}
