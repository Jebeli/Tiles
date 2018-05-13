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
    using System.Collections.Generic;
    using TileEngine.Fonts;

    internal class MapRenderer
    {
        private Engine engine;
        private IGraphics gfx;
        private IBatch batch;
        private bool useRenderLists;
        private int viewWidth;
        private int viewHeight;
        private int tileWidth;
        private int tileHeight;
        private Font font;
        internal MapRenderer(Engine engine)
        {
            useRenderLists = true;
            this.engine = engine;
            gfx = engine.Graphics;
            batch = new TextureBatch(gfx);
            font = engine.Fonts.DefaultFont;
        }

        public bool UseRenderLists
        {
            get { return useRenderLists; }
            set { useRenderLists = value; }
        }

        public delegate void PerTileFunction(Layer layer, Tile tile, int screenX, int screenY, int width, int height);

        private void InitRenderMap()
        {
            viewWidth = gfx.ViewWidth;
            viewHeight = gfx.ViewHeight;
            tileWidth = engine.Camera.TileWidth;
            tileHeight = engine.Camera.TileHeight;
        }

        public void RenderMap(Map map)
        {
            InitRenderMap();
            RenderBaseParallax(map);
            batch.Begin();
            int index = 0;
            foreach (Layer layer in map.Layers)
            {
                if (layer.Visible)
                {
                    if (useRenderLists)
                    {
                        RenderList(GetRenderList(layer));
                    }
                    else
                    {
                        TileLoop(layer, RenderTile);
                    }
                }
                index++;
            }
            batch.End();
            RenderTopParallax(map);
            if (gfx.DebugOptions.ShowGrid || gfx.DebugOptions.ShowTileCounter || gfx.DebugOptions.ShowCoordinates) RenderGrid(map);
            if (gfx.DebugOptions.ShowHighlight) RenderSelected(map);
        }

        private void RenderTile(Layer layer, Tile tile, int screenX, int screenY, int width, int height)
        {
            if (tile.TileId >= 0)
            {
                TextureRegion tex = layer.TileSet.GetTile(tile.TileId);
                if (tex != null)
                {
                    batch.Draw(tex, screenX, screenY);
                }
            }
        }

        private void RenderList(IEnumerable<RenderTextureRegion> list)
        {
            foreach (var r in list)
            {
                batch.Draw(r.TextureRegion, r.ScreenX, r.ScreenY);
            }
        }

        private IList<RenderTextureRegion> GetRenderList(Layer layer)
        {
            IList<RenderTextureRegion> list = layer.RenderList;
            if (list == null)
            {
                list = new List<RenderTextureRegion>();
                int maxOversizeX = layer.OversizeX * layer.TileSet.TileWidth;
                int maxOversizeY = layer.OversizeY * layer.TileSet.TileHeight;
                int maxScreenX = (viewWidth - tileWidth) + maxOversizeX;
                int maxScreenY = (viewHeight - tileHeight) + maxOversizeY;
                int minScreenX = 0 - maxOversizeX;
                int minScreenY = 0 - maxOversizeY;
                int tileCounter = 0;
                for (int y = 0; y < layer.Height; y++)
                {
                    int columnCounter = 0;
                    for (int x = 0; x < layer.Width; x++)
                    {
                        engine.Camera.MapToScreen(x, y, out int sX, out int sY);
                        if (sX >= maxScreenX || sY >= maxScreenY) break;
                        if (sX <= minScreenX || sY <= minScreenY) continue;
                        Tile tile = layer[x, y];
                        if (tile.TileId >= 0)
                        {
                            TextureRegion tex = layer.TileSet.GetTile(tile.TileId);
                            if (tex != null)
                            {
                                list.Add(new RenderTextureRegion(tile.TileId, tex, x, y, sX, sY, layer.TileSet.IsAnimTile(tile.TileId)));
                            }
                        }
                        tileCounter++;
                        columnCounter++;
                    }
                    if (columnCounter == 0 && tileCounter > 0) break;
                }
                layer.RenderList = list;
            }
            else
            {
                UpdateRenderList(layer);
            }
            return list;
        }

        private void UpdateRenderList(Layer layer)
        {
            foreach(var rtr in layer.RenderList)
            {
                if (rtr.IsAnimTile)
                {
                    TextureRegion tex = layer.TileSet.GetTile(rtr.Id);
                    rtr.TextureRegion = tex;
                }
            }
        }

        private void TileLoop(Layer layer, PerTileFunction function)
        {
            int maxOversizeX = layer.OversizeX * layer.TileSet.TileWidth;
            int maxOversizeY = layer.OversizeY * layer.TileSet.TileHeight;
            int maxScreenX = (viewWidth - tileWidth) + maxOversizeX;
            int maxScreenY = (viewHeight - tileHeight) + maxOversizeY;
            int minScreenX = 0 - maxOversizeX;
            int minScreenY = 0 - maxOversizeY;
            int tileCounter = 0;
            for (int y = 0; y < layer.Height; y++)
            {
                int columnCounter = 0;
                for (int x = 0; x < layer.Width; x++)
                {
                    engine.Camera.MapToScreen(x, y, out int sX, out int sY);
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
            int maxOversizeX = 1 * tileWidth;
            int maxOversizeY = 1 * tileHeight;
            int maxScreenX = (viewWidth - tileWidth) + maxOversizeX;
            int maxScreenY = (viewHeight - tileHeight) + maxOversizeY;
            int minScreenX = 0 - maxOversizeX;
            int minScreenY = 0 - maxOversizeY;
            int tileCounter = 0;
            for (int y = 0; y < map.Height; y++)
            {
                int columnCounter = 0;
                for (int x = 0; x < map.Width; x++)
                {
                    engine.Camera.MapToScreen(x, y, out int sX, out int sY, map.Orientation);
                    if (sX >= maxScreenX || sY >= maxScreenY) break;
                    if (sX <= minScreenX || sY <= minScreenY) continue;
                    if (gfx.DebugOptions.ShowGrid) gfx.DrawTileGrid(sX, sY, tileWidth, tileHeight, map.Orientation);
                    if (gfx.DebugOptions.ShowTileCounter) gfx.DrawText(font, tileCounter.ToString(), sX + TEXT_OFFSET_X, sY + TEXT_OFFSET_Y);
                    if (gfx.DebugOptions.ShowCoordinates) gfx.DrawText(font, $"({x}/{y})", sX + TEXT_OFFSET_X, sY + TEXT_OFFSET_Y + TEXT_SIZE_Y);
                    tileCounter++;
                    columnCounter++;

                }
                if (columnCounter == 0 && tileCounter > 0) break;
            }
        }

        private void RenderSelected(Map map)
        {
            int x = engine.Camera.HoverTileX;
            int y = engine.Camera.HoverTileY;
            engine.Camera.MapToScreen(x, y, out int sX, out int sY, map.Orientation);
            gfx.DrawTileSelected(sX, sY, tileWidth, tileHeight, map.Orientation);
        }

        private void RenderBaseParallax(Map map)
        {
            if (map.Parallax != null)
            {
                foreach (var p in map.Parallax.Layers)
                {
                    if (string.IsNullOrEmpty(p.MapLayer))
                    {
                        RenderParallax(map, p);
                    }
                }
            }
        }

        private void RenderTopParallax(Map map)
        {
            if (map.Parallax != null)
            {
                foreach (var p in map.Parallax.Layers)
                {
                    if (!string.IsNullOrEmpty(p.MapLayer))
                    {
                        RenderParallax(map, p);
                    }
                }
            }
        }

        private void RenderParallax(Map map, ParallaxLayer parallax)
        {
            int width = parallax.Texture.Width;
            int height = parallax.Texture.Height;
            float mapCenterX = map.Width / 2;
            float mapCenterY = map.Height / 2;
            float dpX = mapCenterX + (mapCenterX * parallax.Speed) + parallax.FixedOffsetX;
            float dpY = mapCenterY + (mapCenterY * parallax.Speed) + parallax.FixedOffsetY;
            engine.Camera.MapToScreen(dpX, dpY, out int sX, out int sY);
            float cX = sX - width / 2.0f;
            float cY = sY - height / 2.0f;
            int startX = (int)(cX - (int)(Math.Ceiling(engine.Graphics.ViewWidth / 2.0f + cX) / (float)width) * width);
            int startY = (int)(cY - (int)(Math.Ceiling(engine.Graphics.ViewHeight / 2.0f + cY) / (float)height) * height);
            while (startX > 0) startX -= width;
            while (startY > 0) startY -= height;
            int x = startX;
            while (x < engine.Graphics.ViewWidth)
            {
                int y = startY;
                while (y < engine.Graphics.ViewHeight)
                {
                    engine.Graphics.Render(parallax.Texture, x, y);
                    y += height;
                }
                x += width;
            }
        }

    }
}