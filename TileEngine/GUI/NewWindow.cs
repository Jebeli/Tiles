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
    using TileEngine.Screens;

    public class NewWindow
    {
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private IDCMPFlags idcmpflags;
        private WindowFlags flags;
        private string title;
        private IList<Gadget> gadgets = new List<Gadget>();
        private int minWidth;
        private int minHeight;
        private int maxWidth;
        private int maxHeight;
        private IScreen screen;

        public NewWindow()
        {
            width = 100;
            height = 100;
            leftEdge = 0;
            topEdge = 0;
            minWidth = 0;
            minHeight = 0;
            maxWidth = ushort.MaxValue;
            maxHeight = ushort.MaxValue;
        }

        public IScreen Screen
        {
            get { return screen; }
            set { screen = value; }
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

        public IDCMPFlags IDCMPFlags
        {
            get { return idcmpflags; }
            set { idcmpflags = value; }
        }

        public WindowFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public IList<Gadget> Gadgets
        {
            get { return gadgets; }
            set
            {
                gadgets = new List<Gadget>(value);
            }
        }

        public int MinWidth
        {
            get { return minWidth; }
            set { minWidth = value; }
        }
        public int MinHeight
        {
            get { return minHeight; }
            set { minHeight = value; }
        }
        public int MaxWidth
        {
            get { return maxWidth; }
            set { maxWidth = value; }
        }
        public int MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = value; }
        }

    }
}
