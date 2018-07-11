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

namespace TileEngine.Core
{
    public struct FPoint
    {
        public float X { get; set; }
        public float Y { get; set; }

        public FPoint(float x = 0, float y = 0)
        {
            X = x;
            Y = y;
        }

        public FPoint(FPoint p)
        {
            X = p.X;
            Y = p.Y;
        }

        public FPoint(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
