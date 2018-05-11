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
    public enum IconPlacement
    {
        None,
        Center,
        Left,
        Right,
        Top,
        Bottom
    }
    public class Gadget : Widget
    {
        private string text;
        private Icons icon;
        private IconPlacement iconPlacement;
        private Color backgroundColor;
        private bool transparentBackground;
        public Gadget(Widget parent, string text = null, Icons icon = Icons.NONE, EventHandler<EventArgs> clickEventHandler = null)
            : base(parent)
        {
            this.text = text;
            this.icon = icon;
            iconPlacement = IconPlacement.Left;
            backgroundColor = Color.Empty;
            if (clickEventHandler != null) Click += clickEventHandler;
        }

        public event EventHandler<EventArgs> Click;

        public EventHandler<EventArgs> ClickEvent
        {
            set { Click += value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Icons Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public IconPlacement IconPlacement
        {
            get { return iconPlacement; }
            set { iconPlacement = value; }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public bool TransparentBackground
        {
            get { return transparentBackground; }
            set { transparentBackground = value; }
        }

        public override bool MouseMoveEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            widget = this;
            return true;
        }

        public override bool MouseButtonDownEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            if (Enabled && button == MouseButton.Left)
            {
                widget = this;
                return true;
            }
            return false;
        }

        public override bool MouseButtonUpEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            if (Enabled && Selected && button == MouseButton.Left)
            {
                widget = this;
                return true;
            }
            return false;
        }

        protected virtual void OnClick(EventArgs e)
        {
            Click?.Invoke(this, e);
        }

        public override string ToString()
        {
            return $"Gadget: {text}";
        }
    }
}
