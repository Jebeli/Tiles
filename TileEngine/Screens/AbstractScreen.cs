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

namespace TileEngine.Screens
{
    using System;
    using Logging;
    using Core;
    public abstract class AbstractScreen : NamedObject, IScreen
    {
        private bool rendered;
        private TimeSpan startTime;
        protected Engine engine;
        public AbstractScreen(Engine engine, string name)
            : base(name)
        {
            this.engine = engine;
        }

        public virtual void Show()
        {
            Logger.Info("Screen", $"Showing Screen {Name}");
            startTime = engine.GetCurrentTime();
            rendered = false;
        }

        public virtual void Hide()
        {
            Logger.Info("Screen", $"Hiding Screen {Name}");
            rendered = false;
        }

        public virtual void Update(TimeInfo time)
        {

        }

        public virtual void Render(TimeInfo time)
        {
            if (!rendered)
            {
                Logger.Info("Screen", $"Rendering Screen {Name}");
            }
            rendered = true;
        }
    }
}
