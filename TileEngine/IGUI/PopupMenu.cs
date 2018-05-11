using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Input;

namespace TileEngine.IGUI
{
    public class PopupMenu : Window
    {
        internal PopupMenu(Window parent, params (Tags, object)[] tags)
            : base(parent.Screen, TagItems.Empty)
        {
            Parent = parent;
            //Flags |= WindowFlags.Borderless;
            Flags |= WindowFlags.ToolWindow;
            New(tags);
            AdjustBorder();
            Layout(true);
        }
    }

    public class PopupMenuItem : Gadget
    {
        private List<PopupMenuItem> items;
        public PopupMenuItem()
            : this(TagItems.Empty)
        {
        }

        public PopupMenuItem(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            Activation &= ~ActivationFlags.RelVerify;
            New(tags);
        }

        public List<PopupMenuItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        public override HitTestResult HitTest(int x, int y)
        {
            foreach(var item in items)
            {
                item.TogSelect = item == this;
                item.Selected = item == this;
            }
            return base.HitTest(x, y);
        }

        public override GoActiveResult HandleInput(IDCMPFlags idcmp, int x, int y, MouseButton button, InputCode code)
        {
            if (idcmp == IDCMPFlags.GadgetDown)
            {
                Notify(UpdateFlags.Final, (Tags.GA_ID, ID));
            }
            return base.HandleInput(idcmp, x, y, button, code);
        }
    }
}
