using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;

namespace TileEngine.Events
{
    public class Event : NamedObject
    {
        private EventType type;
        private int posX;
        private int posY;
        private int width;
        private int height;
        private int hotPosX;
        private int hotPosY;
        private int hotWidth;
        private int hotHeight;
        private bool repeat = true;
        private bool hotspot = false;
        private List<EventComponent> components;
        private double cooldown;
        private double lastExecTime;

        public Event(EventType type, string name)
            : base(name)
        {
            this.type = type;
            components = new List<EventComponent>();
            cooldown = 0.5;
        }

        public EventType Type
        {
            get { return type; }
        }

        public bool HotSpot
        {
            get { return hotspot; }
            set { hotspot = value; }
        }

        public double LastExecTime
        {
            get { return lastExecTime; }
            set { lastExecTime = value; }
        }

        public IEnumerable<EventComponent> Components
        {
            get { return new List<EventComponent>(components); }
        }

        public EventComponent AddComponent(EventComponent ec)
        {
            components.Add(ec);
            return ec;
        }

        public EventComponent AddComponent(EventComponentType ect, string sp = null)
        {
            return AddComponent(new EventComponent(ect, sp));
        }

        public EventComponent AddComponent(EventComponentType ect, int ip = 0)
        {
            return AddComponent(new EventComponent(ect, ip));
        }

        public int PosX
        {
            get { return posX; }
            set { posX = value; }
        }

        public int PosY
        {
            get { return posY; }
            set { posY = value; }
        }

        public float CenterX
        {
            get { return posX + width / 2.0f; }
        }
        public float CenterY
        {
            get { return posY + height / 2.0f; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int HotPosX
        {
            get { return hotPosX; }
            set { hotPosX = value; }
        }

        public int HotPosY
        {
            get { return hotPosY; }
            set { hotPosY = value; }
        }

        public int HotWidth
        {
            get { return hotWidth; }
            set { hotWidth = value; }
        }

        public int HotHeight
        {
            get { return hotHeight; }
            set { hotHeight = value; }
        }

        public bool IsInCooldown(TimeInfo time)
        {
            if (time == null) return false;
            return (time.TotalGameTime.TotalSeconds - lastExecTime) < cooldown;
        }

        public bool Repeat
        {
            get { return repeat; }
            set { repeat = value; }
        }

        public void HotSpotFromLocation()
        {
            hotPosX = posX;
            hotPosY = posY;
            hotWidth = width;
            hotHeight = height;
        }

        public IList<Point> GetMatchingPositions()
        {
            List<Point> list = new List<Point>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Point p = new Point(posX + x, posY + y);
                    list.Add(p);
                }
            }
            return list;
        }

        public IList<Point> GetMatchingHotSpotPositions()
        {
            List<Point> list = new List<Point>();
            if (hotspot)
            {
                for (int x = 0; x < hotWidth; x++)
                {
                    for (int y = 0; y < hotHeight; y++)
                    {
                        Point p = new Point(hotPosX + x, hotPosY + y);
                        list.Add(p);
                    }
                }
            }
            return list;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(type.ToString());
            sb.Append(" ");
            sb.Append(Name);
            sb.Append(": ");
            sb.Append("(");
            sb.Append(posX);
            sb.Append("/");
            sb.Append(posY);
            sb.Append(") ");
            foreach (EventComponent ec in components)
            {
                sb.Append(ec.ToString());
                sb.Append(" ");
            }
            sb.Length -= 1;
            return sb.ToString();
        }
    }
}
