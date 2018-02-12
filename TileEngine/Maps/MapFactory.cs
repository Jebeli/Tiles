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
    public static class MapFactory
    {
        public static Map MakeNullMap(Engine engine)
        {
            Map map = new Map("null", 1, 1);
            Layer layer = map.AddLayer("null");
            return map;
        }
        public static Map MakeDummyMap(Engine engine)
        {
            Map map = new Map("dummy", 64, 84);
            Layer layer = map.AddLayer("dummy");
            layer.TileSet = engine.GetTileSet("part4_tileset.png");
            layer.TileSet.AutoFill(64, 64);

            layer[0, 3].TileId = 3;
            layer[0, 4].TileId = 3;
            layer[0, 5].TileId = 1;
            layer[0, 6].TileId = 1;
            layer[0, 7].TileId = 1;

            layer[1, 3].TileId = 3;
            layer[1, 4].TileId = 1;
            layer[1, 5].TileId = 1;
            layer[1, 6].TileId = 1;
            layer[1, 7].TileId = 1;

            layer[2, 2].TileId = 3;
            layer[2, 3].TileId = 1;
            layer[2, 4].TileId = 1;
            layer[2, 5].TileId = 1;
            layer[2, 6].TileId = 1;
            layer[2, 7].TileId = 1;

            layer[3, 2].TileId = 3;
            layer[3, 3].TileId = 1;
            layer[3, 4].TileId = 1;
            layer[3, 5].TileId = 1;
            layer[3, 6].TileId = 1;
            layer[3, 7].TileId = 1;
            return map;
        }
    }
}
