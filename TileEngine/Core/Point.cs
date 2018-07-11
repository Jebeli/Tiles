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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                }
            }
        }

        public bool IsEmpty
        {
            get { return X == 0 && Y == 0; }
        }

        public static Point operator +(Point v1, Point v2)
        {
            return new Point(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Point operator -(Point v1, Point v2)
        {
            return new Point(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Point operator *(Point v1, double m)
        {
            return new Point((int)(v1.X * m), (int)(v1.Y * m));
        }

        public static float operator *(Point v1, Point v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static Point operator /(Point v1, double m)
        {
            return new Point((int)(v1.X / m), (int)(v1.Y / m));
        }

        public static double Distance(Point v1, Point v2)
        {
            return Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public override string ToString()
        {
            return $"{{X={X},Y={Y}}}";
        }
    }
}
