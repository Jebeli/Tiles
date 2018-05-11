using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI
{
    public interface IBox
    {
        bool IsEmpty { get; }
        int Left { get; set; }
        int Top { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        int Right { get; set; }
        int Bottom { get; set; }
        int CenterX { get; }
        int CenterY { get; }
        bool ContainsPoint(int x, int y);
        IBox Shrink(int x, int y);
    }

    public interface IBoxProvider
    {
        IBox WinBox { get; }
        IBox ScrBox { get; }
        IBox GetPreferredSize();
        IBox GetFixedSize();
    }

    public class Box : IBox
    {
        private int left;
        private int top;
        private int width;
        private int height;

        public Box()
        {

        }

        public Box(IBox other)
        {
            left = other.Left;
            top = other.Top;
            width = other.Width;
            height = other.Height;
        }
        public bool IsEmpty { get { return width <= 1 || height <= 1; } }

        public int Left
        {
            get { return left; }
            set { left = value; }
        }

        public int Top
        {
            get { return top; }
            set { top = value; }
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

        public int Right
        {
            get { return left + width - 1; }
            set { left = value - width + 1; }
        }

        public int Bottom
        {
            get { return top + height - 1; }
            set { top = value - height + 1; }
        }

        public int CenterX
        {
            get { return left + width / 2; }
        }

        public int CenterY
        {
            get { return top + height / 2; }
        }

        public bool ContainsPoint(int x, int y)
        {
            return (x >= left) && (y >= top) && (x <= Right) && (y <= Bottom);
        }

        public IBox Shrink(int x, int y)
        {
            Box box = new Box(this);
            box.Left += x;
            box.Top += y;
            box.Width -= 2 * x;
            box.Height -= 2 * y;
            return box;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Box)
            {
                Box box = (Box)obj;
                return (box.left == left) && (box.top == top) && (box.width == width) && (box.height == height);
            }
            return false;
        }
        public override string ToString()
        {
            return $"({left}/{top}) - ({Right}/{Bottom}) (W={width}/H={height})";
        }

    }
}
