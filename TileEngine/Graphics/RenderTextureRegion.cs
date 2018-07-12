using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;

namespace TileEngine.Graphics
{
    public class RenderTextureRegion : IComparable<RenderTextureRegion>
    {
        private TextureRegion region;
        private float mapX;
        private float mapY;
        private int screenX;
        private int screenY;
        private bool isAnimTile;
        private int id;
        private long prio;

        public RenderTextureRegion(int id, TextureRegion region, float mapX, float mapY, int screenX = 0, int screenY = 0, bool isAnimTile = false)
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
            internal set { screenX = value; }
        }

        public int ScreenY
        {
            get { return screenY; }
            internal set { screenY = value; }
        }

        public bool IsAnimTile
        {
            get { return isAnimTile; }
        }

        public long Prio
        {
            get { return prio; }
            set { prio = value; }
        }

        public int CompareTo(RenderTextureRegion other)
        {
            return prio.CompareTo(other.Prio);
        }

        public Rect GetDestRect(int x = 0, int y = 0)
        {
            return new Rect(x - region.OffsetX, y - region.OffsetY, region.Width, region.Height);
        }
    }
}
