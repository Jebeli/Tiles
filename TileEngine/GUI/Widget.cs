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

namespace TileEngine.GUI
{
    using Input;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Graphics;

    public abstract class Widget : IWidgetContainer
    {
        private int left;
        private int top;
        private int width;
        private int height;
        private Widget parent;
        private List<Widget> children;
        private bool visible;
        private bool enabled;
        private bool pressed;
        private bool hover;
        private bool clickable;
        private bool toggleSelect;
        private bool selected;
        private bool repeat;
        private NinePatch patch;
        private NinePatch patchHover;
        private NinePatch patchPressed;
        private double repeatMillis = 500;
        private object tag;
        private static Widget hoverWidget;
        private static Widget pressedWidget;
        private static TimeSpan widgetStartTime;

        public Widget()
            : this(null)
        {

        }
        public Widget(NinePatch patch)
            : this(patch, patch, patch)
        {

        }
        public Widget(NinePatch patch, NinePatch patchHover, NinePatch patchPressed)
        {
            children = new List<Widget>();
            visible = true;
            enabled = true;
            clickable = true;
            this.patch = patch;
            this.patchHover = patchHover;
            this.patchPressed = patchPressed;
        }

        public event EventHandler<EventArgs> Click;

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public NinePatch Patch
        {
            get { return patch; }
            set { patch = value; }
        }

        public NinePatch PatchHover
        {
            get { return patchHover; }
            set { patchHover = value; }
        }
        public NinePatch PatchPressed
        {
            get { return patchPressed; }
            set { patchPressed = value; }
        }

        public int Left
        {
            get { return left; }
            set
            {
                if (left != value)
                {
                    left = value;
                    BoundsChanged();
                }
            }
        }

        public int Top
        {
            get { return top; }
            set
            {
                if (top != value)
                {
                    top = value;
                    BoundsChanged();
                }
            }
        }

