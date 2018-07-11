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

namespace TileEngine.Core
{
    using Maps;
    using Screens;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Entities;
    using TileEngine.Events;

    public class MapEventArgs : EventArgs
    {
        private readonly Map map;

        public MapEventArgs(Map map)
        {
            this.map = map;
        }

        public Map Map
        {
            get { return map; }
        }
    }

    public class EntityEventArgs : EventArgs
    {
        private readonly Entity ent;

        public EntityEventArgs(Entity ent)
        {
            this.ent = ent;
        }

        public Entity Entity { get { return ent; } }
    }


    public class EventEventArgs : EventArgs
    {
        private readonly Event evt;
        private bool cancel;

        public EventEventArgs(Event evt)
        {
            this.evt = evt;
        }

        public Event Event
        {
            get { return evt; }
        }

        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }

    public class ScreenEventArgs : EventArgs
    {
        private readonly IScreen screen;

        public ScreenEventArgs(IScreen screen)
        {
            this.screen = screen;
        }

        public IScreen Screen
        {
            get { return screen; }
        }
    }
}
