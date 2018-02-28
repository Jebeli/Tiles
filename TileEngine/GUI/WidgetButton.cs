﻿/*
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
    using TileEngine.Core;
    using TileEngine.Graphics;
    public class WidgetButton : Widget
    {
        private string text;

        public WidgetButton(string text = "")
            : this(WidgetFactory.Button9P, WidgetFactory.Button9PHover, WidgetFactory.Button9PPressed, text)
        {
        }
        public WidgetButton(NinePatch patch, NinePatch patchHover, NinePatch patchPressed, string text = "")
            : base(patch, patchHover, patchPressed)
        {
            this.text = text;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        protected override void Draw(IGraphics graphics, int x, int y, int width, int height)
        {
            if (!DrawNinePatch(graphics, x, y, width, height))
            {
                graphics.RenderWidget(x, y, width, height, Enabled, Hover, Pressed);
            }
            graphics.RenderText(text, x + width / 2, y + height / 2, GetTextColor());

        }

        protected override void BoundsChanged()
        {
        }

        public override string ToString()
        {
            return "Button " + text;
        }
    }
}
