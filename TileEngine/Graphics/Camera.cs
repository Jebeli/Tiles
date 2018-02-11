﻿/*
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
    public class Camera
    {
        private int tileWidth;
        private int tileHeight;
        private int halfTileWidth;
        private int halfTileHeight;

        public Camera()
            : this(64, 64)
        {

        }
        public Camera(int tileWidth, int tileHeight)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            halfTileWidth = tileWidth / 2;
            halfTileHeight = tileHeight / 2;
        }

        public int TileWidth
        {
            get { return tileWidth; }
            set
            {
                tileWidth = value;
                halfTileWidth = value / 2;
            }
        }

        public int TileHeight
        {
            get { return tileHeight; }
            set
            {
                tileHeight = value;
                halfTileHeight = value;
            }
        }
        public void OrthoMapToScreen(float mapX, float mapY, out int screenX, out int screenY)
        {
            screenX = (int)(mapX * tileWidth);
            screenY = (int)(mapY * tileHeight);
        }

        public void OrthoScreenToMap(int screenX, int screenY, out float mapX, out float mapY)
        {
            mapX = ((float)screenX / tileWidth);
            mapY = ((float)screenY / tileHeight);
        }
        public void IsoMapToScreen(float mapX, float mapY, out int screenX, out int screenY)
        {
            screenX = (int)((mapX - mapY) * halfTileWidth);
            screenY = (int)((mapX + mapY) * halfTileHeight);
        }

        public void IsoScreenToMap(int screenX, int screenY, out float mapX, out float mapY)
        {
            mapX = ((float)screenX / tileWidth) + ((float)screenY / tileHeight);
            mapY = ((float)screenY / tileHeight) - ((float)screenX / tileWidth);
        }

    }
}
