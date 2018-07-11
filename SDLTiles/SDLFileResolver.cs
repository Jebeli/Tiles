namespace SDLTiles
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using TileEngine.Files;

    public class SDLFileResolver : AbstractFileResolver
    {
        private IDictionary<string, string> cache;
        private IList<string> dirs;

        public SDLFileResolver(params string[] roots)
        {
            dirs = new List<string>();
            cache = new Dictionary<string, string>();
            foreach (var root in roots)
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

        public override string ResolveDir(string fileId)
        {
            string fileName = null;
            if (!cache.TryGetValue(fileId, out fileName))
            {
                if (Directory.Exists(fileId))
                {
                    fileName = Path.GetFullPath(fileId);
                }
                else
                {
                    foreach (string dir in dirs)
                    {
                        string testId = Path.Combine(dir, fileId);
                        if (Directory.Exists(testId))
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

        public override Stream CreateFile(string fileId)
        {
            string fileName = Resolve(fileId);
            if (fileName == null)
            {
                foreach (string dir in dirs)
                {
                    fileName = Path.Combine(dir, fileId);
                    break;
                }
            }
            if (fileName != null)
            {
                return File.Create(fileName);
            }
            return null;
        }

        public override IList<string> GetVolumes()
        {
            List<string> volumes = new List<string>();
            volumes.AddRange(Directory.GetLogicalDrives());
            return volumes;
        }

        public override IList<string> GetDirectories(string dir)
        {
            return new List<string>(Directory.GetDirectories(dir));
        }

        public override IList<string> GetFiles(string dir)
        {
            return new List<string>(Directory.GetFiles(dir));
        }

        public override string GetParent(string dir)
        {
            return Path.GetDirectoryName(dir);
        }

        public override IList<TileEngine.Files.FileInfo> GetVolumeInfos()
        {
            List<TileEngine.Files.FileInfo> list = new List<TileEngine.Files.FileInfo>();
            string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fi = new TileEngine.Files.FileInfo()
            {
                Path = docs,
                Name = "My Documents",
                IsDirectory = true,
                Size = 0,
                Date = DateTime.Now
            };
            list.Add(fi);
            foreach (var d in Directory.GetLogicalDrives())
            {
                DirectoryInfo di = new DirectoryInfo(d);
                fi = new TileEngine.Files.FileInfo()
                {
                    Path = d,
                    Name = d,
                    IsDirectory = true,
                    Size = 0,
                    Date = di.LastWriteTime
                };
                list.Add(fi);
            }
            return list;
        }

        public override IList<TileEngine.Files.FileInfo> GetFileInfos(string dir)
        {
            List<TileEngine.Files.FileInfo> list = new List<TileEngine.Files.FileInfo>();
            try
            {
                foreach (var d in Directory.GetDirectories(dir))
                {
                    DirectoryInfo di = new DirectoryInfo(d);
                    var fi = new TileEngine.Files.FileInfo()
                    {
                        Path = d,
                        Name = Path.GetFileName(d),
                        Directory = Path.GetDirectoryName(d),
                        IsDirectory = true,
                        Size = 0,
                        Date = di.LastWriteTime
                    };
                    list.Add(fi);
                }
            }
            catch (Exception)
            {

            }
            try
            {
                foreach (var f in Directory.GetFiles(dir))
                {
                    var fi = new System.IO.FileInfo(f);

                    var fin = new TileEngine.Files.FileInfo()
                    {
                        Path = f,
                        Name = Path.GetFileName(f),
                        Directory = Path.GetDirectoryName(f),
                        IsDirectory = false,
                        Size = fi.Length,
                        Date = fi.LastWriteTime
                    };
                    list.Add(fin);
                }
            }
            catch (Exception)
            {

            }
            return list;
        }
    }
}
