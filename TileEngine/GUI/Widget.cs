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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Graphics;

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


        public Widget()
        {
            children = new List<Widget>();
            visible = true;
            enabled = true;
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

        public bool Hover
        {
            get { return hover; }
        }

        public bool Pressed
        {
            get { return pressed; }
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

        public bool CheckMouseUp(int x, int y, ref Widget widget)
        {
            bool wasPressed = pressed;
            if (Visible && Enabled)
            {
                foreach (var w in children)
                {
                    if (w.CheckMouseUp(x, y, ref widget))
                    {
                        pressed = false;
                    }
                }
                if (Visible && Enabled && Contains(x, y))
                {
                    if (pressed)
                    {
                        widget = this;
                        pressed = false;
                    }
                }
            }
            pressed = false;
            return !pressed && wasPressed;
        }

        public bool CheckMouseDown(int x, int y)
        {
            if (Visible && Enabled)
            {
                foreach (var w in children)
                {
                    if (w.CheckMouseDown(x, y))
                    {
                        pressed = true;
                    }
                }
                if (Visible && Enabled && Contains(x, y))
                {
                    pressed = true;
                }
                else
                {
                    pressed = false;
                }
            }
            else
            {
                pressed = false;
            }
            return pressed;
        }

        public  bool CheckMouseHover(int x, int y)
        {
            if (Visible && Enabled)
            {
                foreach (var w in children)
                {
                    if (w.CheckMouseHover(x, y))
                    {
                        hover = true;                        
                    }
                }
                if (Visible && Enabled && Contains(x, y))
                {
                    hover = true;
                }
                else
                {
                    hover = false;
                }
            }
            else
            {
                hover = false;
            }
            return hover;
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

        protected abstract void Draw(IGraphics graphics, int x, int y, int width, int height);

        protected abstract void BoundsChanged();

        private void CalcBounds(out int x, out int y, out int width, out int height)
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
