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
    using TileEngine.Logging;

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

        public void Update(TimeInfo time)
        {
            for (int i = events.Count; i > 0; i--)
            {
                Event evt = events[i - 1];
                if (evt.RemoveNow)
                {
                    RemoveEvent(evt);
                }
                else
                {
                    evt.Update();
                }
            }
        }

        public void ExecuteOnMapLoadEvents()
        {
            foreach (var evt in events)
            {
                if (evt.Type == EventType.Load && IsActive(evt))
                {
                    evt.Execute();
                }
            }
        }

        public void ExecuteOnMapExitEvents()
        {
            foreach (var evt in events)
            {
                if (evt.Type == EventType.Exit && IsActive(evt))
                {
                    evt.Execute();
                }
            }
        }

        public bool IsActive(Event evt)
        {
            return true;
        }

        public void CheckHotSpotClick(float mapX, float mapY)
        {
            var layer = engine.Map.EventLayer;
            int mX = (int)mapX;
            int mY = (int)mapY;
            if (layer.IsValidXY(mX, mY))
            {
                var eventTile = layer[mX, mY];
                foreach (var evt in eventTile.HotSpotEvents)
                {
                    if (!IsActive(evt)) continue;
                    if (evt.IsInCooldown) continue;
                    switch (evt.Type)
                    {
                        case EventType.Trigger:
                        case EventType.None:
                            ExecuteEvent(evt);
                            break;
                    }
                }
            }
        }

        public void CheckHotSpots(float mapX, float mapY)
        {
            var layer = engine.Map.EventLayer;
            var eventTile = layer[(int)mapX, (int)mapY];
            foreach (var evt in eventTile.HotSpotEvents)
            {
                if (!IsActive(evt)) continue;
                if (evt.IsInCooldown) continue;

            }
        }

        public void CheckEvents(float mapX, float mapY)
        {
            var layer = engine.Map.EventLayer;
            var eventTile = layer[(int)mapX, (int)mapY];
            foreach (var evt in eventTile.Events)
            {
                if (!IsActive(evt)) continue;
                if (evt.IsInCooldown) continue;
                switch (evt.Type)
                {
                    case EventType.Trigger:
                    case EventType.None:
                        ExecuteEvent(evt);
                        break;
                }
            }
        }

        public void ExecuteEvent(Event evt)
        {
            if (engine.CanExecuteEvent(evt))
            {
                Logger.Info("Event", $"Executing event {evt}");
                evt.Execute();
            }
        }

        public void AddEvent(Event evt)
        {
            events.Add(evt);
            CacheEvent(evt);
        }

        public void RemoveEvent(Event evt)
        {
            if (evt == null) return;
            var layer = engine.Map.EventLayer;
            if (evt != null && events.Remove(evt))
            {
                foreach (var pt in evt.GetMatchingPositions())
                {
                    layer.RemoveEvent(evt, pt.X, pt.Y);
                }
                foreach (var pt in evt.GetMatchingHotSpotPositions())
                {
                    layer.RemoveHotSpotEvent(evt, pt.X, pt.Y);
                }
            }
        }

        private void CacheEvent(Event evt)
        {
            var layer = engine.Map.EventLayer;
            foreach (var pt in evt.GetMatchingPositions())
            {
                layer.AddEvent(evt, pt.X, pt.Y);
            }
            foreach (var pt in evt.GetMatchingHotSpotPositions())
            {
                layer.AddHotSpotEvent(evt, pt.X, pt.Y);
            }
        }
    }
}
