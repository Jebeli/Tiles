using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI.GadToolsClasses
{
    public class MxClass : FrameButtonGadget, IGadToolsGadget
    {
        public MxClass()
        {
            GadgetImage = Intuition.NewObject(Intuition.SYSICLASS, (Tags.SYSIA_Which, SysImageType.Mx), (Tags.SYSIA_WithBorder, false)) as Image;
            GadgetKind = GadKind.Mx;
        }

        public GadKind GadgetKind { get; private set; }

        public override GadgetActive HandleInput(GadgetInfo gadgetInfo, InputEvent inputEvent, ref int termination, int mouseX, int mouseY)
        {
            GadgetActive ga = base.HandleInput(gadgetInfo, inputEvent, ref termination, mouseX, mouseY);
            if (inputEvent.InputClass == InputClass.GADGETUP)
            {
                int active = MxGadgets.IndexOf(this);
                foreach (Gadget g in MxGadgets)
                {
                    ((MxClass)g).Active = active;
                    if (g == this)
                    {
                        g.Checked = true;
                    }
                    else
                    {
                        g.Checked = false;
                    }
                }
            }

            return ga;
        }

        public List<Gadget> MxGadgets { get; set; }
        public int Active { get; set; }
    }
}
