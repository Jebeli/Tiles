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

    internal static class Utils
    {
        private static Random rnd = new Random();

        internal static int Rand()
        {
            return rnd.Next();
        }

        internal static Point MapToCollision(FPoint p)
        {
            return new Point((int)p.X, (int)p.Y);
        }

        internal static FPoint CollisionToMap(Point p)
        {
            return new FPoint(p.X + 0.5f, p.Y + 0.5f);
        }

        internal static float CalcDist(float x0, float y0, float x1, float y1)
        {
            return (float)Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
        }

        internal static float CalcDist(Point p1, Point p2)
        {
            return (float)Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }

        internal static int CalcDirection(float x0, float y0, float x1, float y1)
        {
            float theta = CalcTheta(x0, y0, x1, y1);
            float val = theta / ((float)(Math.PI) / 4);
            int dir = (int)(((val < 0) ? Math.Ceiling(val - 0.5f) : Math.Floor(val + 0.5f)) + 4);
            dir = (dir + 1) % 8;
            if (dir >= 0 && dir < 8)
                return dir;
            else
                return 0;
        }

        internal static float CalcTheta(float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            float exact_dx = x2 - x1;
            float theta;
            if (exact_dx == 0)
            {
                if (dy > 0.0) theta = (float)Math.PI / 2.0f;
                else theta = (float)(-Math.PI) / 2.0f;
            }
            else
            {
                theta = (float)Math.Atan(dy / dx);
                if (dx < 0.0 && dy >= 0.0) theta += (float)(Math.PI);
                if (dx < 0.0 && dy < 0.0) theta -= (float)(Math.PI);
            }
            return theta;
        }

        internal static void CleanFloat(ref float num)
        {
            num = ((int)(num * 100)) / 100.0f;
        }

        internal static void AlignFPoint(ref float posX, ref float posY)
        {
            posX = (float)Math.Floor(posX / 0.0625f) * 0.0625f;
            posY = (float)Math.Floor(posY / 0.0625f) * 0.0625f;
        }
    }
}
