using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.Nano
{
    public class Slider : Widget
    {
        protected float value;
        protected (float, float) range;
        protected (float, float) highlightedRange;
        protected Color highlightColor;
        public Slider(Widget parent)
            : base(parent)
        {
            value = 0.0f;
            range = (0.0f, 1.0f);
            highlightedRange = (0.0f, 0.0f);
            //highlightColor = new Color(255, 80, 80, 70);
            highlightColor = new Color(80, 80, 70, 255);
        }

        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public (float, float) Range
        {
            get { return range; }
            set { range = value; }
        }

        public (float, float) HighlightedRange
        {
            get { return highlightedRange; }
            set { highlightedRange = value; }
        }

        public Color HighlightColor
        {
            get { return highlightColor; }
            set { highlightColor = value; }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            return new Vector2(70, 16);
        }

        public override void Draw(IGraphics gfx)
        {
            Vector2 center = pos + size * 0.5f;
            float kr = (int)(size.Y * 0.4f);
            float kshadow = 3;
            float startX = kr + kshadow + pos.X;
            float widthX = size.X - 2 * (kr - kshadow);
            Vector2 knobPos = new Vector2((int)(startX + (value - range.Item1) / (range.Item2 - range.Item1) * widthX), (int)(center.Y + 0.5f));
            Color b1 = new Color(0, 0, 0, enabled ? (byte)32 : (byte)10);
            Color b2 = new Color(0, 0, 0, enabled ? (byte)128 : (byte)210);
            gfx.FillRectangle((int)startX, center.Y - 3 + 1, (int)widthX, 6, b1, b2);
            if (highlightedRange.Item1 != highlightedRange.Item2)
            {
                gfx.FillRectangle((int)(startX + highlightedRange.Item1 * size.X), (int)(center.Y - kshadow + 1), (int)(widthX * (highlightedRange.Item2 - highlightedRange.Item1)), (int)(kshadow * 2), highlightColor);
            }
            gfx.FillRectangle((int)(knobPos.X - kr - 5), (int)(knobPos.Y - kr - 5), (int)(kr * 2 + 10), (int)(kr * 2 + 10 + kshadow), new Color(0, 0, 0, 64), new Color(0, 0, 0, 0));
            gfx.FillRectangle((int)(knobPos.X - kr - 4), (int)(knobPos.Y - kr - 4), (int)(kr * 2 + 8), (int)(kr * 2 + 8), theme.BorderLight);
            gfx.FillRectangle((int)(knobPos.X - kr - 3), (int)(knobPos.Y - kr - 3), (int)(kr * 2 + 6), (int)(kr * 2 + 6), theme.BorderDark);
        }

        public override bool MouseButtonEvent(Vector2 p, MouseButton button, bool down)
        {
            if (!enabled) return false;
            float kr = (int)(size.Y * 0.4f);
            float kshadow = 3;
            float startX = kr + kshadow + pos.X - 1;
            float widthX = size.X - 2 * (kr + kshadow);

            float value = (p.X - startX) / widthX;
            value = value * (range.Item2 - range.Item1) + range.Item1;
            this.value = Math.Min(Math.Max(value, range.Item1), range.Item2);
            highlightedRange = (range.Item1, this.value);
            return true;
        }

        public override bool MouseDragEvent(Vector2 p, Vector2 rel, MouseButton button)
        {
            if (!enabled) return false;
            float kr = (int)(size.Y * 0.4f);
            float kshadow = 3;
            float startX = kr + kshadow + pos.X - 1;
            float widthX = size.X - 2 * (kr + kshadow);

            float value = (p.X - startX) / widthX;
            value = value * (range.Item2 - range.Item1) + range.Item1;
            this.value = Math.Min(Math.Max(value, range.Item1), range.Item2);
            highlightedRange = (range.Item1, this.value);
            return true;
        }
    }
}
