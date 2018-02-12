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

namespace MONOTiles
{
    using TileEngine.Graphics;

    public class MONOTexture : Texture
    {
        private Microsoft.Xna.Framework.Graphics.Texture2D texture;
        public MONOTexture(string name, Microsoft.Xna.Framework.Graphics.Texture2D texture)
            : base(name)
        {
            this.texture = texture;
        }

        public override int Width
        {
            get { return texture.Width; }
        }

        public override int Height
        {
            get { return texture.Height; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                texture.Dispose();
            }
        }

        public Microsoft.Xna.Framework.Graphics.Texture2D Texture
        {
            get { return texture; }
        }
    }
}
