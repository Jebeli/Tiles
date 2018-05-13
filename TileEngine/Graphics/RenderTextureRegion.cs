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
        private bool isAnimTile;
        private int id;

        public RenderTextureRegion(int id, TextureRegion region, float mapX, float mapY, int screenX, int screenY, bool isAnimTile = false)
        {
            this.id = id;
            this.region = region;
            this.mapX = mapX;
            this.mapY = mapY;
            this.screenX = screenX;
            this.screenY = screenY;
            this.isAnimTile = isAnimTile;
        }

        public TextureRegion TextureRegion
        {
            get { return region; }
            set { region = value; }
        }

        public int Id
        {
            get { return id; }
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

        public bool IsAnimTile
        {
            get { return isAnimTile; }
        }

    }
}
