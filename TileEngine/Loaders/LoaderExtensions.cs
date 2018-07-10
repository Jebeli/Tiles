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

namespace TileEngine.Loaders
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Events;
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

        public static EventType ToEventType(this string str)
        {
            if (str != null)
            {
                switch (str)
                {
                    case "on_trigger":
                        return EventType.Trigger;
                    case "on_load":
                        return EventType.Load;
                    case "on_mapexit":
                        return EventType.Exit;
                    case "on_leave":
                        return EventType.Leave;
                    case "on_clear":
                        return EventType.Clear;
                    case "static":
                        return EventType.Static;
                }
            }
            return EventType.None;
        }

        public static int ToDuration(this string v, int maxFramesPerSecond = 60)
        {
            int val = 0;
            float div = 1;
            if (v != null)
            {
                if (v.EndsWith("ms", StringComparison.OrdinalIgnoreCase))
                {
                    val = ToIntValue(v.Substring(0, v.Length - 2));
                    div = 1000;
                }
                else if (v.EndsWith("s", StringComparison.OrdinalIgnoreCase))
                {
                    val = ToIntValue(v.Substring(0, v.Length - 1));
                    div = 1;
                }
                else
                {
                    val = ToIntValue(v);
                    div = 1;
                }
            }
            if (val == 0) return 0;
            val = (int)(val * maxFramesPerSecond / div + 0.5f);
            if (val < 1) val = 1;
            return val;
        }
    }
}
