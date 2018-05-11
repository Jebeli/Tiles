using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;
using TileEngine.Graphics;

namespace TileEngine.IGUI
{
    [Flags]
    public enum ImageState
    {
        None = 0,
        Selected = 0x01,
        Hover = 0x02,
        Disabled = 0x04,
        Active = 0x08
    }
    //public class Image : Root
    //{
    //    private int leftEdge;
    //    private int topEdge;
    //    private int width;
    //    private int height;
    //    private ITheme theme;
    //    private Font font;

    //    public Image()
    //       : this(TagItems.Empty)
    //    {
    //    }

    //    public Image(params (Tags, object)[] tags)
    //        : base(TagItems.Empty)
    //    {
    //        New(tags);
    //    }

    //    public ITheme Theme
    //    {
    //        get { return theme; }
    //        set { theme = value; }
    //    }

    //    public Font Font
    //    {
    //        get { return font; }
    //        set { font = value; }
    //    }
    //    public int LeftEdge
    //    {
    //        get { return leftEdge; }
    //        internal set { leftEdge = value; }
    //    }

    //    public int TopEdge
    //    {
    //        get { return topEdge; }
    //        internal set { topEdge = value; }
    //    }

    //    public int Width
    //    {
    //        get { return width; }
    //    }

    //    public int Height
    //    {
    //        get { return height; }
    //    }

    //    public virtual void Draw(IGraphics gfx, ImageState state, int x, int y)
    //    {
    //        gfx.DrawRectangle(x + leftEdge, y + topEdge, width, height, Color.White);
    //    }

    //    public virtual void DrawFrame(IGraphics gfx, ImageState state, int x, int y, int width, int height)
    //    {
    //        gfx.DrawRectangle(x + leftEdge, y + topEdge, width, height, Color.White);
    //    }

    //    protected override int SetTag(Tags tag, object value, bool fromNew = false)
    //    {
    //        switch (tag)
    //        {
    //            case Tags.IA_Top:
    //                topEdge = (int)value;
    //                return 1;
    //            case Tags.IA_Left:
    //                leftEdge = (int)value;
    //                return 1;
    //            case Tags.IA_Width:
    //                width = (int)value;
    //                return 1;
    //            case Tags.IA_Height:
    //                height = (int)value;
    //                return 1;
    //        }
    //        return base.SetTag(tag, value, fromNew);
    //    }
    //}
}
