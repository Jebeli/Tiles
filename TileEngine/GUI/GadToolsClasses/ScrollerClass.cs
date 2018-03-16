using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI.GadToolsClasses
{
    public class ScrollerClass : PropGadget, IGadToolsGadget
    {
        public ScrollerClass()
        {

            GadgetKind = GadKind.Scroller;
        }

        public GadKind GadgetKind { get; private set; }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.GTSC_Top:
                    return base.SetTag(gadgetInfo, set, update, (Tags.PGA_Top, tag.Item2));
                case Tags.GTSC_Visible:
                    return base.SetTag(gadgetInfo, set, update, (Tags.PGA_Visible, tag.Item2));
                case Tags.GTSC_Total:
                    return base.SetTag(gadgetInfo, set, update, (Tags.PGA_Total, tag.Item2));
                case Tags.GTA_ScrollerArrow1:
                    Arrow1 = tag.GetTagData(Arrow1);
                    return 1;
                case Tags.GTA_ScrollerArrow2:
                    Arrow2 = tag.GetTagData(Arrow2);
                    return 1;
                case Tags.GTA_ScrollerDec:
                    Set((Tags.PGA_Top, (Top - Visible) + 1));
                    return 1;
                case Tags.GTA_ScrollerInc:
                    Set((Tags.PGA_Top, (Top + Visible) - 1));
                    return 1;
                default:
                    return base.SetTag(gadgetInfo, set, update, tag);
            }
        }

        public ArrowClass Arrow1 { get; set; }
        public ArrowClass Arrow2 { get; set; }
    }
}
