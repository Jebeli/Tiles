using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI.GadToolsClasses
{
    public class TextClass : FrameButtonGadget, IGadToolsGadget
    {
        public TextClass()
        {
            Flags |= GadgetFlags.GADGHNONE;
            GadgetKind = GadKind.Text;
        }

        public GadKind GadgetKind { get; private set; }


        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.GTA_GadgetKind:
                    GadgetKind = tag.GetTagData(GadKind.Text);
                    return 0;
                case Tags.GTTX_Text:
                    GTText = tag.GetTagData(GTText);
                    return 1;
                case Tags.GTNM_Number:
                case Tags.GTSL_Level:
                    GTNumber = tag.GetTagData(GTNumber);
                    return 1;
                case Tags.GTSL_LevelFormat:
                    Format = tag.GetTagData(Format);
                    return 1;
                case Tags.GTTX_Border:
                    GTBorder = tag.GetTagData(false);
                    return 1;
                default:
                    return base.SetTag(gadgetInfo, set, update, tag);
            }
        }

        protected override int AfterSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, int returnValue)
        {
            MakeFrame();
            return base.AfterSetTags(gadgetInfo, set, update, returnValue);
        }

        public override HitTestResult HitTest(GadgetInfo gadgetInfo, int mouseX, int mouseY)
        {
            return HitTestResult.NoHit;
        }

        public override GadgetActive HandleInput(GadgetInfo gadgetInfo, InputEvent inputEvent, ref int termination, int mouseX, int mouseY)
        {
            return GadgetActive.NoReuse;
        }

        protected override void RenderBase(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw, IBox container)
        {
            base.RenderBase(gadgetInfo, graphics, redraw, container);
            string txt = GTRenderText;
            if (!string.IsNullOrEmpty(txt))
            {
                graphics.RenderText(txt, container.CenterX, container.CenterY, gadgetInfo.DrawInfo.TextPen);
            }
        }

        private void MakeFrame()
        {
            if (GTBorder)
            {
                GadgetImage = Intuition.NewObject(Intuition.FRAMEICLASS,
                    (Tags.IA_Width, Width),
                    (Tags.IA_Height, Height),
                    (Tags.IA_FrameType, FrameType.Button),
                    (Tags.IA_Recessed, true)) as Image;
            }
            else
            {
                GadgetImage = null;
            }
        }

        public bool GTBorder { get; set; }
        public string Format { get; set; }
        public string GTText { get; set; }
        public int GTNumber { get; set; }
        public string GTRenderText
        {
            get
            {
                if (GadgetKind == GadKind.Text)
                {
                    return GTText;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Format))
                    {
                        return string.Format(Format, GTNumber);
                    }
                    else return GTNumber.ToString();
                }
            }
        }
    }
}
