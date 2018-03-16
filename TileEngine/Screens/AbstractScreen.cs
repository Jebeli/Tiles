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

namespace TileEngine.Screens
{
    using System;
    using Logging;
    using Core;
    using Input;
    using System.Collections.Generic;
    using GUI;
    using TileEngine.Graphics;

    public abstract class AbstractScreen : NamedObject, IScreen
    {
        private bool rendered;
        private TimeSpan startTime;
        protected Engine engine;
        private static readonly float[] scales = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f };
        private int scaleIndex = 9;
        private List<Window> windows;
        private Window activeWindow;
        private DrawInfo drawInfo;

        public AbstractScreen(Engine engine, string name)
            : base(name)
        {
            this.engine = engine;
            windows = new List<Window>();
            drawInfo = new DrawInfo()
            {
                DetailPen = Color.Black,
                BlockPen = Color.White,
                TextPen = Color.Black,
                ShinePen = Color.BrightGray,
                ShadowPen = Color.DarkGray,
                FillPen = new Color(62, 92, 154),
                FillTextPen = Color.Black,
                BackgoundPen = Color.Gray,
                HighlightTextPen = Color.White,
                HoverShinePen = Color.BrightGray,
                HoverShadowPen = Color.DarkGray,
                HoverBackgroundPen = new Color(62 + 20, 92 + 20, 154 + 20),
                InactiveHoverBackgroundPen = new Color(210, 210, 210),
                DisabledTextPen = new Color(32, 32, 32, 128),
                PropClearPen = new Color(128, 128, 128),
                Width = Width,
                Height = Height
            };
        }
        public IEnumerable<Window> Windows
        {
            get { return windows; }
        }

        public Window ActiveWindow
        {
            get { return activeWindow; }
            set { activeWindow = value; }
        }
        public int Width { get { return engine.Graphics.ViewWidth; } }
        public int Height { get { return engine.Graphics.ViewHeight; } }

        public void RemoveWindow(Window window)
        {
            Logger.Info("Screen", $"Remove Window \"{window}\"");
            if (activeWindow == window) { activeWindow = null; }
            windows.Remove(window);
            if (windows.Count > 0)
            {
                Intuition.ActivateWindow(windows[windows.Count - 1]);
            }
        }
        public void AddWindow(Window window)
        {
            Logger.Info("Screen", $"Add Window \"{window}\"");
            windows.Add(window);
        }
        public void WindowToFront(Window window)
        {
            if (windows.Remove(window))
            {
                windows.Add(window);
            }
        }
        public void WindowToBack(Window window)
        {
            if (windows.Remove(window))
            {
                for (int i = 0; i < windows.Count; i++)
                {
                    if (!windows[i].HasFlag(WindowFlags.WFLG_BACKDROP))
                    {
                        windows.Insert(i, window);
                        break;
                    }
                }
            }
        }

        public DrawInfo GetDrawInfo()
        {
            drawInfo.Width = Width;
            drawInfo.Height = Height;
            return drawInfo;
        }

        public virtual void Show()
        {
            Logger.Info("Screen", $"Showing Screen {Name}");
            startTime = engine.GetCurrentTime();
            rendered = false;
            LinkInput();
            Intuition.ActivateWindow(activeWindow);
        }

        public virtual void Hide()
        {
            Logger.Info("Screen", $"Hiding Screen {Name}");
            rendered = false;
            UnlinkInput();
        }

        public virtual void Update(TimeInfo time)
        {
        }

        public virtual void Render(TimeInfo time)
        {
            if (!rendered)
            {
                Logger.Info("Screen", $"Rendering Screen {Name}");
            }
            rendered = true;
        }

        protected virtual void OnKeyDown(Key keyData, Key keyCode, char code)
        {
            Intuition.KeyDown(keyData, code);
        }
        protected virtual void OnKeyUp(Key keyData, Key keyCode, char code)
        {
            Intuition.KeyUp(keyData, code);
        }

