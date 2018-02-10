using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace GDITiles
{
    public static class GDIExtensions
    {
        public static System.Drawing.Bitmap GetBitmap(this Texture tex)
        {
            if (tex != null)
            {
                return ((GDITexture)tex).Bitmap;
            }
            return null;
        }
    }
}
