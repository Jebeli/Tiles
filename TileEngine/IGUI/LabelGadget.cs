using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.IGUI
{
    public class LabelGadget : Gadget
    {
        public LabelGadget()
            : this(TagItems.Empty)
        {
        }

        public LabelGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            New(tags);
        }
    }
}
