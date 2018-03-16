using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI.GadToolsClasses
{
    public class StringClass : StringGadget, IGadToolsGadget
    {
        public StringClass()
        {
            var img = Intuition.NewObject(Intuition.FRAMEICLASS,
                (Tags.IA_FrameType, FrameType.Ridge)) as Image;
            GadgetImage = img;
            GadgetKind = GadKind.String;
        }

        public GadKind GadgetKind { get; private set; }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.GTA_GadgetKind:
                    GadgetKind = tag.GetTagData(GadKind.Text);
                    return 0;
                case Tags.GTST_String:
                    StringInfo.Buffer = tag.GetTagData(StringInfo.Buffer);
                    Activation &= ~GadgetActivation.LONGINT;
                    return 1;
                case Tags.GTIN_Number:
                    StringInfo.LongInt = tag.GetTagData(StringInfo.LongInt);
                    StringInfo.Buffer = StringInfo.LongInt.ToString();
                    Activation |= GadgetActivation.LONGINT;
                    return 1;
                default:
                    return base.SetTag(gadgetInfo, set, update, tag);
            }
        }

        protected override int AfterSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, int returnValue)
        {
            GadgetImage.Width = Width;
            GadgetImage.Height = Height;
            return base.AfterSetTags(gadgetInfo, set, update, returnValue);
        }
    }
}
