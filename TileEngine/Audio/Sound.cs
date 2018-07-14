using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Resources;

namespace TileEngine.Audio
{
    public class Sound : Resource
    {
        public const string GLOBAL_VIRTUAL_CHANNEL = "__global__";
        public Sound(string name)
            : base(name)
        {

        }

    }
}
