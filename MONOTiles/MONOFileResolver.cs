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
    public class MONOFileResolver :  AbstractFileResolver
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

        private string MakeAssetName(string fileId)
        {
            if (!string.IsNullOrEmpty(fileId))
            {
                if (fileId.StartsWith("images/", StringComparison.OrdinalIgnoreCase) && fileId.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
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
    }
}
