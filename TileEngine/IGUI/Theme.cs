using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Fonts;
using TileEngine.Graphics;

namespace TileEngine.IGUI
{
    public interface ITheme
    {
        Color ShinePen { get; set; }
        Color ShadowPen { get; set; }
        Color ButtonBackPen { get; set; }
        Color WindowBackPen { get; set; }
        Color DarkWindowBackPen { get; set; }
        Color WindowBorderPen { get; set; }
        Color InactiveWindowBorderPen { get; set; }
        Color TextPen { get; set; }
        Color TitleTextPen { get; set; }
        Color StrPen { get; set; }
        Color InactiveStrPen { get; set; }

        void RenderWindow(IGraphics gfx, Window window);
        void RenderGadget(IGraphics gfx, Gadget gadget);
        void RenderFrame(IGraphics gfx, Gadget gadget);

    }
    public class BaseTheme : ITheme
    {
        public BaseTheme()
        {
            ButtonBackPen = new Color(207, 207, 207);
            //WindowBackPen = new Color(231, 231, 231);
            WindowBackPen = new Color(128, 128, 128);
            DarkWindowBackPen = new Color(77, 77, 77);
            WindowBorderPen = new Color(97, 142, 206);
            InactiveWindowBorderPen = new Color(200, 200, 200);
            //ShinePen = new Color(176, 176, 176);
            //ShadowPen = new Color(127, 127, 127);
            ShinePen = new Color(255, 255, 255, 128);
            ShadowPen = new Color(0, 0, 0, 128);
            TextPen = Color.Black;
            TitleTextPen = Color.White;
            StrPen = new Color(33, 33, 33);
            InactiveStrPen = new Color(33, 33, 33, 100);
        }
        public Color ShinePen { get; set; }
        public Color ShadowPen { get; set; }
        public Color ButtonBackPen { get; set; }
        public Color WindowBackPen { get; set; }
        public Color DarkWindowBackPen { get; set; }
        public Color WindowBorderPen { get; set; }
        public Color InactiveWindowBorderPen { get; set; }
        public Color TextPen { get; set; }
        public Color TitleTextPen { get; set; }
        public Color StrPen { get; set; }
        public Color InactiveStrPen { get; set; }

        public virtual void RenderWindow(IGraphics gfx, Window window)
        {
            Color shine = ShinePen;
            Color shadow = DarkWindowBackPen;
            Color bg = WindowBackPen;
            Color fg = window.Active ? WindowBorderPen : InactiveWindowBorderPen;
            Rect bounds = window.Bounds;
            Rect gzz = window.GZZBounds;
            bounds.Offset(-window.LeftEdge, -window.TopEdge);
            gzz.Offset(-window.LeftEdge, -window.TopEdge);
            gzz.Inflate(1, 1);
            FillBox(gfx, bounds, bg);
            if (!window.Flags.HasFlag(WindowFlags.Borderless))
            {
                RenderBox(gfx, bounds, shine, shadow);
                RenderBox(gfx, gzz, shadow, shine);
                if (window.BorderLeft > 2) gfx.FillRectangle(bounds.Left + 1, bounds.Top + 1, window.BorderLeft - 2, bounds.Height - 2, fg);
                if (window.BorderRight > 2) gfx.FillRectangle(bounds.Right - window.BorderRight + 2, bounds.Top + 1, window.BorderRight - 2, bounds.Height - 2, fg);
                if (window.BorderBottom > 2) gfx.FillRectangle(bounds.Left + 1, bounds.Bottom - window.BorderBottom + 2, bounds.Width - 2, window.BorderBottom - 2, fg);
                if (window.BorderTop > 2) gfx.FillRectangle(bounds.Left + 1, bounds.Top + 1, bounds.Width - 2, window.BorderTop - 2, fg);
                if (!string.IsNullOrEmpty(window.Title))
                {
                    int x = window.BorderLeft + 1;
                    x += window.Flags.HasFlag(WindowFlags.CloseGadget) ? 20 : 0;
                    int y = window.BorderTop / 2;
                    gfx.RenderText(window.Font, window.Title, x, y, TitleTextPen, HorizontalTextAlign.Left);
                }
            }
        }

