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

namespace TileEngine.Loaders
{
    using Core;
    using Maps;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public abstract class AbstractLoader : ILoader
    {
        protected Engine engine;
        protected List<string> extensions;
        public AbstractLoader(Engine engine, params string[] exts)
        {
            this.engine = engine;
            extensions = new List<string>();
            foreach (string ext in exts) AddExtension(ext);
        }
        public abstract FileType DetectFileTpye(string fileId);

        public bool CanLoad(string fileId)
        {
            return DetectFileTpye(fileId) != FileType.None;
        }

        public abstract Map LoadMap(string fileId);
        
        public abstract TileSet LoadTileSet(string fileId);

        protected void AddExtension(string ext)
        {
            extensions.Add(ext);
        }

        protected bool FitsExtension(string fileId)
        {
            foreach(string ext in extensions)
            {
                if (fileId.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        protected Stream GetInputStream(string fileId)
        {
            return engine.FileResolver.OpenFile(fileId);
        }
    }
}
