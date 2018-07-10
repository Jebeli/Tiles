using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Events
{
    public class EventLayer
    {
        private int width;
        private int height;
        private EventTile[] tiles;


        public EventLayer(int width, int height)
        {
            this.width = width;
            this.height = height;
            InitTiles();
        }

        public void AddEvent(Event evt, int x, int y)
        {
            this[x, y].AddEvent(evt);
        }

        public void RemoveEvent(Event evt, int x, int y)
        {
            this[x, y].RemoveEvent(evt);
        }

        public EventTile this[int index]
        {
            get { return tiles[index]; }
        }

        public EventTile this[int x, int y]
        {
            get { return tiles[XYToIndex(x, y)]; }
        }


        private int XYToIndex(int x, int y)
        {
            return y * width + x;
        }


        private void IndexToXY(int index, out int x, out int y)
        {
            y = index / width;
            x = index - y * width;
        }

        private void InitTiles()
        {
            tiles = new EventTile[width * height];
            for (int i = 0; i < tiles.Length; i++)
            {
                IndexToXY(i, out int x, out int y);
                tiles[i] = new EventTile(x, y, this);
            }
        }

    }
}
