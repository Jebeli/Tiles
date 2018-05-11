using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    public class ButtonGadget : Gadget
    {
        public ButtonGadget()
        : this(TagItems.Empty)
        {
        }

        public ButtonGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            New(tags);
        }
    
    }
}
