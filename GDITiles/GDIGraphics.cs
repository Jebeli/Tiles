using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Files;
using TileEngine.Graphics;
using TileEngine.Logging;

namespace GDITiles
{
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
