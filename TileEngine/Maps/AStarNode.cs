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

    internal class AStarNode : IEquatable<AStarNode>, IComparable<AStarNode>
    {
        private const int node_stride = 1;
        private int x;
        private int y;
        private float g;
        private float h;
        private Point parent;

        public AStarNode()
        {

        }

        public AStarNode(Point p)
        {
            x = p.X;
            y = p.Y;
            parent = new Point();
        }

        public AStarNode(AStarNode other)
        {
            x = other.x;
            y = other.y;
            g = other.g;
            h = other.h;
            parent = other.parent;
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public float EstimatedCost
        {
            get { return h; }
            set { h = value; }
        }

        public float ActualCost
        {
            get { return g; }
            set { g = value; }
        }

        public Point Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public float FinalCost
        {
            get { return g + h * 2.0f; }
        }

        public bool Equals(AStarNode other)
        {
            return x == other.x && y == other.y;
        }

        public int CompareTo(AStarNode other)
        {
            return FinalCost.CompareTo(other.FinalCost);
        }

        public List<Point> GetNeighbours(int limitX = 0, int limitY = 0)
        {
            Point toAdd = new Point();
            List<Point> res = new List<Point>();
            if (x > node_stride && y > node_stride)
            {
                toAdd.X = x - node_stride;
                toAdd.Y = y - node_stride;
                res.Add(toAdd);
            }
            if (x > node_stride && (limitY == 0 || y < limitY - node_stride))
            {
                toAdd.X = x - node_stride;
                toAdd.Y = y + node_stride;
                res.Add(toAdd);
            }
            if (y > node_stride && (limitX == 0 || x < limitX - node_stride))
            {
                toAdd.X = x + node_stride;
                toAdd.Y = y - node_stride;
                res.Add(toAdd);
            }
            if ((limitX == 0 || x < limitX - node_stride) && (limitY == 0 || y < limitY - node_stride))
            {
                toAdd.X = x + node_stride;
                toAdd.Y = y + node_stride;
                res.Add(toAdd);
            }
            if (x > node_stride)
            {
                toAdd.X = x - node_stride;
                toAdd.Y = y;
                res.Add(toAdd);
            }
            if (y > node_stride)
            {
                toAdd.X = x;
                toAdd.Y = y - node_stride;
                res.Add(toAdd);
            }
            if (limitX == 0 || x < limitX - node_stride)
            {
                toAdd.X = x + node_stride;
                toAdd.Y = y;
                res.Add(toAdd);
            }
            if (limitY == 0 || y < limitY - node_stride)
            {
                toAdd.X = x;
                toAdd.Y = y + node_stride;
                res.Add(toAdd);
            }

            return res;
        }
    }
}
