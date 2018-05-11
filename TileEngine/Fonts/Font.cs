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

namespace TileEngine.Fonts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Resources;


    public class Font : Resource
    {
        private string fontName;
        private int fontSize;
        public Font(string name, int size)
            : base(GetFontName(name, size))
        {
            fontName = name;
            fontSize = size;
        }

        public string FontName
        {
            get { return fontName; }
        }

        public int FontSize
        {
            get { return fontSize; }
        }

        public static string GetFontName(string name, int size)
        {
            return name + "_" + size;
        }
    }
}
