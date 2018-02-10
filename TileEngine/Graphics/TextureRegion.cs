using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
