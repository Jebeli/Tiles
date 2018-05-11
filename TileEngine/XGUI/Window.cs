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
    public class Window : Widget
    {
        private bool dragging;
        private bool modal;
        private string title;
        private bool borderless;
        private bool thinBorder;
        private bool closeGadget;
        private Button closeButton;
        public Window(Widget parent, string title = "", EventHandler<EventArgs> closeEventHandler = null)
            : base(parent)
        {
            this.title = title;
            CalcBorders();
            InitBorderPanels();
            if (closeEventHandler != null) Close += closeEventHandler;
        }

        public event EventHandler<EventArgs> Close;
        public EventHandler<EventArgs> CloseEvent
        {
            set { Close += value; }
        }

        public void CloseWindow()
        {
            Screen.CloseWindow(this);
        }

        public void MoveWindowToFront()
        {
            Screen.MoveWindowToFront(this);
        }

        public void CenterWindow()
        {
            Screen.CenterWindow(this);
        }
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    CalcBorders();
                }
            }
        }

        public bool CloseGadget
        {
            get { return closeGadget; }
            set
            {
                if (closeGadget != value)
                {
                    closeGadget = value;
                    CalcBorders();
                }
            }
        }

        public bool Modal
        {
            get { return modal; }
            set { modal = value; }
        }

        public bool Borderless
        {
            get { return borderless; }
            set
            {
                if (borderless != value)
                {
                    borderless = value;
                    CalcBorders();
                }
            }
        }

        public bool ThinBorder
        {
            get { return thinBorder; }
            set
            {
                if (thinBorder != value)
                {
                    thinBorder = value;
                    CalcBorders();
                }
            }
        }

        private void CalcBorders()
        {
            if (borderless)
            {
                BorderLeft = 0;
                BorderRight = 0;
                BorderBottom = 0;
                BorderTop = 0;
            }
            else
            {
                if (thinBorder)
                {
                    BorderLeft = 2;
                    BorderRight = 2;
                    BorderBottom = 2;
                    BorderTop = CalcBorderTop();
                }
                else
                {
                    BorderLeft = 4;
                    BorderRight = 4;
                    BorderBottom = 4;
                    BorderTop = CalcBorderTop();
                }
            }
        }
        private int CalcBorderTop()
        {
            if (!string.IsNullOrEmpty(title)) return 32;
            if (closeGadget) return 32;
            if (borderless) return 0;
            if (thinBorder) return 2;
            return 4;
        }

        internal protected virtual void RefreshRelativePlacement()
        {

        }

        public override void Render(IGraphics gfx)
        {
            Theme.RenderWindow(gfx, this);
            base.Render(gfx);
        }

        public override bool MouseDragEvent(Vector2 p, Vector2 rel, MouseButton button)
        {
            if (dragging && button == MouseButton.Left)
            {
                Position += rel;
                return true;
            }
            return false;
        }

        public override bool MouseButtonDownEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            if (base.MouseButtonDownEvent(p, button, ref widget)) return true;
            if (button == MouseButton.Left)
            {
                if ((p.Y - Position.Y) < BorderTop)
                {
                    dragging = true;
                    widget = this;
                    return true;
                }
            }
            return false;
        }

        public override bool MouseButtonUpEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            if (base.MouseButtonUpEvent(p, button, ref widget)) return true;
            if (button == MouseButton.Left)
            {
                dragging = false;
                widget = this;
                return true;
            }
            return false;
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            HideBorder();
            Vector2 result = base.GetPreferredSize(gfx);
            ShowBorder();
            return result;
        }

        public override void PerformLayout(IGraphics gfx)
        {
            HideBorder();
            base.PerformLayout(gfx);
            ShowBorder();
        }

        private void HideBorder()
        {
            closeButton.Visible = false;
            RemoveChild(closeButton);
        }

        private void ShowBorder()
        {
            if (closeGadget)
            {
                AddChild(0, closeButton);
                closeButton.Visible = closeGadget;
                closeButton.Size = new Vector2(25, 25);
                closeButton.Position = new Vector2(32 / 2 - 25 / 2, 32 / 2 - 25 / 2);
            }
        }
        private void InitBorderPanels()
        {
            closeButton = new Button(this, "", Icons.ENTYPO_ICON_CROSS);
            closeButton.FixedSize = new Vector2(25, 25);
            closeButton.IconPlacement = IconPlacement.Center;
            closeButton.TransparentBackground = true;
            closeButton.Visible = closeGadget;
            closeButton.Click += (o, i) => { Close?.Invoke(this, i); };
        }

        public override string ToString()
        {
            return $"Window: {title}";
        }

    }
}
