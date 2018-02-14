using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Graphics
{
    public class NinePatch
    {
        public const int TOP_LEFT = 0;
        public const int TOP_CENTER = 1;
        public const int TOP_RIGHT = 2;
        public const int MIDDLE_LEFT = 3;
        public const int MIDDLE_CENTER = 4;
        public const int MIDDLE_RIGHT = 5;
        public const int BOTTOM_LEFT = 6;
        public const int BOTTOM_CENTER = 7;
        public const int BOTTOM_RIGHT = 8;

        private Texture texture;
        private TextureRegion[] patches;
        private int left;
        private int right;
        private int top;
        private int bottom;

        public NinePatch(Texture texture, int left, int right, int top, int bottom)
        {
            this.texture = texture;
            patches = new TextureRegion[9];
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
            int middleWidth = texture.Width - left - right;
            int middleHeight = texture.Height - top - bottom;
            if (top > 0)
            {
                if (left > 0) patches[TOP_LEFT] = texture.GetRegion(0, 0, left, top);
                if (middleWidth > 0) patches[TOP_CENTER] = texture.GetRegion(left, 0, middleWidth, top);
                if (right > 0) patches[TOP_RIGHT] = texture.GetRegion(left + middleWidth, 0, right, top);
            }
            if (middleHeight > 0)
            {
                if (left > 0) patches[MIDDLE_LEFT] = texture.GetRegion(0, top, left, middleHeight);
                if (middleWidth > 0) patches[MIDDLE_CENTER] = texture.GetRegion(left, top, middleWidth, middleHeight);
                if (right > 0) patches[MIDDLE_RIGHT] = texture.GetRegion(left + middleWidth, top, right, middleHeight);
            }
            if (bottom > 0)
            {
                if (left > 0) patches[BOTTOM_LEFT] = texture.GetRegion(0, top + middleHeight, left, bottom);
                if (middleWidth > 0) patches[BOTTOM_CENTER] = texture.GetRegion(left, top + middleHeight, middleWidth, bottom);
                if (right > 0) patches[BOTTOM_RIGHT] = texture.GetRegion(left + middleWidth, top + middleHeight, right, bottom);
            }
            if (left == 0 && middleWidth == 0)
            {
                patches[TOP_CENTER] = patches[TOP_RIGHT];
                patches[MIDDLE_CENTER] = patches[MIDDLE_RIGHT];
                patches[BOTTOM_CENTER] = patches[BOTTOM_RIGHT];
                patches[TOP_RIGHT] = null;
                patches[MIDDLE_RIGHT] = null;
                patches[BOTTOM_RIGHT] = null;
            }
            if (top == 0 && middleHeight == 0)
            {
                patches[MIDDLE_LEFT] = patches[BOTTOM_LEFT];
                patches[MIDDLE_CENTER] = patches[BOTTOM_CENTER];
                patches[MIDDLE_RIGHT] = patches[BOTTOM_RIGHT];
                patches[BOTTOM_LEFT] = null;
                patches[BOTTOM_CENTER] = null;
                patches[BOTTOM_RIGHT] = null;
            }
        }

        public void Draw(IGraphics graphics, int x, int y, int width, int height)
        {
            graphics.Render(patches[TOP_LEFT], x, y);
            graphics.Render(patches[TOP_CENTER], x + left, y, width - left - right, top);
            graphics.Render(patches[TOP_RIGHT], x + width - right, y);
            graphics.Render(patches[MIDDLE_LEFT], x, y + top, left, height - top - bottom);
            graphics.Render(patches[MIDDLE_CENTER], x + left, y + top, width - left - right, height - top - bottom);
            graphics.Render(patches[MIDDLE_RIGHT], x + width - right, y + top, right, height - top - bottom);
            graphics.Render(patches[BOTTOM_LEFT], x, y + height - bottom);
            graphics.Render(patches[BOTTOM_CENTER], x + left, y + height - bottom, width - left - right, bottom);
            graphics.Render(patches[BOTTOM_RIGHT], x + width - right, y + height - bottom);
        }
    }
}