        public virtual void RenderGadget(IGraphics gfx, Gadget gadget)
        {
            if (gadget.Hidden) return;
            switch (gadget.SysGType)
            {
                case GadgetType.Close:
                case GadgetType.Sizing:
                case GadgetType.WDragging:
                case GadgetType.WDepth:
                case GadgetType.WZoom:
                    RenderSysGadget(gfx, gadget, gadget.Window);
                    break;
                case GadgetType.None:
                default:
                    switch (gadget.GType)
                    {
                        case GadgetType.BoolGadget:
                            RenderBoolGadget(gfx, gadget);
                            break;
                        case GadgetType.StrGadget:
                            RenderStrGadget(gfx, gadget as StrGadget);
                            break;
                        case GadgetType.PropGadget:
                            RenderPropGadget(gfx, gadget as PropGadget);
                            break;
                        case GadgetType.CustomGadget:
                            RenderCustomGadget(gfx, gadget);
                            break;
                    }
                    break;
            }
            RenderDisable(gfx, gadget);
        }

        public void RenderFrame(IGraphics gfx, Gadget gadget)
        {
            var frame = gadget.Bounds;
            RenderBox(gfx, frame, ShinePen, ShadowPen);
            RenderBox(gfx, frame.Inflated(-1, -1), ShadowPen, ShinePen);
            frame.Offset(-1, 0);
            RenderText(gfx, frame, gadget.Font, gadget.Text, ShadowPen, WindowBackPen, gadget.TextPlace);
            frame.Offset(1, 0);
            RenderText(gfx, frame, gadget.Font, gadget.Text, TitleTextPen, gadget.TextPlace);
        }

        private void RenderBoolGadget(IGraphics gfx, Gadget gadget)
        {
            var frame = gadget.Bounds;
            var state = gadget.State;
            FillBox(gfx, frame, ButtonBackPen);
            if (state.HasFlag(ImageState.Selected))
            {
                RenderUpperShadow(gfx, frame, ShadowPen);
                RenderLowerShadow(gfx, frame, ShinePen);
            }
            else
            {
                RenderLowerShadow(gfx, frame, ShadowPen);
                RenderUpperShadow(gfx, frame, ShinePen);
            }
            RenderGadgetText(gfx, gadget);
        }

        private void RenderStrGadget(IGraphics gfx, StrGadget gadget)
        {
            var frame = gadget.Bounds;
            var state = gadget.State;
            bool active = false;
            bool empty = string.IsNullOrEmpty(gadget.Buffer);
            if (state.HasFlag(ImageState.Selected) || state.HasFlag(ImageState.Active))
            {
                active = true;
                gfx.DrawLine(frame.Left, frame.Bottom - 1, frame.Right, frame.Bottom - 1, StrPen);
                gfx.DrawLine(frame.Left, frame.Bottom, frame.Right, frame.Bottom, StrPen);
            }
            else
            {
                gfx.DrawLine(frame.Left, frame.Bottom - 1, frame.Right, frame.Bottom - 1, InactiveStrPen);
            }
            int x = frame.Left;
            int y = frame.Bottom - 2;
            if (empty)
            {
                if (active)
                {
                    if (!string.IsNullOrEmpty(gadget.Placeholder))
                    {
                        gfx.RenderText(gadget.Font, gadget.Placeholder, x, y, InactiveStrPen, HorizontalTextAlign.Left, VerticalTextAlign.Bottom);
                    }
                    y = frame.Top;
                    gfx.RenderText(gadget.Font, gadget.Text, x, y, StrPen, HorizontalTextAlign.Left, VerticalTextAlign.Top);
                }
                else
                {
                    gfx.RenderText(gadget.Font, gadget.Text, x, y, InactiveStrPen, HorizontalTextAlign.Left, VerticalTextAlign.Bottom);
                }
            }
            else
            {
                gfx.RenderText(gadget.Font, gadget.Buffer, x, y, TextPen, HorizontalTextAlign.Left, VerticalTextAlign.Bottom);
                y = frame.Top;
                gfx.RenderText(gadget.Font, gadget.Text, x, y, StrPen, HorizontalTextAlign.Left, VerticalTextAlign.Top);
            }
        }

        private void RenderPropGadget(IGraphics gfx, PropGadget gadget)
        {
            var frame = gadget.Bounds;
            var state = gadget.State;
            FillBox(gfx, frame, ButtonBackPen);
            if (!gadget.PropFlags.HasFlag(PropFlags.Borderless))
            {
                gfx.DrawRectangle(frame.X, frame.Y, frame.Width, frame.Height, DarkWindowBackPen);
                frame.Inflate(-1, -1);
            }
            var knob = gadget.Knob;
            if (gadget.PropFlags.HasFlag(PropFlags.AutoKnob))
            {
                if (gadget.PropFlags.HasFlag(PropFlags.KnobHit))
                {
                    RenderBox(gfx, knob, ShadowPen, ShinePen);
                }
                else
                {
                    RenderBox(gfx, knob, ShinePen, ShadowPen);
                }
            }
        }

