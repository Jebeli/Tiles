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
    using System.Diagnostics;

    public class StopWatchTimeInfoProvider : ITimeInfoProvider
    {
        private Stopwatch totalWatch;
        private Stopwatch relativeWatch;

        public StopWatchTimeInfoProvider()
        {
            totalWatch = new Stopwatch();
            relativeWatch = new Stopwatch();
            totalWatch.Start();
            relativeWatch.Start();

        }

        public TimeInfo GetRenderTimeInfo()
        {
            TimeInfo time = new TimeInfo(totalWatch.Elapsed, relativeWatch.Elapsed);
            relativeWatch.Restart();
            return time;
        }

        public TimeInfo GetUpdateTimeInfo()
        {
            return new TimeInfo(totalWatch.Elapsed, relativeWatch.Elapsed);
        }

        public TimeSpan GetCurrentTime()
        {
            return totalWatch.Elapsed;
        }
    }
}
