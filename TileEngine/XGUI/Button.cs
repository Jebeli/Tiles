using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.XGUI
{
    [Flags]
    public enum ButtonFlags
    {
        NormalButton = 0x01,
        RadioButton = 0x02,
        ToggleButton = 0x04,
        PopupButton = 0x08
    };
    public class Button : Gadget
    {

        private ButtonFlags flags;
        private List<Button> buttonGroup;

        public Button(Widget parent, string text = "", Icons icon = Icons.NONE, EventHandler<EventArgs> eventHandler = null)
            : base(parent, text, icon, eventHandler)
        {
            flags = ButtonFlags.NormalButton;
            buttonGroup = new List<Button>();
        }

        public ButtonFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public override bool MouseButtonUpEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            if (base.MouseButtonUpEvent(p, button, ref widget))
            {
                if (flags.HasFlag(ButtonFlags.RadioButton))
                {
                    if (buttonGroup.Count == 0)
                    {
                        foreach (var child in Parent.Children)
                        {
                            Button b = child as Button;
                            if ((b != this) && (b != null) && b.Flags.HasFlag(ButtonFlags.RadioButton) && b.Checked)
                            {
                                b.Checked = false;
                                //b.Changed?.Invoke(b, EventArgs.Empty);
                            }
                        }
                    }
                    else
                    {
                        foreach (var b in buttonGroup)
                        {
                            if (b != this && b.Flags.HasFlag(ButtonFlags.RadioButton) && b.Checked)
                            {
                                b.Checked = false;
                                //b.Changed?.Invoke(b, EventArgs.Empty);
                            }
                        }
                    }

                }
                if (flags.HasFlag(ButtonFlags.PopupButton))
                {
                    foreach (var child in Parent.Children)
                    {
                        Button b = child as Button;
                        if ((b != this) && (b != null) && b.Flags.HasFlag(ButtonFlags.PopupButton) && b.Checked)
                        {
                            b.Checked = false;
                            //b.Changed?.Invoke(b, EventArgs.Empty);
                        }
                    }
                }

                if (flags.HasFlag(ButtonFlags.ToggleButton)) { Checked = !Checked; }
                if (flags.HasFlag(ButtonFlags.RadioButton)) { Checked = true; }
                if (flags.HasFlag(ButtonFlags.NormalButton)) { Checked = false; }
                OnClick(EventArgs.Empty);
                return true;
            }
            return false;
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            int tw = gfx.MeasureTextWidth(Font, Text);
            int th = tw > 0 ? 32 : 25;
            int iw = Icon != Icons.NONE ? 25 : 0;
            switch (IconPlacement)
            {
                case IconPlacement.Left:
                case IconPlacement.Right:
                    return new Vector2(tw + iw + th, th);
                case IconPlacement.Top:
                case IconPlacement.Bottom:
                    return new Vector2(tw + th, th + iw);
                case IconPlacement.None:
                default:
                    return new Vector2(tw + th, th);
            }
        }

        public override void Render(IGraphics gfx)
        {
            Theme.RenderButton(gfx, this);
            base.Render(gfx);
        }
    }
}
