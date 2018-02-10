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
    using TileEngine.Graphics;

    public class GDITexture : Texture
    {
        private System.Drawing.Bitmap bitmap;

        public GDITexture(string name, System.Drawing.Bitmap bitmap)
            : base(name)
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
