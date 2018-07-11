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

namespace TileEngine.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EntityLoadInfo
    {
        private string name;
        private string entityId;
        private int posX;
        private int posY;

        public EntityLoadInfo()
        {

        }

        public EntityLoadInfo(string name, string entityId, int posX, int posY)
        {
            this.name = name;
            this.entityId = entityId;
            this.posX = posX;
            this.posY = posY;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string EntityId
        {
            get { return entityId; }
            set { entityId = value; }
        }

        public int PosX
        {
            get { return posX; }
            set { posX = value; }
        }

        public int PosY
        {
            get { return posY; }
            set { posY = value; }
        }
    }
}
