using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.GUI.GadToolsClasses
{
    public class SliderClass : PropGadget, IGadToolsGadget
    {
        private bool setVal;
        public SliderClass()
        {
            GadgetKind = GadKind.Slider;
        }

        public GadKind GadgetKind { get; private set; }

        protected override int BeforeSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update)
        {
            setVal = false;
            return base.BeforeSetTags(gadgetInfo, set, update);
        }
        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.GTSL_Level:
                    Level = tag.GetTagData(Level);
                    setVal = true;
                    return 1;
                case Tags.GTSL_Max:
                    Max = tag.GetTagData(Max);
                    setVal = true;
                    return 1;
                case Tags.GTSL_Min:
                    Min = tag.GetTagData(Min);
                    setVal = true;
                    return 1;
                default:
                    return base.SetTag(gadgetInfo, set, update, tag);
            }
        }

        protected override int AfterSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, int returnValue)
        {
            if (setVal)
            {
                SetValues();
                NotifyLevel(gadgetInfo, true);
            }
            return base.AfterSetTags(gadgetInfo, set, update, returnValue);
        }

        public override GadgetActive HandleInput(GadgetInfo gadgetInfo, InputEvent inputEvent, ref int termination, int mouseX, int mouseY)
        {
            GadgetActive ga = base.HandleInput(gadgetInfo, inputEvent, ref termination, mouseX, mouseY);
            switch (inputEvent.InputClass)
            {
                case InputClass.MOUSEMOVE:
                    if ((PropInfo.Flags & PropFlags.KNOBHIT) == PropFlags.KNOBHIT)
                    {
                        UpdateLevel(gadgetInfo, false);
                    }
                    break;
                case InputClass.MOUSEUP:
                    UpdateLevel(gadgetInfo, true);
                    break;
            }
            return ga;
        }

        private void NotifyLevel(GadgetInfo gadgetInfo, bool final)
        {
            UpdateFlags flags = final ? UpdateFlags.Final : UpdateFlags.Interim;
            Notify(gadgetInfo, flags, (Tags.GTSL_Level, Level), (Tags.GA_ID, GadgetId));
        }

        private void UpdateLevel(GadgetInfo gadgetInfo, bool final)
        {
            int newLevel = Min + Top;
            if (newLevel != Level)
            {
                Level = Min + Top;
                NotifyLevel(gadgetInfo, final);
            }
        }

        private void SetValues()
        {
            setVal = false;
            int num = Max - Min + 1;
            Set((Tags.PGA_Total, num),
                (Tags.PGA_Visible, 1),
                (Tags.PGA_Top, (Level - Min)));
        }

        public int Level { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }


}
