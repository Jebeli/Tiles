using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Core
{
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
