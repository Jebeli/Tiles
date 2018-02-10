using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Core
{
    public interface ITimeInfoProvider
    {
        TimeInfo GetRenderTimeInfo();
        TimeInfo GetUpdateTimeInfo();
        TimeSpan GetCurrentTime();
    }
}
