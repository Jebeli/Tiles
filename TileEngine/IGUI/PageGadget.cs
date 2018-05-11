using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    public class PageGadget : Gadget
    {
        private int selectedIndex;

        public PageGadget() : this(TagItems.Empty)
        {
        }

        public PageGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            selectedIndex = 0;
            New(tags);
        }

        public override void Layout(bool initial)
        {
            base.Layout(initial);
            int index = 0;
            var frame = Bounds;
            foreach(Gadget gad in Members)
            {
                if (index == selectedIndex)
                {
                    gad.Hidden = false;
                    gad.LeftEdge = frame.Left;
                    gad.TopEdge = frame.Top;
                    gad.Width = frame.Width;
                    gad.Height = frame.Height;
                    gad.Layout(initial);
                }
                else
                {
                    gad.Hidden = true;
                }
                index++;
            }
        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.PAGE_Add:
                    AddMember(tag.GetTagData<Gadget>());
                    return 1;
                case Tags.PAGE_Remove:
                    RemMember(tag.GetTagData<Gadget>());
                    return 1;
                case Tags.PAGE_Current:
                    selectedIndex = tag.GetTagData(0);
                    return 1;
            }
            return base.SetTag(set, update, tag);
        }
    }
}
