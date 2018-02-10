using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Core
{
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
