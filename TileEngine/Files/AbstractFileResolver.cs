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

namespace TileEngine.Files
{
    using System.Collections.Generic;
    using System.IO;

    public abstract class AbstractFileResolver : IFileResolver
    {
        public abstract string ResolveDir(string fileId);
        public abstract string Resolve(string fileId);
        public abstract Stream OpenFile(string fileId);
        public abstract Stream CreateFile(string fileId);

        public abstract IList<string> GetVolumes();
        public abstract IList<string> GetDirectories(string dir);
        public abstract IList<string> GetFiles(string dir);
        public abstract string GetParent(string dir);

        public abstract IList<FileInfo> GetVolumeInfos();
        public abstract IList<FileInfo> GetFileInfos(string dir);

    }
}
