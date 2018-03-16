using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI
{
    public class IClass
    {
        private Type type;
        public IClass(string name, Type type)
        {
            Name = name;
            this.type = type;
        }
        public IClass(Type type)
        {
            Name = type.FullName;
            this.type = type;
        }

        public string Name { get; set; }

        public Root Create(IList<(Tags, object)> attrList)
        {
            Root root = Activator.CreateInstance(type) as Root;
            if (root != null)
            {
                root.SetNew(attrList);
            }
            return root;
        }
    }
}
