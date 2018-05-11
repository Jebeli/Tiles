using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.YGUI
{
    public class TabHeaderGadget : ButtonGadget
    {
        public TabHeaderGadget(Gadget parent, string label)
            : base(parent, label)
        {
            Sticky = true;
        }


    }
}