        public int Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    width = value;
                    BoundsChanged();
                }
            }
        }

        public int Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    BoundsChanged();
                }
            }
        }

        public void SetBounds(int left, int top, int width, int height)
        {
            if (this.left != left || this.top != top || this.width != width || this.height != height)
            {
                this.left = left;
                this.top = top;
                this.width = width;
                this.height = height;
                BoundsChanged();
            }
        }

        public void SetPosition(int left, int top)
        {
            if (this.left != left || this.top != top)
            {
                this.left = left;
                this.top = top;
                BoundsChanged();
            }

        }

        public void SetSize(int width, int height)
        {
            if (this.width != width || this.height != height)
            {
                this.width = width;
                this.height = height;
                BoundsChanged();
            }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool ToggleSelect
        {
            get { return toggleSelect; }
            set { toggleSelect = value; }
        }

        public bool Hover
        {
            get { return hover; }
        }

        public bool Pressed
        {
            get { return pressed; }
        }

        public bool Clickable
        {
            get { return clickable; }
            set { clickable = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public bool Repeat
        {
            get { return repeat; }
            set { repeat = value; }
        }

        public Widget Parent
        {
            get { return parent; }
        }
        public IList<Widget> Widgets
        {
            get { return children; }
        }
        public void AddWidget(Widget w)
        {
            children.Add(w);
            w.parent = this;
        }

        public void RemoveWidget(Widget w)
        {
            children.Remove(w);
            w.parent = null;
        }

        public void ClearWidgets()
        {
            children.Clear();
        }

        public bool Contains(int mouseX, int mouseY)
        {
            int x;
            int y;
            int width;
            int height;
            CalcBounds(out x, out y, out width, out height);
            return (mouseX >= x && mouseY >= y && mouseX < x + width && mouseY < y + height);
        }

        public static Widget HoverWidget
        {
            get { return hoverWidget; }
            set
            {
                if (hoverWidget != value)
                {
                    if (hoverWidget != null)
                    {
                        hoverWidget.hover = false;
                    }
                    hoverWidget = value;
                    if (hoverWidget != null)
                    {
                        hoverWidget.hover = true;
                    }
                }
            }
        }

        public static Widget PressedWidget
        {
            get { return pressedWidget; }
            set
            {
                if (pressedWidget != value)
                {
                    if (pressedWidget != null)
                    {
                        pressedWidget.pressed = false;
                    }
                    pressedWidget = value;
                    if (pressedWidget != null)
                    {
                        widgetStartTime = TimeSpan.MaxValue;
                        pressedWidget.pressed = true;
                    }
                }
            }
        }

        public static bool CheckMouseMoveWidget(Widget widget, int x, int y, MouseButton button)
        {
            if (widget != null)
            {
                widget.OnMouseMove(x, y, button);
                return true;
            }
            return false;
        }
        public static bool CheckMouseDownWidget(Widget widget, int x, int y, MouseButton button)
        {
            if (widget != null)
            {
                if (widget == pressedWidget)
                {
                    widget.OnMouseDown(x, y, button);
                    return true;
                }
            }
            return false;
        }

        public static bool CheckClickWidget(Widget widget, int x, int y, MouseButton button)
        {
            if (widget != null)
            {
                if (widget == pressedWidget)
                {
                    widget.OnMouseUp(x, y, button);
                    widget.OnClicked();
                    return true;
                }
            }
            return false;
        }

        public static bool CheckRepeatWidget(TimeSpan time, double repeatMillis = 250)
        {
            if (pressedWidget != null && pressedWidget.repeat)
            {
                if (widgetStartTime.Equals(TimeSpan.MaxValue))
                {
                    widgetStartTime = time;
                }
                else
                {
                    var diff = time - widgetStartTime;
                    double millis = diff.TotalMilliseconds;
                    if (millis > repeatMillis)
                    {
                        widgetStartTime = time;
                        pressedWidget.OnClicked();
                        return true;
                    }
                }
            }
            return false;
        }

        public static Widget FindWidget(IWidgetContainer container, int mouseX, int mouseY)
        {
            if (container != null)
            {
                foreach (var w in container.Widgets)
                {
                    if (w.visible && w.enabled && w.clickable)
                    {
                        if (w.Contains(mouseX, mouseY))
                        {
                            Widget widget = FindWidget(w, mouseX, mouseY);
                            if (widget != null)
                            {
                                return widget;
                            }
                            return w;
                        }
                    }
                }
            }
            return null;
        }

        public void Render(IGraphics graphics)
        {
            if (visible)
            {
                int x;
                int y;
                int width;
                int height;
                CalcBounds(out x, out y, out width, out height);
                Draw(graphics, x, y, width, height);
                foreach (var widget in children)
                {
                    if (widget.visible)
                    {
                        widget.Render(graphics);
                    }
                }
            }
        }

        protected bool DrawNinePatch(IGraphics graphics, int x, int y, int width, int height)
        {
            if (pressed || selected)
            {
                if (patchPressed != null)
                {
                    patchPressed.Draw(graphics, x, y, width, height);
                    return true;
                }
            }
            if (hover)
            {
                if (patchHover != null)
                {
                    patchHover.Draw(graphics, x, y, width, height);
                    return true;
                }
            }
            if (patch != null)
            {
                patch.Draw(graphics, x, y, width, height);
                return true;
            }
            return false;
        }

        protected abstract void Draw(IGraphics graphics, int x, int y, int width, int height);

        protected abstract void BoundsChanged();

        protected virtual void OnClicked()
        {
            if (toggleSelect)
            {
                selected = !selected;
            }
            Click?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnMouseUp(int x, int y, MouseButton button)
        {

        }

        protected virtual void OnMouseDown(int x, int y, MouseButton button)
        {

        }

        protected virtual void OnMouseMove(int x, int y, MouseButton button)
        {

        }
        protected void CalcBounds(out int x, out int y, out int width, out int height)
        {
            x = left;
            y = top;
            Widget p = parent;
            while (p != null)
            {
                x += p.left;
                y += p.top;
                p = p.parent;
            }
            width = this.width;
            height = this.height;
        }

    }
}
