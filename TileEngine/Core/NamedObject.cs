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

namespace TileEngine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NamedObject : IEquatable<NamedObject>
    {
        private string name;
        public NamedObject(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool HasName(string name)
        {
            return this.name.Equals(name);
        }

        public override string ToString()
        {
            return name;
        }

        public static bool CheckNameClash<T>(IEnumerable<T> list, string identifier, bool throwException = true) where T : NamedObject
        {
            if (list.Any(m => m.HasName(identifier)))
            {
                if (throwException)
                {
                    throw new ArgumentException($"Name clash: {identifier} already present");
                }
                return false;
            }
            return true;
        }

        public bool Equals(NamedObject other)
        {
            return string.Compare(ToString(), other.ToString()) == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is NamedObject)
            {
                return Equals((NamedObject)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

    }
}
