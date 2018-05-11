using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.Nano
{
    public class ProgressBar : Widget
    {
        protected float value;
        public ProgressBar(Widget parent)
            : base(parent)
        {

        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            return new Vector2(70, 12);
        }

        public override void Draw(IGraphics gfx)
        {
            base.Draw(gfx);
            Color b1 = new Color(0, 0, 0, 32);
            Color b2 = new Color(0, 0, 0, 92);
            gfx.FillRectangle(pos.X, pos.Y, size.X, size.Y, b1, b2);
            float v = Math.Min(Math.Max(0.0f, value), 1.0f);
            int barPos = (int)Math.Round((size.X - 2) * v);
            Color b3 = new Color(220, 220, 220, 100);
            Color b4 = new Color(128, 128, 128, 100);
            gfx.FillRectangle(pos.X+1, pos.Y+1, barPos + 1, size.Y - 2, b3, b4);
        }

        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
