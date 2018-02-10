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
    using System;

    public class TimeInfo
    {
        private readonly TimeSpan elapsedGameTime;
        private readonly TimeSpan totalGameTime;
        public TimeInfo(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
        {
            this.elapsedGameTime = elapsedGameTime;
            this.totalGameTime = totalGameTime;
        }

        public TimeSpan ElapsedGameTime
        {
            get { return elapsedGameTime; }
        }
        public TimeSpan TotalGameTime
        {
            get { return totalGameTime; }
        }

        public TimeSpan GetElapsedTimeSince(TimeSpan time)
        {
            return totalGameTime - time;
        }
    }
}
