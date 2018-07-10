using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.YGUI
{
    public class NumericalGadget : Gadget
    {
        private const int BUT_SIZE = 14;
        private StrGadget strGadget;
        private ButtonGadget upGadget;
        private ButtonGadget downGadget;
        private double increment;

        public NumericalGadget(Gadget parent)
            : base(parent)
        {
            increment = 0.1;
            strGadget = new StrGadget(this)
            {
                Flags = StrFlags.Double,
                GadgetUpEvent = (o,i) => { OnGadgetUp(); }
            };
            upGadget = new ButtonGadget(this, Icons.ENTYPO_ICON_PLUS)
            {
                Repeat = true,
                Width = BUT_SIZE,
                Height = BUT_SIZE,
                GadgetUpEvent = (o,i) => { IncValue(); }
            };
            downGadget = new ButtonGadget(this, Icons.ENTYPO_ICON_MINUS)
            {
                Repeat = true,
                Width = BUT_SIZE,
                Height = BUT_SIZE,
                GadgetUpEvent = (o, i) => { DecValue(); }
            };
        }

        public double Increment
        {
            get { return increment; }
            set { increment = value; }
        }

        public bool IsDouble
        {
            get { return strGadget.Flags.HasFlag(StrFlags.Double); }
        }

        public bool IsInteger
        {
            get { return strGadget.Flags.HasFlag(StrFlags.Integer); }
        }

        public double DoubleValue
        {
            get { return strGadget.DoubleValue; }
            set { strGadget.DoubleValue = value; }
        }

        public int IntValue
        {
            get { return strGadget.IntValue; }
            set { strGadget.IntValue = value; }
        }

        private void IncValue()
        {
            if (IsDouble)
            {
                DoubleValue += increment;
                OnGadgetUp();
            }
            else
            {
                IntValue += (int)increment;
                OnGadgetUp();
            }
        }

        private void DecValue()
        {
            if (IsDouble)
            {
                DoubleValue -= increment;
                OnGadgetUp();
            }
            else
            {
                IntValue -= (int)increment;
                OnGadgetUp();
            }
        }

        public override Point GetPreferredSize(IGraphics gfx)
        {
            Point ps = strGadget.GetPreferredSize(gfx);
            ps.X += upGadget.Width;
            ps.Y = Math.Max(ps.Y, upGadget.Height +downGadget.Height);
            return ps;
        }

        public override void PerformLayout(IGraphics gfx)
        {
            upGadget.LeftEdge = Width - upGadget.Width;
            downGadget.LeftEdge = Width - downGadget.Width;
            downGadget.TopEdge = upGadget.Height;
            strGadget.Width = Width - upGadget.Width;
            strGadget.Height = Height;
            foreach (var c in Children)
            {
                if (c.Visible)
                {
                    c.PerformLayout(gfx);
                }
            }
        }
    }
}
