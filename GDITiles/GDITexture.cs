using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace GDITiles
{
    public class GDITexture : Texture
    {
        private System.Drawing.Bitmap bitmap;

        public GDITexture(System.Drawing.Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public System.Drawing.Bitmap Bitmap
        {
            get { return bitmap; }
        }

        public override int Width
        {
            get
            {
                return bitmap.Width;
            }
        }
        public override int Height
        {
            get
            {
                return bitmap.Height;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bitmap.Dispose();
                bitmap = null;
            }
        }
    }
}
