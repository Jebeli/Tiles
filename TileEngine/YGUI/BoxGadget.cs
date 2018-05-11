using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.YGUI
{
    public class BoxGadget : Gadget
    {
        public BoxGadget(Gadget parent, Orientation orientation, Alignment alignment = Alignment.Middle, int margin = 0, int spacing = 0)
            : base(parent)
        {
            layout = new BoxLayout(orientation, alignment, margin, spacing);
        }

        public override string ToString()
        {
            return $"Box {((BoxLayout)layout).Orientation}";
        }

    }
}
