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
    public class Screen : Widget
    {
        private Engine engine;
        private IGraphics gfx;
        private Vector2 mousePos;
        private MouseButton mouseButton;
        private bool dragging;
        private Widget dragWidget;
        private Widget selectedWidget;
        private Widget hoverWidget;
        private Window activeWindow;
        private Widget activeWidget;
        public Screen(Engine engine)
            : base(null)
        {
            this.engine = engine;
            gfx = engine.Graphics;
            Theme = new DefaultTheme();
        }

        public void PerformLayout()
        {
            PerformLayout(gfx);
        }

        public void Render()
        {
            Render(gfx);
        }

        private void SetHoverWidget(Widget widget)
        {
            if (widget != hoverWidget)
            {
                if (hoverWidget != null)
                {
                    hoverWidget.MouseHover = false;
                }
                hoverWidget = widget;
                if (hoverWidget != null)
                {
                    hoverWidget.MouseHover = true;
                }
            }
        }

        private void SetSelectedWidget(Widget widget)
        {
            if (widget != selectedWidget)
            {
                if (selectedWidget != null)
                {
                    selectedWidget.Selected = false;
                }
                selectedWidget = widget;
                if (selectedWidget != null)
                {
                    selectedWidget.Selected = true;
                }
            }
        }

        private void SetActiveWidget(Widget widget)
        {
            if (widget != activeWidget)
            {
                if (activeWidget != null)
                {
                    activeWidget.Active = false;
                }
                activeWidget = widget;
                if (activeWidget != null)
                {
                    activeWidget.Active = true;
                }
            }
        }

        private bool SetActiveWindow(Window window)
        {
            if (window != activeWindow)
            {
                if (activeWindow != null)
                {
                    if (activeWindow.Modal) return false;
                    activeWindow.Active = false;
                }
                activeWindow = window;
                if (activeWindow != null)
                {
                    activeWindow.Active = true;
                    MoveWindowToFront(activeWindow);
                }
            }
            return true;
        }

        public IList<Popup> GetOpenPopups(Window window)
        {
            List<Popup> list = new List<Popup>();
            foreach (var w in Children)
            {
                Popup p = w as Popup;
                if (p != null && p.ParentWindow == window)
                {
                    list.Add(p);
                }
            }
            return list;
        }


        public void CloseWindow(Window window)
        {
            if (dragWidget == window)
                dragWidget = null;
            bool needNextActive = false;
            if (activeWindow == window)
            {
                activeWindow = null;
                needNextActive = true;
            }
            RemoveChild(window);
            foreach (var p in GetOpenPopups(window))
            {
                CloseWindow(p);
            }
            if (needNextActive)
            {
                foreach (var c in Children.Reverse())
                {
                    if (c is Window)
                    {
                        SetActiveWindow((Window)(c));
                        break;
                    }
                }
            }
        }

        public void InvalidateWindow(Window window)
        {
            window?.PerformLayout(gfx);
        }

        public void CenterWindow(Window window)
        {
            if (window.Size.IsEmpty)
            {
                window.Size = window.GetPreferredSize(gfx);
                window.PerformLayout(gfx);
            }
            window.Position = (Size - window.Size) / 2;
        }

        public void MoveWindowToFront(Window window)
        {
            RemoveChild(window);
            AddChild(window);
            bool changed = false;
            do
            {
                int baseIndex = 0;
                for (int index = 0; index < ChildCount; index++)
                {
                    if (ChildAt(index) == window)
                    {
                        baseIndex = index;
                    }
                }
                changed = false;
                for (int index = 0; index < ChildCount; index++)
                {
                    Popup pw = ChildAt(index) as Popup;
                    if (pw != null && pw.ParentWindow == window && index < baseIndex)
                    {
                        MoveWindowToFront(pw);
                        changed = true;
                        break;
                    }
                }
            } while (changed);
        }


        public void Activate(float x, float y)
        {
            MouseMove(x, y);
        }

        public void UpdateFocus(Widget widget)
        {
            if (widget is Screen)
            {

            }
            else if (widget is Window)
            {
                SetActiveWindow((Window)widget);
            }
            else
            {
                SetActiveWidget(widget);
            }
        }

        public bool KeyUp(Key key)
        {
            if (activeWidget != null)
            {
                return activeWidget.KeyboardEvent(key);
            }
            return false;
        }
        public bool MouseMove(float x, float y)
        {
            Vector2 p = new Vector2((int)x, (int)y);
            bool result = false;
            try
            {
                if (dragging && dragWidget != null)
                {
                    result = dragWidget.MouseDragEvent(p - dragWidget.Parent.AbsolutePosition, p - mousePos, mouseButton);
                }
                if (!result)
                {
                    Widget hoverWidget = null;
                    result = MouseMoveEvent(p, mouseButton, ref hoverWidget);
                    SetHoverWidget(hoverWidget);
                }
                mousePos = p;
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool MouseButtonDown(float x, float y, MouseButton button)
        {
            Vector2 p = new Vector2((int)x, (int)y);
            mouseButton |= button;
            mousePos = p;
            bool result = false;
            try
            {
                if (button == MouseButton.Left)
                {
                    dragWidget = FindWidget(mousePos);
                    if (dragWidget == this) dragWidget = null;
                    dragging = dragWidget != null;
                    if (!dragging) UpdateFocus(null);
                }
                else
                {
                    dragging = false;
                    dragWidget = null;
                }
                Widget selectedWidget = null;
                result = MouseButtonDownEvent(p, button, ref selectedWidget);
                if (result && selectedWidget != null)
                {
                    if (SetActiveWindow(selectedWidget.Window))
                    {
                        SetActiveWidget(selectedWidget);
                    }
                }
                SetSelectedWidget(selectedWidget);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool MouseButtonUp(float x, float y, MouseButton button)
        {
            Vector2 p = new Vector2((int)x, (int)y);
            mouseButton &= ~button;
            mousePos = p;
            bool result = false;
            try
            {
                Widget selectedWidget = null;
                result = MouseButtonUpEvent(p, button, ref selectedWidget);
                dragging = false;
                dragWidget = null;
                SetSelectedWidget(null);
                return result;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Timer()
        {
            try
            {
                if (activeWidget != null)
                {
                    return activeWidget.TimerEvent(mousePos, mouseButton);
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