        private void RenderCustomGadget(IGraphics gfx, Gadget gadget)
        {
            if (gadget is CheckBoxGadget cbg) { RenderCheckBoxGadget(gfx, cbg); }
            else if (gadget is ToolButton tb) { RenderToolButton(gfx, tb); }
            else if (gadget is ChooserGadget cg) { RenderChooserGadget(gfx, cg); }
            else if (gadget is PopupMenuItem pmi) { RenderPopupMenuItem(gfx, pmi); }
            else if (gadget is LabelGadget lg) { RenderLabelGadget(gfx, lg); }
            else if (gadget is ScrollerGadget sg) { RenderScrollerGadget(gfx, sg); }
            else if (gadget is TabHeaderGadget thg) { RenderTabHeaderGadget(gfx, thg); }
            else if (gadget is ClickTabGadget ctg) { RenderClickTabGadget(gfx, ctg); }
            else if (gadget is ListBrowserGadget lbg) { RenderListBrowserGadget(gfx, lbg); }
        }

        private void RenderListBrowserGadget(IGraphics gfx, ListBrowserGadget gadget)
        {
            var frame = gadget.Bounds;
            frame.Width -= 20;
            gfx.DrawRectangle(frame.X, frame.Y, frame.Width, frame.Height, DarkWindowBackPen);
            frame.Inflate(-1, -1);
            int x = frame.X + 1;
            int y = frame.Y + 1;
            gfx.SaveState();
            gfx.SetClip(frame.X, frame.Y, frame.Width, frame.Height);
            int index = 0;
            while (index < gadget.Labels.Count && y < frame.Bottom)
            {
                string lab = gadget.Labels[index];
                gfx.RenderText(gadget.Font, lab, x, y, TextPen, HorizontalTextAlign.Left, VerticalTextAlign.Top);
                y += 20;
                index++;
            }
            gfx.RestoreState();
        }

        private void RenderClickTabGadget(IGraphics gfx, ClickTabGadget gadget)
        {
            var frame = gadget.Bounds;
            frame.Y += 24;
            frame.Height -= 24;
            gfx.DrawLine(frame.Left, frame.Top, frame.Left, frame.Bottom, DarkWindowBackPen);
            gfx.DrawLine(frame.Left + 1, frame.Bottom, frame.Right - 1, frame.Bottom, DarkWindowBackPen);
            gfx.DrawLine(frame.Right, frame.Top, frame.Right, frame.Bottom, DarkWindowBackPen);
            foreach (var mem in gadget.Members)
            {
                if (mem is TabHeaderGadget gad)
                {
                    var bb = gad.Bounds;
                    if (!gad.TogSelect)
                    {
                        gfx.DrawLine(bb.Left - 1, bb.Bottom + 1, bb.Right + 1, bb.Bottom + 1, DarkWindowBackPen);
                    }
                    else
                    {
                        gfx.DrawLine(bb.Left - 1, bb.Bottom + 1, bb.Left, bb.Bottom + 1, DarkWindowBackPen);
                        gfx.DrawLine(bb.Right, bb.Bottom + 1, bb.Right + 1, bb.Bottom + 1, DarkWindowBackPen);
                        gfx.DrawLine(bb.Left + 1, bb.Bottom + 1, bb.Right - 1, bb.Bottom + 1, WindowBorderPen);
                    }
                }
            }
            var tf = frame;
            gfx.FillRectangle(tf.Left + 1, tf.Top + 1, tf.Width - 2, 3, WindowBorderPen);
            gfx.DrawLine(frame.Left + 1, frame.Top + 4, frame.Right - 1, frame.Top + 4, DarkWindowBackPen);
        }