        protected void InvalidateAllWindows()
        {
            foreach (var win in windows)
            {
                win.Invalidate();
            }
        }

        protected virtual void OnMouseWheel(float x, float y, int delta)
        {
            if (delta > 0)
            {
                ZoomOut();
            }
            else if (delta < 0)
            {
                ZoomIn();
            }
        }
        protected virtual bool OnMouseDown(float x, float y, MouseButton button)
        {
            Intuition.MouseDown((int)x, (int)y, button);
            return true;
        }

        protected virtual bool OnMouseUp(float x, float y, MouseButton button)
        {
            Intuition.MouseUp((int)x, (int)y, button);
            return true;
        }
        protected virtual void OnMouseMove(float x, float y, MouseButton button)
        {
            Intuition.MouseMove((int)x, (int)y, button);
        }

        protected virtual void OnIntuitionMessage(IntuiMessage message)
        {
            switch (message.Message)
            {
                case IDCMPFlags.GADGETUP:
                    OnGadgetClick(message.Gadget);
                    break;
                case IDCMPFlags.AUTOREQUEST:
                    OnAutoRequest(message.Code);
                    break;
                case IDCMPFlags.CLOSEWINDOW:
                    OnCloseWindow(message.Window);
                    break;
            }
        }

        protected virtual void OnCloseWindow(Window window)
        {
            Logger.Info("Screen", $"Close Window \"{window}\" selected");
        }

        protected virtual void OnGadgetClick(Gadget gadget)
        {
            Logger.Info("Screen", $"Gadget \"{gadget}\" selected");
        }

        protected virtual void OnAutoRequest(int gadNum)
        {
            Logger.Info("Screen", $"Requester Gadget {gadNum} selected");
        }

        private void LinkInput()
        {
            Intuition.Message += Intuition_Message;
            engine.Input.OnMouseDown += Input_OnMouseDown;
            engine.Input.OnMouseUp += Input_OnMouseUp;
            engine.Input.OnMouseMove += Input_OnMouseMove;
            engine.Input.OnMouseWheel += Input_OnMouseWheel;
            engine.Input.OnKeyDown += Input_OnKeyDown;
            engine.Input.OnKeyUp += Input_OnKeyUp;
        }

        private void UnlinkInput()
        {
            Intuition.Message -= Intuition_Message;
            engine.Input.OnMouseDown -= Input_OnMouseDown;
            engine.Input.OnMouseUp -= Input_OnMouseUp;
            engine.Input.OnMouseMove -= Input_OnMouseMove;
            engine.Input.OnMouseWheel -= Input_OnMouseWheel;
            engine.Input.OnKeyDown -= Input_OnKeyDown;
            engine.Input.OnKeyUp -= Input_OnKeyUp;
        }

        private void Intuition_Message(object sender, IntuiMessage e)
        {
            OnIntuitionMessage(e);
        }

        private void Input_OnKeyUp(object sender, KeyEventArgs e)
        {
            OnKeyUp(e.KeyData, e.KeyCode, e.Code);
        }

        private void Input_OnKeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData, e.KeyCode, e.Code);
        }

        private void Input_OnMouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e.X, e.Y, e.Button);
        }

        private void Input_OnMouseUp(object sender, MouseEventArgs e)
        {
            OnMouseUp(e.X, e.Y, e.Button);
        }

        private void Input_OnMouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e.X, e.Y, e.Button);
        }
        private void Input_OnMouseWheel(object sender, MouseEventArgs e)
        {
            OnMouseWheel(e.X, e.Y, e.Delta);
        }

        private void ZoomIn()
        {
            scaleIndex++;
            if (scaleIndex >= scales.Length) scaleIndex = scales.Length - 1;
            InvalidateAllWindows();
            engine.SetViewScale(scales[scaleIndex]);
        }

        private void ZoomOut()
        {
            scaleIndex--;
            if (scaleIndex < 0) scaleIndex = 0;
            InvalidateAllWindows();
            engine.SetViewScale(scales[scaleIndex]);
        }
    }
}
