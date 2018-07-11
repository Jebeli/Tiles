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
    using TileEngine.Core;

    public class Event : NamedObject
    {
        private Engine engine;
        private EventType type;
        private int posX;
        private int posY;
        private int width;
        private int height;
        private int hotPosX;
        private int hotPosY;
        private int hotWidth;
        private int hotHeight;
        private int cooldown;
        private int cooldownTicks;
        private bool repeat = true;
        private bool hotspot = false;
        private bool removeNow;
        private List<EventComponent> components;

        public Event(Engine engine, EventType type, string name)
            : base(name)
        {
            this.engine = engine;
            this.type = type;
            components = new List<EventComponent>();
            cooldown = 0;
            repeat = true;
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

        public bool RemoveNow
        {
            get { return removeNow; }
            set { removeNow = value; }
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

        public bool IsInCooldown
        {
            get { return cooldownTicks > 0; }
        }

        public int Cooldown
        {
            get { return cooldown; }
            set { cooldown = value; }
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

        public void Update()
        {
            if (cooldownTicks > 0) cooldownTicks--;
        }

        public void Execute()
        {
            foreach(var ec in components)
            {
                engine.ExecuteEventComponent(this, ec);
            }
            cooldownTicks = cooldown;
            if (!repeat) removeNow = true;
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
