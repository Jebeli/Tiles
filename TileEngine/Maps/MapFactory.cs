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
    using System;
    using System.Linq;

    public static class MapFactory
    {
        private static Random rnd = new Random();
        public static Map MakeNullMap(Engine engine)
        {
            Map map = new Map("null", 1, 1, 64, 32);
            Layer layer = map.AddLayer("null");
            return map;
        }

        public static void MakeTest(Engine engine)
        {
            Map map = engine.LoadMap("maps/frontier_outpost.xml");
            Layer layer = map.Layers.First();
            TileSet tileSet = layer.TileSet;
            engine.SaveTileSet(tileSet);
        }
        public static Map MakeIniMap(Engine engine)
        {
            Map map = engine.LoadMap("maps/frontier_outpost.xml");
            //engine.SaveTileSet(map.Layers.First().TileSet, "tilesetdefs/tileset_grassland.xml");
            //engine.SaveMap(map, "maps/frontier_outpost.xml");
            return map;
        }

        public static TileSet MakeMediTileSet(Engine engine)
        {
            TileSet tileSet = engine.GetTileSet("images/tilesets/tileset_building.png");
            tileSet.AutoFill(64, 192, 0, -160);
            tileSet.Name = "tilesetdefs/tileset_building.xml";
            engine.SaveTileSet(tileSet, "tilesetdefs/tileset_building.xml");
            return tileSet;
        }

        public static Map MakeDummyOrthoMap(Engine engine)
        {
            Map map = engine.LoadMap("part2_map.xml");
            if (map != null) return map;

            TileSet tileSet = engine.LoadTileSet("part2_tileset.xml");
            if (tileSet == null)
            {
                tileSet = engine.GetTileSet("images/part2_tileset.png");
                tileSet.AutoFill(48, 48);
                engine.SaveTileSet(tileSet, "part2_tileset.xml");
            }
            map = new Map("dummy", 64, 84, 48, 48, MapOrientation.Orthogonal);
            Layer layer = map.AddLayer("ground");
            layer.TileSet = tileSet;
            layer.Fill(0);
            layer = map.AddLayer("lake");
            layer.TileSet = tileSet;
            layer[0, 0].TileId = 9 * 12;
            layer[0, 1].TileId = 4 * 12 + 1;
            layer[1, 0].TileId = 9 * 12;
            layer[1, 1].TileId = 4 * 12 + 1;
            layer[1, 0].TileId = 4 * 12 + 3;
            layer[1, 1].TileId = 4 * 12 + 11;
            for (int y = 0; y < layer.Height; y++)
            {
                layer[29, y].TileId = 3 * 12 + 2;
                layer[30, y].TileId = 3;
                layer[31, y].TileId = 3 * 12 + 3;
            }
            engine.SaveMap(map, "part2_map.xml");
            return map;
        }
        public static Map MakeDummyMap(Engine engine)
        {
            Map map = engine.LoadMap("part4_map.xml");
            if (map != null) return map;

            TileSet tileSet = engine.LoadTileSet("part4_tileset.xml");
            if (tileSet == null)
            {
                tileSet = engine.GetTileSet("images/part4_tileset.png");
                tileSet.AutoFill(64, 64, 0, -32);
                engine.SaveTileSet(tileSet, "part4_tileset.xml");
            }
            map = new Map("dummy", 64, 84, 64, 32);
            Layer layer = map.AddLayer("ground");
            layer.TileSet = tileSet;
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
            layer.TileSet = tileSet;


            layer[10, 10].TileId = 10 * 8 + 3;
            layer[10, 9].TileId = 10 * 8 + 7;
            layer[10, 8].TileId = 10 * 8 + 9;

            layer[11, 10].TileId = 10 * 8 + 1;
            layer[11, 9].TileId = 10 * 8 + 4;
            layer[11, 8].TileId = 10 * 8 + 8;

            layer[12, 10].TileId = 10 * 8 + 0;
            layer[12, 9].TileId = 10 * 8 + 2;
            layer[12, 8].TileId = 10 * 8 + 6;


            engine.SaveMap(map, "part4_map.xml");
            return map;
        }
    }
}
