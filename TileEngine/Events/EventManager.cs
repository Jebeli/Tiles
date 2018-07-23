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
    using TileEngine.Input;
    using TileEngine.Logging;

    public class EventManager
    {
        private Engine engine;
        private List<Event> events;
        private List<Event> delayedEvents;

        public EventManager(Engine engine)
        {
            this.engine = engine;
            events = new List<Event>();
            delayedEvents = new List<Event>();
        }

        public void Clear()
        {
            events.Clear();
            delayedEvents.Clear();
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

        public void UpdateDelayedEvents()
        {
            int i = delayedEvents.Count;
            while (i > 0)
            {
                i--;
                Event evt = delayedEvents[i];
                if (evt.DelayTicks > 0)
                {
                    evt.DelayTicks--;
                }
                else
                {
                    delayedEvents.RemoveAt(i);
                    ExecuteDelayedEvent(evt);
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
            foreach(var ec in evt.Components)
            {
                if (!engine.CampaignManager.CheckAllRequirements(ec))
                {
                    return false;
                }
            }
            return true;
        }

        public void CheckHotSpots(float mapX, float mapY)
        {
            Point tipPos = new Point();
            foreach (var e in events)
            {
                if (!IsActive(e)) continue;
                if (e.IsInCooldown) continue;
                if (!e.HotSpot) continue;
                for (int x = e.HotPosX; x < e.HotPosX + e.HotWidth; x++)
                {
                    for (int y = e.HotPosY; y < e.HotPosY + e.HotHeight; y++)
                    {
                        bool matched = false;
                        bool isNpc = false;
                        EventComponent npc = e.GetComponent(EventComponentType.NPCHotspot);
                        if (npc != null)
                        {
                            isNpc = true;
                            engine.Camera.MapToScreen(x, y, out int pX, out int pY);
                            Point p = engine.Camera.CenterTile(pX, pY);
                            Rect dest = new Rect(p.X + npc.MapX, p.Y + npc.MapY, npc.MapWidth, npc.MapHeight);
                            if (dest.Contains(engine.Input.ScaledMouseX, engine.Input.ScaledMouseY))
                            {
                                matched = true;
                                tipPos = new Point(dest.X + dest.Width / 2, (int)(p.Y - engine.Camera.TileHeight * 1.8f));
                            }
                        }
                        else
                        {
                            engine.Camera.MapToScreen(x, y, out int pX, out int pY);
                            Point p = engine.Camera.CenterTile(pX, pY);
                            foreach (var layer in engine.Map.Layers)
                            {
                                if (layer.Visible)
                                {
                                    var tex = layer.GetTileTexture(x, y);
                                    if (tex != null)
                                    {
                                        Rect dest = new Rect(p.X - tex.OffsetX, p.Y - tex.OffsetY, tex.Width, tex.Height);
                                        if (dest.Contains(engine.Input.ScaledMouseX, engine.Input.ScaledMouseY))
                                        {
                                            matched = true;
                                            engine.Camera.MapToScreen(e.CenterX, e.CenterY, out int tipX, out int tipY);
                                            tipY -= engine.Camera.TileHeight;
                                            //tipY -= tex.Height;
                                            tipPos = new Point(tipX, tipY);
                                        }
                                    }
                                }
                            }
                        }
                        if (matched)
                        {
                            var tt = e.GetComponent(EventComponentType.Tooltip);
                            if (tt != null)
                            {
                                engine.MapScreen.ShowTooltip(tt.StringParam, tipPos);
                            }
                            else
                            {
                                engine.MapScreen.HideTooltip();
                            }
                            if (Utils.CalcDist(mapX, mapY, e.CenterX, e.CenterY) < engine.InteractRange)
                            {
                                if (engine.Camera.MapClicked)
                                {
                                    engine.Camera.MapClickDone = true;
                                    ExecuteEvent(e);
                                }
                            }
                            return;
                        }
                    }
                }
            }
            engine.MapScreen.HideTooltip();
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
            if (IsActive(evt) && engine.CanExecuteEvent(evt))
            {
                if (evt.Delay > 0)
                {
                    evt.DelayTicks = evt.Delay;
                    evt.CooldownTicks = evt.Cooldown + evt.Delay;
                    delayedEvents.Add(evt);
                }
                else
                {
                    Logger.Info("Event", $"Executing event {evt}");
                    evt.Execute();
                }
            }
        }

        private void ExecuteDelayedEvent(Event evt)
        {
            Logger.Info("Event", $"Executing delayed event {evt}");
            evt.Execute();
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
                //foreach (var pt in evt.GetMatchingHotSpotPositions())
                //{
                //    layer.RemoveHotSpotEvent(evt, pt.X, pt.Y);
                //}
            }
        }

        private void CacheEvent(Event evt)
        {
            var layer = engine.Map.EventLayer;
            foreach (var pt in evt.GetMatchingPositions())
            {
                layer.AddEvent(evt, pt.X, pt.Y);
            }
            //foreach (var pt in evt.GetMatchingHotSpotPositions())
            //{
            //    layer.AddHotSpotEvent(evt, pt.X, pt.Y);
            //}
        }
    }
}
