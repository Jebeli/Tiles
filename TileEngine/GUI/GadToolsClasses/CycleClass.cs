using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI.GadToolsClasses
{
    public class CycleClass : FrameButtonGadget, IGadToolsGadget
    {
        private const int CYCLEIMAGEWIDTH = 19;
        public CycleClass()
        {
            Flags |= GadgetFlags.GADGIMAGE;
            var img = Intuition.NewObject(Intuition.FRAMEICLASS, (Tags.IA_FrameType, FrameType.Button));
            GadgetImage = img as Image;
            GadgetKind = GadKind.Cycle;
        }

        public GadKind GadgetKind { get; private set; }


        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.GTCY_Active:
                    Active = tag.GetTagData(Active);
                    return 1;
                case Tags.GTCY_Labels:
                    Labels = tag.GetTagData(Labels);
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
                Active = (Active + 1) % NumLabels;
            }
            return ga;
        }

        protected override void RenderBase(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw, IBox container)
        {
            base.RenderBase(gadgetInfo, graphics, redraw, container);
            RenderCycleLabel(gadgetInfo, graphics, redraw, container, CurrentLabel);
        }

        private void RenderCycleLabel(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw, IBox container, string label)
        {
            if (!string.IsNullOrEmpty(label))
            {
                int x = container.LeftEdge + ((container.Width - CYCLEIMAGEWIDTH) / 2);
                int y = container.TopEdge + (container.Height / 2);
                graphics.RenderText(label, x, y, gadgetInfo.DrawInfo.TextPen);
                x = container.LeftEdge + container.Width - CYCLEIMAGEWIDTH;
                graphics.RectFill(x + 1, container.TopEdge + 2, x + 1, container.BottomEdge - 2, gadgetInfo.DrawInfo.ShinePen);
                graphics.RectFill(x, container.TopEdge + 2, x, container.BottomEdge - 2, gadgetInfo.DrawInfo.ShadowPen);
                int h = container.Height / 2;
                x += 6;
                for (y = 0; y < 4; y++)
                {
                    graphics.RectFill(x + y, container.BottomEdge - h - y - 1, x + 6 - y, container.BottomEdge - h - y - 1, gadgetInfo.DrawInfo.ShadowPen);
                    graphics.RectFill(x + y, container.TopEdge + h + y + 1, x + 6 - y, container.TopEdge + h + y + 1, gadgetInfo.DrawInfo.ShadowPen);
                }
            }
        }

        public string CurrentLabel
        {
            get
            {
                if (Labels != null && Active >= 0 && Active < Labels.Count)
                {
                    return Labels[Active].ToString();
                }
                return null;
            }
        }
        public int NumLabels { get { return Labels != null ? Labels.Count : 0; } }
        public int Active { get; set; }
        public IList<object> Labels { get; set; }

    }
}
