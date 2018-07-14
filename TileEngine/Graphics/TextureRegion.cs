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

namespace TileEngine.Graphics
{
    public class TextureRegion
    {
        private Texture texture;
        private int x;
        private int y;
        private int width;
        private int height;
        private int offsetX;
        private int offsetY;

        public TextureRegion(Texture texture)
        {
            this.texture = texture;
            x = 0;
            y = 0;
            width = texture.Width;
            height = texture.Height;
            offsetX = 0;
            offsetY = 0;
        }

        public TextureRegion(Texture texture, int x, int y, int width, int height, int offsetX, int offsetY)
        {
            this.texture = texture;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }

        public virtual void Draw(IGraphics graphics, int x, int y, int width, int height)
        {
            graphics.Render(this, x, y, width, height);
        }

        public void Draw(IGraphics graphics, int x, int y)
        {
            Draw(graphics, x, y, width, height);
        }

        public Texture Texture
        {
            get { return texture; }
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int OffsetX
        {
            get { return offsetX; }
        }

        public int OffsetY
        {
            get { return offsetY; }
        }
    }
}
