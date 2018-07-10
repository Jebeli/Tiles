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
    using TileEngine.Input;

    public class CheckBoxGadget : Gadget
    {
        private bool _checked;
        public CheckBoxGadget(Gadget parent, string label)
            : base(parent, label)
        {
            Size = new Point(64, 24);
        }

        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

        public override Point GetPreferredSize(IGraphics gfx)
        {
            int textWidth = gfx.MeasureTextWidth(Font, Label);
            return new Point(textWidth + 24 + 2 * 4, 24);            
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

        protected override void HandleSelectUp(Point p)
        {
            _checked = !_checked;
            base.HandleSelectUp(p);
        }

        public override string ToString()
        {
            return $"CheckBox {Label}";
        }
    }
}
