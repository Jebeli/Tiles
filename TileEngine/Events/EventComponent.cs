using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Events
{
    public class EventComponent
    {
        private EventComponentType type;
        private string stringParam;
        private int intParam;
        private int mapX;
        private int mapY;
        private List<MapMod> mapMods;
        private List<MapSpawn> mapSpawns;
        public EventComponent()
        {
            mapMods = new List<MapMod>();
            mapSpawns = new List<MapSpawn>();
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
                case EventComponentType.Msg:
                case EventComponentType.SoundFX:
                case EventComponentType.Tooltip:
                default:
                    sb.Append(stringParam);
                    break;
            }
            return sb.ToString();
        }
    }
}