        private void RenderTabHeaderGadget(IGraphics gfx, TabHeaderGadget gadget)
        {
            var frame = gadget.Bounds;
            var state = gadget.State;
            var back = ButtonBackPen;
            if (gadget.TogSelect) { back = WindowBorderPen; }
            FillBox(gfx, frame, back);
            gfx.DrawLine(frame.Left, frame.Bottom, frame.Left, frame.Top, DarkWindowBackPen);
            gfx.DrawLine(frame.Left + 1, frame.Top, frame.Right - 1, frame.Top, DarkWindowBackPen);
            gfx.DrawLine(frame.Right, frame.Top, frame.Right, frame.Bottom, DarkWindowBackPen);
            frame.Y += 1;
            frame.X += 1;
            frame.Width -= 2;
            frame.Height -= 1;
            //frame.Inflate(-1, -1);
            if (gadget.Selected)
            {
                RenderHalfBox(gfx, frame, ShadowPen, ShinePen);
            }
            else
            {
                RenderHalfBox(gfx, frame, ShinePen, ShadowPen);
            }
            RenderGadgetText(gfx, gadget);
        }

        private void RenderScrollerGadget(IGraphics gfx, ScrollerGadget gadget)
        {
            var frame = gadget.Bounds;
            var state = gadget.State;
            gfx.DrawRectangle(frame.X, frame.Y, frame.Width, frame.Height, DarkWindowBackPen);
        }

        private void RenderLabelGadget(IGraphics gfx, LabelGadget gadget)
        {
            var frame = gadget.Bounds;
            var state = gadget.State;
            string text = gadget.Text;
            Icons icon = gadget.Icon;
            RenderText(gfx, frame, gadget.Font, text, TextPen);
        }

        private void RenderPopupMenuItem(IGraphics gfx, PopupMenuItem gadget)
        {
            var frame = gadget.Bounds;
            var state = gadget.State;
            if (state.HasFlag(ImageState.Selected))
            {
                FillBox(gfx, frame, ButtonBackPen);
            }
            string text = gadget.Text;
            RenderText(gfx, frame, gadget.Font, text, TextPen);
        }

        private void RenderChooserGadget(IGraphics gfx, ChooserGadget gadget)
        {
            var frame = gadget.Bounds;
            var state = gadget.State;
            int w = frame.Width;
            int h = frame.Height;
            int x = frame.X + w / 2;
            int y = frame.Y + h / 2;
            Rect arFrame = frame;
            arFrame = new Rect(frame.Right - h, frame.Top, h, h);
            FillBox(gfx, frame, ButtonBackPen);
            if (state.HasFlag(ImageState.Selected))
            {
                RenderBox(gfx, frame, ShadowPen, ShinePen);
            }
            else
            {
                RenderBox(gfx, frame, ShinePen, ShadowPen);
            }
            frame.Width -= h;
            string text = gadget.Text;
            if (gadget.PopUp)
            {
                text = gadget.SelectedItem.ToString();
            }
            RenderText(gfx, frame, gadget.Font, text, TextPen);
            gfx.RenderIcon((int)Icons.ENTYPO_ICON_CHEVRON_SMALL_DOWN, arFrame.Left + arFrame.Width / 2, arFrame.Top + arFrame.Height / 2, TextPen);
        }

        private void RenderToolButton(IGraphics gfx, ToolButton gadget)
        {
            var frame = gadget.Bounds;
            var state = gadget.State;
            int w = frame.Width;
            int h = frame.Height;
            int x = frame.X + w / 2;
            int y = frame.Y + h / 2;
            FillBox(gfx, frame, ButtonBackPen);
            if (state.HasFlag(ImageState.Selected))
            {
                RenderBox(gfx, frame, ShadowPen, ShinePen);
            }
            else
            {
                RenderBox(gfx, frame, ShinePen, ShadowPen);
            }
            gfx.RenderIcon((int)gadget.Icon, x, y, TextPen);
        }

        private void RenderCheckBoxGadget(IGraphics gfx, CheckBoxGadget gadget)
        {
            var frame = gadget.Bounds;
            var state = gadget.State;
            int h = frame.Height;
            int x = frame.X;
            int y = frame.Y + h / 2;
            Rect cbFrame = frame;
            if (gadget.TextPlace == TextPlace.LeftCenter)
            {
                cbFrame = new Rect(frame.Right - h, frame.Top, h, h);
                x = frame.Right - h / 2;


                gfx.RenderText(gadget.Font, gadget.Text, frame.Right - h - 8, y, TextPen, HorizontalTextAlign.Right, VerticalTextAlign.Center);
            }
            else // right center
            {
                cbFrame.Width = h;
                x = frame.X + h / 2;
                gfx.RenderText(gadget.Font, gadget.Text, frame.Left + h + 8, y, TextPen, HorizontalTextAlign.Left, VerticalTextAlign.Center);
            }
            gfx.DrawRectangle(cbFrame.Left, cbFrame.Top, cbFrame.Width, cbFrame.Height, DarkWindowBackPen);
            if (gadget.Selected)
            {
                RenderBox(gfx, cbFrame.Inflated(-1, -1), ShinePen, ShadowPen);
            }
            else
            {
                RenderBox(gfx, cbFrame.Inflated(-1, -1), ShadowPen, ShinePen);
            }
            if (gadget.TogSelect)
            {
                gfx.RenderIcon((int)Icons.ENTYPO_ICON_CHECK, x, y, TextPen);
            }
        }

