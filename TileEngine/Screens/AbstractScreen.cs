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

    public abstract class AbstractScreen : NamedObject, IScreen
    {
        private bool rendered;
        private TimeSpan startTime;
        protected Engine engine;
        private static readonly float[] scales = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f };
        private int scaleIndex = 9;
        private List<Window> windows;
        private Window activeWindow;

        public AbstractScreen(Engine engine, string name)
            : base(name)
        {
            this.engine = engine;
            windows = new List<Window>();
        }
        public IList<Window> Windows
        {
            get { return windows; }
        }

        public Window ActiveWindow
        {
            get { return activeWindow; }
            set { activeWindow = value; }
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
            }
        }

        protected virtual void OnGadgetClick(Gadget gadget)
        {
            Logger.Info("Screen", $"Gadget {gadget} Selected");
        }

        protected virtual void OnAutoRequest(int gadNum)
        {
            Logger.Info("Screen", $"Requester Gadget {gadNum} Selected");
        }

        protected static ValueTuple<WATags, object> Tag(WATags tag, object value)
        {
            return tag.T(value);
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
