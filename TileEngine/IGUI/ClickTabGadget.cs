using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    public class ClickTabGadget : Gadget
    {
        private List<object> labels;
        private int selectedIndex;
        private PageGadget page;
        private bool pageBorder;

        public ClickTabGadget() : this(TagItems.Empty)
        {
        }

        public ClickTabGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            labels = new List<object>();
            selectedIndex = 0;
            pageBorder = true;
            New(tags);
            MakeTabs();
        }

        public bool PageBorder
        {
            get { return pageBorder; }
        }

        private void MakeTabs()
        {
            int index = 0;
            foreach (var s in labels)
            {
                MakeTab(s.ToString(), index);
                index++;
            }
        }

        private void MakeTab(string label, int index)
        {
            var tab = new TabHeaderGadget((Tags.GA_Text, label),
                (Tags.ICA_TARGET, this),
                (Tags.GA_ID, index)
                );
            if (index == selectedIndex)
            {
                tab.TogSelect = true;
            }
            AddMember(tab);
        }

        private void LayoutTabs()
        {
            var frame = Bounds;
            frame.Offset(-Window.BorderLeft, -Window.BorderTop);
            int count = labels.Count;
            double x = frame.Left + 1;
            double w = frame.Width;
            w /= count;
            double gw = w - 2;
            int index = 0;
            int h = 24;
            foreach (Gadget gad in Members)
            {
                if (gad == page && page != null)
                {
                    page.LeftEdge = frame.Left + 1;
                    page.TopEdge = frame.Top + h;
                    page.Width = frame.Width - 9;
                    page.Height = frame.Height - h * 2;
                    page.Set((Tags.PAGE_Current, selectedIndex));
                    page.Layout(false);
                }
                else
                {
                    gad.LeftEdge = (int)x;
                    gad.TopEdge = frame.Top;
                    gad.Width = (int)gw;
                    gad.Height = h;
                    gad.TogSelect = index == selectedIndex;
                    x += w;
                    index++;
                }
            }

        }

        protected override void OnNotifyClick(int id)
        {
            selectedIndex = id;
            Layout(false);
        }

        public override void Layout(bool initial)
        {
            base.Layout(initial);
            LayoutTabs();
        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.CLICKTAB_Labels:
                    labels = new List<object>(tag.GetTagData<IEnumerable<object>>());
                    return 1;
                case Tags.CLICKTAB_Current:
                    selectedIndex = tag.GetTagData(0);
                    return 1;
                case Tags.CLICKTAB_PageGroup:
                    page = tag.GetTagData<PageGadget>();
                    AddMember(page);
                    return 1;
                case Tags.CLICKTAB_PageGroupBorder:
                    pageBorder = tag.GetTagData(true);
                    return 1;
            }
            return base.SetTag(set, update, tag);
        }
    }
}
