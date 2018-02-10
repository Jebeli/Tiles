using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Files
{
    public interface IFileResolver
    {
        string Resolve(string fileId);
        Stream OpenFile(string fileId);
    }
}
