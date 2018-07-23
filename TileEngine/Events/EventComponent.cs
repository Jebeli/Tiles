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

namespace TileEngine.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EventComponent
    {
        private EventComponentType type;
        private string stringParam;
        private int intParam;
        private int mapX;
        private int mapY;
        private int mapWidth;
        private int mapHeight;
        private List<MapMod> mapMods;
        private List<MapSpawn> mapSpawns;
        private List<string> stringParams;

        public EventComponent()
        {
            mapMods = new List<MapMod>();
            mapSpawns = new List<MapSpawn>();
            stringParams = new List<string>();
        }

        public EventComponent(EventComponentType type, IList<string> sp)
        {
            this.type = type;
            stringParams = new List<string>(sp);
        }

        public EventComponent(EventComponentType type, string sp)
        {
            this.type = type;
            stringParam = sp;
        }

        public EventComponent(EventComponentType type, int ip)
        {
            this.type = type;
            intParam = ip;
        }

        public EventComponentType Type
        {
            get { return type; }
        }

        public string StringParam
        {
            get { return stringParam; }
            set { stringParam = value; }
        }

        public int IntParam
        {
            get { return intParam; }
            set { intParam = value; }
        }

        public bool BoolParam
        {
            get { return intParam != 0; }
            set
            {
                if (value) intParam = 1;
                else intParam = 0;
            }
        }

        public IList<string> StringParams
        {
            get { return stringParams; }
            set
            {
                stringParams.Clear();
                if (value != null)
                {
                    stringParams.AddRange(value);
                }
            }
        }

        public int MapX
        {
            get { return mapX; }
            set { mapX = value; }
        }

        public int MapY
        {
            get { return mapY; }
            set { mapY = value; }
        }

        public int MapWidth
        {
            get { return mapWidth; }
            set { mapWidth = value; }
        }

        public int MapHeight
        {
            get { return mapHeight; }
            set { mapHeight = value; }
        }

        public List<MapMod> MapMods
        {
            get { return mapMods; }
            set { mapMods = value; }
        }

        public List<MapSpawn> MapSpawns
        {
            get { return mapSpawns; }
            set { mapSpawns = value; }
        }

        public string GetIniTypeString()
        {
            switch (type)
            {
                case EventComponentType.RequiresNotStatus:
                    return "requires_not_status";
                case EventComponentType.RequiresStatus:
                    return "requires_status";
                case EventComponentType.SetStatus:
                    return "set_status";
                case EventComponentType.UnsetStatus:
                    return "unset_status";
                default:
                    return type.ToString().ToLowerInvariant();
            }
        }

        public string ToIniString()
        {
            switch (type)
            {
                case EventComponentType.MapMod:
                    string mm = "";
                    foreach (var mapMod in mapMods)
                    {
                        mm += $"{mapMod.Layer},{mapMod.MapX},{mapMod.MapY},{mapMod.Value}";
                        mm += ";";
                    }
                    mm = mm.TrimEnd(';');
                    return mm;
                case EventComponentType.Spawn:
                    string sp = "";
                    foreach (var spawn in mapSpawns)
                    {
                        sp += $"{spawn.Type},{spawn.MapX},{spawn.MapY}";
                        sp += ";";
                    }
                    sp = sp.TrimEnd(';');
                    return sp;
                case EventComponentType.InterMap:
                    return $"{stringParam},{mapX},{mapY}";
                case EventComponentType.IntraMap:
                    return $"{mapX},{mapY}";
                case EventComponentType.SoundFX:
                    if (mapX == -1 && mapY == -1)
                    {
                        return stringParam;
                    }
                    else
                    {
                        return $"{stringParam},{mapX},{mapY}";
                    }
                case EventComponentType.ShakyCam:
                    return intParam.ToString();
                case EventComponentType.Msg:
                case EventComponentType.Tooltip:
                case EventComponentType.Music:
                    return stringParam;
                case EventComponentType.Repeat:
                case EventComponentType.Stash:
                    return BoolParam ? "true" : "false";
                case EventComponentType.RequiresStatus:
                case EventComponentType.RequiresNotStatus:
                case EventComponentType.SetStatus:
                case EventComponentType.UnsetStatus:
                    StringBuilder sb = new StringBuilder();
                    foreach (var s in stringParams)
                    {
                        sb.Append(s);
                        sb.Append(",");
                    }
                    if (sb.Length > 1) sb.Length -= 1;
                    return sb.ToString();
            }
            return "";
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(type);
            sb.Append(": ");
            switch (type)
            {
                case EventComponentType.InterMap:
                    sb.Append(stringParam);
                    sb.Append(" (");
                    sb.Append(mapX);
                    sb.Append("/");
                    sb.Append(mapY);
                    sb.Append(")");
                    break;
                case EventComponentType.IntraMap:
                    sb.Append("(");
                    sb.Append(mapX);
                    sb.Append("/");
                    sb.Append(mapY);
                    sb.Append(")");
                    break;
                case EventComponentType.MapMod:
                    sb.Append(mapMods.Count);
                    sb.Append(" tile");
                    if (mapMods.Count > 1)
                    {
                        sb.Append("s");
                    }
                    break;
                case EventComponentType.Spawn:
                    sb.Append(mapSpawns.Count);
                    sb.Append(" spwan");
                    if (mapSpawns.Count > 1)
                    {
                        sb.Append("s");
                    }
                    break;
                case EventComponentType.ShakyCam:
                    sb.Append(intParam);
                    break;
                case EventComponentType.Repeat:
                case EventComponentType.Stash:
                    sb.Append(BoolParam ? "true" : "false");
                    break;
                case EventComponentType.Msg:
                case EventComponentType.SoundFX:
                case EventComponentType.Tooltip:
                default:
                    if (stringParams != null && stringParams.Count > 0)
                    {
                        sb.Append(string.Join(",", stringParams));
                    }
                    else
                    {
                        sb.Append(stringParam);

                    }
                    break;
            }
            return sb.ToString();
        }
    }
}
