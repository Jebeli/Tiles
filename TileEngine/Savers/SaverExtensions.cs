using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Events;

namespace TileEngine.Savers
{
    static class SaverExtensions
    {
        public static string ToIniString(this EventType et)
        {
            switch (et)
            {
                case EventType.Clear:
                    return "clear";
                case EventType.Trigger:
                    return "on_trigger";
                case EventType.Load:
                    return "on_load";
                case EventType.Exit:
                    return "on_mapexit";
                case EventType.Leave:
                    return "on_leave";
                case EventType.Static:
                    return "static";
            }
            return "on_trigger";
        }

        public static string ToDuration(this int duration, string ms = "ms", int maxFramesPerSecond = 60)
        {
            int div = 1000;
            //val = (int)(val * maxFramesPerSecond / div + 0.5f);
            // x*div = val*maxFrames
            // x*div/maxFrames = val
            int val = duration * div / maxFramesPerSecond;
            return $"{val}{ms}";
        }
    }
}
