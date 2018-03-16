using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI.GadToolsClasses
{
    public class ButtonClass : FrameButtonGadget, IGadToolsGadget
    {
        public ButtonClass()
        {
            Flags |= GadgetFlags.GADGIMAGE;
            var img = Intuition.NewObject(Intuition.FRAMEICLASS, (Tags.IA_FrameType, FrameType.Button));
            GadgetImage = img as Image;
            GadgetKind = GadKind.Button;
        }

        public GadKind GadgetKind { get; private set; }

    }
}
