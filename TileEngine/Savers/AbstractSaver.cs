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


namespace TileEngine.Savers
{
    using Maps;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public abstract class AbstractSaver : ISaver
    {
        protected Engine engine;
        protected List<string> extensions;

        public AbstractSaver(Engine engine, params string[] exts)
        {
            this.engine = engine;
            extensions = new List<string>();
            foreach (string ext in exts) AddExtension(ext);


        }
        public abstract void Save(Map map, string fileId);

        public abstract void Save(TileSet tileSet, string fileId);

        public abstract void Save(MapParallax parallax, string fileId);

        public bool FitsExtension(string fileId)
        {
            foreach (string ext in extensions)
            {
                if (fileId.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        protected Stream GetOutputStream(string fileId)
        {
            return engine.FileResolver.CreateFile(fileId);
        }

        protected void AddExtension(string ext)
        {
            extensions.Add(ext);
        }

        protected abstract string AdjustName(string fileId);
    }
}
