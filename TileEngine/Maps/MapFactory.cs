﻿/*
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
    using System;

    public static class MapFactory
    {
        private static Random rnd = new Random();
        public static Map MakeNullMap(Engine engine)
        {
            Map map = new Map("null", 1, 1);
            Layer layer = map.AddLayer("null");
            return map;
        }
        public static Map MakeDummyMap(Engine engine)
        {
            Map map = new Map("dummy", 64, 84);
            Layer layer = map.AddLayer("ground");
            layer.TileSet = engine.GetTileSet("images/part4_tileset.png");
            layer.TileSet.AutoFill(64, 64);

            for (int x = 0; x < layer.Width; x++)
            {
                for (int y = 0; y < layer.Height; y++)
                {
                    int row = rnd.Next() % 3;
                    switch (row)
                    {
                        case 0:
                            layer[x, y].TileId = 0 + rnd.Next() % 7;
                            break;
                        case 1:
                            layer[x, y].TileId = 10 + rnd.Next() % 8;
                            break;
                        case 2:
                            layer[x, y].TileId = 20 + rnd.Next() % 4;
                            break;
                    }

                }
            }

            layer = map.AddLayer("lake");
            layer.TileSet = engine.GetTileSet("images/part4_tileset.png");


            layer[10, 10].TileId = 10 * 8 + 3;
            layer[10, 9].TileId = 10 * 8 + 7;
            layer[10, 8].TileId = 10 * 8 + 9;

            layer[11, 10].TileId = 10 * 8 + 1;
            layer[11, 9].TileId = 10 * 8 + 4;
            layer[11, 8].TileId = 10 * 8 + 8;

            layer[12, 10].TileId = 10 * 8 + 0;
            layer[12, 9].TileId = 10 * 8 + 2;
            layer[12, 8].TileId = 10 * 8 + 6;

            return map;
        }
    }
}
