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

namespace TileEngine.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Graphics;

    public class TextTitleScreen : AbstractScreen
    {
        protected string title;
        public TextTitleScreen(Engine engine, string name, string title) : base(engine, name)
        {
            this.title = title;
        }

        public override void Render(TimeInfo time)
        {
            engine.Graphics.RenderText(Font, title, engine.Graphics.ViewWidth / 2, engine.Graphics.ViewHeight / 2, Color.White);
            base.Render(time);
        }

    }
}
