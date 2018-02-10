using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Files;

namespace GDITiles
{
    public class GDIFileResolver: AbstractFileResolver
    {
        private IDictionary<string, string> cache;
        private IList<string> dirs;

        public GDIFileResolver(params string[] roots)
        {
            dirs = new List<string>();
            cache = new Dictionary<string, string>();
            foreach(var root in roots)
            {
                AddRoot(root);
            }
        }

        public void AddRoot(string dir)
        {
            if (Directory.Exists(dir))
            {
                dirs.Add(dir);
                foreach (string subDir in Directory.GetDirectories(dir, "*", SearchOption.AllDirectories))
                {
                    dirs.Add(subDir);
                }

            }
        }
        public override string Resolve(string fileId)
        {
            string fileName = null;
            if (!cache.TryGetValue(fileId, out fileName))
            {
                if (File.Exists(fileId))
                {
                    fileName = Path.GetFullPath(fileId);
                }
                else
                {
                    foreach (string dir in dirs)
                    {
                        string testId = Path.Combine(dir, fileId);
                        if (File.Exists(testId))
                        {
                            fileName = Path.GetFullPath(testId);
                            break;
                        }
                    }
                }
                cache[fileId] = fileName;
            }
            return fileName;
        }

        public override Stream OpenFile(string fileId)
        {
            string fileName = Resolve(fileId);
            if (File.Exists(fileName))
            {
                return File.Open(fileName, FileMode.Open);
            }
            return null;
        }
    }
}
