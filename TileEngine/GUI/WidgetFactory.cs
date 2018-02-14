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
    public static class WidgetFactory
    {
        public static void AddDemoButtons(IWidgetContainer container)
        {
            WidgetButton button1 = new WidgetButton("Exit");
            button1.SetBounds(0, 0, 64, 32);
            WidgetButton button2 = new WidgetButton("Load");
            button2.SetBounds(64, 0, 64, 32);
            WidgetButton button3 = new WidgetButton("Save");
            button3.SetBounds(64 * 2, 0, 64, 32);
            container.AddWidget(button1);
            container.AddWidget(button2);
            container.AddWidget(button3);
        }
    }
}
