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
        private static NinePatch button9P;
        private static NinePatch button9PH;
        private static NinePatch button9PP;
        private static NinePatch window9P;


        public static NinePatch Button9P
        {
            get { return button9P; }
        }

        public static NinePatch Button9PHover
        {
            get { return button9PH; }
        }

        public static NinePatch Button9PPressed
        {
            get { return button9PP; }
        }

        public static NinePatch Window9P
        {
            get { return window9P; }
        }
        public static void InitDefault(Engine engine)
        {
            button9P = GetNinePatch(engine, "gui/buttonMid.png");
            button9PH = GetNinePatch(engine, "gui/buttonLight.png");
            button9PP = GetNinePatch(engine, "gui/buttonDark.png");
            window9P = GetNinePatch(engine, "gui/window.png");

        }
        public static void AddDemoButtons(Engine engine, IWidgetContainer container)
        {
            WidgetWindow window1 = new WidgetWindow();
            window1.SetBounds(10, 10, 3 * 64 + 10, 32 + 10);
            WidgetButton button1 = new WidgetButton("Exit");
            button1.SetBounds(5, 5, 64, 32);
            WidgetButton button2 = new WidgetButton("Load");
            button2.SetBounds(5 + 64, 5, 64, 32);
            WidgetButton button3 = new WidgetButton("Save");
            button3.SetBounds(5 + 64 * 2, 5, 64, 32);
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