        private void RenderDisable(IGraphics gfx, Gadget gadget)
        {
            if (gadget.Disabled)
            {
                FillBox(gfx, gadget.Bounds, new Color(255, 255, 255, 128));
            }
        }

        private void RenderSysGadget(IGraphics gfx, Gadget gadget, Window window)
        {
            var frame = gadget.Bounds;
            Color white = window.Active ? new Color(235, 235, 235) : InactiveWindowBorderPen;
            Color black = window.Active ? Color.Black : DarkWindowBackPen;
            Color grey = Color.Gray;
            if (gadget.MouseHover)
            {
                black = HoverColor(black);
                white = HoverColor(white);
                grey = HoverColor(grey);
            }
            switch (gadget.SysGType)
            {
                case GadgetType.Close:
                    RenderSBarVert(gfx, frame.Right - 1, frame.Y + 4, frame.Height - 8);
                    if (gadget.Selected) frame.Offset(1, 1);
                    RenderCloseGadget(gfx, frame, black, white);
                    break;
                case GadgetType.WDepth:
                    //RenderSBarVert(gfx, frame.Left, frame.Y + 4, frame.Height - 8);
                    if (gadget.Selected) frame.Offset(1, 1);
                    RenderDepthGadget(gfx, frame, black, white, grey);
                    break;
                case GadgetType.WZoom:
                    //RenderSBarVert(gfx, frame.Left, frame.Y + 4, frame.Height - 8);
                    if (gadget.Selected) frame.Offset(1, 1);
                    RenderZoomGadget(gfx, frame, black, white, grey);
                    break;
                case GadgetType.Sizing:
                    if (window.Flags.HasFlag(WindowFlags.SizeBBottom))
                    {
                        //RenderSBarVert(gfx, frame.Left, frame.Y + 4, frame.Height - 8);
                    }
                    if (window.Flags.HasFlag(WindowFlags.SizeBRight))
                    {
                        //RenderSBarHoriz(gfx, frame.Left + 4, frame.Top, frame.Width - 8);
                    }
                    if (gadget.Selected) frame.Offset(1, 1);
                    RenderSizeGadget(gfx, frame, black, white);
                    break;
            }
        }

