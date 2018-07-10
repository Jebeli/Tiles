using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Events
{
    public class EventTile
    {
        private int x;
        private int y;
        private EventLayer eventLayer;
        private List<Event> events;

        public EventTile(int x, int y, EventLayer layer)
        {
            this.x = x;
            this.y = y;
            eventLayer = layer;
            events = new List<Event>();
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

        public void Clear()
        {
            events.Clear();
        }

        public void AddEvent(Event evt)
        {
            events.Add(evt);
        }

        public void RemoveEvent(Event evt)
        {
            events.Remove(evt);
        }
    }
}
