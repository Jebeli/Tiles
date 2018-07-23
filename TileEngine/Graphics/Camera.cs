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
        private Engine engine;
        private int tileWidth;
        private int tileHeight;
        private int halfTileWidth;
        private int halfTileHeight;
        private int viewWidth;
        private int viewHeight;
        private int halfViewWidth;
        private int halfViewHeight;
        private float unitsPerPixelX;
        private float unitsPerPixelY;
        private float shakyCamX;
        private float shakyCamY;
        private float cameraX;
        private float cameraY;
        private int hoverTileX;
        private int hoverTileY;
        private int clickTileX;
        private int clickTileY;
        private bool mapClicked;
        private bool mapClickDone;
        private int selectedTileX;
        private int selectedTileY;
        private float cameraSpeed;
        private bool immediateCamera;
        private Map map;
        private MapOrientation orientation;
        private int shakyCamTicks;

        public Camera(Engine engine, Map map, int posX = -1, int posY = -1)
        {
            this.engine = engine;
            this.map = map;
            cameraSpeed = 10.0f;
            immediateCamera = false;
            selectedTileX = -1;
            selectedTileY = -1;
            orientation = map.Orientation;
            tileWidth = map.TileWidth;
            tileHeight = map.TileHeight;
            halfTileWidth = tileWidth / 2;
            halfTileHeight = tileHeight / 2;
            if (orientation == MapOrientation.Isometric)
            {
                unitsPerPixelX = 2.0f / tileWidth;
                unitsPerPixelY = 2.0f / tileHeight;
            }
            else
            {
                unitsPerPixelX = 1.0f / tileWidth;
                unitsPerPixelY = 1.0f / tileHeight;
            }
            viewWidth = engine.Graphics.ViewWidth;
            halfViewWidth = viewWidth / 2;
            viewHeight = engine.Graphics.ViewHeight;
            halfViewHeight = viewHeight / 2;
            if (posX >= 0 && posY >= 0)
            {
                SetPosition(posX, posY);
            }
            else if (map.StartX >= 0 && map.StartY >= 0)
            {
                SetPosition(map.StartX, map.StartY);
            }
            else
            {
                SetPosition(map.Width / 2.0f, map.Height / 2.0f);
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

        public float CameraX
        {
            get { return cameraX; }
            set { cameraX = value; }
        }

        public float CameraY
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

        public int SelectedTileX
        {
            get { return selectedTileX; }
            set { selectedTileX = value; }
        }

        public int SelectedTileY
        {
            get { return selectedTileY; }
            set { selectedTileY = value; }
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

        public bool MapClicked
        {
            get { return mapClicked; }
            set
            {
                mapClicked = value;
                if (value) mapClickDone = false;
            }
        }

        public bool MapClickDone
        {
            get { return mapClickDone; }
            set { mapClickDone = value; }
        }

        public int ShakyCamTicks
        {
            get { return shakyCamTicks; }
            set { shakyCamTicks = value; }
        }

        public void Update()
        {
            if (shakyCamTicks > 0) shakyCamTicks--;
            if (shakyCamTicks > 0)
            {
                shakyCamX = cameraX + (Utils.Rand() % 16 - 8) * 0.0078125f;
                shakyCamY = cameraY + (Utils.Rand() % 16 - 8) * 0.0078125f;
            }
            else
            {
                shakyCamX = cameraX;
                shakyCamY = cameraY;
            }
            if (mapClickDone)
            {
                mapClicked = false;
            }
        }

        public void SetPosition(float posX, float posY)
        {
            if (cameraX != posX || cameraY != posY)
            {
                cameraX = posX;
                cameraY = posY;
                Utils.CleanFloat(ref cameraX);
                Utils.CleanFloat(ref cameraY);
                Update();
            }
        }

        public void SetMapPosition(float mapX, float mapY)
        {
            if (immediateCamera)
            {
                SetPosition(mapX, mapY);
            }
            else
            {
                float camDx = Utils.CalcDist(cameraX, mapX, mapX, mapY) / cameraSpeed;
                float camDy = Utils.CalcDist(mapX, cameraY, mapX, mapY) / cameraSpeed;
                float newX = cameraX;
                float newY = cameraY;
                if (newX < mapX)
                {
                    newX += camDx;
                    if (newX > mapX) newX = mapX;
                }
                else if (newX > mapX)
                {
                    newX -= camDx;
                    if (newX < mapX) newX = mapX;
                }
                if (newY < mapY)
                {
                    newY += camDy;
                    if (newY > mapY) newY = mapY;
                }
                else if (newY > mapY)
                {
                    newY -= camDy;
                    if (newY < mapY) newY = mapY;
                }
                SetPosition(newX, newY);
            }
        }

        public void Shift(float dX, float dY)
        {
            ScreenToMap(halfViewWidth + dX, halfViewHeight + dY, out float mX, out float mY);
            SetPosition(mX, mY);
        }

        public Point CenterTile(Point p)
        {
            if (orientation == MapOrientation.Orthogonal)
            {
                return new Point(p.X + halfTileWidth, p.Y + halfTileHeight);
            }
            else
            {
                return new Point(p.X, p.Y + halfTileHeight);
            }
        }

        public Point CenterTile(int x, int y)
        {
            if (orientation == MapOrientation.Orthogonal)
            {
                return new Point(x + halfTileWidth, y + halfTileHeight);
            }
            else
            {
                return new Point(x, y + halfTileHeight);
            }
        }

        public Point GetUpperLeft()
        {
            ScreenToMap(0, 0, out float mX, out float mY);
            return new Point((int)mX, (int)mY);
        }

        public void MapToScreen(float mapX, float mapY, out int screenX, out int screenY)
        {
            MapToScreen(mapX, mapY, out screenX, out screenY, orientation);
        }

        public void MapToScreen(float mapX, float mapY, float camX, float camY, out int screenX, out int screenY)
        {
            MapToScreen(mapX, mapY, camX, camY, out screenX, out screenY, orientation);
        }


        public void MapToScreen(float mapX, float mapY, float camX, float camY, out int screenX, out int screenY, MapOrientation orientation)
        {
            if (orientation == MapOrientation.Isometric)
            {
                IsoMapToScreen(mapX, mapY, camX, camY, out screenX, out screenY);
            }
            else
            {
                OrthoMapToScreen(mapX, mapY, camX, camY, out screenX, out screenY);
            }
        }

        public void MapToScreen(float mapX, float mapY, out int screenX, out int screenY, MapOrientation orientation)
        {
            MapToScreen(mapX, mapY, shakyCamX, shakyCamY, out screenX, out screenY, orientation);
        }

        public void OrthoMapToScreen(float mapX, float mapY, out int screenX, out int screenY)
        {
            OrthoMapToScreen(mapX, mapY, shakyCamX, shakyCamY, out screenX, out screenY);
        }

        public void OrthoMapToScreen(float mapX, float mapY, float camX, float camY, out int screenX, out int screenY)
        {
            float adjustX = (halfViewWidth + 0.5f) * unitsPerPixelX;
            float adjustY = (halfViewHeight + 0.5f) * unitsPerPixelY;
            screenX = (int)((mapX - camX + adjustX) / unitsPerPixelX);
            screenY = (int)((mapY - camY + adjustY) / unitsPerPixelY);
        }

        public void OrthoScreenToMap(float screenX, float screenY, out float mapX, out float mapY)
        {
            OrthoScreenToMap(screenX, screenY, shakyCamX, shakyCamY, out mapX, out mapY);
        }

        public void OrthoScreenToMap(float screenX, float screenY, float camX, float camY, out float mapX, out float mapY)
        {
            mapX = (screenX - halfViewWidth) * unitsPerPixelX + camX;
            mapY = (screenY - halfViewHeight) * unitsPerPixelY + camY;
        }

        public void IsoScreenToMap(float screenX, float screenY, out float mapX, out float mapY)
        {
            IsoScreenToMap(screenX, screenY, shakyCamX, shakyCamY, out mapX, out mapY);
        }

        public void IsoScreenToMap(float screenX, float screenY, float camX, float camY, out float mapX, out float mapY)
        {
            float srcx = (screenX - halfViewWidth) * 0.5f;
            float srcy = (screenY - halfViewHeight) * 0.5f;
            mapX = (unitsPerPixelX * srcx) + (unitsPerPixelY * srcy) + camX;
            mapY = (unitsPerPixelY * srcy) - (unitsPerPixelX * srcx) + camY;
        }

        public void IsoMapToScreen(float mapX, float mapY, out int screenX, out int screenY)
        {
            IsoMapToScreen(mapX, mapY, shakyCamX, shakyCamY, out screenX, out screenY);
        }

        public void IsoMapToScreen(float mapX, float mapY, float camX, float camY, out int screenX, out int screenY)
        {
            float adjustX = (halfViewWidth + 0.5f) * unitsPerPixelX;
            float adjustY = (halfViewHeight + 0.5f) * unitsPerPixelY;
            screenX = (int)(Math.Floor(((mapX - camX - mapY + camY + adjustX) / unitsPerPixelX) + 0.5f));
            screenY = (int)(Math.Floor(((mapX - camX + mapY - camY + adjustY) / unitsPerPixelY) + 0.5f));
        }

        public void ScreenToMap(float screenX, float screenY, out float mapX, out float mapY)
        {
            ScreenToMap(screenX, screenY, shakyCamX, shakyCamY, out mapX, out mapY, orientation);
        }

        public void ScreenToMap(float screenX, float screenY, float camX, float camY, out float mapX, out float mapY)
        {
            ScreenToMap(screenX, screenY, camX, camY, out mapX, out mapY, orientation);
        }

        public void ScreenToMap(float screenX, float screenY, out float mapX, out float mapY, MapOrientation orientation)
        {
            ScreenToMap(screenX, screenY, shakyCamX, shakyCamY, out mapX, out mapY, orientation);
        }

        public void ScreenToMap(float screenX, float screenY, float camX, float camY, out float mapX, out float mapY, MapOrientation orientation)
        {
            if (orientation == MapOrientation.Isometric)
            {
                IsoScreenToMap(screenX, screenY, camX, camY, out mapX, out mapY);
            }
            else
            {
                OrthoScreenToMap(screenX, screenY, camX, camY, out mapX, out mapY);
            }
        }


        public void MapToTile(float mapX, float mapY, out int tileX, out int tileY)
        {
            tileX = (int)(Math.Floor(mapX));
            tileY = (int)(Math.Floor(mapY));
        }

    }
}
