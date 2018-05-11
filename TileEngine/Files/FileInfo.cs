using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Files
{
    public class FileInfo
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Directory { get; set; }
        public long Size { get; set; }
        public DateTime Date { get; set; }
        public bool IsDirectory { get; set; }
        public bool IsFile
        {
            get { return !IsDirectory; }
            set { IsDirectory = !value; }
        }
    }
}
