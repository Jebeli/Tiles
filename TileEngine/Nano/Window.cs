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
    public enum WindowFlags
    {
        None = 0x00,
        CloseButton = 0x01,
        MaximizeButton = 0x02,
        MinimizeButton = 0x04
    }
    public class Window : Widget
    {
        protected string title;
        protected Widget buttonPanel;
        protected ToolButton closeButton;
        protected ToolButton maximizeButton;
        protected ToolButton minimizeButton;
        protected bool modal;
        protected bool drag;
        protected WindowFlags flags;

        public Window(Widget parent, string title = "")
            : base(parent)
        {
            this.title = title;
            flags = WindowFlags.None;
        }

        public event EventHandler<EventArgs> CloseClick;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public bool Modal
        {
            get { return modal; }
            set { modal = value; }
        }

        public WindowFlags Flags
        {
            get { return flags; }
            set
            {
                if (flags != value)
                {
                    flags = value;
                    if (flags == WindowFlags.None)
                    {
                        buttonPanel = null;
                        closeButton = null;
                        minimizeButton = null;
                        maximizeButton = null;
                    }
                    else
                    {
                        InitButtonPanel();
                    }
                }
            }
        }

        public Widget ButtonPanel
        {
            get
            {
                if (buttonPanel == null)
                {
                    InitButtonPanel();
                }
                return buttonPanel;
            }
        }
        internal protected virtual void RefreshRelativePlacement()
        {

        }
        private void InitButtonPanel()
        {
            buttonPanel = new Widget(this);
            buttonPanel.Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 4);
            minimizeButton = new ToolButton(buttonPanel, Icons.ENTYPO_ICON_CW);
            maximizeButton = new ToolButton(buttonPanel, Icons.ENTYPO_ICON_CUP);
            closeButton = new ToolButton(buttonPanel, Icons.ENTYPO_ICON_CROSS)
            {
                Flags = ButtonFlags.NormalButton
            };
            closeButton.Click += CloseButton_Click;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            CloseClick?.Invoke(this, e);
        }


        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            if (buttonPanel != null) buttonPanel.Visible = false;
            Vector2 result = base.GetPreferredSize(gfx);
            if (buttonPanel != null) buttonPanel.Visible = true;
            Vector2 bounds = new Vector2(gfx.MeasureTextWidth(Font, title), fontSize);
            return new Vector2(Math.Max(result.X, bounds.X + 20), Math.Max(result.Y, bounds.Y));
        }

        public override void PerformLayout(IGraphics gfx)
        {
            if (buttonPanel == null)
            {
                base.PerformLayout(gfx);
            }
            else
            {
                buttonPanel.Visible = false;
                base.PerformLayout(gfx);
                foreach (var w in buttonPanel.Children)
                {
                    w.FixedSize = new Vector2(22, 22);
                    w.FontSize = 15;
                }
                closeButton.Visible = flags.HasFlag(WindowFlags.CloseButton);
                maximizeButton.Visible = flags.HasFlag(WindowFlags.MaximizeButton);
                minimizeButton.Visible = flags.HasFlag(WindowFlags.MinimizeButton);
                buttonPanel.Visible = true;
                buttonPanel.Size = new Vector2(Width, 22);
                buttonPanel.Position = new Vector2(Width - (buttonPanel.GetPreferredSize(gfx).X + 5), 3);
                buttonPanel.PerformLayout(gfx);
            }
        }

        public int HeaderHeight
        {
            get
            {
                if (!string.IsNullOrEmpty(title)) return theme.WindowHeaderHeight;
                if (flags != WindowFlags.None) return theme.WindowHeaderHeight;
                return 0;
            }
        }

        public override void Draw(IGraphics gfx)
        {
            int hh = HeaderHeight;
            gfx.FillRectangle(pos.X, pos.Y, size.X, size.Y, mouseFocused ? theme.WindowFillFocused : theme.WindowFillUnfocused);
            if (hh > 0)
            {
                gfx.FillRectangle(pos.X, pos.Y, size.X, hh - 3, theme.WindowHeader, theme.WindowHeaderBot);
                gfx.FillRectangle(pos.X, pos.Y + hh - 2, size.X, 1, theme.BorderDark);
                gfx.FillRectangle(pos.X, pos.Y + hh - 1, size.X, 1, theme.BorderLight);
                Color tc = focused ? theme.WindowTitleFocused : theme.WindowTitleUnfocused;
                gfx.RenderText(Font, title, pos.X + size.X / 2, pos.Y + hh / 2 - 1, tc);
            }
            base.Draw(gfx);
        }

        public void Center()
        {
            Screen.CenterWindow(this);
        }

        public void Close()
        {
            Screen.CloseWindow(this);
        }

        public void MoveToFront()
        {
            Screen.MoveWindowToFront(this);
        }

        public override bool MouseDragEvent(Vector2 p, Vector2 rel, MouseButton button)
        {
            if (drag && button == MouseButton.Left)
            {
                pos += rel;
                return true;
            }
            return false;
        }

        public override bool MouseButtonEvent(Vector2 p, MouseButton button, bool down)
        {
            if (base.MouseButtonEvent(p, button, down)) return true;
            if (button == MouseButton.Left)
            {
                drag = down && ((p.Y - pos.Y) < HeaderHeight);
                return true;
            }
            return false;
        }
    }
}
