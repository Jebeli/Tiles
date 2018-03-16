using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI
{
    public interface IBox
    {
        int LeftEdge { get; set; }
        int TopEdge { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        int RightEdge { get; }
        int BottomEdge { get; }
        int CenterX { get; }
        int CenterY { get; }
        bool ContainsPoint(int x, int y);
    }

    public class Box : IBox
    {
        public int LeftEdge { get; set; }
        public int TopEdge { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int RightEdge { get { return LeftEdge + Width - 1; } }
        public int BottomEdge { get { return TopEdge + Height - 1; } }
        public int CenterX { get { return LeftEdge + Width / 2; } }
        public int CenterY { get { return TopEdge + Height / 2; } }
        public bool ContainsPoint(int x, int y)
        {
            return ((LeftEdge <= x) && (TopEdge <= y) && (RightEdge >= x) && (BottomEdge >= y));
        }

        public override string ToString()
        {
            return $"({LeftEdge}/{TopEdge}) - ({Width}/{Height})";
        }
    }
}
