using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Core
{
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
