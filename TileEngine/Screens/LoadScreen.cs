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
    using Maps;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;

    public class LoadScreen : TextTitleScreen
    {
        private MapLoadInfo mapLoadInfo;
        private bool mapDone;
        public LoadScreen(Engine engine)
            : base(engine, "LoadScreen", "LOADING")
        {
        }

        public MapLoadInfo MapLoadInfo
        {
            get { return mapLoadInfo; }
            set { mapLoadInfo = value; }
        }

        public override void Show()
        {
            base.Show();
            mapDone = false;
            engine.MapLoaded += Engine_MapLoaded;
            Task.Run(() =>
            {
                engine.LoadMap(mapLoadInfo.MapId, mapLoadInfo.PosX, mapLoadInfo.PosY);
            });
        }

        public override void Hide()
        {
            base.Hide();
            engine.MapLoaded -= Engine_MapLoaded;
        }

        public override void Update(TimeInfo time)
        {
            base.Update(time);
            if (mapDone) engine.SwitchToMapScreen();
        }

        private void Engine_MapLoaded(object sender, MapEventArgs e)
        {
            mapDone = true;
        }
    }
}
