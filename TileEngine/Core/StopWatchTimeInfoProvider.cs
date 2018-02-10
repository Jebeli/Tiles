using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Core
{
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
