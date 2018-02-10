using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Resources
{
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
