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

    public class TileSet : Resource
    {
        private Texture texture;
        private List<TextureRegion> tiles;
        private int tileWidth;
        private int tileHeight;
        public TileSet(string name, Texture texture)
            : base(name)
        {
            this.texture = texture;
            tiles = new List<TextureRegion>();
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

        public Texture Texture
        {
            get { return texture; }
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

        public void AddTile(int index, int clipX, int clipY, int clipW, int clipH, int offsetX, int offsetY)
        {
            if (texture != null)
            {
                TextureRegion region = texture.GetRegion(clipX, clipY, clipW, clipH, offsetX, offsetY);
                EnsureIndex(index);
                tiles[index] = region;
            }
        }

        public TextureRegion GetTile(int id)
        {
            if (id >= 0 && id < tiles.Count)
            {
                return tiles[id];
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
            }
        }
    }
}
