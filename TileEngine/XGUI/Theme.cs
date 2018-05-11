using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.XGUI
{

    public interface ITheme
    {
        Color ShinePen { get; set; }
        Color ShadowPen { get; set; }
        Color BackPen { get; set; }
        Color HoverBackPen { get; set; }
        Color DarkBackPen { get; set; }
        Color TextPen { get; set; }
        Color WindowBorderPen { get; set; }
        Color HoverWindowBorderPen { get; set; }
        Color InactiveWindowBorderPen { get; set; }
        Color InactiveHoverWindowBorderPen { get; set; }
        Icons MessageInformationIcon { get; set; }
        Icons MessageQuestionIcon { get; set; }
        Icons MessageWarningIcon { get; set; }
        Icons MessageAltButtonIcon { get; set; }
        Icons MessagePrimaryButtonIcon { get; set; }
        Icons PopupChevronRightIcon { get; set; }
        Icons PopupChevronLeftIcon { get; set; }


        void RenderWindow(IGraphics gfx, Window window);
        void RenderButton(IGraphics gfx, Gadget button);
        void RenderLabel(IGraphics gfx, Label label);
        void RenderProp(IGraphics gfx, PropGadget prop);
        void RenderScroller(IGraphics gfx, Scroller scroller);
        void RenderCheckBox(IGraphics gfx, CheckBox checkbox);
        void RenderStr(IGraphics gfx, StrGadget str);
    }


    public class BasicTheme : ITheme
    {
        public BasicTheme()
        {
            ShinePen = Color.LightGray;
            ShadowPen = Color.DarkGray;
            BackPen = new Color(100, 100, 100, 230);
            HoverBackPen = new Color(120, 120, 120, 230);
            DarkBackPen = new Color(55, 55, 55, 230);
            TextPen = new Color(238, 238, 238);
            InactiveWindowBorderPen = new Color(130, 130, 130);
            InactiveHoverWindowBorderPen = new Color(130, 130, 140);
            WindowBorderPen = new Color(103, 136, 187);
            HoverWindowBorderPen = new Color(103, 136, 197);

            MessageAltButtonIcon = Icons.ENTYPO_ICON_CIRCLE_WITH_CROSS;
            MessagePrimaryButtonIcon = Icons.ENTYPO_ICON_CHECK;
            MessageInformationIcon = Icons.ENTYPO_ICON_INFO_WITH_CIRCLE;
            MessageQuestionIcon = Icons.ENTYPO_ICON_HELP_WITH_CIRCLE;
            MessageWarningIcon = Icons.ENTYPO_ICON_WARNING;
            PopupChevronRightIcon = Icons.ENTYPO_ICON_CHEVRON_RIGHT;
            PopupChevronLeftIcon = Icons.ENTYPO_ICON_CHEVRON_LEFT;

        }


        public Color ShinePen { get; set; }
        public Color ShadowPen { get; set; }
        public Color BackPen { get; set; }
        public Color HoverBackPen { get; set; }
        public Color DarkBackPen { get; set; }
        public Color TextPen { get; set; }
        public Color WindowBorderPen { get; set; }
        public Color HoverWindowBorderPen { get; set; }
        public Color InactiveWindowBorderPen { get; set; }
        public Color InactiveHoverWindowBorderPen { get; set; }

        public Icons MessageInformationIcon { get; set; }
        public Icons MessageQuestionIcon { get; set; }
        public Icons MessageWarningIcon { get; set; }

        public Icons MessageAltButtonIcon { get; set; }
        public Icons MessagePrimaryButtonIcon { get; set; }

        public Icons PopupChevronRightIcon { get; set; }
        public Icons PopupChevronLeftIcon { get; set; }


        public virtual void RenderWindow(IGraphics gfx, Window window)
        {
            if (!window.Borderless)
            {
                Color fc = window.Active ? window.MouseHover ? HoverWindowBorderPen : WindowBorderPen : window.MouseHover ? InactiveHoverWindowBorderPen : InactiveWindowBorderPen;
                RenderBox(gfx, window.Bounds, ShinePen, ShadowPen);
                RenderBox(gfx, window.ClientBounds.Inflated(1, 1), ShadowPen, ShinePen);
                if (window.BorderLeft > 2) gfx.FillRectangle(window.Left + 1, window.Top + 1, window.BorderLeft - 2, window.Height - 2, fc);
                if (window.BorderRight > 2) gfx.FillRectangle(window.Right - window.BorderRight + 2, window.Top + 1, window.BorderRight - 2, window.Height - 2, fc);
                if (window.BorderBottom > 2) gfx.FillRectangle(window.Left + 1, window.Bottom - window.BorderBottom + 2, window.Width - 2, window.BorderBottom - 2, fc);
                if (window.BorderTop > 2) gfx.FillRectangle(window.Left + 1, window.Top + 1, window.Width - 2, window.BorderTop - 2, fc);
                if (window.BorderTop > 0)
                {
                    gfx.RenderText(window.Font, window.Title, window.CenterX, window.Top + window.BorderTop / 2, TextPen);
                }
            }
            ClearBox(gfx, window.ClientBounds, BackPen);
        }



        public virtual void RenderButton(IGraphics gfx, Gadget button)
        {
            Color bgColor = BackPen;
            Color shine = ShinePen;
            Color shadow = ShadowPen;
            Color textColor = TextPen;
            if (button.MouseHover)
            {
                bgColor = HoverBackPen;
            }
            if (button.Selected)
            {
                shine = ShadowPen;
                shadow = ShinePen;
                bgColor = DarkBackPen;
            }
            if (button.Checked)
            {
                Color temp = shine;
                shine = shadow;
                shadow = temp;
                bgColor = DarkBackPen;
            }
            if (!button.TransparentBackground)
            {
                if (!button.BackgroundColor.IsEmpty)
                {
                    ClearBox(gfx, button.ClientBounds, button.BackgroundColor);
                }
                ClearBox(gfx, button.ClientBounds, bgColor);
            }
            if (button is ToolButton)
            {
                RenderToolButtonBox(gfx, button.Bounds, shine, shadow);
            }
            else
            {
                RenderButtonBox(gfx, button.Bounds, shine, shadow);
            }
            int ix = 0;
            int iy = 0;
            if (button.Icon != Icons.NONE && button.IconPlacement != IconPlacement.None)
            {
                switch (button.IconPlacement)
                {
                    case IconPlacement.Left:
                        ix = button.Left + 25 / 2 - 7;
                        iy = button.CenterY - 8;
                        break;
                    case IconPlacement.Right:
                        ix = button.Right - 25 / 2 - 9;
                        iy = button.CenterY - 8;
                        break;
                    case IconPlacement.Top:
                        ix = button.CenterX - 8;
                        iy = button.Top + 20 / 2 - 8;
                        break;
                    case IconPlacement.Bottom:
                        ix = button.CenterX - 8;
                        iy = button.Bottom - 20 / 2 - 8;
                        break;
                    case IconPlacement.Center:
                        ix = button.CenterX - 8;
                        iy = button.CenterY - 8;
                        break;
                }
                gfx.RenderIcon((int)button.Icon, ix, iy);
            }
            if (!string.IsNullOrEmpty(button.Text))
            {
                gfx.RenderText(button.Font, button.Text, button.CenterX, button.CenterY, textColor);
            }
            if (!button.Enabled) RenderDisabledBox(gfx, button.Bounds);
        }

        public virtual void RenderLabel(IGraphics gfx, Label label)
        {
            int ix = 0;
            int iy = 0;
            int iw = 0;
            if (label.Icon != Icons.NONE)
            {
                iw = 25;
                ix = label.Left + 25 / 2 - 7;
                iy = label.CenterY - 8;
                gfx.RenderIcon((int)label.Icon, ix, iy);
            }
            if (!string.IsNullOrEmpty(label.Text))
            {
                var lines = label.Text.Split('\n');
                int numLines = lines.Length;
                int ty = label.CenterY;
                ty -= (16 * numLines / 2);
                for (int i = 0; i < numLines; i++)
                {
                    gfx.RenderText(label.Font, lines[i], label.CenterX + iw, ty, TextPen);
                    ty += 16;
                }
                //gfx.RenderText(label.Font, label.Text, label.CenterX + iw, label.CenterY, TextPen);
            }
        }

        public virtual void RenderProp(IGraphics gfx, PropGadget prop)
        {
            Rect rect = prop.Bounds;
            if (!prop.Flags.HasFlag(PropFlags.Borderless))
            {
                RenderBox(gfx, rect, ShinePen, ShadowPen);
                rect.Inflate(-1, -1);
                RenderBox(gfx, rect, ShadowPen, ShinePen);
                rect.Inflate(-1, -1);
            }
            ClearBox(gfx, rect, DarkBackPen);
            Color shine = ShinePen;
            Color shadow = ShadowPen;
            Color bg = BackPen;
            if (prop.Selected && prop.Flags.HasFlag(PropFlags.KnobHit))
            {
                shine = ShadowPen;
                shadow = ShinePen;
            }
            if (prop.MouseHover)
            {
                bg = HoverBackPen;
            }
            RenderKnob(gfx, prop.Knob, shine, shadow, bg);
        }

        public virtual void RenderStr(IGraphics gfx, StrGadget str)
        {
            Rect rect = str.Bounds;
            RenderBox(gfx, rect, ShinePen, ShadowPen);
            rect.Inflate(-1, -1);
            RenderBox(gfx, rect, ShadowPen, ShinePen);
            rect.Inflate(-1, -1);
            ClearBox(gfx, rect, DarkBackPen);
            int tx = rect.Left + 1;
            int ty = str.CenterY;
            gfx.RenderText(str.TopazFont, str.Buffer, tx, ty, TextPen, HorizontalTextAlign.Left, VerticalTextAlign.Center);
        }

        public virtual void RenderScroller(IGraphics gfx, Scroller scroller)
        {
            Rect rect = scroller.Bounds;
            RenderBox(gfx, rect, ShinePen, ShadowPen);
            rect.Inflate(-1, -1);
            RenderBox(gfx, rect, ShadowPen, ShinePen);
            rect.Inflate(-1, -1);
        }

        public virtual void RenderCheckBox(IGraphics gfx, CheckBox checkbox)
        {
            Rect rect = checkbox.Bounds;
            rect.Width = 20;
            ClearBox(gfx, rect.Inflated(-1, -1), DarkBackPen);
            Color shine = ShadowPen;
            Color shadow = ShinePen;
            if (checkbox.Selected)
            {
                shine = ShinePen;
                shadow = ShadowPen;
            }
            RenderBox(gfx, rect, shine, shadow);
            if (checkbox.Checked)
            {
                gfx.RenderIcon((int)Icons.ENTYPO_ICON_CHECK, rect.Left + 2, rect.Top + 1);
            }
            gfx.RenderText(checkbox.Font, checkbox.Text, checkbox.Left + 25, checkbox.CenterY, TextPen, HorizontalTextAlign.Left);
        }


        public static void RenderKnob(IGraphics gfx, Rect knob, Color shinePen, Color shadowPen, Color bgPen)
        {
            RenderBox(gfx, knob, shinePen, shadowPen);
            ClearBox(gfx, knob.Inflated(-1, -1), bgPen);
        }

        public static void ClearBox(IGraphics gfx, Rect rect, Color color)
        {
            gfx.FillRectangle(rect.Left, rect.Top, rect.Width, rect.Height, color);
        }

        public static void RenderBox(IGraphics gfx, Rect rect, Color shinePen, Color shadowPen)
        {
            gfx.DrawLine(rect.Left, rect.Top, rect.Right - 1, rect.Top, shinePen);
            gfx.DrawLine(rect.Left, rect.Top, rect.Left, rect.Bottom - 1, shinePen);
            gfx.DrawLine(rect.Left, rect.Bottom, rect.Right, rect.Bottom, shadowPen);
            gfx.DrawLine(rect.Right, rect.Top, rect.Right, rect.Bottom, shadowPen);
        }
        public static void RenderToolButtonBox(IGraphics gfx, Rect rect, Color shinePen, Color shadowPen)
        {
            //gfx.DrawRectangle(rect.Left, rect.Top, rect.Width, rect.Height, Color.Black);
            RenderBox(gfx, rect, shinePen, shadowPen);
        }

        public static void RenderButtonBox(IGraphics gfx, Rect rect, Color shinePen, Color shadowPen)
        {
            gfx.DrawRectangle(rect.Left, rect.Top, rect.Width, rect.Height, Color.Black);
            RenderBox(gfx, rect.Inflated(-1, -1), shinePen, shadowPen);
        }

        public static void RenderDisabledBox(IGraphics gfx, Rect rect)
        {
            gfx.FillRectangle(rect.Left, rect.Top, rect.Width, rect.Height, new Color(128, 128, 128, 128));
        }


    }

    public class DefaultTheme : BasicTheme
    {

    }
}
