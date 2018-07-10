using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Events
{
    public class EventManager
    {
        private Engine engine;
        private List<Event> events;
        public EventManager(Engine engine)            
        {
            this.engine = engine;
            events = new List<Event>();
        }

        public void Clear()
        {
            events.Clear();
        }

        public void AddEvent(Event evt)
        {
            events.Add(evt);
            CacheEvent(evt);
        }

        public void RemoveEvent(Event evt)
        {
            if (evt == null) return;
            if (evt != null && events.Remove(evt))
            {
                foreach (var pt in evt.GetMatchingPositions())
                {
                    engine.Map.EventLayer.RemoveEvent(evt, pt.X, pt.Y);
                }
                foreach (var pt in evt.GetMatchingHotSpotPositions())
                {
                    //Tile tile = engine.Map.GetTileAt(pt.X, pt.Y);
                    //if (tile != null)
                    //{
                    //    tile.RemoveHotSpotEvent(evt);
                    //}
                }
            }
        }

        private void CacheEvent(Event evt)
        {
            foreach (var pt in evt.GetMatchingPositions())
            {
                engine.Map.EventLayer.AddEvent(evt, pt.X, pt.Y);
            }
            foreach (var pt in evt.GetMatchingHotSpotPositions())
            {
                //Tile tile = engine.Map.GetTileAt(pt.X, pt.Y);
                //if (tile != null)
                //{
                //    tile.AddHotSpotEvent(evt);
                //}
            }
        }
    }
}
