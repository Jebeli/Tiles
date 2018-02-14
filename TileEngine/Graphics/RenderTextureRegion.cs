using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Graphics
{
    public class RenderTextureRegion
    {
        private TextureRegion region;
        private float mapX;
        private float mapY;
        private int screenX;
        private int screenY;

        public RenderTextureRegion(TextureRegion region, float mapX, float mapY, int screenX, int screenY)
        {
            this.region = region;
            this.mapX = mapX;
            this.mapY = mapY;
            this.screenX = screenX;
            this.screenY = screenY;
        }
        public TextureRegion TextureRegion
        {
            get { return region; }
        }
        public float MapX
        {
            get { return mapX; }
        }

        public float MapY
        {
            get { return mapY; }
        }

        public int ScreenX
        {
            get { return screenX; }
        }

        public int ScreenY
        {
            get { return screenY; }
        }
    }
}
