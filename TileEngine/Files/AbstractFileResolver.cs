using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Files
{
    public abstract class AbstractFileResolver : IFileResolver
    {
        public abstract string Resolve(string fileId);
        public abstract Stream OpenFile(string fileId);

    }
}
