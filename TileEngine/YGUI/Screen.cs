/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
 */

namespace TileEngine.YGUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;
    using TileEngine.Graphics;
    using TileEngine.Input;


    public class Screen : Gadget
    {
        private Engine engine;
        private Window hoverWindow;
        private Gadget hoverGadget;
        private Window selectedWindow;
        private Gadget selectedGadget;
        private Window focusedWindow;
        private Gadget focusedGadget;
        private Gadget dragGadget;
        private Window dragWindow;
        private bool dragging;
        private Point mousePos;
        private MouseButton mouseButton;
        private bool focusedToTop;

        public Screen(Engine engine)
            : base(null)
        {
            this.engine = engine;
            focusedToTop = false;
            theme = new BaseTheme();
        }

        public Engine Engine
        {
            get { return engine; }
        }

        public bool FocusedToTop
        {
            get { return focusedToTop; }
            set { focusedToTop = value; }
        }

        public void PerformLayout()
        {
            PerformLayout(engine.Graphics);
        }

        public void Render()
        {
            Render(engine.Graphics);
        }

        public void SetHoverWindow(Window window)
        {
            if (window != hoverWindow)
            {
                if (hoverWindow != null)
                {
                    hoverWindow.Hover = false;
                }
                hoverWindow = window;
                if (hoverWindow != null)
                {
                    hoverWindow.Hover = true;
                }
            }
        }

        public void SetHoverGadget(Gadget gadget)
        {
            if (gadget is Window) return;
            if (gadget != hoverGadget)
            {
                if (hoverGadget != null)
                {
                    hoverGadget.Hover = false;
                }
                hoverGadget = gadget;
                if (hoverGadget != null)
                {
                    hoverGadget.Hover = true;
                }
            }
        }

        public void SetSelectedWindow(Window window)
        {
            if (window != selectedWindow)
            {
                if (selectedWindow != null)
                {
                    selectedWindow.MouseSelected = false;
                }
                selectedWindow = window;
                if (selectedWindow != null)
                {
                    selectedWindow.MouseSelected = true;
                }
            }
        }

        private void SetSelectedGadget(Gadget gadget)
        {
            if (gadget is Window) return;
            if (gadget != null && !gadget.Enabled) return;
            if (gadget != selectedGadget)
            {
                if (selectedGadget != null)
                {
                    selectedGadget.MouseSelected = false;
                }
                selectedGadget = gadget;
                if (selectedGadget != null)
                {
                    selectedGadget.MouseSelected = true;
                }
            }
        }

        public void SetFocusedWindow(Window window)
        {
            if (window is PopupWindow)
            {
                window = ((PopupWindow)window).ParentWindow;
            }
            if (window != focusedWindow)
            {
                if (focusedWindow != null)
                {
                    focusedWindow.Focused = false;
                }
                focusedWindow = window;
                if (focusedWindow != null)
                {
                    focusedWindow.Focused = true;
                    if (focusedToTop)
                    {
                        WindowToFront(focusedWindow);
                    }
                }
            }
        }

        public void SetFocusedGadget(Gadget gadget)
        {
            if (gadget is Window) return;
            if (gadget != null && !gadget.Enabled) return;
            if (gadget != focusedGadget)
            {
                if (focusedGadget != null)
                {
                    focusedGadget.Focused = false;
                }
                focusedGadget = gadget;
                if (focusedGadget != null)
                {
                    focusedGadget.Focused = true;
                }
            }
        }

        private Window FindWindow(Point pos)
        {
            Window win = null;
            foreach (var w in Children)
            {
                if (w.Visible && w.Contains(pos) && w is Window)
                {
                    win = (Window)w;
                }
            }
            return win;
        }


        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderScreen(gfx, this);
        }

        public void CloseWindow(Window window)
        {
            window.Visible = false;
            foreach (var p in GetPopupWindows(window))
            {
                CloseWindow(p);
            }
            RemChild(window);
            ActivateNextWindow(window);
            PerformLayout();
            ActivateMouse();

        }

        public void ActivateNextWindow(Window window)
        {
            Window p = GetParentWindow(window);
            if (p != null)
            {
                ActivateWindow(p);
            }
            else
            {
                foreach (var w in Children.Reverse<Gadget>())
                {
                    if (w.Visible && w is Window)
                    {
                        ActivateWindow((Window)w);
                        break;
                    }
                }
            }
        }

        public void WindowToggleDepth(Window window)
        {
            if (Children.IndexOf(window) < (Children.Count - 1))
            {
                WindowToFront(window);
            }
            else
            {
                WindowToBack(window);
            }
        }

        public void InvalidateWindow(Window window)
        {
            if (window.Visible)
            {
                window.PerformLayout(engine.Graphics);
            }
        }

        public void ShowWindow(Window window)
        {
            window.Visible = true;
            PerformLayout();
            window.InitMinSize();
            ActivateMouse();
        }

        public void HideWindow(Window window)
        {
            window.Visible = false;
            foreach (var p in GetPopupWindows(window))
            {
                p.Visible = false;
            }
            ActivateNextWindow(window);
            PerformLayout();
            ActivateMouse();
        }

        public void WindowToFront(Window window)
        {
            RemChild(window);
            AddChild(window);
            ActivateMouse();
        }

        public void WindowToBack(Window window)
        {
            RemChild(window);
            AddChild(0, window);
            ActivateMouse();
        }

        public void ActivateWindow(Window window)
        {
            SetFocusedWindow(window);
            ActivateMouse();
        }

        public void MoveWindow(Window window, Point rel)
        {
            window.Position += rel;
            foreach (var p in GetPopupWindows(window))
            {
                p.Position += rel;
            }
        }

        private Window GetParentWindow(Window window)
        {
            PopupWindow popup = window as PopupWindow;
            if (popup != null)
            {
                return popup.ParentWindow;
            }
            return null;
        }

        private List<PopupWindow> GetPopupWindows(Window window)
        {
            List<PopupWindow> list = new List<PopupWindow>();
            foreach (var w in Children)
            {
                if (w is PopupWindow)
                {
                    PopupWindow popup = (PopupWindow)w;
                    if (popup.ParentWindow == window)
                    {
                        list.Add(popup);
                    }
                }
            }
            return list;
        }

        public void KeyDown(Key keyData, Key keyCode, char code)
        {
            focusedGadget?.HandleKeyDown(keyData, keyCode, code);
        }

        public void KeyUp(Key keyData, Key keyCode, char code)
        {
            focusedGadget?.HandleKeyUp(keyData, keyCode, code);
        }

        public void ActivateMouse()
        {
            ActivateMouse(mousePos.X, mousePos.Y);
        }

        public void ActivateMouse(int x, int y)
        {
            MouseMove(x, y);
        }

        public bool MouseButtonDown(int x, int y, MouseButton button)
        {
            bool result = false;
            Point pos = new Point(x, y);
            Gadget gadget = null;
            Window window = FindWindow(pos);
            if (window != null)
            {
                window.ClearDraggingAndSizing();
                gadget = window.FindGadget(pos);
                if (button == MouseButton.Left)
                {
                    dragGadget = gadget;
                    if (dragGadget == this) dragGadget = null;
                    dragging = dragGadget != null;
                    if (dragging) dragWindow = dragGadget.Window;
                    SetFocusedWindow(window);
                    SetFocusedGadget(gadget);
                }
                else
                {
                    dragging = false;
                    dragGadget = null;
                    dragWindow = null;
                }
            }
            else
            {
                SetFocusedWindow(null);
                SetFocusedGadget(null);
            }
            if (gadget != null)
            {
                result = gadget.HandleMouseButtonDown(gadget.ScreenToGadget(pos), button);
            }
            SetSelectedWindow(window);
            SetSelectedGadget(gadget);
            mousePos = pos;
            mouseButton |= button;
            return result;
        }

        public bool MouseButtonUp(int x, int y, MouseButton button)
        {
            bool result = false;
            Point pos = new Point(x, y);
            Gadget gadget = null;
            Window window = FindWindow(pos);
            if (window != null)
            {
                window.ClearDraggingAndSizing();
                gadget = window.FindGadget(pos);
            }
            dragging = false;
            dragGadget = null;
            dragWindow = null;
            if (selectedGadget != null)
            {
                if (!selectedGadget.RelVerify || (selectedGadget == gadget))
                {
                    result = selectedGadget.HandleMouseButtonUp(selectedGadget.ScreenToGadget(pos), button);
                }
            }
            SetSelectedWindow(null);
            SetSelectedGadget(null);
            mousePos = pos;
            mouseButton &= ~button;
            return result;
        }

        public bool MouseMove(int x, int y)
        {
            bool result = false;
            Point pos = new Point(x, y);
            Gadget gadget = null;
            Window window = FindWindow(pos);
            if (dragging && dragGadget != null)
            {
                result = dragGadget.HandleMouseDrag(dragGadget.ScreenToGadget(pos), pos - mousePos, mouseButton);
            }
            if (!result && dragging && dragWindow != null)
            {
                result = dragWindow.HandleMouseDrag(dragWindow.ScreenToGadget(pos), pos - mousePos, mouseButton);
            }
            if (window != null)
            {
                gadget = window.FindGadget(pos);
            }
            if (!result && gadget != null)
            {
                result = gadget.HandleMouseMove(gadget.ScreenToGadget(pos), mouseButton);
            }
            SetHoverWindow(window);
            SetHoverGadget(gadget);
            mousePos = pos;
            return result;
        }

        public void Timer()
        {
            if (focusedGadget != null)
            {
                focusedGadget.HandleTimer(focusedGadget.ScreenToGadget(mousePos), mouseButton);
            }
        }

        public override string ToString()
        {
            return $"Screen {Label}";
        }

    }
}
