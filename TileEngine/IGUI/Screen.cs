using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;
using TileEngine.Graphics;
using TileEngine.Input;
using TileEngine.Logging;

namespace TileEngine.IGUI
{
    public class Screen
    {
        private Engine engine;
        private IGraphics gfx;
        private List<Window> windows;
        private int wborTop;
        private int wborLeft;
        private int wborRight;
        private int wborBottom;
        private Font font;

        private ITheme theme;
        private Window hoverWindow;
        private Window activeWindow;
        private Gadget hoverGadget;
        private Gadget selectedGadget;
        private Gadget activeGadget;
        private int mouseX;
        private int mouseY;
        private MouseButton mouseButtons;

        public Screen(Engine engine)
        {
            this.engine = engine;
            gfx = engine.Graphics;
            theme = new BaseTheme();
            font = engine.Fonts.DefaultFont;
            windows = new List<Window>();
            wborTop = 4;
            wborLeft = 4;
            wborRight = 4;
            wborBottom = 4;
        }

        public event EventHandler<InputEvent> Input;

        protected void OnInputEvent(InputEvent ie)
        {
            Input?.Invoke(this, ie);
        }

        protected void OnInputEvent(IDCMPFlags flags, InputCode code, Window window, Gadget gadget, int x, int y, MouseButton button)
        {
            if (window != null && (window.IDCMPFlags & flags) == flags)
            {
                OnInputEvent(new InputEvent(flags, window, gadget, x, y, code, button));
            }
        }

        public int Width
        {
            get { return gfx.ViewWidth; }
        }

        public int Height
        {
            get { return gfx.ViewHeight; }
        }

        public int WBorTop
        {
            get { return wborTop; }
        }

        public int WBorLeft
        {
            get { return wborLeft; }
        }

        public int WBorRight
        {
            get { return wborRight; }
        }

        public int WBorBottom
        {
            get { return wborBottom; }
        }

        public Font Font
        {
            get { return font; }
        }

        public ITheme Theme
        {
            get { return theme; }
            set
            {
                if (theme != value)
                {
                    theme = value;
                }
            }
        }

        public void Activate(int x, int y)
        {
            MouseMove(x, y);
        }

        public Window OpenWindow(params (Tags, object)[] tags)
        {
            Window window = new Window(this, tags);
            windows.Add(window);
            if (window.Flags.HasFlag(WindowFlags.Activate))
            {
                SetActiveWindow(window);
            }
            MouseMove(mouseX, mouseY);
            return window;
        }

        public PopupMenu OpenPopupMenu(Window window, params (Tags, object)[] tags)
        {
            PopupMenu popup = new PopupMenu(window, tags);
            popup.LeftEdge += window.LeftEdge;// + window.BorderLeft;
            popup.TopEdge += window.TopEdge;// + window.BorderTop;
            windows.Add(popup);
            MouseMove(mouseX, mouseY);
            return popup;
        }

        public void CloseWindow(Window window)
        {
            windows.Remove(window);
            if (window.Parent != null)
            {
                SetActiveWindow(window.Parent);
            }
            else if (windows.Count > 0)
            {
                SetActiveWindow(windows[windows.Count - 1]);
            }
            MouseMove(mouseX, mouseY);
        }

        public void SizeWindow(Window window, int dx, int dy)
        {
            int w = window.Width + dx;
            int h = window.Height + dy;
            if (w < 40) w = 40;
            if (h < 40) h = 40;
            window.Width = w;
            window.Height = h;
            window.Layout();
        }

        public void MoveWindow(Window window, int dx, int dy)
        {
            int x = window.LeftEdge + dx;
            int y = window.TopEdge + dy;
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            window.LeftEdge = x;
            window.TopEdge = y;
        }

        public void MoveWindowToFront(Window window)
        {
            windows.Remove(window);
            windows.Add(window);
        }

        public void MoveWindowToBack(Window window)
        {
            windows.Remove(window);
            windows.Insert(0, window);
        }

        public void ToggleWindowDepth(Window window)
        {
            int idx = windows.IndexOf(window);
            if (idx >= windows.Count - 1)
            {
                MoveWindowToBack(window);
            }
            else
            {
                MoveWindowToFront(window);
            }
        }

