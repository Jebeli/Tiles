using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI
{
    public abstract class Root : IDisposable
    {
        private List<Root> members;
        private Root target;
        private IList<(Tags, Tags)> attrMap;
        private int loop;
        public Root()
        {
            members = new List<Root>();
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
            members.Add(member);
            return members.IndexOf(member);
        }

        public virtual bool RemMember(Root member)
        {
            return members.Remove(member);
        }

        public virtual void Get((Tags, object) attr)
        {

        }

        internal int SetNew(IList<(Tags, object)> attrList)
        {
            return SetTags(null, SetFlags.New, UpdateFlags.Final, attrList);
        }
        public int Set(params (Tags, object)[] attrList)
        {
            return SetTags(null, SetFlags.Set, UpdateFlags.Final, attrList);
        }
        public int Set(IList<(Tags, object)> attrList)
        {
            return SetTags(null, SetFlags.Set, UpdateFlags.Final, attrList);
        }
        public int Set(GadgetInfo gadgetInfo, params (Tags, object)[] attrList)
        {
            return SetTags(gadgetInfo, SetFlags.Set, UpdateFlags.Final, attrList);
        }
        public int Set(GadgetInfo gadgetInfo, IList<(Tags, object)> attrList)
        {
            return SetTags(gadgetInfo, SetFlags.Set, UpdateFlags.Final, attrList);
        }
        public int Update(UpdateFlags flags, params (Tags, object)[] attrList)
        {
            return UpdateTags(null, flags, attrList);
        }
        public int Update(UpdateFlags flags, IList<(Tags, object)> attrList)
        {
            return UpdateTags(null, flags, attrList);
        }
        public int Update(GadgetInfo gadgetInfo, UpdateFlags flags, params (Tags, object)[] attrList)
        {
            return UpdateTags(gadgetInfo, flags, attrList);
        }
        public int Update(GadgetInfo gadgetInfo, UpdateFlags flags, IList<(Tags, object)> attrList)
        {
            return UpdateTags(gadgetInfo, flags, attrList);
        }
        public void Notify(UpdateFlags flags, params (Tags, object)[] attrList)
        {
            NotifyTags(null, flags, attrList);
        }
        public void Notify(UpdateFlags flags, IList<(Tags, object)> attrList)
        {
            NotifyTags(null, flags, attrList);
        }
        public void Notify(GadgetInfo gadgetInfo, UpdateFlags flags, params (Tags, object)[] attrList)
        {
            NotifyTags(gadgetInfo, flags, attrList);
        }
        public void Notify(GadgetInfo gadgetInfo, UpdateFlags flags, IList<(Tags, object)> attrList)
        {
            NotifyTags(gadgetInfo, flags, attrList);
        }

        protected virtual void NotifyTags(GadgetInfo gadgetInfo, UpdateFlags flags, IList<(Tags, object)> attrList)
        {
            if (target != null)
            {
                if (CheckLoop())
                {
                    SetLoop();
                    target.Update(gadgetInfo, flags, attrList.MapTags(attrMap));
                    ClearLoop();
                }
            }
        }

        protected virtual int UpdateTags(GadgetInfo gadgetInfo, UpdateFlags updateFlags, IList<(Tags, object)> attrList)
        {
            return SetTags(gadgetInfo, SetFlags.Update, updateFlags, attrList);
        }

        protected int SetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags updateFlags, IList<(Tags, object)> attrList)
        {
            int retVal = BeforeSetTags(gadgetInfo, set, updateFlags);
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
                            retVal |= SetTag(gadgetInfo, set, updateFlags, tag);
                            break;
                    }
                }
            }
            return AfterSetTags(gadgetInfo, set, updateFlags, retVal);
        }

        protected abstract int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag);
        protected abstract int BeforeSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update);
        protected abstract int AfterSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, int returnValue);

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
        protected IEnumerable<Root> Members
        {
            get { return members; }
        }

    }
}
