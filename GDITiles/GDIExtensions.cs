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

        public static TileEngine.Input.MouseButton GetMouseButton(this System.Windows.Forms.MouseButtons mb)
        {
            if ((mb & System.Windows.Forms.MouseButtons.Left) == System.Windows.Forms.MouseButtons.Left) return TileEngine.Input.MouseButton.Left;
            if ((mb & System.Windows.Forms.MouseButtons.Right) == System.Windows.Forms.MouseButtons.Right) return TileEngine.Input.MouseButton.Right;
            if ((mb & System.Windows.Forms.MouseButtons.Middle) == System.Windows.Forms.MouseButtons.Middle) return TileEngine.Input.MouseButton.Middle;
            return TileEngine.Input.MouseButton.None;
        }
    }
}
