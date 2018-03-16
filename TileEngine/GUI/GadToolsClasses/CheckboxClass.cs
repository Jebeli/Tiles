using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI.GadToolsClasses
{
    public class CheckboxClass : FrameButtonGadget, IGadToolsGadget
    {
        public CheckboxClass()
        {
            Flags |= GadgetFlags.GADGIMAGE;
            var img = Intuition.NewObject(Intuition.SYSICLASS, (Tags.SYSIA_Which, SysImageType.Check));
            GadgetImage = img as Image;
            GadgetKind = GadKind.Checkbox;
        }
        public GadKind GadgetKind { get; private set; }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.GTCB_Checked:
                    if (tag.GetTagData(false))
                    {
                        Flags |= GadgetFlags.CHECKED;
                    }
                    else
                    {
                        Flags &= ~GadgetFlags.CHECKED;
                    }
                    return 1;
                default:
                    return base.SetTag(gadgetInfo, set, update, tag);
            }
        }

        public override GadgetActive HandleInput(GadgetInfo gadgetInfo, InputEvent inputEvent, ref int termination, int mouseX, int mouseY)
        {
            GadgetActive ga = base.HandleInput(gadgetInfo, inputEvent, ref termination, mouseX, mouseY);
            if (inputEvent.InputClass == InputClass.GADGETUP)
            {
                if (Checked)
                {
                    Flags &= ~GadgetFlags.CHECKED;
                }
                else
                {
                    Flags |= GadgetFlags.CHECKED;
                }
            }
            return ga;
        }
    }
}
