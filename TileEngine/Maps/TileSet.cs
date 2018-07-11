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
    using System.Collections.Generic;
    using Graphics;
    using Resources;
    using System;

    public class TileSet : Resource
    {
        private Texture texture;
        private List<TextureRegion> tiles;
        private List<string> tileNames;
        private Dictionary<int, TileAnim> anims;
        private int tileWidth;
        private int tileHeight;
        private int oversizeX;
        private int oversizeY;
        private float speed = 0.5f;        

        public TileSet(string name, Texture texture)
            : base(name)
        {
            this.texture = texture;
            tiles = new List<TextureRegion>();
            tileNames = new List<string>();
            anims = new Dictionary<int, TileAnim>();
        }      

        public static TileSet GetCollisionTileSet(Texture tex)
        {
            
            TileSet cts = new TileSet("Collision", tex);
            foreach (BlockType bt in Enum.GetValues(typeof(BlockType)))
            {
                cts.AddTile((int)bt, 0, 0, tex.Width, tex.Height, 0, 0, bt.ToString());
            }
            return cts;
        }

        public int TileWidth
        {
            get { return tileWidth; }
            set { tileWidth = value; }
        }

        public int TileHeight
        {
            get { return tileHeight; }
            set { tileHeight = value; }
        }

        public int OversizeX
        {
            get { return oversizeX; }
        }

        public int OversizeY
        {
            get { return oversizeY; }
        }

        public Texture Texture
        {
            get { return texture; }
        }

        public int TileCount
        {
            get { return tiles.Count; }
        }

        public IEnumerable<int> Tiles
        {
            get
            {
                List<int> list = new List<int>();

                for (int i = 0; i < tiles.Count; i++)
                {
                    if (tiles[i] != null)
                    {
                        list.Add(i);
                    }
                }
                return list;
            }
        }

        public IEnumerable<int> AnimTiles
        {
            get
            {
                List<int> list = new List<int>();
                foreach (int i in anims.Keys)
                {
                    list.Add(i);
                }
                return list;
            }
        }

        public bool Update()
        {
            bool updates = false;
            foreach (var anim in anims.Values)
            {
                if (anim.Update())
                {
                    updates = true;
                }
            }
            return updates;
        }

        public void AutoFill(int tileWidth, int tileHeight, int offsetX = 0, int offsetY = 0)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            int index = 0;
            int y = 0;
            while (y < texture.Height)
            {
                int x = 0;
                while (x < texture.Width)
                {
                    AddTile(index, x, y, tileWidth, tileHeight, offsetX, offsetY);
                    x += tileWidth;
                    index++;
                }
                y += tileHeight;
            }
        }

        public void AddTile(int index, int clipX, int clipY, int clipW, int clipH, int offsetX, int offsetY, string name = "")
        {
            if (texture != null)
            {
                AdjustOversizeAndTileSize(clipW, clipH, offsetX, offsetY);
                TextureRegion region = texture.GetRegion(clipX, clipY, clipW, clipH, offsetX, offsetY);
                EnsureIndex(index);
                tiles[index] = region;
                tileNames[index] = name;
            }
        }

        public void AddAnim(int index, int posX, int posY, int duration)
        {
            if (texture != null)
            {
                TextureRegion tile = GetTile(index);
                if (tile != null)
                {
                    TileAnim anim = null;
                    if (!anims.TryGetValue(index, out anim))
                    {
                        anim = new TileAnim(index);
                        anims[index] = anim;
                    }
                    anim.AddFrame(texture, (int)(duration * speed), posX, posY, tile.Width, tile.Height, tile.OffsetX, tile.OffsetY);
                }
            }
        }

        private void AdjustOversizeAndTileSize(int clipW, int clipH, int offsetX, int offsetY)
        {
            if (tileWidth == 0) tileWidth = clipW;
            if (tileHeight == 0) tileHeight = clipH;
            oversizeX = Math.Max(oversizeX, clipW / tileWidth + 1);
            oversizeY = Math.Max(oversizeY, clipH / tileHeight + 1);

            //oversizeX = Math.Max(oversizeX, 1 + (clipW - offsetX) / tileWidth);
            //oversizeY = Math.Max(oversizeY, 1 + (clipH - offsetY) / tileHeight);
        }

        public TextureRegion GetTile(int id)
        {
            TileAnim anim;
            if (anims.TryGetValue(id, out anim))
            {
                return anim.CurrentFrame;
            }
            if (id >= 0 && id < tiles.Count)
            {
                return tiles[id];
            }
            return null;
        }

        public TileAnim GetTileAnim(int id)
        {
            TileAnim anim;
            if (anims.TryGetValue(id, out anim))
            {
                return anim;
            }
            return null;
        }

        public bool IsAnimTile(int id)
        {
            return GetTileAnim(id) != null;
        }

        public string GetTileName(int id)
        {
            if (id >= 0 && id < tileNames.Count)
            {
                return tileNames[id];
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                texture.Dispose();
            }
        }

        private void EnsureIndex(int index)
        {
            int diff = index + 1 - tiles.Count;
            if (diff > 0)
            {
                tiles.AddRange(new TextureRegion[diff]);
                tileNames.AddRange(new string[diff]);
            }
        }
    }
}
