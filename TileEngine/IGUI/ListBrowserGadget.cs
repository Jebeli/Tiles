using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    public class ListBrowserGadget : Gadget
    {
        private ScrollerGadget vertScroller;
        private List<string> labels;

        public ListBrowserGadget() : this(TagItems.Empty)
        {
        }

        public ListBrowserGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            labels = new List<string>();
            vertScroller = new ScrollerGadget(
                (Tags.SCROLLER_Orientation, Orientation.Vertical),
                (Tags.ICA_TARGET, this),
                (Tags.ICA_MAP, (Tags.SCROLLER_Top, Tags.LISTBROWSER_VPropTop))
                );
            AddMember(vertScroller);
            New(tags);
        }

        public IList<string> Labels
        {
            get { return labels; }
        }

        public override void Layout(bool initial)
        {
            base.Layout(initial);
            WriteChanges();
            vertScroller.Layout(initial);
        }

        private void WriteChanges()
        {
            var frame = Bounds;
            frame.Offset(-Window.BorderLeft, -Window.BorderTop);
            int h = frame.Height;
            int itemH = 20;
            vertScroller.LeftEdge = frame.Right - 20;
            vertScroller.Width = 20;
            vertScroller.TopEdge = frame.Top;
            vertScroller.Height = frame.Height;
            vertScroller.ScrollerTotal = labels.Count * itemH;
            vertScroller.ScrollerVisible = h;

        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.LISTBROWSER_Labels:
                    labels = new List<string>(tag.GetTagData<IEnumerable<string>>());
                    return 1;
                case Tags.LISTBROWSER_VPropTop:
                    vertScroller.ScrollerTop = tag.GetTagData(0);
                    return 1;
            }
            return base.SetTag(set, update, tag);
        }
    }
}
