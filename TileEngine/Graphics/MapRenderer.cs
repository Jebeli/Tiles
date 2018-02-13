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
        private int oversizeX;
        private int oversizeY;
        private IBatch batch;
        internal MapRenderer(Engine engine)
        {
            this.engine = engine;
            gfx = engine.Graphics;
            batch = new TextureBatch(gfx);
            oversizeX = 1;
            oversizeY = 1;
        }

        public delegate void PerTileFunction(Layer layer, Tile tile, int screenX, int screenY, int width, int height);
        public void RenderMap(Map map)
        {
            batch.Begin();
            foreach (Layer layer in map.Layers)
            {
                RenderLayer(layer);
            }
            batch.End();
            if (gfx.DebugOptions.ShowGrid || gfx.DebugOptions.ShowTileCounter || gfx.DebugOptions.ShowCoordinates) RenderGrid(map);
            if (gfx.DebugOptions.ShowHighlight) RenderSelected(map);
        }

        private void RenderLayer(Layer layer)
        {
            TileLoop(layer, RenderTile);
        }

        private void RenderTile(Layer layer, Tile tile, int screenX, int screenY, int width, int height)
        {
            if (tile.TileId >= 0)
            {
                TextureRegion tex = layer.TileSet.GetTile(tile.TileId);
                batch.Draw(tex, screenX, screenY);
            }
        }

        private void TileLoop(Layer layer, PerTileFunction function)
        {
            int tileWidth = engine.Camera.TileWidth;
            int tileHeight = engine.Camera.TileHeight;
            int maxOversizeX = oversizeX * tileWidth;
            int maxOversizeY = oversizeY * tileHeight;
            int maxScreenX = (engine.Camera.ViewWidth - tileWidth) + maxOversizeX;
            int maxScreenY = (engine.Camera.ViewHeight - tileHeight) + maxOversizeY;
            int minScreenX = 0 - maxOversizeX;
            int minScreenY = 0 - maxOversizeY;
            int tileCounter = 0;
            for (int y = 0; y < layer.Height; y++)
            {
                int columnCounter = 0;
                for (int x = 0; x < layer.Width; x++)
                {
                    int sX;
                    int sY;
                    engine.Camera.IsoMapToScreen(x, y, out sX, out sY);
                    if (sX >= maxScreenX || sY >= maxScreenY) break;
                    if (sX <= minScreenX || sY <= minScreenY) continue;
                    function(layer, layer[x, y], sX, sY, tileWidth, tileHeight);
                    tileCounter++;
                    columnCounter++;
                }
                if (columnCounter == 0 && tileCounter > 0) break;
            }

        }

        private void RenderGrid(Map map)
        {
            const int TEXT_OFFSET_X = 18;
            const int TEXT_OFFSET_Y = 5;
            const int TEXT_SIZE_Y = 9;
            int tileWidth = engine.Camera.TileWidth;
            int tileHeight = engine.Camera.TileHeight;
            int maxOversizeX = oversizeX * tileWidth;
            int maxOversizeY = oversizeY * tileHeight;
            int maxScreenX = (engine.Camera.ViewWidth - tileWidth) + maxOversizeX;
            int maxScreenY = (engine.Camera.ViewHeight - tileHeight) + maxOversizeY;
            int minScreenX = 0 - maxOversizeX;
            int minScreenY = 0 - maxOversizeY;
            int tileCounter = 0;
            for (int y = 0; y < map.Height; y++)
            {
                int columnCounter = 0;
                for (int x = 0; x < map.Width; x++)
                {
                    int sX;
                    int sY;
                    engine.Camera.IsoMapToScreen(x, y, out sX, out sY);
                    if (sX >= maxScreenX || sY >= maxScreenY) break;
                    if (sX <= minScreenX || sY <= minScreenY) continue;
                    if (gfx.DebugOptions.ShowGrid) gfx.DrawTileGrid(sX, sY, tileWidth, tileHeight);
                    if (gfx.DebugOptions.ShowTileCounter) gfx.DrawText(tileCounter.ToString(), sX + TEXT_OFFSET_X, sY + TEXT_OFFSET_Y);
                    if (gfx.DebugOptions.ShowCoordinates) gfx.DrawText($"({x}/{y})", sX + TEXT_OFFSET_X, sY + TEXT_OFFSET_Y + TEXT_SIZE_Y);
                    tileCounter++;
                    columnCounter++;

                }
                if (columnCounter == 0 && tileCounter > 0) break;
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