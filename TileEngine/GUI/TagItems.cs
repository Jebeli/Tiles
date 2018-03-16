using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI
{
    public static class TagItems
    {

        public static IList<(Tags, object)> JoinTags(this IList<(Tags, object)> tags, IList<(Tags, object)> moreTags)
        {
            var joined = new List<(Tags, object)>();
            if (tags != null)
            {
                joined.AddRange(tags);
            }
            if (moreTags != null)
            {
                joined.AddRange(moreTags);
            }
            return joined;
        }

        public static IList<(Tags, object)> AddTag(this IList<(Tags, object)> tags, (Tags, object) tag)
        {
            var joined = new List<(Tags, object)>();
            if (tags != null)
            {
                joined.AddRange(tags);
            }
            joined.Add(tag);
            return joined;
        }

        public static IList<(Tags, object)> MapTags(this IList<(Tags, object)> tags, IList<(Tags, Tags)> attrMap)
        {
            if ((attrMap == null) || attrMap.Count == 0) return tags;
            var mapped = new List<(Tags, object)>();
            if (tags != null)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    var tag = tags[i];
                    var map = attrMap.FirstOrDefault(x => x.Item1.Equals(tag.Item1));
                    if (map.Item1 != Tags.TAG_None)
                    {
                        mapped.Add((map.Item2, tag.Item2));
                    }
                    else
                    {
                        mapped.Add(tag);
                    }
                }
            }
            return mapped;
        }


        public static T GetTagData<T>(this (Tags, object) tag)
        {
            return GetTagData(tag, default(T));
        }
        public static T GetTagData<T>(this (Tags, object) tag, T def)
        {
            if (tag.Item2 != null && tag.Item2 is T)
            {
                return (T)tag.Item2;
            }
            return def;
        }

        public static T GetTagData<T>(this (Tags, object)[] tags, Tags tag)
        {
            return GetTagData(tags, tag, default(T));
        }

        public static T GetTagData<T>(this (Tags, object)[] tags, Tags tag, T def)
        {
            T result = def;
            if (tags != null)
            {
                foreach (var t in tags)
                {
                    if (t.Item1 == tag)
                    {
                        object o = t.Item2;
                        if (o != null && o is T)
                        {
                            return (T)o;
                        }
                    }
                }

            }
            return result;
        }

        public static bool GetBoolTag(this (Tags, object)[] tags, Tags tag, bool def = false)
        {
            return GetTagData(tags, tag, def);
        }

        public static int GetIntTag(this (Tags, object)[] tags, Tags tag, int def = 0)
        {
            return GetTagData(tags, tag, def);
        }
        public static string GetStrTag(this (Tags, object)[] tags, Tags tag, string def = "")
        {
            return GetTagData(tags, tag, def);
        }

    }
}
