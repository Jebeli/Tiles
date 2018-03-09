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

    public class IntuiText
    {
        private Color frontPen;
        private Color backPen;
        private int leftEdge;
        private int topEdge;
        private string itext;
        private HorizontalTextAlign horizontalTextAlign;
        private VerticalTextAlign verticalTextAlign;
        private IntuiText nextText;

        public IntuiText(string text)
        {
            frontPen = Intuition.ColorText;
            backPen = Intuition.ColorWindow;
            horizontalTextAlign = HorizontalTextAlign.Center;
            verticalTextAlign = VerticalTextAlign.Center;
            itext = text;
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

        public string IText
        {
            get { return itext; }
            set { itext = value; }
        }

        public IntuiText NextText
        {
            get { return nextText; }
            set { nextText = value; }
        }

        public VerticalTextAlign VerticalTextAlign
        {
            get { return verticalTextAlign; }
            set { verticalTextAlign = value; }
        }

        public HorizontalTextAlign HorizontalTextAlign
        {
            get { return horizontalTextAlign; }
            set { horizontalTextAlign = value; }
        }
    }
}
