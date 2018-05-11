using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.YGUI
{
    public class ProgressbarGadget : Gadget
    {
        private int min;
        private int max;
        private int value;
        private bool showPercent;
        private bool showPart;

        public ProgressbarGadget(Gadget parent)
            : base(parent)
        {

        }

        public bool ShowPercent
        {
            get { return showPercent; }
            set
            {
                showPercent = value;
                if (showPercent) showPart = false;
            }
        }

        public bool ShowPart
        {
            get { return showPart; }
            set
            {
                showPart = value;
                if (showPart) showPercent = false;
            }
        }

        public string PercentText
        {
            get
            {
                int total = max - min;
                int part = value - min;
                int perc = total * part / 100;
                return $"{perc} %";
            }
        }

        public string PartText
        {
            get
            {
                int total = max - min;
                int part = value - min;
                return $"{part}/{total}";
            }
        }

        public int Min
        {
            get { return min; }
            set { min = value; }
        }

        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public Rect ProgressRect
        {
            get
            {
                var rect = Bounds;
                rect.Inflate(-1, -1);
                int total = max - min;
                int part = value - min;
                int all = rect.Width;
                double prog = all;
                prog /= total;
                prog *= part;
                rect.Width = (int)prog;
                return rect;
            }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            return new Vector2(75, 24);
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

        public override string ToString()
        {
            return $"Progressbar {PartText}";
        }

    }
}
