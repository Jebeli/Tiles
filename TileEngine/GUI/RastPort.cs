using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI
{
    public static class RastPort
    {
        public static void WritePixel(this IGraphics rport, int left, int top, Color c)
        {
            RectFill(rport, left, top, left + 1, top + 1, c);
        }
        public static void RectFill(this IGraphics rport, IBox box, Color c)
        {
            RectFill(rport, box.LeftEdge, box.TopEdge, box.RightEdge, box.BottomEdge, c);
        }
        public static void RectFill(this IGraphics rport, int left, int top, int right, int bottom, Color c)
        {
            if (rport != null)
            {
                rport.FillRectangle(left, top, right - left + 1, bottom - top + 1, c);
            }
        }
        public static void DrawRect(this IGraphics rport, IBox box, Color c)
        {
            DrawRect(rport, box.LeftEdge, box.TopEdge, box.RightEdge, box.BottomEdge, c);
        }
        public static void DrawRect(this IGraphics rport, int left, int top, int right, int bottom, Color c)
        {
            if (rport != null)
            {
                rport.DrawRectangle(left, top, right - left, bottom - top, c);
            }
        }
        public static void DrawThickLine(this IGraphics rport, int x1, int y1, int x2, int y2, Color c)
        {
            if (rport != null)
            {
                rport.DrawLine(x1, y1, x2, y2, c);
                rport.DrawLine(x1 + 1, y1, x2 + 1, y2, c);
            }
        }

        public static int TextFit(this IGraphics rport, string text, ref IBox textExtent, IBox constrainingExtent, int constrainingBitWidth, int constrainingBitHeight)
        {
            if (textExtent == null) textExtent = new Box();
            int tw = rport.MeasureTextWidth(text);
            textExtent.Width = tw;
            int retVal = text.Length;
            if (constrainingExtent != null)
            {
                constrainingBitWidth = constrainingExtent.Width;
                constrainingBitHeight = constrainingExtent.Height;
            }
            if (tw <= constrainingBitWidth)
            {
                return retVal;
            }
            while (retVal > 0)
            {
                retVal--;
                string txt = text.Substring(0, retVal);
                tw = rport.MeasureTextWidth(txt);
                if (tw <= constrainingBitWidth)
                {
                    return retVal;
                }
            }
            return 0;
        }

    }
}
