using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.NGUI
{
    public interface IGUIDimensions
    {
        int Left { get; set; }
        int Top { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        DimFlags DimFlags { get; set; }
        void SetBounds(int left, int top, int width, int height);
        void SetBounds(int left, int top, int width, int height, DimFlags flags);
        void Layout();
    }

    public class GUIDimensions : IGUIDimensions
    {
        private int left;
        private int top;
        private int width;
        private int height;
        private DimFlags dimFlags;
        public int Left
        {
            get { return left; }
            set { SetBounds(value, top, width, height, dimFlags); }
        }
        public int Top
        {
            get { return top; }
            set { SetBounds(left, value, width, height, dimFlags); }
        }
        public int Width
        {
            get { return width; }
            set { SetBounds(left, top, value, height, dimFlags); }
        }
        public int Height
        {
            get { return height; }
            set { SetBounds(left, top, width, value, dimFlags); }
        }
        public DimFlags DimFlags
        {
            get { return dimFlags; }
            set { SetBounds(left, top, width, height, value); }
        }

        public void SetBounds(int left, int top, int width, int height)
        {
            SetBounds(left, top, width, height, dimFlags);
        }
        public void SetBounds(int left, int top, int width, int height, DimFlags flags)
        {
            if ((left != this.left) || (top != this.top) || (width != this.width) || (height != this.height) || (flags != dimFlags))
            {
                this.left = left;
                this.top = top;
                this.width = width;
                this.height = height;
                dimFlags = flags;
                Layout();
            }
        }

        public virtual void Layout()
        {

        }
    }
}
