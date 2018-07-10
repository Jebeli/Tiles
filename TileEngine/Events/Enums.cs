using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Events
{
    public enum EventComponentType
    {
        None,
        Tooltip,
        InterMap,
        IntraMap,
        MapMod,
        SoundFX,
        Msg,
        ShakyCam,
        Spawn
    }

    public enum EventType
    {
        None,
        Trigger,
        Load,
        Exit,
        Leave,
        Clear,
        Static
    }
}
