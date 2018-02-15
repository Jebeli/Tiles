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
    using Core;
    using Maps;
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
        private int clickTileX;
        private int clickTileY;
        private Map map;
        private MapOrientation orientation;

        public Camera(Map map, int posX = -1, int posY = -1)
        {
            this.map = map;
            orientation = map.Orientation;
            tileWidth = map.TileWidth;
            tileHeight = map.TileHeight;
            halfTileWidth = tileWidth / 2;
            halfTileHeight = tileHeight / 2;
            if (posX >= 0 && posY >= 0)
            {
                SetMapPosition(posX, posY);
            }
            else
            {
                SetMapPosition(map.Width / 2, map.Height / 2);
            }
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

        public int ClickTileX
        {
            get { return clickTileX; }
            set { clickTileX = value; }
        }

        public int ClickTileY
        {
            get { return clickTileY; }
            set { clickTileY = value; }
        }

        public void SetPosition(float posX, float posY)
        {
            cameraX = (int)posX;
            cameraY = (int)posY;
            map.InvalidateRenderLists();
        }

        public void SetMapPosition(float mapX, float mapY)
        {
            float screenX = 0;
            float screenY = 0;
            switch (orientation)
            {
                case MapOrientation.Orthogonal:
                    screenX = -mapX * tileWidth;
                    screenY = -mapY * tileHeight;
                    break;
                case MapOrientation.Isometric:
                    screenX = -((mapX - mapY) * halfTileWidth);
                    screenY = -((mapX + mapY) * halfTileHeight);
                    break;
            }
            SetPosition(screenX, screenY);
        }
        public void Shift(float dX, float dY)
        {
            SetPosition(cameraX - dX, cameraY - dY);
        }

        public void MapToScreen(float mapX, float mapY, out int screenX, out int screenY)
        {
            MapToScreen(mapX, mapY, out screenX, out screenY, orientation);
        }

        public void MapToScreen(float mapX, float mapY, out int screenX, out int screenY, MapOrientation orientation)
        {
            if (orientation == MapOrientation.Isometric)
            {
                IsoMapToScreen(mapX, mapY, out screenX, out screenY);
            }
            else
            {
                OrthoMapToScreen(mapX, mapY, out screenX, out screenY);
            }
        }

        public void OrthoMapToScreen(float mapX, float mapY, out int screenX, out int screenY)
        {
            screenX = (int)(mapX * tileWidth) + cameraX + halfViewWidth;
            screenY = (int)(mapY * tileHeight) + cameraY + halfViewHeight;
        }

        public void OrthoScreenToMap(float screenX, float screenY, out float mapX, out float mapY)
        {
            screenX -= (cameraX + halfViewWidth);
            screenY -= (cameraY + halfViewHeight);
            mapX = (screenX / tileWidth);
            mapY = (screenY / tileHeight);
        }
        public void IsoMapToScreen(float mapX, float mapY, out int screenX, out int screenY)
        {
            screenX = (int)((mapX - mapY) * halfTileWidth) + cameraX + halfViewWidth;
            screenY = (int)((mapX + mapY) * halfTileHeight) + cameraY + halfViewHeight;
        }
        public void ScreenToMap(float screenX, float screenY, out float mapX, out float mapY)
        {
            ScreenToMap(screenX, screenY, out mapX, out mapY, orientation);
        }
        public void ScreenToMap(float screenX, float screenY, out float mapX, out float mapY, MapOrientation orientation)
        {
            if (orientation == MapOrientation.Isometric)
            {
                IsoScreenToMap(screenX, screenY, out mapX, out mapY);
            }
            else
            {
                OrthoScreenToMap(screenX, screenY, out mapX, out mapY);
            }
        }

        public void IsoScreenToMap(float screenX, float screenY, out float mapX, out float mapY)
        {
            screenX -= (cameraX + halfViewWidth);
            screenY -= (cameraY + halfViewHeight);
            screenX -= halfTileWidth;
            mapX = (screenX / tileWidth) + (screenY / tileHeight);
            mapY = (screenY / tileHeight) - (screenX / tileWidth);
        }

        public void MapToTile(float mapX, float mapY, out int tileX, out int tileY)
        {
            tileX = (int)(Math.Floor(mapX));
            tileY = (int)(Math.Floor(mapY));
        }

    }
}
