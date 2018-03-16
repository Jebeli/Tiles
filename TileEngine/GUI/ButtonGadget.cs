using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI
{
    public class ButtonGadget : Gadget
    {
        public ButtonGadget()
        {
            GadgetType &= ~GadgetType.GTYPEMASK;
            GadgetType |= GadgetType.BOOLGADGET;
            Flags = GadgetFlags.BOOPSIGADGET;
        }

        public override HitTestResult HitTest(GadgetInfo gadgetInfo, int mouseX, int mouseY)
        {
            return HitTestResult.GadgetHit;
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
                case InputClass.TIMER:
                    break;
            }

            return ga;

        }

        public override void Render(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw)
        {
            IBox container = null;
            GetGadgetIBox(gadgetInfo, ref container);
            if ((container.Width <= 1) || (container.Height <= 1))
                return;
            RenderBase(gadgetInfo, graphics, redraw, container);
            RenderLabel(gadgetInfo, graphics, ImageState, container);
        }

        protected virtual void RenderBase(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw, IBox container)
        {
            if ((Flags & GadgetFlags.GADGIMAGE) != GadgetFlags.GADGIMAGE)
            {
                if ((SelectRender != null) && ((Flags & GadgetFlags.SELECTED) == GadgetFlags.SELECTED))
                {
                    Intuition.DrawBorder(graphics, SelectRender, container.LeftEdge, container.TopEdge);
                }
                else if (GadgetRender != null)
                {
                    Intuition.DrawBorder(graphics, GadgetRender, container.LeftEdge, container.TopEdge);
                }
            }
            else
            {
                if ((SelectImage != null) && ((Flags & GadgetFlags.SELECTED) == GadgetFlags.SELECTED))
                {
                    int x = container.LeftEdge + (container.Width / 2) - (GadgetImage.Width / 2);
                    int y = container.TopEdge + (container.Height / 2) - (GadgetImage.Height / 2);
                    Intuition.DrawImageState(graphics, SelectImage, x, y, ImageState, gadgetInfo.DrawInfo);
                }
                else if (GadgetImage != null)
                {
                    int x = container.LeftEdge + (container.Width / 2) - (GadgetImage.Width / 2);
                    int y = container.TopEdge + (container.Height / 2) - (GadgetImage.Height / 2);
                    Intuition.DrawImageState(graphics, GadgetImage, x, y, ImageState, gadgetInfo.DrawInfo);
                }
            }
        }

        protected virtual void RenderLabel(GadgetInfo gadgetInfo, IGraphics rPort, ImageState state, IBox container)
        {
            if (GadgetText == null) return;
            bool disabled = (state & ImageState.Disabled) == ImageState.Disabled;
            if (disabled)
            {
                GadgetText.FrontPen = gadgetInfo.DrawInfo.DisabledTextPen;
            }
            else
            {
                GadgetText.FrontPen = gadgetInfo.DrawInfo.TextPen;
            }
            switch (Flags & GadgetFlags.LABELMASK)
            {
                case GadgetFlags.LABELITEXT:
                    Intuition.DrawIntuiText(rPort, GadgetText, container.LeftEdge, container.TopEdge);
                    break;
                case GadgetFlags.LABELSTRING:
                    Intuition.DrawIntuiText(rPort, GadgetText, container.LeftEdge, container.TopEdge);
                    break;
                case GadgetFlags.LABELIMAGE:
                    Intuition.DrawIntuiText(rPort, GadgetText, container.LeftEdge, container.TopEdge);
                    break;
            }
        }
    }
}
