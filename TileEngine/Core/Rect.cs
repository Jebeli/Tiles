namespace TileEngine.Core
{
    using System;

    public struct Rect
    {

        public static readonly Rect Empty = new Rect();

        private int x;
        private int y;
        private int width;
        private int height;

        public Rect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public Rect(Point location, Point size)
        {
            x = location.X;
            y = location.Y;
            width = size.X;
            height = size.Y;
        }

        public static Rect FromLTRB(int left, int top, int right, int bottom)
        {
            return new Rect(left, top, right - left, bottom - top);
        }

        public Point Location
        {
            get
            {
                return new Point(X, Y);
            }
            set
            {
                x = value.X;
                y = value.Y;
            }
        }

        public Point Size
        {
            get
            {
                return new Point(width, height);
            }
            set
            {
                width = value.X;
                height = value.Y;
            }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Left
        {
            get { return x; }
        }
        public int Top
        {
            get { return y; }
        }
        public int Right
        {
            get { return x + width - 1; }
        }
        public int Bottom
        {
            get { return y + height - 1; }
        }

        public int CenterX
        {
            get { return x + width / 2; }
        }

        public int CenterY
        {
            get { return y + height / 2; }
        }

        public bool IsEmpty
        {
            get { return height <= 0 || width <= 0; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Rect))
                return false;

            Rect comp = (Rect)obj;

            return (comp.x == x) &&
            (comp.y == y) &&
            (comp.width == width) &&
            (comp.height == height);
        }
        public static bool operator ==(Rect left, Rect right)
        {
            return (left.x == right.x
                    && left.y == right.y
                    && left.width == right.width
                    && left.height == right.height);
        }

        public static bool operator !=(Rect left, Rect right)
        {
            return !(left == right);
        }
        public bool Contains(int x, int y)
        {
            return this.x <= x && x < this.x + width && this.y <= y && y < this.y + height;
        }

        public bool Contains(Point pt)
        {
            return Contains(pt.X, pt.Y);
        }

        public bool Contains(Rect rect)
        {
            return (x <= rect.X) && ((rect.x + rect.width) <= (x + width)) && (y <= rect.y) && ((rect.y + rect.height) <= (y + height));
        }

        public override int GetHashCode()
        {
            return (int)((UInt32)x ^ (((UInt32)y << 13) | ((UInt32)y >> 19)) ^ (((UInt32)width << 26) | ((UInt32)width >> 6)) ^ (((UInt32)height << 7) | ((UInt32)height >> 25)));
        }

        public Rect Inflated(int width, int height)
        {
            Rect inflated = this;
            inflated.Inflate(width, height);
            return inflated;
        }

        public void Inflate(int width, int height)
        {
            x -= width;
            y -= height;
            this.width += 2 * width;
            this.height += 2 * height;
        }

        public void Inflate(Point size)
        {
            Inflate(size.X, size.Y);
        }

        public static Rect Inflate(Rect rect, int x, int y)
        {
            Rect r = rect;
            r.Inflate(x, y);
            return r;
        }

        public void Intersect(Rect rect)
        {
            Rect result = Intersect(rect, this);
            x = result.x;
            y = result.y;
            width = result.width;
            height = result.height;
        }
        public static Rect Intersect(Rect a, Rect b)
        {
            int x1 = Math.Max(a.x, b.x);
            int x2 = Math.Min(a.x + a.width, b.x + b.width);
            int y1 = Math.Max(a.y, b.y);
            int y2 = Math.Min(a.y + a.height, b.y + b.height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new Rect(x1, y1, x2 - x1, y2 - y1);
            }
            return Empty;
        }

        public bool IntersectsWith(Rect rect)
        {
            return (rect.x < x + width) && (x < (rect.x + rect.width)) && (rect.y < y + height) && (y < rect.y + rect.height);
        }

        public static Rect Union(Rect a, Rect b)
        {
            int x1 = Math.Min(a.x, b.x);
            int x2 = Math.Max(a.x + a.width, b.x + b.width);
            int y1 = Math.Min(a.y, b.y);
            int y2 = Math.Max(a.y + a.height, b.y + b.height);
            return new Rect(x1, y1, x2 - x1, y2 - y1);
        }

        public void Offset(Point pos)
        {
            Offset(pos.X, pos.Y);
        }

        public void Offset(int x, int y)
        {
            this.x += x;
            this.y += y;
        }

        public override string ToString()
        {
            return $"{{X={x},Y={y},Width={width},Height={height}}}";
        }
    }
}
