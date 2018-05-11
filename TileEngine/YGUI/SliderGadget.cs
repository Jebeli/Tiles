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

    public class SliderGadget : PropGadget
    {
        private Orientation orientation;
        private int min;
        private int max;
        private int value;
        private int increment;
        private Gadget labelGadget;
        private string labelFormat;

        public SliderGadget(Gadget parent, Orientation orientation)
            : base(parent)
        {
            labelFormat = "{0}";
            increment = 1;
            this.orientation = orientation;
            FreeHoriz = orientation == Orientation.Horizontal;
            FreeVert = orientation == Orientation.Vertical;
        }

        public Gadget LabelGadget
        {
            get { return labelGadget; }
            set
            {
                labelGadget = value;
                AdjustLabel();
            }
        }

        public string LabelFormat
        {
            get { return labelFormat; }
            set
            {
                labelFormat = value;
                AdjustLabel();
            }

        }
        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    FreeHoriz = orientation == Orientation.Horizontal;
                    FreeVert = orientation == Orientation.Vertical;
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

        private void AdjustLabel()
        {
            if (labelGadget != null)
            {
                string txt = string.Format(labelFormat, value);
                labelGadget.Label = txt;
            }
        }

        protected override void OnPropChanged()
        {
            int total = max - min + 1;
            int pot = orientation == Orientation.Horizontal ? HorizPot : VertPot;
            value = FindSliderLevel(total, pot) + min;
            AdjustLabel();
            base.OnPropChanged();
        }

        private void AdjustProp()
        {
            int total = max - min + 1;
            FindSliderValues(total, value + min, out int body, out int pot);
            ModifyProp(FreeHoriz, FreeVert, pot, pot, body, body);

        }

        private void FindSliderValues(int numLevels, int level, out int body, out int pot)
        {
            if (numLevels > 0)
            {
                body = MAXBODY / numLevels;
            }
            else
            {
                body = MAXBODY;
            }
            if (numLevels > 1)
            {
                pot = (MAXPOT * level) / (numLevels - 1);
            }
            else
            {
                pot = 0;
            }
        }

        private int FindSliderLevel(int numLevels, int pot)
        {
            if (numLevels > 1)
            {
                return (pot * (numLevels - 1) + MAXPOT / 2) / MAXPOT;
            }
            else
            {
                return 0;
            }
        }
    }
}
