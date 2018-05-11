using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    [Flags]
    public enum UpdateFlags
    {
        Final = 0x00,
        Interim = 0x01
    }

    public enum SetFlags
    {
        New = 0x00,
        Set = 0x01,
        Update = 0x02
    }
    public class Root : IDisposable
    {
        private List<Root> members;
        private Root target;
        private IList<(Tags, Tags)> attrMap;
        private int loop;

        public Root()
            : this(TagItems.Empty)
        {
        }

        public Root(params (Tags, object)[] tags)
        {
            members = new List<Root>();
            New(tags);
        }
        protected void New(params (Tags, object)[] tags)
        {
            SetTags(SetFlags.New, UpdateFlags.Final, tags);
        }

        public int Set(params (Tags, object)[] tags)
        {
            return SetTags(SetFlags.Set, UpdateFlags.Final, tags);
        }
        public int Set(IList<(Tags, object)> attrList)
        {
            return SetTags(SetFlags.Set, UpdateFlags.Final, attrList);
        }

        public int Update(UpdateFlags flags, params (Tags, object)[] attrList)
        {
            return UpdateTags(flags, attrList);
        }
        public int Update(UpdateFlags flags, IList<(Tags, object)> attrList)
        {
            return UpdateTags(flags, attrList);
        }

        public void Notify(UpdateFlags flags, params (Tags, object)[] attrList)
        {
            NotifyTags(flags, attrList);
        }
        public void Notify(UpdateFlags flags, IList<(Tags, object)> attrList)
        {
            NotifyTags(flags, attrList);
        }

        public void Get(params (Tags, object)[] tags)
        {
            for (int i = 0; i < tags.Length; i++)
            {
                tags[i].Item2 = GetTag(tags[i].Item1);
            }
        }

        public virtual int AddTail(IList<Root> list)
        {
            list.Add(this);
            return list.IndexOf(this);
        }

        public virtual bool Remove(IList<Root> list)
        {
            return list.Remove(this);
        }

        public virtual int AddMember(Root member)
        {
            return member.AddTail(members);
        }

        public virtual bool RemMember(Root member)
        {
            return member.Remove(members);
        }

        public IEnumerable<Root> Members
        {
            get { return members; }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var mem in members)
                {
                    mem.Dispose();
                }
                members.Clear();
            }
        }

        protected int SetTags(SetFlags set, UpdateFlags updateFlags, IList<(Tags, object)> attrList)
        {
            int retVal = 0;
            if (attrList != null && attrList.Count > 0)
            {
                foreach (var tag in attrList)
                {
                    switch (tag.Item1)
                    {
                        case Tags.ICA_MAP:
                            attrMap = tag.GetTagData<IList<(Tags, Tags)>>();
                            break;
                        case Tags.ICA_TARGET:
                            target = tag.GetTagData<Root>();
                            break;
                        default:
                            retVal |= SetTag(set, updateFlags, tag);
                            break;
                    }
                }
            }
            return retVal;
        }

        protected virtual int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            return 0;
        }

        protected virtual object GetTag(Tags tag)
        {
            return null;
        }

        protected virtual int UpdateTags(UpdateFlags updateFlags, IList<(Tags, object)> attrList)
        {
            return SetTags(SetFlags.Update, updateFlags, attrList);
        }

        protected virtual void NotifyTags(UpdateFlags flags, IList<(Tags, object)> attrList)
        {
            if (target != null)
            {
                if (CheckLoop())
                {
                    SetLoop();
                    target.Update(flags, attrList.MapTags(attrMap));
                    ClearLoop();
                }
            }
        }

        protected virtual void SetLoop()
        {
            loop++;
        }

        protected virtual void ClearLoop()
        {
            loop--;
            if (loop < 0) loop = 0;
        }

        protected virtual bool CheckLoop()
        {
            return loop == 0;
        }

    }
}
