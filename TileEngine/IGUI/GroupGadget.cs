using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    public class GroupGadget : Gadget
    {
        public GroupGadget()
         : this(TagItems.Empty)
        {
        }

        public GroupGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            Width = 0;
            Height = 0;
            New(tags);
        }

        public override int AddMember(Root member)
        {
            int pos = base.AddMember(member);
            if (member is Gadget gad)
            {
                gad.Set(
                    (Tags.GA_Left, gad.LeftEdge + LeftEdge),
                    (Tags.GA_Top, gad.TopEdge + TopEdge)
                    );
            }
            RecalcGroupSize();
            return pos;
        }

        public override bool RemMember(Root member)
        {
            bool ok = base.RemMember(member);
            if (member is Gadget gad)
            {
                gad.Set(
                    (Tags.GA_Left, gad.LeftEdge - LeftEdge),
                    (Tags.GA_Top, gad.TopEdge - TopEdge)
                    );
            }
            RecalcGroupSize();
            return ok;
        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.GA_Left:
                    SetLeftEdge(tag.GetTagData(0));
                    return 1;
                case Tags.GA_Top:
                    SetTopEdge(tag.GetTagData(0));
                    return 1;
            }
            return base.SetTag(set, update, tag);
        }

        private void SetLeftEdge(int newEdge)
        {
            int dx = newEdge - LeftEdge;
            LeftEdge = newEdge;
            foreach (Gadget gad in Members)
            {
                gad.Set((Tags.GA_Left, gad.LeftEdge + dx));
            }
        }
        private void SetTopEdge(int newEdge)
        {
            int dy = newEdge - TopEdge;
            TopEdge = newEdge;
            foreach (Gadget gad in Members)
            {
                gad.Set((Tags.GA_Top, gad.TopEdge + dy));
            }
        }

        private void RecalcGroupSize()
        {
            int w = 0;
            int h = 0;
            int width = 0;
            int height = 0;
            foreach (Gadget gad in Members)
            {
                w = gad.LeftEdge - LeftEdge + gad.Width;
                h = gad.TopEdge - TopEdge + gad.Height;
                if (w > width) width = w;
                if (h > height) height = h;
            }
            Width = width;
            Height = height;
        }
    }
}
