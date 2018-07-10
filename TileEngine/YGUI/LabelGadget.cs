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

    public class LabelGadget : Gadget
    {
        private bool frame;

        public LabelGadget(Gadget parent, string label)
            : base(parent, label)
        {
            TransparentBackground = true;
        }

        public bool Frame
        {
            get { return frame; }
            set { frame = value; }
        }

        public override Point GetPreferredSize(IGraphics gfx)
        {
            int textWidth = gfx.MeasureTextWidth(Font, Label);
            return new Point(textWidth + 4, 24);
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

        public override string ToString()
        {
            return $"Label {Label}";
        }
    }
}
