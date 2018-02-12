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
    using Maps;

    internal class MapRenderer
    {
        private Engine engine;
        private IGraphics gfx;
        internal MapRenderer(Engine engine)
        {
            this.engine = engine;
            gfx = engine.Graphics;
        }
        public void RenderMap(Map map)
        {
            RenderGrid(map);
            RenderSelected(map);
        }

        private void RenderGrid(Map map)
        {
            int tileWidth = engine.Camera.TileWidth;
            int tileHeight = engine.Camera.TileHeight;
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    int sX;
                    int sY;
                    engine.Camera.IsoMapToScreen(x, y, out sX, out sY);
                    gfx.DrawTileGrid(sX, sY, tileWidth, tileHeight);
                }
            }
        }

        private void RenderSelected(Map map)
        {
            int tileWidth = engine.Camera.TileWidth;
            int tileHeight = engine.Camera.TileHeight;
            int x = engine.Camera.HoverTileX;
            int y = engine.Camera.HoverTileY;
            int sX;
            int sY;
            engine.Camera.IsoMapToScreen(x, y, out sX, out sY);
            gfx.DrawTileSelected(sX, sY, tileWidth, tileHeight);
        }
    }
}