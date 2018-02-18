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

namespace TileEngine.Maps
{
    public class Tile
    {
        private int x;
        private int y;
        private Layer layer;
        private int tileId;
        internal Tile(Layer layer, int x, int y)
        {
            this.layer = layer;
            this.x = x;
            this.y = y;
            tileId = -1;
        }

        public Layer Layer
        {
            get { return layer; }
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public int TileId
        {
            get { return tileId; }
            set { tileId = value; }
        }

        public override string ToString()
        {
            return $"{tileId}";
        }
    }
}
