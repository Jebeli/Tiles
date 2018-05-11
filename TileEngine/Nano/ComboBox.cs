using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Nano
{
    public class ComboBox : PopupButton
    {
        public ComboBox(Widget parent)
            : base(parent)
        {
            this.items = new List<object>();
            this.shortItems = new List<object>();
        }

        public ComboBox(Widget parent, IList<object> items)
            : base(parent)
        {
            this.items = new List<object>();
            this.shortItems = new List<object>();
            SetItems(items);
        }

        public ComboBox(Widget parent, IList<object> items, IList<object> shortItems)
            : base(parent)
        {
            this.items = new List<object>();
            this.shortItems = new List<object>();
            SetItems(items, shortItems);
        }

        public void SetItems(IList<object> items)
        {
            SetItems(items, items);
        }
        public void SetItems(IList<object> items, IList<object> shortItems)
        {
            this.items.Clear();
            this.shortItems.Clear();
            this.items.AddRange(items);
            this.shortItems.AddRange(shortItems);
            if (selectedIndex < 0 || selectedIndex >= items.Count)
            {
                selectedIndex = 0;
            }
            while (popup.ChildCount > 0)
            {
                popup.RemoveChild(popup.ChildCount - 1);
            }
            popup.Layout = new GroupLayout(10);
            int index = 0;
            foreach (var item in this.items)
            {
                Button button = new Button(popup, item.ToString());
                button.Flags = ButtonFlags.RadioButton;
                button.Changed += (o, i) =>
                {
                    selectedIndex = popup.ChildIndex(button);
                    Caption = shortItems[selectedIndex].ToString();
                    Pushed = false;
                    popup.Visible = false;
                };
                index++;
            }
            SelectedIndex = selectedIndex;
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (shortItems.Count == 0) return;
                ((Button)popup.Children[selectedIndex]).Pushed = false;
                ((Button)popup.Children[value]).Pushed = true;
                selectedIndex = value;
                Caption = shortItems[value].ToString();
            }
        }

        protected List<object> items;
        protected List<object> shortItems;

        protected int selectedIndex;

    }
}
