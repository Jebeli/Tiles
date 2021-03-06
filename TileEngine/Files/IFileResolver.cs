﻿/*
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

    public interface IFileResolver
    {
        string ResolveDir(string dirId);
        string Resolve(string fileId);
        Stream OpenFile(string fileId);
        Stream CreateFile(string fileId);

        IList<string> GetVolumes();
        IList<string> GetDirectories(string dir);
        IList<string> GetFiles(string dir);
        string GetParent(string dir);

        IList<FileInfo> GetVolumeInfos();
        IList<FileInfo> GetFileInfos(string dir);
    }
}
