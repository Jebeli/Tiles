using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.YGUI
{
    public class PopupWindow : Window
    {
        private Gadget parentGadget;
        private Window parentWindow;

        public PopupWindow(Gadget parent)
            : base(parent.Screen)
        {
            layout = new BoxLayout(Orientation.Vertical, Alignment.Fill);
            parentGadget = parent;
            parentWindow = parent.Window;
            ThinBorder = true;
        }

        public Window ParentWindow
        {
            get { return parentWindow; }
        }

        public Gadget ParentGadget
        {
            get { return parentGadget; }
        }


    }
}
