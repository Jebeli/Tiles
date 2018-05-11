using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.Nano
{
    [Flags]
    public enum ButtonFlags
    {
        NormalButton = (1 << 0), ///< A normal Button.
        RadioButton = (1 << 1), ///< A radio Button.
        ToggleButton = (1 << 2), ///< A toggle Button.
        PopupButton = (1 << 3)  ///< A popup Button.
    };

    public class Button : Widget
    {
        protected string caption;
        protected bool pushed;
        protected Color backgroundColor;
        protected Color textColor;
        protected ButtonFlags flags;
        protected List<Button> buttonGroup;
        protected Icons icon;
        public Button(Widget parent, string caption = "Untitled", Icons buttonIcon = Icons.NONE)
            : base(parent)
        {
            this.icon = buttonIcon;
            this.caption = caption;
            buttonGroup = new List<Button>();
            flags = ButtonFlags.NormalButton;
            backgroundColor = Color.Empty;
            textColor = Color.Empty;
        }

        public event EventHandler<EventArgs> Click;
        public event EventHandler<EventArgs> Changed;

        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public ButtonFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public bool Pushed
        {
            get { return pushed; }
            set { pushed = value; }
        }

        public Icons Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            int fontSize = theme.ButtonFontSize;
            int tw = gfx.MeasureTextWidth(Font, caption);
            int iw = icon != Icons.NONE ? 25 : 0;
            return new Vector2((tw + iw + 20), fontSize + 10);
        }

        public override void Draw(IGraphics gfx)
        {
            base.Draw(gfx);
            Color grad = theme.ButtonUnfocused;
            Color gradBot = theme.ButtonBotUnfocused;
            if (pushed)
            {
                grad = theme.ButtonPushed;
                gradBot = theme.ButtonBotPushed;
            }
            else if (mouseFocused && enabled)
            {
                grad = theme.ButtonFocused;
                gradBot = theme.ButtonBotFocused;
            }
            if (!backgroundColor.IsEmpty)
            {
                gfx.FillRectangle(pos.X + 1, pos.Y + 2, size.X - 2, size.Y - 4, backgroundColor);
                byte a = 204;
                if (!pushed)
                {
                    a = enabled ? (byte)200 : (byte)100;
                }
                grad.Alpha = a;
                gradBot.Alpha = a;
            }
            gfx.FillRectangle(pos.X + 1, pos.Y + 2, size.X - 2, size.Y - 4, grad, gradBot);
            if (!pushed)
            {
                gfx.DrawRoundedRectangle(pos.X, pos.Y + 1, size.X, size.Y - 1, theme.BorderLight);
                gfx.DrawRoundedRectangle(pos.X, pos.Y, size.X, size.Y - 1, theme.BorderDark);

            }
            else
            {
                gfx.DrawRoundedRectangle(pos.X, pos.Y + 1, size.X, size.Y - 1, theme.BorderLight);
                gfx.DrawRoundedRectangle(pos.X, pos.Y, size.X, size.Y - 1, theme.BorderDark);
                //gfx.DrawRoundedRectangle(pos.X + 1, pos.Y + 1, size.X - 2, size.Y - 2, theme.BorderLight);
                //gfx.DrawRoundedRectangle(pos.X, pos.Y, size.X, size.Y, theme.BorderDark);
            }

            Color tc = textColor.IsEmpty ? theme.TextColor : textColor;
            if (!enabled) tc = theme.DisabledTextColor;
            Vector2 center = pos + size * 0.5;
            if (icon > 0)
            {
                int ix = pos.X + Math.Min(size.X / 2, 25 / 2) - 8;
                int iy = pos.Y + size.Y / 2 - 9;
                gfx.RenderIcon((int)icon, ix, iy);
                gfx.RenderText(Font, caption, pos.X + 25, center.Y, tc, HorizontalTextAlign.Left);
            }
            else
            {
                gfx.RenderText(Font, caption, center.X, center.Y, tc);
            }
        }

        //public override bool FocusEvent(bool focused)
        //{
        //    base.FocusEvent(focused);
        //    if (!focused && pushed && !flags.HasFlag(ButtonFlags.ToggleButton))
        //    {
        //        pushed = false;
        //    }
        //    return false;
        //}

        public override bool MouseButtonEvent(Vector2 p, MouseButton button, bool down)
        {
            base.MouseButtonEvent(p, button, down);
            if ((button == MouseButton.Left) && enabled)
            {
                bool pushedBackup = pushed;
                if (down)
                {
                    if (flags.HasFlag(ButtonFlags.RadioButton))
                    {
                        if (buttonGroup.Count == 0)
                        {
                            foreach (var widget in parent.Children)
                            {
                                Button b = widget as Button;
                                if ((b != this) && (b != null) && b.Flags.HasFlag(ButtonFlags.RadioButton) && b.pushed)
                                {
                                    b.pushed = false;
                                    b.Changed?.Invoke(b, EventArgs.Empty);
                                }
                            }
                        }
                        else
                        {
                            foreach (var b in buttonGroup)
                            {
                                if (b != this && b.Flags.HasFlag(ButtonFlags.RadioButton) && b.pushed)
                                {
                                    b.pushed = false;
                                    b.Changed?.Invoke(b, EventArgs.Empty);
                                }
                            }
                        }
                    }
                    if (flags.HasFlag(ButtonFlags.PopupButton))
                    {
                        foreach (var widget in parent.Children)
                        {
                            Button b = widget as Button;
                            if ((b != this) && (b != null) && b.Flags.HasFlag(ButtonFlags.PopupButton) && b.pushed)
                            {
                                b.pushed = false;
                                b.Changed?.Invoke(b, EventArgs.Empty);
                            }
                        }
                    }
                    if (flags.HasFlag(ButtonFlags.ToggleButton))
                    {
                        pushed = !pushed;
                    }
                    else
                    {
                        pushed = true;
                    }
                }
                else if (pushed)
                {
                    if (Contains(p) && Click != null) Click?.Invoke(this, EventArgs.Empty);
                    if (flags.HasFlag(ButtonFlags.NormalButton)) pushed = false;

                }
                if (pushed != pushedBackup && Changed != null) Changed?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}
