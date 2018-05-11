using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.IGUI
{
    public enum Orientation
    {
        None,
        Horizontal,
        Vertical
    }

    public class LayoutGadget : Gadget
    {
        private Orientation orientation;
        private bool spaceInner;
        private bool spaceOuter;
        private int innerSpacing;
        private int topSpacing;
        private int bottomSpacing;
        private int leftSpacing;
        private int rightSpacing;
        private bool frame;

        public LayoutGadget()
           : this(TagItems.Empty)
        {
        }

        public LayoutGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            orientation = Orientation.Horizontal;
            TextPlace = TextPlace.TopLeft;
            spaceInner = true;
            spaceOuter = true;
            innerSpacing = 8;
            leftSpacing = 8;
            topSpacing = 8;
            bottomSpacing = 8;
            rightSpacing = 8;
            New(tags);
            if (!string.IsNullOrEmpty(Text))
            {
                frame = true;
            }
        }

        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public bool Frame
        {
            get { return frame; }
            set { frame = value; }
        }

        public override void Layout(bool initial)
        {
            base.Layout(initial);
            if (Hidden) return;
            int weightX = 0;
            int weightY = 0;
            int fixedX = 0;
            int fixedY = 0;
            int countWX = 0;
            int countWY = 0;
            int count = 0;
            foreach (Gadget g in Members)
            {
                g.Set((Tags.LAYOUT_Parent, this));

                int prefWidth = 0;
                int prefHeight = 0;

                g.GetPreferredSize(ref prefWidth, ref prefHeight);

                weightX += g.WeightedWidth;
                weightY += g.WeightedHeight;
                if (g.WeightedWidth == 0)
                {
                    fixedX += prefWidth;
                }
                else
                {
                    countWX++;
                }
                if (g.WeightedHeight == 0)
                {
                    fixedY += prefHeight;
                }
                else
                {
                    countWY++;
                }
                count++;
            }
            int totalSpace = spaceInner ? (count - 1) * innerSpacing : 0;
            int space = spaceInner ? innerSpacing : 0;
            int ls = spaceOuter ? leftSpacing : 0;
            int rs = spaceOuter ? rightSpacing : 0;
            int ts = spaceOuter ? topSpacing : 0;
            int bs = spaceOuter ? bottomSpacing : 0;
            int width = Bounds.Width - ls - rs;
            int rwidth = width - fixedX - totalSpace;
            int height = Bounds.Height - ts - bs;
            int rheight = height - fixedY - totalSpace;
            double top = ts;
            double left = ls;
            double hh = 0;
            double ww = 0;
            int ihh = 0;
            int iww = 0;
            int offsetX = Bounds.Left - Window.BorderLeft;
            int offsetY = Bounds.Top - Window.BorderTop;
            switch (orientation)
            {
                case Orientation.Horizontal:
                    foreach (Gadget g in Members)
                    {
                        if (g.WeightedWidth == 0)
                        {
                            g.GetPreferredSize(ref iww, ref ihh);
                            ww = iww;
                        }
                        else
                        {
                            ww = g.WeightedWidth * rwidth;
                            ww /= weightX;
                        }
                        g.Set((Tags.GA_Width, (int)ww),
                            (Tags.GA_Left, (int)(left + offsetX)),
                            (Tags.GA_Top, (int)(ts + offsetY)),
                            (Tags.GA_Height, height)
                            );
                        left += ww;
                        left += space;
                    }
                    break;
                case Orientation.Vertical:
                    foreach (Gadget g in Members)
                    {
                        if (g.WeightedHeight == 0)
                        {
                            g.GetPreferredSize(ref iww, ref ihh);
                            hh = ihh;
                        }
                        else
                        {
                            hh = g.WeightedHeight * rheight;
                            hh /= weightY;
                        }
                        g.Set((Tags.GA_Width, width),
                            (Tags.GA_Left, (int)(ls + offsetX)),
                            (Tags.GA_Top, (int)(top + offsetY)),
                            (Tags.GA_Height, (int)hh)
                            );
                        top += hh;
                        top += space;
                    }
                    break;
            }
            foreach (Gadget g in Members)
            {
                g.Layout(initial);
            }
        }

        private void AdjustBorder()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                switch (TextPlace)
                {
                    case TextPlace.TopCenter:
                    case TextPlace.TopLeft:
                    case TextPlace.TopRight:
                        topSpacing = 10;
                        bottomSpacing = 8;
                        break;
                    case TextPlace.BottomCenter:
                    case TextPlace.BottomLeft:
                    case TextPlace.BottomRight:
                        bottomSpacing = 10;
                        topSpacing = 8;
                        break;
                    default:
                        topSpacing = 8;
                        bottomSpacing = 8;
                        break;
                }
            }
            else
            {
                topSpacing = 8;
                bottomSpacing = 8;
            }
        }

        public override void Render(IGraphics gfx)
        {
            base.Render(gfx);
            if (!Hidden && frame)
            {
                Theme.RenderFrame(gfx, this);
            }
        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.LAYOUT_Orientation:
                    orientation = tag.GetTagData(Orientation.Horizontal);
                    return 1;
                case Tags.LAYOUT_AddChild:
                    AddMember(tag.GetTagData<Gadget>());
                    return 1;
                case Tags.LAYOUT_AddChildren:
                    foreach (var g in tag.GetTagData<IEnumerable<Gadget>>())
                    {
                        AddMember(g);
                    }
                    return 1;
                case Tags.LAYOUT_SpaceInner:
                    spaceInner = tag.GetTagData(false);
                    return 1;
                case Tags.LAYOUT_SpaceOuter:
                    spaceOuter = tag.GetTagData(false);
                    return 1;
                case Tags.LAYOUT_InnerSpacing:
                    innerSpacing = tag.GetTagData(0);
                    return 1;
                case Tags.LAYOUT_Label:
                    Text = tag.GetTagData("");
                    AdjustBorder();
                    return 1;
                case Tags.LAYOUT_LabelPlace:
                    TextPlace = tag.GetTagData(TextPlace.TopLeft);
                    AdjustBorder();
                    return 1;
                case Tags.LAYOUT_Frame:
                    frame = tag.GetTagData(false);
                    return 1;
            }
            return base.SetTag(set, update, tag);
        }


    }
}
