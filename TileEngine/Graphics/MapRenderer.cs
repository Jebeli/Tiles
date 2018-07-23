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
    using System.Linq;
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

        public delegate void PerTileFunction(Layer layer, Tile tile, int screenX, int screenY);

        private void InitRenderMap()
        {
            viewWidth = gfx.ViewWidth;
            viewHeight = gfx.ViewHeight;
            tileWidth = engine.Camera.TileWidth;
            tileHeight = engine.Camera.TileHeight;
        }

        private bool InitMapValues(Map map)
        {
            if (map != null)
            {
                viewWidth = gfx.ViewWidth;
                viewHeight = gfx.ViewHeight;
                tileWidth = engine.Camera.TileWidth;
                tileHeight = engine.Camera.TileHeight;
                return true;
            }
            return false;
        }

        private void CalculatePriosIso(IList<RenderTextureRegion> r)
        {
            foreach (var it in r)
            {
                uint tilex = (uint)(Math.Floor(it.MapX));
                uint tiley = (uint)(Math.Floor(it.MapY));
                int commax = (int)((it.MapX - tilex) * (2 << 16));
                int commay = (int)((it.MapY - tiley) * (2 << 16));
                long p1 = tilex + tiley;
                p1 <<= 54;
                long p2 = tilex;
                p2 <<= 42;
                long p3 = commax + commay;
                p3 <<= 16;
                it.Prio += (p1 + p2 + p3);
            }
        }

        public void RenderMap(Map map, List<RenderTextureRegion> renderList, List<RenderTextureRegion> deadRenderList)
        {
            if (InitMapValues(map))
            {
                RenderParallax(map, "");
                CalculatePriosIso(renderList);
                CalculatePriosIso(deadRenderList);
                renderList.Sort();
                deadRenderList.Sort();
                RenderIso(map, renderList, deadRenderList);
                if (gfx.DebugOptions.ShowGrid || gfx.DebugOptions.ShowTileCounter || gfx.DebugOptions.ShowCoordinates) RenderGrid(map);
                if (gfx.DebugOptions.ShowHighlight) RenderHighlight(map);
                if (gfx.DebugOptions.ShowSelected) RenderSelected(map);
                if (gfx.DebugOptions.ShowEntities || gfx.DebugOptions.ShowPaths) RenderEntitiesPaths(map, gfx.DebugOptions.ShowEntities, gfx.DebugOptions.ShowPaths);

            }
        }

        private void RenderEntitiesPaths(Map map, bool es, bool ps)
        {
            Color colorEntity = new Color(0, 255, 0, 255);
            Color colorPath = new Color(0, 0, 255, 255);

            int crossSize = tileHeight / 8;
            foreach (var e in engine.EntityManager.Entities)
            {
                engine.Camera.MapToScreen(e.MapPosX, e.MapPosY, out int sX, out int sY);
                if (es) DrawCross(sX, sY, crossSize, colorEntity);
                if (ps) DrawPath(e.Path, crossSize, colorPath);
            }

        }

        private void DrawPath(IList<FPoint> path, int crossSize, Color color)
        {
            if (path != null && path.Count > 0)
            {
                foreach (var p in path)
                {
                    engine.Camera.MapToScreen(p.X, p.Y, out int sX, out int sY);
                    DrawCross(sX, sY, crossSize, color);
                }
            }
        }

        private void DrawCross(int x, int y, int crossSize, Color color)
        {
            gfx.DrawLine(x - crossSize, y, x + crossSize, y, color);
            gfx.DrawLine(x, y - crossSize, x, y + crossSize, color);
        }

        private void RenderIso(Map map, IList<RenderTextureRegion> r, IList<RenderTextureRegion> rDead)
        {
            foreach (var layer in map.Layers)
            {
                if (layer.ObjectLayer)
                {
                    RenderIsoBackObjects(rDead);
                    RenderIsoFrontObjects(layer, r);
                }
                else if (layer.Visible)
                {
                    RenderIsoLayer(layer);
                }
                RenderParallax(map, layer.Name);
            }
        }

        private void RenderIsoBackObjects(IList<RenderTextureRegion> r)
        {
            RenderList(r);
        }

        private void RenderIsoFrontObjects(Layer layer, IList<RenderTextureRegion> r)
        {
            batch.Begin();
            int w = layer.Width;
            int h = layer.Height;
            TileSet tset = layer.TileSet;
            Point upperLeft = engine.Camera.GetUpperLeft();
            int maxTilesWidth = (viewWidth / tileWidth) + 2 * tset.OversizeX;
            int maxTilesHeight = ((viewHeight / tileHeight) + 2 * (tset.OversizeY)) * 2;
            int j = upperLeft.Y - tset.OversizeY / 2 + tset.OversizeX;
            int i = upperLeft.X - tset.OversizeY / 2 - tset.OversizeX;
            int rCursor = 0;
            int rEnd = r.Count;
            while (rCursor != rEnd && ((int)r[rCursor].MapX + (int)r[rCursor].MapY < i + j || (int)r[rCursor].MapX < i))
            {
                ++rCursor;
            }
            List<int> renderBehindSW = new List<int>();
            List<int> renderBehindNE = new List<int>();
            List<int> renderBehindNone = new List<int>();
            int[,] drawnTiles = new int[w, h];
            for (uint y = (uint)maxTilesHeight; y != 0; --y)
            {
                int tilesWidth = 0;
                if (i < -1)
                {
                    j = j + i + 1;
                    tilesWidth = tilesWidth - (i + 1);
                    i = -1;
                }

                int d = j - h;
                if (d >= 0)
                {
                    j = j - d;
                    tilesWidth = tilesWidth + d;
                    i = i + d;
                }

                int jEnd = Math.Max(j + i - w + 1, Math.Max(j - maxTilesWidth, 0));
                engine.Camera.MapToScreen(i, j, out int pX, out int pY);
                Point p = engine.Camera.CenterTile(pX, pY);
                bool isLastNeTile = false;
                while (j > jEnd)
                {
                    --j;
                    ++i;
                    ++tilesWidth;
                    p.X += tileWidth;
                    bool drawTile = true;
                    int rPreCursor = rCursor;
                    while (rPreCursor != rEnd)
                    {
                        int rCursorX = (int)r[rPreCursor].MapX;
                        int rCursorY = (int)r[rPreCursor].MapY;
                        if ((rCursorX - 1 == i && rCursorY + 1 == j) || (rCursorX + 1 == i && rCursorY - 1 == j))
                        {
                            drawTile = false;
                            break;
                        }
                        else if ((rCursorX + 1 > i) || (rCursorY + 1 > j))
                        {
                            break;
                        }
                        ++rPreCursor;
                    }
                    if (drawTile && drawnTiles[i, j] == 0)
                    {
                        RenderTile(layer, layer[i, j], p.X, p.Y);
                        drawnTiles[i, j] = 1;
                    }
                    if (rCursor == rEnd)
                    {
                        continue;
                    }
                    doLastNETile:
                    GetTileBounds(i - 2, j + 2, layer, out Rect tileSWBounds, out Point tileSWCenter);
                    GetTileBounds(i - 1, j + 2, layer, out Rect tileSBounds, out Point tileSCenter);
                    GetTileBounds(i, j, layer, out Rect tileNEBounds, out Point tileNECenter);
                    GetTileBounds(i, j + 1, layer, out Rect tileEBounds, out Point tileECenter);
                    bool drawSWTile = false;
                    bool drawNETile = false;
                    while (rCursor != rEnd)
                    {
                        int rCursorX = (int)r[rCursor].MapX;
                        int rCursorY = (int)r[rCursor].MapY;
                        if ((rCursorX + 1 == i) && (rCursorY - 1 == j))
                        {
                            drawSWTile = true;
                            drawNETile = !isLastNeTile;
                            engine.Camera.MapToScreen(r[rCursor].MapX, r[rCursor].MapY, out pX, out pY);
                            Point rCursorLeft = new Point(pX, pY);
                            rCursorLeft.Y -= r[rCursor].TextureRegion.OffsetY;
                            Point rCursorRight = new Point(rCursorLeft.X, rCursorLeft.Y);
                            rCursorLeft.X -= r[rCursor].TextureRegion.OffsetX;
                            rCursorRight.X += r[rCursor].TextureRegion.Width - r[rCursor].TextureRegion.OffsetX;
                            bool isBehindSW = false;
                            bool isBehindNE = false;
                            if (IsWithinRect(tileSBounds, rCursorRight) && IsWithinRect(tileSWBounds, rCursorLeft))
                            {
                                isBehindSW = true;
                            }
                            if (drawNETile && IsWithinRect(tileEBounds, rCursorLeft) && IsWithinRect(tileNEBounds, rCursorRight))
                            {
                                isBehindNE = true;
                            }
                            if (isBehindSW)
                            {
                                renderBehindSW.Add(rCursor);
                            }
                            else if (!isBehindSW && isBehindNE)
                            {
                                renderBehindNE.Add(rCursor);
                            }
                            else
                            {
                                renderBehindNone.Add(rCursor);
                            }
                            ++rCursor;
                        }
                        else
                        {
                            break;
                        }
                    }

                    while (renderBehindSW.Count > 0)
                    {
                        RenderRenderable(r[renderBehindSW[0]]);
                        renderBehindSW.RemoveAt(0);
                    }

                    if (drawSWTile && i - 2 >= 0 && j + 2 < h && drawnTiles[i - 2, j + 2] == 0)
                    {
                        RenderTile(layer, layer[i - 2, j + 2], tileSWCenter.X, tileSWCenter.Y);
                        drawnTiles[i - 2, j + 2] = 1;
                    }

                    while (renderBehindNE.Count > 0)
                    {
                        RenderRenderable(r[renderBehindNE[0]]);
                        renderBehindNE.RemoveAt(0);
                    }

                    if (drawNETile && !drawTile && drawnTiles[i, j] == 0)
                    {
                        RenderTile(layer, layer[i, j], tileNECenter.X, tileNECenter.Y);
                        drawnTiles[i, j] = 1;
                    }

                    while (renderBehindNone.Count > 0)
                    {
                        RenderRenderable(r[renderBehindNone[0]]);
                        renderBehindNone.RemoveAt(0);
                    }

                    if (isLastNeTile)
                    {
                        ++j;
                        --i;
                        isLastNeTile = false;
                    }
                    else if (i == w - 1 || j == 0)
                    {
                        --j;
                        ++i;
                        isLastNeTile = true;
                        goto doLastNETile;
                    }

                }
                j = j + tilesWidth;
                i = i - tilesWidth;
                if ((y % 2) != 0)
                {
                    i++;
                }
                else
                {
                    j++;
                }
                while (rCursor != rEnd && ((int)r[rCursor].MapX + (int)r[rCursor].MapY < i + j || (int)r[rCursor].MapX <= i))
                {
                    ++rCursor;
                }
            }
            batch.End();
        }

        private void GetTileBounds(int x, int y, Layer layer, out Rect bounds, out Point center)
        {
            bounds = new Rect();
            center = new Point();
            if ((x >= 0) && (y >= 0) && (x < layer.Width) && (y < layer.Height))
            {
                var tileIndex = layer[x, y];
                if (tileIndex != null && tileIndex.TileId > 0)
                {
                    var tile = layer.TileSet.GetTile(tileIndex.TileId);
                    if (tile == null || tile.Texture == null)
                    {
                        return;
                    }
                    engine.Camera.MapToScreen(x, y, out int pX, out int pY);
                    center = engine.Camera.CenterTile(pX, pY);
                    bounds.X = center.X;
                    bounds.Y = center.Y;
                    bounds.Width = tile.Texture.Width;
                    bounds.Height = tile.Texture.Height;
                }
            }
        }

        private static bool IsWithinRect(Rect r, Point target)
        {
            return target.X >= r.X && target.Y >= r.Y && target.X < r.X + r.Width && target.Y < r.Y + r.Height;
        }

        private void RenderIsoLayer(Layer layer)
        {
            batch.Begin();
            int w = layer.Width;
            int h = layer.Height;
            TileSet tset = layer.TileSet;
            Point upperLeft = engine.Camera.GetUpperLeft();
            int maxTilesWidth = (viewWidth / tileWidth) + 2 * tset.OversizeX;
            int maxTilesHeight = (2 * viewHeight / tileHeight) + 2 * (tset.OversizeY + 1);
            int j = upperLeft.Y - tset.OversizeY / 2 + tset.OversizeX;
            int i = upperLeft.X - tset.OversizeY / 2 - tset.OversizeX;
            for (int y = maxTilesHeight; y >= 0; --y)
            {
                int tilesWidth = 0;
                if (i < -1)
                {
                    j = j + i + 1;
                    tilesWidth = tilesWidth - (i + 1);
                    i = -1;
                }

                int d = j - h;
                if (d >= 0)
                {
                    j = j - d;
                    tilesWidth = tilesWidth + d;
                    i = i + d;
                }

                int jEnd = Math.Max(j + i - w + 1, Math.Max(j - maxTilesWidth, 0));
                engine.Camera.MapToScreen(i, j, out int pX, out int pY);
                Point p = engine.Camera.CenterTile(pX, pY);
                while (j > jEnd)
                {
                    --j;
                    ++i;
                    ++tilesWidth;
                    p.X += tileWidth;
                    RenderTile(layer, layer[i, j], p.X, p.Y);
                }
                j = j + tilesWidth;
                i = i - tilesWidth;
                if ((y % 2) != 0)
                {
                    i++;
                }
                else
                {
                    j++;
                }
            }
            batch.End();
        }

        public void OldRenderMap(Map map, IList<RenderTextureRegion> renderList, IList<RenderTextureRegion> deadRenderList)
        {
            InitRenderMap();
            //foreach (var pl in map.ParallaxLayers)
            //{
            //    RenderParallax(map, pl);
            //}
            int index = 0;
            foreach (Layer layer in map.Layers)
            {
                if (layer.Visible)
                {
                    if (useRenderLists)
                    {
                        if (layer.ObjectLayer)
                        {
                            RenderList(GetRenderList(layer), renderList, deadRenderList);
                        }
                        else
                        {
                            RenderList(GetRenderList(layer));
                        }
                    }
                    else
                    {
                        TileLoop(layer, RenderTile);
                    }
                    //foreach (var pl in layer.ParallaxLayers)
                    //{
                    //    RenderParallax(map, pl);
                    //}
                }
                index++;
            }
            if (gfx.DebugOptions.ShowGrid || gfx.DebugOptions.ShowTileCounter || gfx.DebugOptions.ShowCoordinates) RenderGrid(map);
            if (gfx.DebugOptions.ShowHighlight) RenderHighlight(map);
            if (gfx.DebugOptions.ShowSelected) RenderSelected(map);
        }

        private void RenderTile(Layer layer, Tile tile, int screenX, int screenY)
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
            batch.Begin();
            foreach (var r in list)
            {
                batch.Draw(r.TextureRegion, r.ScreenX, r.ScreenY);
            }
            batch.End();
        }

        private void RenderRenderable(RenderTextureRegion r)
        {
            engine.Camera.MapToScreen(r.MapX, r.MapY, out int sX, out int sY);
            r.ScreenX = sX;
            r.ScreenY = sY;
            batch.Draw(r.TextureRegion, r.ScreenX, r.ScreenY);
        }

        private void RenderList(IEnumerable<RenderTextureRegion> list, IList<RenderTextureRegion> rLive, IList<RenderTextureRegion> rDead)
        {
            RenderList(rDead);
            RenderList(list);
            RenderList(rLive);
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
            foreach (var rtr in layer.RenderList)
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
            batch.Begin();
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
                    function(layer, layer[x, y], sX, sY);
                    tileCounter++;
                    columnCounter++;
                }
                if (columnCounter == 0 && tileCounter > 0) break;
            }
            batch.End();
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
            int x = engine.Camera.SelectedTileX;
            int y = engine.Camera.SelectedTileY;
            if ((x >= 0) && (y >= 0))
            {
                engine.Camera.MapToScreen(x, y + 1, out int sX, out int sY, map.Orientation);
                gfx.DrawTileSelected(sX, sY, tileWidth, tileHeight, map.Orientation);
            }
        }

        private void RenderHighlight(Map map)
        {
            int x = engine.Camera.HoverTileX;
            int y = engine.Camera.HoverTileY;
            engine.Camera.MapToScreen(x, y + 1, out int sX, out int sY, map.Orientation);
            gfx.DrawTileSelected(sX, sY, tileWidth, tileHeight, map.Orientation);
        }

        private void RenderParallax(Map map, string mapLayer)
        {
            if (map.Parallax != null)
            {
                foreach (var p in map.Parallax.GetMatchingLayers(mapLayer))
                {
                    RenderParallax(map, p);
                }
            }
        }

        private void RenderParallax(Map map, ParallaxLayer parallax)
        {
            batch.Begin();
            int width = parallax.Texture.Width;
            int height = parallax.Texture.Height;
            float mapCenterX = map.Width / 2.0f;
            float mapCenterY = map.Height / 2.0f;
            float dpX = mapCenterX + engine.Camera.CameraX;
            float dpY = mapCenterY + engine.Camera.CameraY;
            float sdpX = mapCenterX + (dpX * parallax.Speed) + parallax.FixedOffsetX;
            float sdpY = mapCenterY + (dpY * parallax.Speed) + parallax.FixedOffsetY;
            engine.Camera.MapToScreen(sdpX, sdpY, out int sX, out int sY);
            int centerX = sX - width / 2;
            int centerY = sY - height / 2;
            int drawPosX = centerX - (int)(Math.Ceiling((engine.Graphics.ViewWidth / 2.0f + centerX) / width)) * width;
            int drawPosY = centerY - (int)(Math.Ceiling((engine.Graphics.ViewHeight / 2.0f + centerY) / height)) * height;
            int startX = drawPosX;
            int startY = drawPosY;
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
            batch.End();
        }


        private void OldRenderParallax(Map map, ParallaxLayer parallax)
        {
            batch.Begin();
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
            batch.End();
        }

    }
}