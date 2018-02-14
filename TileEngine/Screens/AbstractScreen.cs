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
    using GUI;
    using System.Collections.Generic;

    public abstract class AbstractScreen : NamedObject, IScreen
    {
        private bool rendered;
        private TimeSpan startTime;
        protected Engine engine;
        private static readonly float[] scales = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f };
        private int scaleIndex = 9;
        private List<Widget> widgets;

        public AbstractScreen(Engine engine, string name)
            : base(name)
        {
            this.engine = engine;
            widgets = new List<Widget>();
        }

        public IList<Widget> Widgets
        {
            get { return widgets; }
        }

        public void AddWidget(Widget w)
        {
            widgets.Add(w);
        }

        public void RemoveWidget(Widget w)
        {
            widgets.Remove(w);
        }

        public void ClearWidgets()
        {
            widgets.Clear();
        }
        public virtual void Show()
        {
            Logger.Info("Screen", $"Showing Screen {Name}");
            startTime = engine.GetCurrentTime();
            rendered = false;
            LinkInput();
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
            RenderWidgets();
        }

        protected void RenderWidgets()
        {
            foreach (var w in widgets)
            {
                if (w.Visible)
                {
                    w.Render(engine.Graphics);
                }
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
        protected virtual void OnMouseDown(float x, float y, MouseButton button)
        {
            foreach (var w in widgets)
            {
                w.CheckMouseDown((int)x, (int)y);
            }
        }

        protected virtual void OnMouseUp(float x, float y, MouseButton button)
        {
            Widget widget = null;
            foreach (var w in widgets)
            {
                if (w.CheckMouseUp((int)x, (int)y, ref widget))
                {
                    OnWidgetClick(widget);
                }
            }
        }
        protected virtual void OnMouseMove(float x, float y, MouseButton button)
        {
            foreach (var w in widgets)
            {
                w.CheckMouseHover((int)x, (int)y);
            }
        }

        protected virtual void OnWidgetClick(Widget widget)
        {
            Logger.Info("Widget", $"{widget} clicked");
        }

        private void LinkInput()
        {
            engine.Input.OnMouseDown += Input_OnMouseDown;
            engine.Input.OnMouseUp += Input_OnMouseUp;
            engine.Input.OnMouseMove += Input_OnMouseMove;
            engine.Input.OnMouseWheel += Input_OnMouseWheel;
        }

        private void UnlinkInput()
        {
            engine.Input.OnMouseDown -= Input_OnMouseDown;
            engine.Input.OnMouseUp -= Input_OnMouseUp;
            engine.Input.OnMouseMove -= Input_OnMouseMove;
            engine.Input.OnMouseWheel -= Input_OnMouseWheel;
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
