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

namespace TileEngine.Graphics
{
    using System;
    public class Camera
    {
        private int tileWidth;
        private int tileHeight;
        private int halfTileWidth;
        private int halfTileHeight;
        private int viewWidth;
        private int viewHeight;
        private int halfViewWidth;
        private int halfViewHeight;
        private int cameraX;
        private int cameraY;
        private int hoverTileX;
        private int hoverTileY;

        public Camera()
            : this(64, 32)
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

        public int ViewWidth
        {
            get { return viewWidth; }
            set
            {
                viewWidth = value;
                halfViewWidth = value / 2;
            }
        }

        public int ViewHeight
        {
            get { return viewHeight; }
            set
            {
                viewHeight = value;
                halfViewHeight = value / 2;
            }
        }

        public int CameraX
        {
            get { return cameraX; }
            set { cameraX = value; }
        }

        public int CameraY
        {
            get { return cameraY; }
            set { cameraY = value; }
        }

        public int HoverTileX
        {
            get { return hoverTileX; }
            set { hoverTileX = value; }
        }

        public int HoverTileY
        {
            get { return hoverTileY; }
            set { hoverTileY = value; }
        }
        public void SetPosition(float posX, float posY)
        {
            cameraX = (int)posX;
            cameraY = (int)posY;
        }
        public void Shift(float dX, float dY)
        {
            SetPosition(cameraX - dX, cameraY - dY);
        }
        public void OrthoMapToScreen(float mapX, float mapY, out int screenX, out int screenY)
        {
            screenX = (int)(mapX * tileWidth);
            screenY = (int)(mapY * tileHeight);
        }

        public void OrthoScreenToMap(float screenX, float screenY, out float mapX, out float mapY)
        {
            mapX = (screenX / tileWidth);
            mapY = (screenY / tileHeight);
        }
        public void IsoMapToScreen(float mapX, float mapY, out int screenX, out int screenY)
        {
            screenX = (int)((mapX - mapY) * halfTileWidth) + cameraX + halfViewWidth;
            screenY = (int)((mapX + mapY) * halfTileHeight) + cameraY + halfViewHeight;
        }

        public void IsoScreenToMap(float screenX, float screenY, out float mapX, out float mapY)
        {
            screenX -= (cameraX + halfViewWidth);
            screenY -= (cameraY + halfViewHeight);
            screenX -= halfTileWidth;
            mapX = (screenX / tileWidth) + (screenY / tileHeight);
            mapY = (screenY / tileHeight) - (screenX / tileWidth);
        }

        public void IsoMapToTile(float mapX, float mapY, out int tileX, out int tileY)
        {
            tileX = (int)(Math.Floor(mapX));
            tileY = (int)(Math.Floor(mapY));
        }

    }
}
