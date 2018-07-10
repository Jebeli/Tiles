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
    using TileEngine.Logging;

    public class ButtonGadget : Gadget
    {
        private bool repeat;
        private int timerDelay;

        public ButtonGadget(Gadget parent, Icons icon = Icons.NONE)
            : base(parent, "", icon)
        {
            Size = new Point(24, 24);
        }

        public ButtonGadget(Gadget parent, string label, Icons icon = Icons.NONE)
            : base(parent, label, icon)
        {
            Size = new Point(64, 24);
        }

        public bool Repeat
        {
            get { return repeat; }
            set { repeat = value; }
        }

        public override Point GetPreferredSize(IGraphics gfx)
        {
            Point res = new Point(24, 24);
            res.X = Math.Max(gfx.MeasureTextWidth(Font, Label) + 8, res.X);
            return res;
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

        protected override void HandleSelectDown(Point p)
        {
            timerDelay = 2;
            base.HandleSelectDown(p);
        }

        protected override void HandleSelectUp(Point p)
        {
            if (Sticky)
            {
                Selected = !Selected;
            }
            base.HandleSelectUp(p);
        }

        public override void HandleTimer(Point p, MouseButton button)
        {
            timerDelay--;
            if (timerDelay <= 0)
            {
                timerDelay = 0;
                if (repeat && Enabled && button == MouseButton.Left && MouseSelected)
                {
                    OnGadgetUp();
                }
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Label))
            {
                return $"Button {Label}";
            }
            if (Icon != Icons.NONE)
            {
                return $"Button {Icon}";
            }
            return "Button";
        }

    }
}
