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

namespace TileEngine.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

        public void AddHotSpotEvent(Event evt, int x, int y)
        {
            this[x, y].AddHotSpotEvent(evt);
        }

        public void RemoveHotSpotEvent(Event evt, int x, int y)
        {
            this[x, y].RemoveHotSpotEvent(evt);
        }

        public bool IsValidXY(int x, int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
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