        public void ZipWindow(Window window)
        {

        }

        public void ActivateWindow(Window window)
        {
            SetActiveWindow(window);
        }

        public void Render()
        {
            foreach (var window in windows)
            {
                window.Render(gfx);
            }
        }

        private void HandleInput(IDCMPFlags idcmp, Gadget gadget, Window window, int x, int y, MouseButton button, InputCode code)
        {
            if (gadget != null && window != null)
            {
                int gzzx = x - window.LeftEdge;
                int gzzy = y - window.TopEdge;
                gzzx -= gadget.Bounds.Left;
                gzzy -= gadget.Bounds.Top;
                if (idcmp != IDCMPFlags.IntuiTicks)
                {
                    Logger.Info("GUI", $"Handle Input {idcmp} {gzzx}/{gzzy} {button} {code}");
                }
                var res = gadget.HandleInput(idcmp, gzzx, gzzy, button, code);
                switch (res)
                {
                    case GoActiveResult.NextActive:
                        break;
                    case GoActiveResult.PrevActive:
                        break;
                }
            }
        }

        public void InputEvent(IDCMPFlags idcmp, Gadget gadget, Window window, int x, int y, MouseButton button, InputCode code)
        {
            HandleInput(idcmp, gadget, window, x, y, button, code);
            OnInputEvent(idcmp, code, window, gadget, x, y, button);
        }

        public void Timer()
        {
            HandleInput(IDCMPFlags.IntuiTicks, activeGadget, activeWindow, mouseX, mouseY, mouseButtons, InputCode.None);
        }

        public bool MouseMove(int x, int y)
        {
            Window window = FindWindow(x, y);
            Gadget gadget = FindGadget(window, x, y);
            SetHoverWindow(window);
            SetHoverGadget(gadget);
            int dx = x - mouseX;
            int dy = y - mouseY;
            mouseX = x;
            mouseY = y;
            if (activeGadget != null && activeGadget.Selected)
            {
                switch (activeGadget.SysGType)
                {
                    case GadgetType.WDragging:
                        MoveWindow(activeWindow, dx, dy);
                        break;
                    case GadgetType.Sizing:
                        SizeWindow(activeWindow, dx, dy);
                        break;
                }
                HandleInput(IDCMPFlags.MouseMove, activeGadget, window, x, y, mouseButtons, InputCode.None);
                OnInputEvent(IDCMPFlags.MouseMove, InputCode.None, window, activeGadget, x, y, mouseButtons);
            }
            return true;
        }

        public bool MouseButtonDown(int x, int y, MouseButton button)
        {
            mouseButtons |= button;
            Window window = FindWindow(x, y);
            Gadget gadget = FindGadget(window, x, y);
            SetActiveWindow(window);
            if (button == MouseButton.Left)
            {
                SetActiveGadget(gadget);
                SetSelectedGadget(gadget);
            }
            bool gaddown = false;
            if (gadget != null && button == MouseButton.Left)
            {
                gaddown = gadget.Activation.HasFlag(ActivationFlags.Immediate) && !gadget.Disabled;
            }
            HandleInput(IDCMPFlags.MouseButtons, activeGadget, window, x, y, button, InputCode.Pressed);
            if (gaddown)
            {
                HandleInput(IDCMPFlags.GadgetDown, activeGadget, window, x, y, button, InputCode.None);
                OnInputEvent(IDCMPFlags.GadgetDown, InputCode.None, window, gadget, x, y, button);
            }
            return gaddown;
        }

