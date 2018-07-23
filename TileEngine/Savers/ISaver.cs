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


using TileEngine.Maps;

namespace TileEngine.Savers
{
    public interface ISaver
    {
        void Save(Map map, string fileId);
        void Save(TileSet tileSet, string fileId);
        void Save(MapParallax parallax, string fileId);
        bool FitsExtension(string fileId);
    }
}
