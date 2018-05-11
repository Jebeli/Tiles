using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.NGUI.Gadgets
{
    public class SliderGadget : PropGadget
    {
        private Orientation orientation;
        private int level;
        private int min;
        private int max;

        public SliderGadget()
        {
            VertOverlap = 0;
            HorizOverlap = 0;
            VertVisible = 1;
            HorizVisible = 1;
        }

        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    WriteChanges();
                }
            }
        }
        public int Level
        {
            get
            {
                ReadChanges();
                return level;
            }
            set
            {
                if (level != value)
                {
                    level = value;
                    WriteChanges();
                }
            }
        }

        public int Max
        {
            get { return max; }
            set
            {
                if (max != value)
                {
                    max = value;
                    WriteChanges();
                }
            }
        }

        public int Min
        {
            get { return min; }
            set
            {
                if (min != value)
                {
                    min = value;
                    WriteChanges();
                }
            }
        }

        private void ReadChanges()
        {
            int num = FreeTotal;
            level = FreeTop + min;
        }

        private void WriteChanges()
        {
            if (orientation == Orientation.Horizontal)
            {
                PropFlags &= ~PropFlags.FreeVert;
                PropFlags |= PropFlags.FreeHoriz;
            }
            else
            {
                PropFlags &= ~PropFlags.FreeHoriz;
                PropFlags |= PropFlags.FreeVert;
            }
            int num = max - min + 1;
            FreeTotal = num;
            FreeVisible = 1;
            FreeTop = (level - min);
        }

        public override void Layout()
        {
            base.Layout();
            WriteChanges();
        }

        public override void Notify()
        {
            base.Notify();
            ReadChanges();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetType().Name);
            sb.Append(": ");
            sb.Append(Text);
            sb.Append(" L:");
            sb.Append(Level);
            sb.Append("/");
            sb.Append(max);
            return sb.ToString();
        }
    }
}
