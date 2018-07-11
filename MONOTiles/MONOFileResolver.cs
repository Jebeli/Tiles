/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
*/

namespace MONOTiles
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using TileEngine.Files;
    public class MONOFileResolver : AbstractFileResolver
    {
        private IDictionary<string, string> cache;
        private IList<string> dirs;

        public MONOFileResolver(params string[] roots)
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
                string assetName = MakeAssetName(fileId);
                if (assetName != null)
                {
                    fileName = assetName;
                }
                else if (File.Exists(fileId))
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
            foreach (var d in Directory.GetLogicalDrives())
            {
                DirectoryInfo di = new DirectoryInfo(d);
                var fi = new TileEngine.Files.FileInfo()
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
            return list;
        }


        private string MakeAssetName(string fileId)
        {
            if (!string.IsNullOrEmpty(fileId))
            {
                if (fileId.StartsWith("images/", StringComparison.OrdinalIgnoreCase) && fileId.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    return MakeAssetId(fileId);
                }
                else if (fileId.StartsWith("gui/", StringComparison.OrdinalIgnoreCase) && fileId.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    return MakeAssetId(fileId);
                }
                else if (fileId.StartsWith("music/", StringComparison.OrdinalIgnoreCase) && fileId.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase))
                {
                    return MakeAssetId(fileId);
                }
                else if (fileId.StartsWith("music/", StringComparison.OrdinalIgnoreCase) && fileId.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    return MakeAssetId(fileId);
                }
                else if (fileId.StartsWith("soundfx/", StringComparison.OrdinalIgnoreCase) && fileId.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase))
                {
                    return MakeAssetId(fileId);
                }
                else if (fileId.StartsWith("soundfx/", StringComparison.OrdinalIgnoreCase) && fileId.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    return MakeAssetId(fileId);
                }
                else if (fileId.StartsWith("fonts/", StringComparison.OrdinalIgnoreCase) && fileId.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase))
                {
                    return MakeFontAssetId(fileId);
                }
            }
            return null;
        }

        private static string MakeAssetId(string fileId)
        {
            string assetName = fileId;
            assetName = Path.ChangeExtension(assetName, null);
            assetName = assetName.Trim('.');
            return assetName;
        }

        private static string MakeFontAssetId(string fileId)
        {
            if (fileId.Contains("entypo"))
            {
                return "fonts/Icon";
            }
            return "fonts/Small";

        }
    }
}
