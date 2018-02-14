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
    using Core;
    using Graphics;
    using System.Collections.Generic;
    using System.Text;

    public class Layer : NamedObject
    {
        private int width;
        private int height;
        private Map map;
        private Tile[] tiles;
        private TileSet tileSet;
        private bool visible;
        private int oversizeX;
        private int oversizeY;
        private IList<RenderTextureRegion> renderList;

        internal Layer(string name, Map map, int width, int height)
            : base(name)
        {
            this.width = width;
            this.height = height;
            this.map = map;
            visible = true;
            InitTiles();
            oversizeX = 1;
            oversizeY = 1;
        }

        public Map Map
        {
            get { return map; }
        }

        public IList<RenderTextureRegion> RenderList
        {
            get { return renderList; }
            set { renderList = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public TileSet TileSet
        {
            get { return tileSet; }
            set
            {
                tileSet = value;
                oversizeX = value.OversizeX;
                oversizeY = value.OversizeY;
            }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int OversizeX
        {
            get { return oversizeX; }
        }

        public int OversizeY
        {
            get { return oversizeY; }
        }
        public Tile this[int index]
        {
            get { return tiles[index]; }
        }
        public Tile this[int x, int y]
        {
            get { return tiles[XYToIndex(x, y)]; }
        }

        public void Fill(int tileId)
        {
            foreach (var tile in tiles)
            {
                tile.TileId = tileId;
            }
        }

        public void SetCSV(string csv)
        {
            int index = 0;
            foreach (var s in csv.Split(',', '\n', '\r'))
            {
                int value;
                if (int.TryParse(s, out value))
                {
                    tiles[index].TileId = value;
                    index++;
                }
                if (index >= tiles.Length)
                    break;
            }
        }

        public string GetCSV()
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sb.Append(tiles[index].TileId);
                    index++;
                    if (index < tiles.Length) sb.Append(",");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private int XYToIndex(int x, int y)
        {
            return y * width + x;
        }
        private void IndexToXY(int index, out int x, out int y)
        {
            y = index / width;
            x = index - y * width;
        }
        private void InitTiles()
        {
            tiles = new Tile[width * height];
            for (int i = 0; i < tiles.Length; i++)
            {
                int x;
                int y;
                IndexToXY(i, out x, out y);
                tiles[i] = new Tile(this, x, y);
            }
        }
    }
}
