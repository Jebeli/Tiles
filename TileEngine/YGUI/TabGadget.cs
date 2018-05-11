using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.YGUI
{
    public class TabGadget : BoxGadget
    {
        public TabGadget(Gadget parent, string label, Orientation orientation, Alignment alignment = Alignment.Middle, int margin = 0, int spacing = 0)
            : base(parent, orientation, alignment, margin, spacing)
        {
            Label = label;
        }
    }
}
