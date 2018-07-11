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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;

    internal class AStarCloseContainer
    {
        private int size;
        private int node_limit;
        private int map_width;
        private int map_height;
        private AStarNode[] nodes;
        private int[,] map_pos;
        public AStarCloseContainer(int _map_width, int _map_height, int _node_limit)
        {
            node_limit = _node_limit;
            map_width = _map_width;
            map_height = _map_height;
            nodes = new AStarNode[node_limit];
            map_pos = new int[map_width, map_height];
            for (int x = 0; x < map_width; x++)
            {
                for (int y = 0; y < map_height; y++)
                {
                    map_pos[x, y] = -1;
                }
            }
        }

        public int Size
        {
            get { return size; }
        }

        public void Add(AStarNode node)
        {
            if (size >= node_limit) return;
            nodes[size] = node;
            map_pos[node.X, node.Y] = size;
            size++;
        }

        public AStarNode Get(int x, int y)
        {
            return nodes[map_pos[x, y]];
        }

        public bool Exists(Point pos)
        {
            return map_pos[pos.X, pos.Y] != -1;
        }

        public AStarNode GetShortestH()
        {
            AStarNode current = null;
            float lowest_score = 3.402823466e+38F;
            for (int i = 0; i < size; i++)
            {
                if (nodes[i].EstimatedCost < lowest_score)
                {
                    lowest_score = nodes[i].EstimatedCost;
                    current = nodes[i];
                }
            }
            return current;
        }
    }
}
