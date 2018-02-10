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

namespace TileEngine.Resources
{
    using System.Collections.Generic;

    public class ResourceManager<T> where T : Resource
    {
        private Dictionary<string, T> resourceMap;

        public ResourceManager()
        {
            resourceMap = new Dictionary<string, T>();
        }

        public int ResourceCount
        {
            get { return resourceMap.Count; }
        }

        public bool Exists(string name)
        {
            return resourceMap.ContainsKey(name);
        }

        public T this[string name]
        {
            get { return Get(name); }
        }

        public void Clear()
        {
            foreach (var res in resourceMap.Values)
            {
                res.Dispose();
            }
            resourceMap.Clear();
        }

        public T Get(string name)
        {
            T res;
            if (resourceMap.TryGetValue(name, out res))
            {
                return res;
            }
            return null;
        }

        public bool Free(string name)
        {
            return Free(Get(name));
        }
        public bool Free(T resource)
        {
            if (resource != null)
            {
                foreach (var kvp in resourceMap)
                {
                    if (kvp.Value == resource)
                    {
                        if (resourceMap.Remove(kvp.Key))
                        {
                            kvp.Value.Dispose();
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        public T Add(T resource)
        {
            if (resource != null)
            {
                if (!resourceMap.ContainsKey(resource.Name))
                {
                    resourceMap.Add(resource.Name, resource);
                }
                return resourceMap[resource.Name];
            }
            return resource;
        }

    }
}
