using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI
{
    public class SysImage : Image
    {
        private const int HSPACING = 3;
        private const int VSPACING = 3;
        private const int HSPACING_MIDDLE = 2;
        private const int VSPACING_MIDDLE = 2;
        private const int HSPACING_SMALL = 1;
        private const int VSPACING_SMALL = 1;
        private static bool thickMxImage = false;
        public SysImage()
        {
        }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.SYSIA_DrawInfo:
                    DrawInfo = tag.GetTagData<DrawInfo>();
                    return 0;
                case Tags.SYSIA_Flags:
                    SysImageFlags = tag.GetTagData<SysImageFlags>();
                    return 1;
                case Tags.SYSIA_WithBorder:
                    if (tag.GetTagData(false))
                    {
                        SysImageFlags &= ~SysImageFlags.NoBorder;
                    }
                    else
                    {
                        SysImageFlags |= SysImageFlags.NoBorder;
                    }
                    return 1;
                case Tags.SYSIA_Which:
                    SysImageType = tag.GetTagData<SysImageType>();
                    switch (SysImageType)
                    {
                        case SysImageType.Depth:
                        case SysImageType.Zoom:
                            Width = 24;
                            Height = 20;
                            break;
                        case SysImageType.Size:
                            Width = 18;
                            Height = 18;
                            break;
                        case SysImageType.Close:
                            Width = 20;
                            Height = 20;
                            break;
                        case SysImageType.SDepth:
                            Width = 23;
                            Height = 20;
                            break;
                        case SysImageType.Left:
                        case SysImageType.Right:
                        case SysImageType.Up:
                        case SysImageType.Down:
                            Width = 23;
                            Height = 22;
                            break;
                        case SysImageType.Check:
                            Width = 26;
                            Height = 11;
                            break;
                        case SysImageType.Mx:
                            Width = 17;
                            Height = 9;
                            break;
                        case SysImageType.Drag:
                            Width = -(24 * 2 + 20);
                            Height = 20;
                            break;
                    }
                    return 1;
                case Tags.SYSIA_Size:
                    return 1;
                default:
                    return base.SetTag(gadgetInfo, set, update, tag);

            }
        }

        public override void Draw(IGraphics rport, int x, int y, ImageState state, DrawInfo drawInfo = null)
        {
            InternalDrawImage(rport, x, y, Width, Height, state, drawInfo);
        }
        public override void DrawFrame(IGraphics rport, int x, int y, int width, int height, ImageState state, DrawInfo drawInfo = null)
        {
            InternalDrawImage(rport, x, y, width, height, state, drawInfo);
        }

        private void InternalDrawImage(IGraphics rport, int left, int top, int width, int height, ImageState state, DrawInfo drawInfo = null)
        {
            int right = left + width - 1;
            int bottom = top + height - 1;
            int h_spacing;
            int v_spacing;
            int cy;
            int cx;
            Color bg = drawInfo.FillPen;
            Color fg = drawInfo.ShinePen;
            Color afg = drawInfo.ShinePen;
            Color sg = drawInfo.ShinePen;
            Color dg = drawInfo.ShadowPen;
            bool inactive = (state & ImageState.Inactive) == ImageState.Inactive;
            if (inactive)
            {
                bg = drawInfo.BackgoundPen;
                if ((state & ImageState.Hover) == ImageState.Hover)
                {
                    bg = drawInfo.InactiveHoverBackgroundPen;
                }
                afg = drawInfo.ShinePen;
            }
            else
            {
                if ((state & ImageState.Hover) == ImageState.Hover)
                {
                    bg = drawInfo.HoverBackgroundPen;
                }
            }
            if ((state & ImageState.Selected) == ImageState.Selected)
            {
                fg = drawInfo.BackgoundPen;
                sg = drawInfo.ShadowPen;
                dg = drawInfo.ShinePen;
            }
            switch (SysImageType)
            {
                case SysImageType.Check:
                    if (WithBorder)
                    {
                        RenderImageFrame(rport, SysImageType, left, top, width, height, state, drawInfo);
                        left++;
                        top++;
                        right--;
                        bottom--;
                        width -= 2;
                        height -= 2;
                    }
                    rport.RectFill(left, top, right, bottom, bg);
                    h_spacing = width / 4;
                    v_spacing = height / 4;
                    if ((state & ImageState.Checked) == ImageState.Checked)
                    {
                        left += h_spacing;
                        right -= h_spacing;
                        width -= h_spacing * 2;
                        top += v_spacing;
                        bottom -= v_spacing;
                        height -= v_spacing * 2;
                        rport.DrawThickLine(left, top + height / 3, left, bottom, drawInfo.ShadowPen);
                        rport.DrawThickLine(left + 1, bottom, right - 1, top, drawInfo.ShadowPen);
                    }
                    break;
                case SysImageType.Mx:
                    if (WithBorder)
                    {
                        RenderImageFrame(rport, SysImageType, left, top, width, height, state, drawInfo);
                        left++;
                        top++;
                        right--;
                        bottom--;
                        width -= 2;
                        height -= 2;
                        
                    }
                    rport.RectFill(left+1, top+1, right-1, bottom-1, bg);
                    if (thickMxImage)
                    {
                        rport.RectFill(left + 2, top, right - 3, top + 1, sg);
                        rport.RectFill(left + 1, top + 2, left + 2, top + 3, sg);
                        rport.RectFill(left, top + 4, left + 1, bottom - 4, sg);
                        rport.RectFill(left + 1, bottom - 3, left + 2, bottom - 2, sg);
                        rport.RectFill(left + 2, bottom - 1, left + 2, bottom, sg);

                        rport.RectFill(right - 2, top, right - 2, top + 1, dg);
                        rport.RectFill(right - 2, top + 2, right - 1, top + 3, dg);
                        rport.RectFill(right - 1, top + 4, right, bottom - 4, dg);
                        rport.RectFill(right - 2, bottom - 3, right - 1, bottom - 2, dg);
                        rport.RectFill(left + 3, bottom - 1, right - 2, bottom, dg);
                        if ((state & ImageState.Checked) == ImageState.Checked)
                        {
                            left += 4;
                            right -= 4;
                            width -= 8;
                            top += 4;
                            bottom -= 4;
                            height -= 8;
                            if ((width >= 3) && (height >= 3))
                            {
                                rport.RectFill(left + 1, top, right - 1, top, drawInfo.FillPen);
                                rport.RectFill(left, top + 1, right, bottom - 1, drawInfo.FillPen);
                                rport.RectFill(left + 1, bottom, right - 1, bottom, drawInfo.FillPen);
                            }
                            else
                            {
                                rport.RectFill(left, top, right, bottom, drawInfo.FillPen);
                            }
                        }
                    }
                    else
                    {
                        rport.RectFill(left + 3, top, right - 3, top, sg);
                        rport.WritePixel(left + 2, top + 1, sg);
                        rport.RectFill(left + 1, top + 2, left + 1, top + 3, sg);
                        rport.RectFill(left, top + 4, left, bottom - 4, sg);
                        rport.RectFill(left + 1, bottom - 3, left + 1, bottom - 2, sg);
                        rport.WritePixel(left + 2, bottom - 1, sg);

                        rport.WritePixel(right - 2, top + 1, dg);
                        rport.RectFill(right - 1, top + 2, right - 1, top + 3, dg);
                        rport.RectFill(right, top + 4, right, bottom - 4, dg);
                        rport.RectFill(right - 1, bottom - 3, right - 1, bottom - 2, dg);
                        rport.WritePixel(right - 2, bottom - 1, dg);
                        rport.RectFill(left + 3, bottom, right - 3, bottom, dg);

                        if ((state & ImageState.Checked) == ImageState.Checked)
                        {
                            left += 3;
                            right -= 3;
                            width -= 6;
                            top += 3;
                            bottom -= 3;
                            height -= 6;
                            if ((width >= 5) && (height >= 5))
                            {
                                rport.RectFill(left, top + 2, left, bottom - 2, drawInfo.FillPen);
                                rport.RectFill(left + 1, top + 1, left + 1, bottom - 1, drawInfo.FillPen);
                                rport.RectFill(left + 2, top, right - 2, bottom, drawInfo.FillPen);
                                rport.RectFill(right - 1, top + 1, right - 1, bottom - 1, drawInfo.FillPen);
                                rport.RectFill(right, top + 2, right, bottom - 2, drawInfo.FillPen);
                            }
                            else
                            {
                                rport.RectFill(left, top, right, bottom, drawInfo.FillPen);
                            }
                        }
                    }
                    break;
                case SysImageType.Left:
                    if (WithBorder)
                    {
                        RenderImageFrame(rport, SysImageType, left, top, width, height, state, drawInfo);
                        left++;
                        top++;
                        right--;
                        bottom--;
                        width -= 2;
                        height -= 2;
                    }
                    rport.RectFill(left, top, right, bottom, bg);
                    h_spacing = HSPACING;
                    v_spacing = VSPACING;
                    if (width <= 12) { h_spacing = HSPACING_MIDDLE; }
                    if (width <= 10) { h_spacing = HSPACING_SMALL; }
                    if (height <= 12) { v_spacing = VSPACING_MIDDLE; }
                    if (height <= 10) { v_spacing = VSPACING_SMALL; }
                    if ((SysImageFlags & SysImageFlags.GadTools) == SysImageFlags.GadTools)
                    {
                        cy = height / 2;
                        rport.DrawLine(left + width - 1 - h_spacing, top + v_spacing + 1, left + h_spacing, top + height - cy, drawInfo.ShadowPen);
                        rport.DrawLine(left + h_spacing, top + height - cy, left + width - 1 - h_spacing, top + v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + width - 1 - h_spacing, top + v_spacing, left + h_spacing, top + height - cy - 1, drawInfo.ShadowPen);

                        rport.DrawLine(left + width - 1 - h_spacing, top + height - 1 - v_spacing - 1, left + h_spacing, top + cy - 1, drawInfo.ShadowPen);
                        rport.DrawLine(left + h_spacing, top + cy - 1, left + width - 1 - h_spacing, top + height - 1 - v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + width - 1 - h_spacing, top + height - 1 - v_spacing, left + h_spacing, top + cy, drawInfo.ShadowPen);
                    }
                    else
                    {
                        left += h_spacing;
                        top += v_spacing;
                        width -= h_spacing * 2;
                        height -= v_spacing * 2;
                        right = left + width - 1;
                        bottom = top + height - 1;

                        cy = (height + 1) / 2;
                        for (int i = 0; i < cy; i++)
                        {
                            rport.RectFill(left + (cy - i - 1) * width / cy, top + i, right - i * width / cy / 2, top + i, drawInfo.ShadowPen);
                            rport.RectFill(left + (cy - i - 1) * width / cy, bottom - i, right - i * width / cy / 2, bottom - i, drawInfo.ShadowPen);
                        }
                    }
                    break;
                case SysImageType.Up:
                    if (WithBorder)
                    {
                        RenderImageFrame(rport, SysImageType, left, top, width, height, state, drawInfo);
                        left++;
                        top++;
                        right--;
                        bottom--;
                        width -= 2;
                        height -= 2;
                    }
                    rport.RectFill(left, top, right, bottom, bg);
                    h_spacing = HSPACING;
                    v_spacing = VSPACING;
                    if (width <= 12) { h_spacing = HSPACING_MIDDLE; }
                    if (width <= 10) { h_spacing = HSPACING_SMALL; }
                    if (height <= 12) { v_spacing = VSPACING_MIDDLE; }
                    if (height <= 10) { v_spacing = VSPACING_SMALL; }
                    if ((SysImageFlags & SysImageFlags.GadTools) == SysImageFlags.GadTools)
                    {
                        cx = width / 2;
                        rport.DrawLine(left + h_spacing + 1, top + height - 1 - v_spacing, left + width - cx, top + v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + width - cx, top + v_spacing, left + h_spacing, top + height - 1 - v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + h_spacing, top + height - 1 - v_spacing, left + width - cx - 1, top + v_spacing, drawInfo.ShadowPen);

                        rport.DrawLine(left + width - 1 - h_spacing - 1, top + height - 1 - v_spacing, left + cx - 1, top + v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + cx - 1, top + v_spacing, left + width - 1 - h_spacing, top + height - 1 - v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + width - 1 - h_spacing, top + height - 1 - v_spacing, left + cx, top + v_spacing, drawInfo.ShadowPen);
                    }
                    else
                    {
                        left += h_spacing;
                        top += v_spacing;
                        width -= h_spacing * 2;
                        height -= v_spacing * 2;
                        right = left + width - 1;
                        bottom = top + height - 1;

                        cx = (width + 1) / 2;
                        for (int i = 0; i < cx; i++)
                        {
                            rport.RectFill(left + i, top + (cx - i - 1) * height / cx, left + i, bottom - i * height / cx / 2, drawInfo.ShadowPen);
                            rport.RectFill(right - i, top + (cx - i - 1) * height / cx, right - i, bottom - i * height / cx / 2, drawInfo.ShadowPen);
                        }
                    }
                    break;
                case SysImageType.Right:
                    if (WithBorder)
                    {
                        RenderImageFrame(rport, SysImageType, left, top, width, height, state, drawInfo);
                        left++;
                        top++;
                        right--;
                        bottom--;
                        width -= 2;
                        height -= 2;
                    }
                    rport.RectFill(left, top, right, bottom, bg);
                    h_spacing = HSPACING;
                    v_spacing = VSPACING;
                    if (width <= 12) { h_spacing = HSPACING_MIDDLE; }
                    if (width <= 10) { h_spacing = HSPACING_SMALL; }
                    if (height <= 12) { v_spacing = VSPACING_MIDDLE; }
                    if (height <= 10) { v_spacing = VSPACING_SMALL; }
                    if ((SysImageFlags & SysImageFlags.GadTools) == SysImageFlags.GadTools)
                    {
                        cy = height / 2;

                        rport.DrawLine(left + h_spacing, top + v_spacing + 1, left + width - 1 - h_spacing, top + height - cy, drawInfo.ShadowPen);
                        rport.DrawLine(left + width - 1 - h_spacing, top + height - cy, left + h_spacing, top + v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + h_spacing, top + v_spacing, left + width - 1 - h_spacing, top + height - cy - 1, drawInfo.ShadowPen);

                        rport.DrawLine(left + h_spacing, top + height - 1 - v_spacing - 1, left + width - 1 - h_spacing, top + cy - 1, drawInfo.ShadowPen);
                        rport.DrawLine(left + width - 1 - h_spacing, top + cy - 1, left + h_spacing, top + height - 1 - v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + h_spacing, top + height - 1 - v_spacing, left + width - 1 - h_spacing, top + cy, drawInfo.ShadowPen);
                    }
                    else
                    {
                        left += h_spacing;
                        top += v_spacing;
                        width -= h_spacing * 2;
                        height -= v_spacing * 2;
                        right = left + width - 1;
                        bottom = top + height - 1;

                        cy = (height + 1) / 2;
                        for (int i = 0; i < cy; i++)
                        {
                            rport.RectFill(left + i * width / cy / 2, top + i, right - (cy - i - 1) * width / cy, top + i, drawInfo.ShadowPen);
                            rport.RectFill(left + i * width / cy / 2, bottom - i, right - (cy - i - 1) * width / cy, bottom - i, drawInfo.ShadowPen);
                        }
                    }
                    break;
                case SysImageType.Down:
                    if (WithBorder)
                    {
                        RenderImageFrame(rport, SysImageType, left, top, width, height, state, drawInfo);
                        left++;
                        top++;
                        right--;
                        bottom--;
                        width -= 2;
                        height -= 2;
                    }
                    rport.RectFill(left, top, right, bottom, bg);
                    h_spacing = HSPACING;
                    v_spacing = VSPACING;
                    if (width <= 12) { h_spacing = HSPACING_MIDDLE; }
                    if (width <= 10) { h_spacing = HSPACING_SMALL; }
                    if (height <= 12) { v_spacing = VSPACING_MIDDLE; }
                    if (height <= 10) { v_spacing = VSPACING_SMALL; }
                    if ((SysImageFlags & SysImageFlags.GadTools) == SysImageFlags.GadTools)
                    {
                        cx = width / 2;

                        rport.DrawLine(left + h_spacing + 1, top + v_spacing, left + width - cx, top + height - 1 - v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + width - cx, top + height - 1 - v_spacing, left + h_spacing, top + v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + h_spacing, top + v_spacing, left + width - cx - 1, top + height - 1 - v_spacing, drawInfo.ShadowPen);

                        rport.DrawLine(left + width - 1 - h_spacing - 1, top + v_spacing, left + cx - 1, top + height - 1 - v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + cx - 1, top + height - 1 - v_spacing, left + width - 1 - h_spacing, top + v_spacing, drawInfo.ShadowPen);
                        rport.DrawLine(left + width - 1 - h_spacing, top + v_spacing, left + cx, top + height - 1 - v_spacing, drawInfo.ShadowPen);
                    }
                    else
                    {
                        left += h_spacing;
                        top += v_spacing;
                        width -= h_spacing * 2;
                        height -= v_spacing * 2;
                        right = left + width - 1;
                        bottom = top + height - 1;

                        cx = (width + 1) / 2;
                        for (int i = 0; i < cx; i++)
                        {
                            rport.RectFill(left + i, top + i * height / cx / 2, left + i, bottom - (cx - i - 1) * height / cx, drawInfo.ShadowPen);
                            rport.RectFill(right - i, top + i * height / cx / 2, right - i, bottom - (cx - i - 1) * height / cx, drawInfo.ShadowPen);
                        }
                    }
                    break;
                case SysImageType.Close:
                    if (WithBorder)
                    {
                        RenderImageFrame(rport, SysImageType, left, top, width, height, state, drawInfo);
                        left++;
                        top++;
                        right--;
                        bottom--;
                        width -= 2;
                        height -= 2;
                    }
                    rport.RectFill(left, top, right, bottom, bg);
                    h_spacing = width * 4 / 10;
                    v_spacing = height * 3 / 10;
                    left += h_spacing;
                    right -= h_spacing;
                    top += v_spacing;
                    bottom -= v_spacing;
                    rport.RectFill(left, top, right, bottom, drawInfo.ShadowPen);
                    left++;
                    top++;
                    right--;
                    bottom--;
                    rport.RectFill(left, top, right, bottom, fg);
                    break;
                case SysImageType.Zoom:
                    if (WithBorder)
                    {
                        RenderImageFrame(rport, SysImageType, left, top, width, height, state, drawInfo);
                        left++;
                        top++;
                        right--;
                        bottom--;
                        width -= 2;
                        height -= 2;
                    }
                    rport.RectFill(left, top, right, bottom, bg);
                    h_spacing = width / 6;
                    v_spacing = height / 6;
                    left += h_spacing;
                    right -= h_spacing;
                    top += v_spacing;
                    bottom -= v_spacing;
                    rport.RectFill(left, top, right, bottom, drawInfo.ShadowPen);
                    rport.RectFill(left + 1, top + 1, right - 1, bottom - 1, afg);
                    right = left + (right - left + 1) / 2;
                    bottom = top + (bottom - top + 1) / 2;
                    if (right - left < 4) right = left + 4;
                    rport.RectFill(left, top, right, bottom, drawInfo.ShadowPen);
                    left += 2;
                    right -= 2;
                    top += 1;
                    bottom -= 1;
                    rport.RectFill(left, top, right, bottom, drawInfo.BackgoundPen);
                    break;
                case SysImageType.Depth:
                    if (WithBorder)
                    {
                        RenderImageFrame(rport, SysImageType, left, top, width, height, state, drawInfo);
                        left++;
                        top++;
                        right--;
                        bottom--;
                        width -= 2;
                        height -= 2;
                    }
                    rport.RectFill(left, top, right, bottom, bg);
                    h_spacing = width / 6;
                    v_spacing = height / 6;
                    left += h_spacing;
                    top += v_spacing;

                    width -= h_spacing * 2;
                    height -= v_spacing * 2;

                    right = left + width - 1;
                    bottom = top + height - 1;
                    rport.DrawRect(left, top, right - (width / 3), bottom - (height / 3), drawInfo.ShadowPen);
                    rport.RectFill(left + 1, top + 1, right - (width / 3) - 1, bottom - (height / 3) - 1, drawInfo.BackgoundPen);
                    rport.DrawRect(left + (width / 3), top + (height / 3), right, bottom, drawInfo.ShadowPen);
                    rport.RectFill(left + (width / 3) + 1, top + (height / 3) + 1, right - 1, bottom - 1, afg);
                    break;
                case SysImageType.Size:
                    if (WithBorder)
                    {
                        RenderImageFrame(rport, SysImageType, left, top, width, height, state, drawInfo);
                        left++;
                        top++;
                        right--;
                        bottom--;
                        width -= 2;
                        height -= 2;
                    }
                    rport.RectFill(left, top, right, bottom, bg);
                    h_spacing = width / 5;
                    v_spacing = height / 5;
                    left += h_spacing;
                    top += v_spacing;

                    right = left + width - 1 - (h_spacing * 2);
                    bottom = top + height - 1 - (v_spacing * 2);

                    width = right - left + 1;
                    height = bottom - top + 1;
                    if ((state & ImageState.Inactive) != ImageState.Inactive)
                    {
                        for (int y = top; y <= bottom; y++)
                        {
                            int x = left + (bottom - y) * width / height;
                            rport.RectFill(x, y, right, y, drawInfo.ShinePen);
                        }
                    }
                    rport.DrawLine(left, bottom, right, top, drawInfo.ShadowPen);
                    rport.DrawLine(right, top, right, bottom, drawInfo.ShadowPen);
                    rport.DrawLine(right, bottom, left, bottom, drawInfo.ShadowPen);
                    break;
                default:
                    if (WithBorder)
                    {
                        RenderImageFrame(rport, SysImageType, left, top, width, height, state, drawInfo);
                        left++;
                        top++;
                        right--;
                        bottom--;
                        width -= 2;
                        height -= 2;
                    }
                    rport.FillRectangle(left, top, width, height, bg);
                    break;
            }
        }

        private void RenderImageFrame(IGraphics rport, SysImageType which, int left, int top, int width, int height, ImageState state, DrawInfo drawInfo)
        {
            bool selected = (state & ImageState.Selected) == ImageState.Selected;
            bool hover = (state & ImageState.Hover) == ImageState.Hover;
            Color shine = Color.White;
            Color shadow = Color.Black;
            Color fillColor = Color.Gray;
            Color backColor = Color.Black;
            if (drawInfo != null)
            {
                shine = drawInfo.ShinePen;
                shadow = drawInfo.ShadowPen;
                if (hover)
                {
                    shine = drawInfo.HoverShinePen;
                    shadow = drawInfo.HoverShadowPen;
                }
                fillColor = drawInfo.FillPen;
                backColor = drawInfo.BackgoundPen;
            }
            if (selected)
            {
                Color temp = shine;
                shine = shadow;
                shadow = temp;
            }
            int right = left + width - 1;
            int bottom = top + height - 1;
            bool leftedgegodown = false;
            bool topedgegoright = false;
            switch (which)
            {
                //case SysImageType.Close:
                //    rport.FillRectangle(right, top, 1, height - 2, drawInfo.ShinePen);
                //    rport.FillRectangle(right, bottom, 1, 1, drawInfo.ShadowPen);
                //    right--;
                //    break;
                //case SysImageType.Zoom:
                //case SysImageType.Depth:
                //case SysImageType.SDepth:
                //    rport.FillRectangle(left, top, 1, 1, drawInfo.ShinePen);
                //    rport.FillRectangle(left, top + 1, 1, height - 1, drawInfo.ShadowPen);
                //    left++;
                //    break;
                case SysImageType.Up:
                case SysImageType.Down:
                    leftedgegodown = true;
                    break;
                case SysImageType.Left:
                case SysImageType.Right:
                    topedgegoright = true;
                    break;
            }

            if (left == 0) leftedgegodown = true;
            if (top == 0) topedgegoright = true;
            rport.RectFill(left, top, left, bottom - (leftedgegodown ? 0 : 1), shine);
            rport.RectFill(left + 1, top, right - (topedgegoright ? 0 : 1), top, shine);
            rport.RectFill(right, top + (topedgegoright ? 1 : 0), right, bottom, shadow);
            rport.RectFill(left + (leftedgegodown ? 1 : 0), bottom, right - 1, bottom, shadow);
        }

        public SysImageType SysImageType { get; set; }
        public SysImageFlags SysImageFlags { get; set; }
        private DrawInfo DrawInfo { get; set; }
        private bool WithBorder { get { return (SysImageFlags & SysImageFlags.NoBorder) != SysImageFlags.NoBorder; } }

    }
}