        public bool MouseButtonUp(int x, int y, MouseButton button)
        {
            mouseButtons &= ~button;
            Window window = FindWindow(x, y);
            Gadget gadget = FindGadget(window, x, y);
            bool gadup = false;
            bool selected = false;
            if (gadget != null && !gadget.Disabled && button == MouseButton.Left)
            {
                selected = gadget.TogSelect;
                switch (gadget.SysGType)
                {
                    case GadgetType.WDepth:
                        if (gadget == selectedGadget)
                        {
                            ToggleWindowDepth(window);
                        }
                        break;
                    case GadgetType.Close:
                        if (gadget == selectedGadget)
                        {
                            OnInputEvent(IDCMPFlags.CloseWindow, InputCode.None, window, gadget, x, y, button);
                        }
                        break;
                    case GadgetType.WZoom:
                        if (gadget == selectedGadget)
                        {
                            ZipWindow(window);
                        }
                        break;
                    case GadgetType.None:
                        gadup = (gadget.Activation.HasFlag(ActivationFlags.RelVerify) && (gadget == selectedGadget)) || (!gadget.Activation.HasFlag(ActivationFlags.RelVerify));
                        break;
                }
            }

            SetSelectedGadget(null);
            if (gadget != null && !gadget.Disabled && gadget.Activation.HasFlag(ActivationFlags.ToggleSelect) && gadup && button == MouseButton.Left)
            {
                gadget.TogSelect = !selected;
            }
            HandleInput(IDCMPFlags.MouseButtons, activeGadget, window, x, y, button, InputCode.Released);
            if (gadup)
            {
                HandleInput(IDCMPFlags.GadgetUp, activeGadget, window, x, y, button, InputCode.None);
                OnInputEvent(IDCMPFlags.GadgetUp, InputCode.None, window, gadget, x, y, button);
            }
            return gadup;
        }

        private Window FindWindow(int x, int y)
        {
            Window win = null;
            foreach (var w in windows)
            {
                if (w.LeftEdge <= x && w.TopEdge <= y && w.LeftEdge + w.Width > x && w.TopEdge + w.Height > y)
                {
                    win = w;
                }
            }
            return win;
        }

        private Gadget FindGadget(Window window, int x, int y)
        {
            if (window != null)
            {
                return window.FindGadget(x, y);
            }
            return null;
        }

        private void SetHoverWindow(Window window)
        {
            if (window != hoverWindow)
            {
                if (hoverWindow != null)
                {
                    hoverWindow.MouseHover = false;
                }
                hoverWindow = window;
                if (hoverWindow != null)
                {
                    hoverWindow.MouseHover = true;
                }
            }
        }

        private void SetActiveWindow(Window window)
        {
            if (window != null && window.Flags.HasFlag(WindowFlags.ToolWindow))
            {
                window = window.Parent;
            }
            if (window != activeWindow)
            {
                if (activeWindow != null)
                {
                    activeWindow.Active = false;
                    OnInputEvent(IDCMPFlags.InactiveWindow, InputCode.None, activeWindow, null, 0, 0, MouseButton.None);
                }
                activeWindow = window;
                if (activeWindow != null)
                {
                    activeWindow.Active = true;
                    OnInputEvent(IDCMPFlags.ActiveWindow, InputCode.None, activeWindow, null, 0, 0, MouseButton.None);
                }
            }
        }

        private void SetHoverGadget(Gadget gadget)
        {
            if (gadget != hoverGadget)
            {
                if (hoverGadget != null)
                {
                    hoverGadget.MouseHover = false;
                }
                hoverGadget = gadget;
                if (hoverGadget != null)
                {
                    hoverGadget.MouseHover = true;
                }
            }
        }

        private void SetActiveGadget(Gadget gadget)
        {
            if (gadget != activeGadget)
            {
                GoActiveResult result = GoActiveResult.MeActive;
                if (gadget != null)
                {
                    int termination = 0;
                    result = gadget.GoActive(mouseX, mouseY, ref termination);
                }
                if ((result & ~GoActiveResult.Verify) == GoActiveResult.MeActive)
                {
                    if (activeGadget != null)
                    {
                        activeGadget.GoInactive(true);
                        activeGadget.Active = false;
                    }
                    activeGadget = gadget;
                    if (activeGadget != null)
                    {
                        activeGadget.Active = true;
                    }
                }
            }
        }

        private void SetSelectedGadget(Gadget gadget)
        {
            if (gadget != selectedGadget)
            {
                if (selectedGadget != null)
                {
                    selectedGadget.Selected = false;
                }
                selectedGadget = gadget;
                if (selectedGadget != null)
                {
                    selectedGadget.Selected = selectedGadget.Active;
                }
            }
        }

    }
}
