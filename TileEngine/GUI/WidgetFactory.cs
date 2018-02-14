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
    using Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public static class WidgetFactory
    {
        public static void AddDemoButtons(Engine engine, IWidgetContainer container)
        {
            NinePatch button9P = GetNinePatch(engine, "gui/buttonMid.png");
            NinePatch button9PH = GetNinePatch(engine, "gui/buttonLight.png");
            NinePatch button9PP = GetNinePatch(engine, "gui/buttonDark.png");
            NinePatch window9P = GetNinePatch(engine, "gui/window.png");
            WidgetWindow window1 = new WidgetWindow();
            window1.SetBounds(10, 10, 3 * 64 + 10, 32 + 10);
            window1.Patch = window9P;
            window1.PatchHover = window9P;
            window1.PatchPressed = window9P;
            WidgetButton button1 = new WidgetButton("Exit");
            button1.SetBounds(5, 5, 64, 32);
            button1.Patch = button9P;
            button1.PatchHover = button9PH;
            button1.PatchPressed = button9PP;
            WidgetButton button2 = new WidgetButton("Load");
            button2.SetBounds(5 + 64, 5, 64, 32);
            button2.Patch = button9P;
            button2.PatchHover = button9PH;
            button2.PatchPressed = button9PP;
            WidgetButton button3 = new WidgetButton("Save");
            button3.SetBounds(5 + 64 * 2, 5, 64, 32);
            button3.Patch = button9P;
            button3.PatchHover = button9PH;
            button3.PatchPressed = button9PP;
            window1.AddWidget(button1);
            window1.AddWidget(button2);
            window1.AddWidget(button3);
            container.AddWidget(window1);
        }

        public static NinePatch GetNinePatch(Engine engine, string imageId)
        {
            Texture tex = engine.GetTexture(imageId);
            NinePatch patch = new NinePatch(tex, 8, 8, 20, 20);
            return patch;
        }
    }
}
