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
    using TileEngine.Graphics;

    [Flags]
    public enum ReqFlags
    {
        POINTREL = 0x1,
        PREDRAWN = 0x2,
        REQOFFWINDOW = 0x4,
        REQACTIVE = 0x8,
        SYSREQUEST = 0x10
    }

    public class Requester
    {
        private Requester olderRequest;
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private int relLeft;
        private int relTop;
        private List<Gadget> reqGadgets;
        private IntuiText reqText;
        private Border reqBorder;
        private ReqFlags flags;
        private Color backFill;

        public Requester()
        {
            reqGadgets = new List<Gadget>();
        }

        public Requester OlderRequest
        {
            get { return olderRequest; }
            internal set { olderRequest = value; }
        }

        public int LeftEdge
        {
            get { return leftEdge; }
            set { leftEdge = value; }
        }

        public int TopEdge
        {
            get { return topEdge; }
            set { topEdge = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int RelLeft
        {
            get { return relLeft; }
            set { relLeft = value; }
        }

        public int RelTop
        {
            get { return relTop; }
            set { relTop = value; }
        }

        public IList<Gadget> ReqGadgets
        {
            get { return reqGadgets; }
            set
            {
                reqGadgets = new List<Gadget>(value);
            }
        }

        public IntuiText ReqText
        {
            get { return reqText; }
            set { reqText = value; }
        }

        public Border ReqBorder
        {
            get { return reqBorder; }
            set { reqBorder = value; }
        }

        public ReqFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public Color BackFill
        {
            get { return backFill; }
            set { backFill = value; }
        }

    }
}
