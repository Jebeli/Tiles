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
    using System.IO;

    public abstract class AbstractSaver : ISaver
    {
        protected Engine engine;
        public AbstractSaver(Engine engine)
        {
            this.engine = engine;
        }
        public abstract void Save(Map map, string fileId);
        public abstract void Save(TileSet tileSet, string fileId);

        protected Stream GetOutputStream(string fileId)
        {
            return engine.FileResolver.CreateFile(fileId);
        }

        protected abstract string AdjustName(string fileId);
    }
}
