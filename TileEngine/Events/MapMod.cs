using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Events
{
    public class MapMod
    {
        private string layer;
        private int mapX;
        private int mapY;
        private int value;

        public MapMod()
        {
        }

        public string Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        public int MapX
        {
            get { return mapX; }
            set { mapX = value; }
        }

        public int MapY
        {
            get { return mapY; }
            set { mapY = value; }
        }

        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