        private void RenderCloseGadget(IGraphics gfx, Rect rect, Color bg, Color fg)
        {
            int hspacing = rect.Width * 4 / 10;
            int vspacing = rect.Height * 3 / 10;
            rect.Inflate(-hspacing, -vspacing);
            gfx.DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, bg);
            rect.Inflate(-1, -1);
            gfx.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height, fg);
        }

        private void RenderSizeGadget(IGraphics gfx, Rect rect, Color bg, Color fg)
        {
            int hspacing = rect.Width / 5;
            int vspacing = rect.Height / 5;
            int left = rect.Left;
            int top = rect.Top;
            int right = rect.Right;
            int bottom = rect.Bottom;
            int width = rect.Width;
            int heigh = rect.Height;
            left += hspacing;
            top += vspacing;
            right = left + width - 1 - (hspacing * 2);
            bottom = top + heigh - 1 - (vspacing * 2);
            width = right - left + 1;
            heigh = bottom - top + 1;
            for (int y = top; y <= bottom; y++)
            {
                int x = left + (bottom - y) * width / heigh;
                gfx.DrawLine(x, y, right, y, bg);
            }
            for (int y = top + 1; y <= bottom - 1; y++)
            {
                int x = left + (bottom - y) * width / heigh;
                if (x + 1 <= right - 1)
                    gfx.DrawLine(x + 1, y, right - 1, y, fg);
            }

        }
        private void RenderZoomGadget(IGraphics gfx, Rect rect, Color bg, Color fg, Color gr)
        {
            int hspacing = rect.Width / 6;
            int vspacing = rect.Height / 6;
            rect.Inflate(-hspacing, -vspacing);
            gfx.DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, bg);
            gfx.FillRectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2, fg);
            int left = rect.Left;
            int top = rect.Top;
            int right = rect.Right;
            int bottom = rect.Bottom;
            right = left + (right - left + 1) / 2;
            bottom = top + (bottom - top + 1) / 2;
            if (right - left < 4) right = left + 4;
            gfx.DrawRectangle(left, top, right - left, bottom - top, bg);
            gfx.FillRectangle(left + 1, top + 1, right - left - 2, bottom - top - 2, gr);
        }

        private void RenderDepthGadget(IGraphics gfx, Rect rect, Color bg, Color fg, Color gr)
        {
            int hspacing = rect.Width / 6;
            int vspacing = rect.Height / 6;
            int left = rect.Left;
            int top = rect.Top;
            int right = rect.Right;
            int bottom = rect.Bottom;
            int width = rect.Width;
            int heigh = rect.Height;
            left += hspacing;
            top += vspacing;
            width -= 2 * hspacing;
            heigh -= 2 * vspacing;
            gfx.DrawRectangle(left, top, width - (width / 3), heigh - (heigh / 3), bg);
            gfx.FillRectangle(left + 1, top + 1, width - (width / 3) - 2, heigh - (heigh / 3) - 2, fg);
            gfx.DrawRectangle(left + (width / 3), top + (heigh / 3), width - (width / 3), heigh - (heigh / 3), bg);
            gfx.FillRectangle(left + (width / 3) + 1, top + (heigh / 3) + 1, width - (width / 3) - 2, heigh - (heigh / 3) - 2, gr);
        }

        private void RenderSBarVert(IGraphics gfx, int x, int y, int height)
        {
            gfx.DrawLine(x, y + 1, x, y + height - 2, ShadowPen);
            gfx.DrawLine(x + 1, y, x + 1, y + height - 1, ShinePen);
        }

        private void RenderSBarHoriz(IGraphics gfx, int x, int y, int width)
        {
            gfx.DrawLine(x, y, x + width - 1, y, ShadowPen);
            gfx.DrawLine(x + 1, y + 1, x + width - 2, y + 1, ShinePen);
        }

        private void RenderGadgetText(IGraphics gfx, Gadget gadget)
        {
            RenderText(gfx, gadget.Bounds, gadget.Font, gadget.Text, TextPen, gadget.TextPlace);
        }

        public static void FillBox(IGraphics gfx, Rect rect, Color color)
        {
            gfx.FillRectangle(rect.Left, rect.Top, rect.Width, rect.Height, color);
        }

        public static void RenderLowerShadow(IGraphics gfx, Rect rect, Color color)
        {
            gfx.FillRectangle(rect.Left, rect.Bottom - 1, rect.Width - 1, 2, color);
            gfx.FillRectangle(rect.Right, rect.Top, 1, rect.Height, color);
        }

        public static void RenderUpperShadow(IGraphics gfx, Rect rect, Color color)
        {
            gfx.FillRectangle(rect.Left + 1, rect.Top, rect.Width - 1, 2, color);
            gfx.FillRectangle(rect.Left, rect.Top, 1, rect.Height, color);
        }

        public static void RenderBox(IGraphics gfx, Rect rect, Color shinePen, Color shadowPen)
        {
            gfx.DrawLine(rect.Left + 1, rect.Top, rect.Right - 1, rect.Top, shinePen);
            gfx.DrawLine(rect.Left, rect.Top, rect.Left, rect.Bottom - 1, shinePen);
            gfx.DrawLine(rect.Left, rect.Bottom, rect.Right - 1, rect.Bottom, shadowPen);
            gfx.DrawLine(rect.Right, rect.Top, rect.Right, rect.Bottom, shadowPen);
        }

        public static void RenderHalfBox(IGraphics gfx, Rect rect, Color shinePen, Color shadowPen)
        {
            gfx.DrawLine(rect.Left + 1, rect.Top, rect.Right - 1, rect.Top, shinePen);
            gfx.DrawLine(rect.Left, rect.Top, rect.Left, rect.Bottom - 1, shinePen);
            //gfx.DrawLine(rect.Left, rect.Bottom, rect.Right - 1, rect.Bottom, shadowPen);
            gfx.DrawLine(rect.Right, rect.Top, rect.Right, rect.Bottom, shadowPen);
        }

        public static void RenderText(IGraphics gfx, Rect rect, Font font, string text, Color color, TextPlace place = TextPlace.InCenter)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (font == null) return;
            int x = 0;
            int y = 0;
            VerticalTextAlign vta = VerticalTextAlign.Center;
            HorizontalTextAlign hta = HorizontalTextAlign.Center;
            switch (place)
            {
                case TextPlace.TopCenter:
                case TextPlace.InCenter:
                case TextPlace.BottomCenter:
                    x = rect.Width / 2;
                    break;
                case TextPlace.TopLeft:
                case TextPlace.InLeft:
                case TextPlace.BottomLeft:
                    x = 8;
                    hta = HorizontalTextAlign.Left;
                    break;
                case TextPlace.TopRight:
                case TextPlace.InRight:
                case TextPlace.BottomRight:
                    x = rect.Width - 8;
                    hta = HorizontalTextAlign.Right;
                    break;
                case TextPlace.LeftCenter:
                    x = 0 - 8;
                    hta = HorizontalTextAlign.Right;
                    break;
                case TextPlace.RightCenter:
                    x = rect.Width + 8;
                    hta = HorizontalTextAlign.Left;
                    break;
            }
            switch (place)
            {
                case TextPlace.TopLeft:
                case TextPlace.TopCenter:
                case TextPlace.TopRight:
                    y = 0;
                    vta = VerticalTextAlign.Center;
                    break;
                case TextPlace.InLeft:
                case TextPlace.InCenter:
                case TextPlace.InRight:
                case TextPlace.LeftCenter:
                case TextPlace.RightCenter:
                    y = rect.Height / 2;
                    break;
                case TextPlace.BottomLeft:
                case TextPlace.BottomCenter:
                case TextPlace.BottomRight:
                    y = rect.Height - 1;
                    vta = VerticalTextAlign.Center;
                    break;
            }
            gfx.RenderText(font, text, rect.Left + x, rect.Top + y, color, hta, vta);
        }

        public static void RenderText(IGraphics gfx, Rect rect, Font font, string text, Color color, Color bg, TextPlace place)
        {
            int x = 0;
            int y = 0;
            VerticalTextAlign vta = VerticalTextAlign.Center;
            HorizontalTextAlign hta = HorizontalTextAlign.Center;
            switch (place)
            {
                case TextPlace.TopCenter:
                case TextPlace.InCenter:
                case TextPlace.BottomCenter:
                    x = rect.Width / 2;
                    break;
                case TextPlace.TopLeft:
                case TextPlace.InLeft:
                case TextPlace.BottomLeft:
                    x = 8;
                    hta = HorizontalTextAlign.Left;
                    break;
                case TextPlace.TopRight:
                case TextPlace.InRight:
                case TextPlace.BottomRight:
                    x = rect.Width - 8;
                    hta = HorizontalTextAlign.Right;
                    break;
                case TextPlace.LeftCenter:
                    x = 0 - 4;
                    hta = HorizontalTextAlign.Right;
                    break;
                case TextPlace.RightCenter:
                    x = rect.Width + 8;
                    hta = HorizontalTextAlign.Left;
                    break;
            }
            switch (place)
            {
                case TextPlace.TopLeft:
                case TextPlace.TopCenter:
                case TextPlace.TopRight:
                    y = 0;
                    vta = VerticalTextAlign.Center;
                    break;
                case TextPlace.InLeft:
                case TextPlace.InCenter:
                case TextPlace.InRight:
                case TextPlace.LeftCenter:
                case TextPlace.RightCenter:
                    y = rect.Height / 2;
                    break;
                case TextPlace.BottomLeft:
                case TextPlace.BottomCenter:
                case TextPlace.BottomRight:
                    y = rect.Height - 1;
                    vta = VerticalTextAlign.Center;
                    break;
            }
            gfx.RenderText(font, " " + text + " ", rect.Left + x, rect.Top + y, color, bg, hta, vta);
        }


        public static Color HoverColor(Color c)
        {
            return new Color((byte)(c.R + 20), (byte)(c.G + 20), (byte)(c.B + 20), c.Alpha);
        }

        public static void Swap(ref Color c1, ref Color c2)
        {
            Color temp = c1;
            c1 = c2;
            c2 = temp;
        }

    }
}
