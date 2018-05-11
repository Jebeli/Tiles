/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
 */

namespace TileEngine.YGUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;
    using TileEngine.Graphics;

    public enum ArrowPlace
    {
        None,
        LeftTop,
        RightBottom,
        Split
    }

    public class ScrollbarGadget : Gadget
    {
        private ArrowPlace arrowPlace;
        private PropGadget prop;
        private ButtonGadget arrowInc;
        private ButtonGadget arrowDec;
        private Orientation orientation;
        private int min;
        private int max;
        private int value;
        private int visibleAmount;
        private int overlap;
        private int increment;

        public ScrollbarGadget(Gadget parent, Orientation orientation = Orientation.Horizontal)
            : base(parent)
        {
            arrowPlace = ArrowPlace.RightBottom;
            overlap = 1;
            increment = 1;
            this.orientation = orientation;
            bool vert = orientation == Orientation.Vertical;
            prop = new PropGadget(this)
            {
                FreeHoriz = !vert,
                FreeVert = vert,
                Borderless = true
            };
            prop.PropChanged += Prop_PropChanged;
            arrowInc = new ButtonGadget(this, vert ? theme.ArrowDown : theme.ArrowRight)
            {
                Width = 20,
                Height = 20,
                Repeat = true,
                Borderless = false
            };
            arrowInc.GadgetUp += ArrowInc_GadgetUp;
            arrowDec = new ButtonGadget(this, vert ? theme.ArrowUp : theme.ArrowLeft)
            {
                Height = 20,
                Width = 20,
                Repeat = true,
                Borderless = false
            };
            arrowDec.GadgetUp += ArrowDec_GadgetUp;
        }

        public ArrowPlace ArrowPlace
        {
            get { return arrowPlace; }
            set { arrowPlace = value; }
        }

        private void Prop_PropChanged(object sender, EventArgs e)
        {
            int total = max - min;
            int pot = orientation == Orientation.Horizontal ? prop.HorizPot : prop.VertPot;
            value = PropGadget.FindScrollerTop(total, visibleAmount, pot);
        }

        private void ArrowDec_GadgetUp(object sender, EventArgs e)
        {
            Value -= increment;
        }

        private void ArrowInc_GadgetUp(object sender, EventArgs e)
        {
            Value += increment;
        }

        public int Increment
        {
            get { return increment; }
            set { increment = value; }
        }

        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    if (orientation == Orientation.Horizontal)
                    {
                        arrowInc.Icon = theme.ArrowRight;
                        arrowDec.Icon = theme.ArrowLeft;
                        prop.FreeHoriz = true;
                        prop.FreeVert = false;
                    }
                    else
                    {
                        arrowInc.Icon = theme.ArrowDown;
                        arrowDec.Icon = theme.ArrowUp;
                        prop.FreeHoriz = false;
                        prop.FreeVert = true;
                    }
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
                    AdjustProp();
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
                    AdjustProp();
                }
            }
        }

        public int Value
        {
            get { return value; }
            set
            {
                if (value > max) value = max;
                if (value < min) value = min;
                if (this.value != value)
                {
                    this.value = value;
                    AdjustProp();
                }
            }
        }

        public int VisibleAmount
        {
            get { return visibleAmount; }
            set
            {
                if (visibleAmount != value)
                {
                    visibleAmount = value;
                    AdjustProp();
                }
            }
        }

        public int Overlap
        {
            get { return overlap; }
            set
            {
                if (overlap != value)
                {
                    overlap = value;
                    AdjustProp();
                }
            }
        }

        private void AdjustProp()
        {

            int total = max - min;
            PropGadget.FindScrollerValues(total, visibleAmount, value, overlap, out int body, out int pot);
            prop.ModifyProp(prop.FreeHoriz, prop.FreeVert, pot, pot, body, body);

        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            Vector2 pSize = prop.GetPreferredSize(gfx);
            if (arrowPlace != ArrowPlace.None)
            {
                if (orientation == Orientation.Horizontal)
                {
                    pSize.X += 2 * PropGadget.PROP_SIZE;
                }
                else
                {
                    pSize.Y += 2 * PropGadget.PROP_SIZE;
                }
            }
            return pSize;
        }

        private Gadget[] GetGadgets()
        {
            switch (arrowPlace)
            {
                case ArrowPlace.Split:
                    return new Gadget[] { arrowDec, prop, arrowInc };
                case ArrowPlace.LeftTop:
                    return new Gadget[] { arrowDec, arrowInc, prop };
                case ArrowPlace.RightBottom:
                    return new Gadget[] { prop, arrowDec, arrowInc };
                case ArrowPlace.None:
                default:
                    return new Gadget[] { prop };
            }
        }

        public override void PerformLayout(IGraphics gfx)
        {
            int x = 1;
            int y = 1;
            int size = PropGadget.PROP_SIZE - 2;
            int propSize = orientation == Orientation.Horizontal ? Width - 2 : Height - 2;
            if (arrowPlace != ArrowPlace.None)
            {
                propSize -= 2 * size;
            }
            arrowDec.Visible = arrowPlace != ArrowPlace.None;
            arrowInc.Visible = arrowPlace != ArrowPlace.None;
            Gadget[] gadgets = GetGadgets();
            foreach (var gad in gadgets)
            {
                if (orientation == Orientation.Horizontal)
                {
                    gad.LeftEdge = x;
                    gad.TopEdge = y;
                    gad.Width = size;
                    gad.Height = size;
                    if (gad is PropGadget)
                    {
                        gad.Width = propSize;
                    }
                    x += gad.Width;
                }
                else
                {
                    gad.LeftEdge = x;
                    gad.TopEdge = y;
                    gad.Width = size;
                    gad.Height = size;
                    if (gad is PropGadget)
                    {
                        gad.Height = propSize;
                    }
                    y += gad.Height;
                }
            }
            foreach (var c in Children)
            {
                if (c.Visible)
                {
                    c.PerformLayout(gfx);
                }
            }
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

        public override string ToString()
        {
            return $"ScrollBar {Value}/{Min}-{Max}";
        }
    }
}
