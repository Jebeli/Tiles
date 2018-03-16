using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI
{
    public class GroupGadget : Gadget
    {
        private int oldGroupLeft;
        private int oldGroupTop;
        public GroupGadget() 
        {
            GadgetType &= ~GadgetType.GTYPEMASK;
            GadgetType |= GadgetType.CUSTOMGADGET;
            Flags = GadgetFlags.BOOPSIGADGET;
            Activation &= ~GadgetActivation.GADGIMMEDIATE;
            Activation &= ~GadgetActivation.RELVERIFY;
            Width = 0;
            Height = 0;
        }

        protected override int BeforeSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update)
        {
            oldGroupLeft = LeftEdge;
            oldGroupTop = TopEdge;
            return 0;
        }

        protected override int AfterSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, int returnValue)
        {
            int newGroupLeft = LeftEdge;
            int newGroupTop = TopEdge;
            int dx = newGroupLeft - oldGroupLeft;
            int dy = newGroupTop - oldGroupTop;
            if (dx != 0 || dy != 0)
            {
                foreach (var mem in Members)
                {
                    if (mem is Gadget gad)
                    {
                        gad.Set(
                            (Tags.GA_Left, gad.LeftEdge + dx),
                            (Tags.GA_Top, gad.TopEdge + dy)
                            );
                    }
                }
            }
            return base.AfterSetTags(gadgetInfo, set, update, returnValue);
        }

        public override int AddMember(Root member)
        {
            int pos = base.AddMember(member);
            if (member is Gadget gad)
            {
                gad.Set(
                    (Tags.GA_Left, gad.LeftEdge + LeftEdge),
                    (Tags.GA_Top, gad.TopEdge + TopEdge)
                    );
            }
            RecalcGroupSize();
            return pos;
        }

        public override bool RemMember(Root member)
        {
            bool ok = base.RemMember(member);
            if (member is Gadget gad)
            {
                gad.Set(
                    (Tags.GA_Left, gad.LeftEdge - LeftEdge),
                    (Tags.GA_Top, gad.TopEdge - TopEdge)
                    );
            }
            RecalcGroupSize();
            return ok;
        }

        public override void Render(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw)
        {
            IBox container = null;
            GetGadgetIBox(gadgetInfo, ref container);
            if ((container.Width <= 1) || (container.Height <= 1))
                return;
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
            PrintGadgetLabel(gadgetInfo, ImageState, graphics);
        }

        private void RecalcGroupSize()
        {
            int w = 0;
            int h = 0;
            int width = 0;
            int height = 0;
            foreach (var mem in Members)
            {
                if (mem is Gadget gad)
                {
                    w = gad.LeftEdge - LeftEdge + gad.Width;
                    h = gad.TopEdge - TopEdge + gad.Height;
                    if (w > width) width = w;
                    if (h > height) height = h;
                }
            }
            Width = width;
            Height = height;
        }
    }
}
