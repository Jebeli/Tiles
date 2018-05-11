using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    public class TabHeaderGadget : ButtonGadget
    {
        public TabHeaderGadget()
         : this(TagItems.Empty)
        {
        }

        public TabHeaderGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            Activation |= ActivationFlags.ToggleSelect;
            New(tags);
        }

    }
}
