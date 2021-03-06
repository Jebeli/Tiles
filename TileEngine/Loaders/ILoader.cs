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


namespace TileEngine.Loaders
{
    using Core;
    using Maps;
    using Graphics;
    using Entities;

    public interface ILoader
    {
        FileType DetectFileTpye(string fileId);
        bool CanLoad(string fileId);
        Map LoadMap(string fileId);
        TileSet LoadTileSet(string fileId);
        MapParallax LoadParallax(string fileId);
        AnimationSet LoadAnimation(string fileId);
        Entity LoadEntity(string fileId);
    }
}
