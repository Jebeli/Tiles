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
    }
}
