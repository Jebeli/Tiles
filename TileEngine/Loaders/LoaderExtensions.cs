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

namespace TileEngine.Loaders
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Graphics;

    static class LoaderExtensions
    {
        public static int ToIntValue(this string v)
        {
            int res = 0;
            if (v != null)
            {
                int.TryParse(v, out res);
            }
            return res;
        }

        public static float ToFloatValue(this string v)
        {
            float res = 0;
            if (v != null)
            {
                float.TryParse(v, out res);
            }
            return res;
        }

        public static int[] ToIntValues(this string v)
        {
            List<int> list = new List<int>();
            if (v != null)
            {
                foreach (var s in v.Split(','))
                {
                    int i;
                    if (int.TryParse(s, out i))
                    {
                        list.Add(i);
                    }
                }
            }
            return list.ToArray();
        }

        public static string[] ToStrValues(this string v, char sep = ',')
        {
            List<string> list = new List<string>();
            if (v != null)
            {
                foreach (var s in v.Split(sep))
                {
                    var ss = s.Trim();
                    if (!string.IsNullOrEmpty(ss))
                    {
                        list.Add(ss);
                    }
                }
            }
            return list.ToArray();
        }

        public static Color ToColor(this string v)
        {

            byte r = 0;
            byte g = 0;
            byte b = 0;
            byte a = 255;
            int[] values = v.ToIntValues();
            if (values.Length >= 3)
            {
                r = (byte)values[0];
                g = (byte)values[1];
                b = (byte)values[2];
            }
            if (values.Length >= 4)
            {
                a = (byte)values[3];
            }
            return new Color(r, g, b, a);
        }
    }
}
