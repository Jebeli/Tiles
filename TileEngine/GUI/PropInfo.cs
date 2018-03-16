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

namespace TileEngine.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PropInfo
    {
        public const int KNOBHMIN = 6;
        public const int KNOBVMIN = 4;
        public const int MAXBODY = 0xFFFF;
        public const int MAXPOT = 0xFFFF;

        private PropFlags flags;
        private int horizPot;
        private int vertPot;
        private int horizBody;
        private int vertBody;
        private int cwidth;
        private int cheight;
        private int hpotres;
        private int vpotres;
        private int leftBorder;
        private int topBorder;

        public PropFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public int HorizPot
        {
            get { return horizPot; }
            set
            {
                if (value > 0xFFFF) value = 0xFFFF;
                if (value < 0) value = 0;
                horizPot = value;
            }
        }

        public int VertPot
        {
            get { return vertPot; }
            set
            {
                if (value > 0xFFFF) value = 0xFFFF;
                if (value < 0) value = 0;
                vertPot = value;
            }
        }

        public int HorizBody
        {
            get { return horizBody; }
            set { horizBody = value; }
        }

        public int VertBody
        {
            get { return vertBody; }
            set { vertBody = value; }
        }

        public int CWidth
        {
            get { return cwidth; }
            set { cwidth = value; }
        }

        public int CHeight
        {
            get { return cheight; }
            set { cheight = value; }
        }

        public int HPotRes
        {
            get { return hpotres; }
            set { hpotres = value; }
        }

        public int VPotRes
        {
            get { return vpotres; }
            set { vpotres = value; }
        }

        public int LeftBorder
        {
            get { return leftBorder; }
            set { leftBorder = value; }
        }

        public int TopBorder
        {
            get { return topBorder; }
            set { topBorder = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Flags);
            sb.Append(" ");
            sb.Append($"{HorizPot}/{HorizBody} {VertPot}/{VertBody}");
            return sb.ToString();
        }

    }
}
