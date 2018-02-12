using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Core
{
    public class FrameCounter
    {
        private static readonly TimeSpan oneSec = new TimeSpan(0, 0, 1);
        private TimeSpan timer = oneSec;
        private int frameCounter;
        private int framesPerSecond;

        public int FramesPerSecond
        {
            get { return framesPerSecond; }
        }

        public void FrameRendering(TimeInfo time)
        {
            frameCounter++;
            timer += time.ElapsedGameTime;
            if (timer <= oneSec) return;
            framesPerSecond = frameCounter;
            frameCounter = 0;
            timer -= oneSec;
        }
    }
}
