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

    public class Border
    {
        private int leftEdge;
        private int topEdge;
        private Color frontPen;
        private Color backPen;
        private List<int> xy;
        private Border nextBorder;

        public Border()
        {
            xy = new List<int>();
        }
        public Border(int width, int height)
        {
            xy = new List<int>();
            xy.Add(0);
            xy.Add(height - 1);
            xy.Add(0);
            xy.Add(0);
            xy.Add(width - 1);
            xy.Add(0);
            frontPen = Intuition.ColorLightEdge;
            nextBorder = new Border();
            nextBorder.xy.Add(width - 1);
            nextBorder.xy.Add(0);
            nextBorder.xy.Add(width - 1);
            nextBorder.xy.Add(height - 1);
            nextBorder.xy.Add(0);
            nextBorder.xy.Add(height - 1);
            nextBorder.frontPen = Intuition.ColorDarkEdge;
        }

        public Color FrontPen
        {
            get { return frontPen; }
            set { frontPen = value; }
        }

        public Color BackPen
        {
            get { return backPen; }
            set { backPen = value; }
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

        public List<int> XY
        {
            get { return xy; }
            set { xy = value; }
        }

        public Border NextBorder
        {
            get { return nextBorder; }
            set { nextBorder = value; }
        }
    }
}
