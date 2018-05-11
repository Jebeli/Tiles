using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    public class ChooserGadget : Gadget
    {
        private List<object> labels;
        private int selectedIndex;
        private int chooserWidth;
        private int chooserItemHeight;
        private int maxLabels;
        private bool popup;
        private PopupMenu popupMenu;
        public ChooserGadget()
          : this(TagItems.Empty)
        {
        }

        public ChooserGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            labels = new List<object>();
            maxLabels = 12;
            chooserWidth = -1;
            chooserItemHeight = 20;
            selectedIndex = -1;
            New(tags);
        }

        public int ChooserWidth
        {
            get { return chooserWidth <= 0 ? Bounds.Width : chooserWidth; }
        }

        public int MaxLabels
        {
            get { return maxLabels; }
        }

        public object SelectedItem
        {
            get
            {
                if ((selectedIndex >= 0) && (selectedIndex < labels.Count))
                {
                    return labels[selectedIndex];
                }
                return "";
            }
        }

        public IList<string> Labels
        {
            get
            {
                List<string> list = new List<string>();
                foreach (var o in labels)
                {
                    list.Add(o.ToString());
                }
                return list;
            }
        }

        public bool PopUp
        {
            get { return popup; }
        }

        public bool DropDown
        {
            get { return !popup; }
        }

        private List<Gadget> GetPopupMenuItems()
        {
            var map = new(Tags, Tags)[] { (Tags.GA_ID, Tags.CHOOSER_Selected) };
            List<Gadget> glist = new List<Gadget>();
            List<PopupMenuItem> items = new List<PopupMenuItem>();
            int width = ChooserWidth - 4;
            int height = chooserItemHeight;
            int left = 0;
            int top = 0;
            int index = 0;
            foreach (var s in Labels)
            {
                var item = new PopupMenuItem(
                (Tags.GA_ID, index),
                (Tags.GA_Left, left),
                (Tags.GA_Width, width),
                (Tags.GA_Height, height),
                (Tags.GA_Top, top),
                (Tags.GA_Text, s),
                (Tags.GA_ToggleSelect, true),
                (Tags.GA_Selected, index == selectedIndex),
                (Tags.ICA_TARGET, this),
                (Tags.ICA_MAP, map)
                );
                item.Items = items;
                top += height;
                index++;
                glist.Add(item);
                items.Add(item);
            }
            return glist;
        }

        private void ShowMenu()
        {
            var items = GetPopupMenuItems();
            popupMenu?.Close();
            popupMenu = Window.OpenPopupMenu(
                (Tags.WA_Left, Bounds.Left),
                (Tags.WA_Top, Bounds.Bottom),
                (Tags.WA_Width, ChooserWidth),
                (Tags.WA_Height, items.Count * chooserItemHeight + 4),
                (Tags.WA_Gadgets, items)
                );

        }

        private void HideMenu()
        {
            popupMenu?.Close();
            popupMenu = null;
        }

        private void ToggleMenu()
        {
            if (popupMenu != null)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }
        }

        public override GoActiveResult GoActive(int x, int y, ref int termination)
        {
            var res = base.GoActive(x, y, ref termination);
            if (res == GoActiveResult.MeActive)
            {
                ToggleMenu();
            }
            return res;
        }

        public override void GoInactive(bool abort)
        {
            base.GoInactive(abort);
            HideMenu();
        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.CHOOSER_Title:
                    Text = tag.GetTagData("");
                    return 1;
                case Tags.CHOOSER_Labels:
                case Tags.CHOOSER_LabelArray:
                    labels = new List<object>(tag.GetTagData<IEnumerable<object>>());
                    return 1;
                case Tags.CHOOSER_Selected:
                    selectedIndex = tag.GetTagData(0);
                    return 1;
                case Tags.CHOOSER_Width:
                    chooserWidth = tag.GetTagData(0);
                    return 1;
                case Tags.CHOOSER_MaxLabels:
                    maxLabels = tag.GetTagData(12);
                    return 1;
                case Tags.CHOOSER_DropDown:
                    popup = !tag.GetTagData(false);
                    return 1;
                case Tags.CHOOSER_PopUp:
                    popup = tag.GetTagData(false);
                    return 1;
            }
            return base.SetTag(set, update, tag);
        }
    }
}
