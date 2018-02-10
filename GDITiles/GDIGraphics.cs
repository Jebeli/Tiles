using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace GDITiles
{
    public class GDIGraphics : AbstractGraphics
    {
        private System.Drawing.Imaging.PixelFormat pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;

        public override Texture CreateTexture(int width, int height)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height, pixelFormat);
            return new GDITexture(bmp);
        }

        public override Texture GetTexture(string textureId)
        {
            return LoadTexture(textureId);
        }

        private GDITexture LoadTexture(string fileName)
        {
            System.Drawing.Bitmap bmp = System.Drawing.Image.FromFile(fileName) as System.Drawing.Bitmap;
            if (bmp != null)
            {
                return new GDITexture(bmp);
            }
            return null;
        }
    }
}
