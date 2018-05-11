using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Fonts;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.Nano
{
    public class Screen : Widget
    {
        protected string caption;
        protected Color background;
        protected IGraphics gfx;
        protected Vector2 mousePos;
        protected MouseButton mouseButton;
        protected bool dragActive;
        protected Widget dragWidget;
        protected List<Widget> focusPath;

        public Screen(IGraphics gfx, Vector2 size, Font font, string caption = "")
            : base(null)
        {
            focusPath = new List<Widget>();
            this.gfx = gfx;
            this.caption = caption;
            this.font = font;
            Size = size;
            Theme = new Theme();
        }

        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public Color Background
        {
            get { return background; }
            set { background = value; }
        }

        public void PerformLayout()
        {
            PerformLayout(gfx);
        }

        public void Draw()
        {
            Draw(gfx);
        }

        public override void Draw(IGraphics gfx)
        {
            if (!background.IsEmpty)
            {
                gfx.FillRectangle(0, 0, Width, Height, background);
            }
            base.Draw(gfx);
        }

        public void CenterWindow(Window window)
        {
            if (window.Size.IsEmpty)
            {
                window.Size = window.GetPreferredSize(gfx);
                window.PerformLayout(gfx);
            }
            window.Position = (size - window.Size) / 2;
        }

        public void CloseWindow(Window window)
        {
            if (focusPath.Contains(window))
                focusPath.Clear();
            if (dragWidget == window)
                dragWidget = null;
            RemoveChild(window);
            foreach (var p in GetOpenPopups(window))
            {
                CloseWindow(p);
            }
        }

        public IList<Popup> GetOpenPopups(Window window)
        {
            List<Popup> list = new List<Popup>();
            foreach(var w in children)
            {
                Popup p = w as Popup;
                if (p != null && p.ParentWindow == window)
                {
                    list.Add(p);
                }
            }
            return list;
        }

        public void UpdateFocus(Widget widget)
        {
            foreach (var w in focusPath)
            {
                if (!w.Focused) continue;
                w.FocusEvent(false);
            }
            focusPath.Clear();
            Window window = null;
            while (widget != null)
            {
                focusPath.Add(widget);
                if (widget is Window)
                {
                    window = (Window)widget;
                }
                widget = widget.Parent;
            }
            foreach (var w in focusPath) w.FocusEvent(true);
            if (window != null) MoveWindowToFront(window);
        }

        public void MoveWindowToFront(Window window)
        {
            children.Remove(window);
            children.Add(window);
            bool changed = false;
            do
            {
                int baseIndex = 0;
                for (int index =0; index < children.Count; index++)
                {
                    if (children[index] == window)
                    {
                        baseIndex = index;
                    }
                }
                changed = false;
                for (int index = 0; index < children.Count; index++)
                {
                    Popup pw = children[index] as Popup;
                    if (pw != null && pw.ParentWindow == window && index < baseIndex)
                    {
                        MoveWindowToFront(pw);
                        changed = true;
                        break;
                    }
                }
            } while (changed);
        }

        public bool MousePositionCallbackEvent(float x, float y)
        {
            Vector2 p = new Vector2((int)x, (int)y);
            bool ret = false;
            try
            {
                if (!dragActive)
                {
                    Widget widget = FindWidget(p);
                    if (widget != null)
                    {
                        // cursor
                    }
                }
                else
                {
                    ret = dragWidget.MouseDragEvent(p - dragWidget.Parent.AbsolutePosition, p - mousePos, mouseButton);
                }
                if (!ret)
                    ret = MouseMotionEvent(p, p - mousePos, mouseButton);
                mousePos = p;
                return ret;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool MouseButtonCallbackEvent(MouseButton button, bool down)
        {
            try
            {
                if (focusPath.Count > 1)
                {
                    Window window = focusPath[focusPath.Count - 2] as Window;
                    if (window != null && window.Modal)
                    {
                        if (!window.Contains(mousePos))
                        {
                            return false;
                        }
                    }
                }
                if (down)
                {
                    mouseButton |= button;
                }
                else
                {
                    mouseButton &= ~button;
                }
                var dropWidget = FindWidget(mousePos);
                if (dragActive && !down && dropWidget != dragWidget)
                {
                    dragWidget.MouseButtonEvent(mousePos - dragWidget.Parent.AbsolutePosition, mouseButton, false);
                }
                if (dropWidget != null)
                {
                    // cursor
                }
                if (down && ((button == MouseButton.Left) || (button == MouseButton.Right)))
                {
                    dragWidget = FindWidget(mousePos);
                    if (dragWidget == this) dragWidget = null;
                    dragActive = dragWidget != null;
                    if (!dragActive) UpdateFocus(null);
                }
                else
                {
                    dragActive = false;
                    dragWidget = null;                    
                }
                return MouseButtonEvent(mousePos, button, down);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
