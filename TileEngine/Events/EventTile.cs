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

    public class EventTile
    {
        private int x;
        private int y;
        private EventLayer eventLayer;
        private List<Event> events;
        //private List<Event> hotspotEvents;

        public EventTile(int x, int y, EventLayer layer)
        {
            this.x = x;
            this.y = y;
            eventLayer = layer;
            events = new List<Event>();
            //hotspotEvents = new List<Event>();
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public EventLayer EventLayer
        {
            get { return eventLayer; }
        }

        public IList<Event> Events
        {
            get { return events; }
        }

        //public IList<Event> HotSpotEvents
        //{
        //    get { return hotspotEvents; }
        //}

        public void Clear()
        {
            events.Clear();
            //hotspotEvents.Clear();
        }

        public void AddEvent(Event evt)
        {
            events.Add(evt);
        }

        public void RemoveEvent(Event evt)
        {
            events.Remove(evt);
        }

        //public void AddHotSpotEvent(Event evt)
        //{
        //    hotspotEvents.Add(evt);
        //}

        //public void RemoveHotSpotEvent(Event evt)
        //{
        //    hotspotEvents.Remove(evt);
        //}
    }
}
