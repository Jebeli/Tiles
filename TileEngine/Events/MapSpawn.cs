using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Events
{
    public class MapSpawn
    {
        private string type;
        private int mapX;
        private int mapY;

        public string Type
        {
            get { return type; }
            set { type = value; }
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
    }
}
