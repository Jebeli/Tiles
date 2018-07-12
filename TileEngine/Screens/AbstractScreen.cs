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
    using TileEngine.Graphics;
    using TileEngine.Fonts;
    using YGUI;

    public abstract class AbstractScreen : NamedObject, IScreen
    {
        private bool firstShown;
        private bool rendered;
        private TimeSpan startTime;
        protected Engine engine;
        private static readonly float[] scales = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f };
        private int scaleIndex = 9;
        protected Screen screen;
        private Font font;
        private Font topazFont;
        private Color backgroundColor;
        private static TimeSpan tickDuration = TimeSpan.FromMilliseconds(100);
        private static TimeSpan lastTick;


        public AbstractScreen(Engine engine, string name)
            : base(name)
        {
            backgroundColor = new Color(0, 0, 0, 255);
            this.engine = engine;
            firstShown = false;
        }

        public Font Font
        {
            get
            {
                if (font != null) return font;
                return engine.Fonts.DefaultFont;
            }
            set
            {
                font = value;
            }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            protected set { backgroundColor = value; }
        }

        public Font TopazFont
        {
            get
            {
                if (topazFont != null) return topazFont;
                return engine.Fonts.TopazFont;
            }
            set
            {
                topazFont = value;
            }
        }

        protected Screen Screen
        {
            get { return screen; }
        }

        public void PerformLayout()
        {
            screen?.PerformLayout();
        }

        public void HideTooltip()
        {
            screen?.HideTooltip();
        }

        public void ShowTooltip(string text, Point pos)
        {
            screen?.SetTooltip(text, pos);
        }

        public virtual void FirstShow()
        {
            firstShown = true;
            Logger.Info("Screen", $"First Show Screen {Name}");
            screen = new Screen(engine);
            screen.Font = Font;
            //screen.Input += Screen_Input;
            InitGUI(screen);
            SizeChanged(engine.Graphics.ViewWidth, engine.Graphics.ViewHeight);
        }


        public virtual void Show()
        {
            if (!firstShown) FirstShow();
            Logger.Info("Screen", $"Showing Screen {Name}");
            startTime = engine.GetCurrentTime();
            rendered = false;
            LinkInput();
            SizeChanged(engine.Graphics.ViewWidth, engine.Graphics.ViewHeight);
            screen.ActivateMouse(engine.Input.MouseX, engine.Input.MouseY);
        }

        public virtual void Hide()
        {
            Logger.Info("Screen", $"Hiding Screen {Name}");
            rendered = false;
            UnlinkInput();
        }

        public virtual void Update(TimeInfo time)
        {
            CheckScreenTimer(time);
        }

        public virtual void Render(TimeInfo time)
        {
            if (!rendered)
            {
                Logger.Info("Screen", $"Rendering Screen {Name}");
            }
            rendered = true;
            screen.Render();
        }

        public void SizeChanged(int width, int height)
        {
            if (screen != null)
            {
                //screen.Size = new Vector2(width, height);
                OnScreenSizeChanged(width, height);
                screen.PerformLayout();
            }
        }

        //private void Screen_Input(object sender, InputEvent e)
        //{
        //    switch (e.Class)
        //    {
        //        case IDCMPFlags.GadgetDown:
        //            OnGadgetDown(e.Gadget);
        //            break;
        //        case IDCMPFlags.GadgetUp:
        //            OnGadgetUp(e.Gadget);
        //            break;
        //        case IDCMPFlags.CloseWindow:
        //            OnCloseWindow(e.Window);
        //            break;
        //    }
        //}

        protected virtual void InitGUI(Screen screen)
        {

        }

        protected virtual void OnCloseWindow(Window window)
        {

        }

        protected virtual void OnGadgetDown(Gadget gadget)
        {

        }

        protected virtual void OnGadgetUp(Gadget gadget)
        {

        }

        protected virtual void OnScreenSizeChanged(int width, int height)
        {

        }

        protected virtual void OnKeyDown(Key keyData, Key keyCode, char code)
        {
            screen?.KeyDown(keyData, keyCode, code);
        }

        protected virtual void OnKeyUp(Key keyData, Key keyCode, char code)
        {
            screen?.KeyUp(keyData, keyCode, code);
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
            if (screen != null) return screen.MouseButtonDown((int)x, (int)y, button);
            return false;
        }

        protected virtual bool OnMouseUp(float x, float y, MouseButton button)
        {
            if (screen != null) return screen.MouseButtonUp((int)x, (int)y, button);
            return false;
        }
        protected virtual bool OnMouseMove(float x, float y, MouseButton button)
        {
            if (screen != null) return screen.MouseMove((int)x, (int)y);
            return false;
        }

        private void CheckScreenTimer(TimeInfo time)
        {
            var timeDiff = time.TotalGameTime - lastTick;
            if (timeDiff > tickDuration)
            {
                lastTick = time.TotalGameTime;
                screen.Timer();
            }
        }
        private void LinkInput()
        {
            engine.Input.OnMouseDown += Input_OnMouseDown;
            engine.Input.OnMouseUp += Input_OnMouseUp;
            engine.Input.OnMouseMove += Input_OnMouseMove;
            engine.Input.OnMouseWheel += Input_OnMouseWheel;
            engine.Input.OnKeyDown += Input_OnKeyDown;
            engine.Input.OnKeyUp += Input_OnKeyUp;
        }

        private void UnlinkInput()
        {
            engine.Input.OnMouseDown -= Input_OnMouseDown;
            engine.Input.OnMouseUp -= Input_OnMouseUp;
            engine.Input.OnMouseMove -= Input_OnMouseMove;
            engine.Input.OnMouseWheel -= Input_OnMouseWheel;
            engine.Input.OnKeyDown -= Input_OnKeyDown;
            engine.Input.OnKeyUp -= Input_OnKeyUp;
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
            engine.SetViewScale(scales[scaleIndex]);
        }

        private void ZoomOut()
        {
            scaleIndex--;
            if (scaleIndex < 0) scaleIndex = 0;
            engine.SetViewScale(scales[scaleIndex]);
        }

    }
}
