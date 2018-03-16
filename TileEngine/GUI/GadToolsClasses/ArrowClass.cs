using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI.GadToolsClasses
{
    public class ArrowClass : FrameButtonGadget
    {
        public ArrowClass()
        {

        }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.GTA_ArrowScroller:
                    Scroller = tag.GetTagData(Scroller);
                    return 1;
                case Tags.GTA_Arrow_Type:
                    ArrowType = tag.GetTagData(SysImageType.Left);
                    return 1;
                default:
                    return base.SetTag(gadgetInfo, set, update, tag);
            }
        }

        protected override int AfterSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, int returnValue)
        {
            GadgetImage = Intuition.NewObject(Intuition.SYSICLASS, (Tags.SYSIA_Which, ArrowType), (Tags.SYSIA_Flags, SysImageFlags.GadTools)) as Image;
            GadgetImage.Width = Width;
            GadgetImage.Height = Height;
            return base.AfterSetTags(gadgetInfo, set, update, returnValue);
        }

        private void UpdateScroller()
        {
            if (Scroller != null)
            {
                switch (ArrowType)
                {
                    case SysImageType.Up:
                    case SysImageType.Left:
                        Scroller.Set((Tags.GTA_ScrollerDec, true));
                        break;
                    case SysImageType.Down:
                    case SysImageType.Right:
                        Scroller.Set((Tags.GTA_ScrollerInc, true));
                        break;
                }
            }
        }

        public override GadgetActive HandleInput(GadgetInfo gadgetInfo, InputEvent inputEvent, ref int termination, int mouseX, int mouseY)
        {
            GadgetActive ga = base.HandleInput(gadgetInfo, inputEvent, ref termination, mouseX, mouseY);
            switch (inputEvent.InputClass)
            {
                case InputClass.MOUSEMOVE:
                    break;
                case InputClass.MOUSEUP:
                    break;
                case InputClass.GADGETUP:
                    break;
                case InputClass.GADGETDOWN:
                    UpdateScroller();
                    break;
                case InputClass.TIMER:
                    if (Selected)
                        UpdateScroller();
                    break;
            }

            return ga;
        }

        public ScrollerClass Scroller { get; set; }
        public SysImageType ArrowType { get; set; }
    }
}
