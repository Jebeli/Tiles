using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Fonts;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.XGUI
{
    public class Widget
    {
        private Widget parent;
        private List<Widget> children;
        private int id;
        private bool visible;
        private bool enabled;
        private bool mouseHover;
        private bool selected;
        private bool @checked;
        private bool active;

        private Vector2 position;
        private Vector2 size;
        private Vector2 fixedSize;
        private Layout layout;
        private ITheme theme;
        private Font font;
        private Font topazFont;
        private int borderTop;
        private int borderLeft;
        private int borderRight;
        private int borderBottom;

        public Widget(Widget parent)
        {
            visible = true;
            enabled = true;
            selected = false;
            mouseHover = false;
            children = new List<Widget>();
            parent?.AddChild(this);
        }

        public Widget Parent
        {
            get { return parent; }
        }

        public ITheme Theme
        {
            get { return theme; }
            set
            {
                if (theme != value)
                {
                    theme = value;
                    foreach (var child in children) child.Theme = value;
                }
            }
        }

        public Font Font
        {
            get
            {
                if (font != null) return font;
                if (parent != null) return parent.Font;
                return null;
            }
            set
            {
                font = value;
            }
        }

        public Font TopazFont
        {
            get
            {
                if (topazFont != null) return topazFont;
                if (parent != null) return parent.TopazFont;
                return null;
            }
            set
            {
                topazFont = value;
            }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool VisibleRecursive
        {
            get
            {
                bool result = true;
                Widget w = this;
                while (w != null)
                {
                    result &= w.Visible;
                    w = w.parent;
                }
                return result;
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool MouseHover
        {
            get { return mouseHover; }
            set { mouseHover = value; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public bool Checked
        {
            get { return @checked; }
            set { @checked = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 ClientPosition
        {
            get { return position + new Vector2(borderLeft, borderTop); }
        }

        public Vector2 ClientSize
        {
            get { return size - new Vector2(borderLeft + borderRight, borderTop + borderBottom); }
        }

        public Rect ClientBounds
        {
            get { return new Rect(ClientPosition, ClientSize); }
        }

        public int ClientLeft
        {
            get { return position.X + borderLeft; }
        }

        public int ClientTop
        {
            get { return position.Y + borderTop; }
        }

        public int ClientWidth
        {
            get { return size.X - borderLeft - borderRight; }
        }

        public int ClientHeight
        {
            get { return size.Y - borderTop - borderBottom; }
        }

        public Vector2 AbsolutePosition
        {
            get { return parent != null ? parent.AbsolutePosition + position : position; }
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public Rect Bounds
        {
            get { return new Rect(position, size); }
            set
            {
                position = value.Location;
                size = value.Size;
            }
        }

        public int Left
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public int Top
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public int Width
        {
            get { return size.X; }
            set { size.X = value; }
        }

        public int Height
        {
            get { return size.Y; }
            set { size.Y = value; }
        }

        public int Right
        {
            get { return position.X + size.X - 1; }
        }

        public int Bottom
        {
            get { return position.Y + size.Y - 1; }
        }

        public int BorderLeft
        {
            get { return borderLeft; }
            set { borderLeft = value; }
        }

        public int BorderRight
        {
            get { return borderRight; }
            set { borderRight = value; }
        }

        public int BorderTop
        {
            get { return borderTop; }
            set { borderTop = value; }
        }

        public int BorderBottom
        {
            get { return borderBottom; }
            set { borderBottom = value; }
        }

        public Vector2 FixedSize
        {
            get { return fixedSize; }
            set { fixedSize = value; }
        }

        public int FixedWidth
        {
            get { return fixedSize.X; }
            set { fixedSize.X = value; }
        }

        public int FixedHeight
        {
            get { return fixedSize.Y; }
            set { fixedSize.Y = value; }
        }

        public int CenterX
        {
            get { return position.X + size.X / 2; }
        }

        public int CenterY
        {
            get { return position.Y + size.Y / 2; }
        }

        public bool Contains(Vector2 p)
        {
            var d = p - position;
            return d.X >= 0 && d.Y >= 0 && d.X < size.X && d.Y < size.Y;
        }

        public Widget FindWidget(Vector2 p)
        {
            foreach (var child in children.Reverse<Widget>())
            {
                var d = p - position;
                if (child.Visible && child.Contains(d))
                {
                    return child.FindWidget(d);
                }
            }
            return Contains(p) ? this : null;
        }
        public int ChildCount
        {
            get { return children.Count; }
        }

        public int VisibleChildCount
        {
            get { return children.Count(x => x.Visible); }
        }

        public IEnumerable<Widget> Children
        {
            get { return children; }
        }
        public virtual void AddChild(int index, Widget widget)
        {
            children.Insert(index, widget);
            widget.parent = this;
            widget.Theme = theme;
        }

        public void AddChild(Widget widget)
        {
            AddChild(ChildCount, widget);
        }

        public void RemoveChild(int index)
        {
            RemoveChild(children[index]);
        }

        public void RemoveChild(Widget widget)
        {
            children.Remove(widget);
        }

        public Widget VisibleChildAt(int index)
        {
            return children.Where(x => x.Visible).ToList()[index];
        }
        public Widget ChildAt(int index)
        {
            return children[index];
        }

        public int ChildIndex(Widget widget)
        {
            return children.IndexOf(widget);
        }

        public Window Window
        {
            get
            {
                Widget widget = this;
                while (true)
                {
                    if (widget == null) return null;
                    if (widget is Window window) return window;
                    widget = widget.Parent;
                }
            }
        }

        public Screen Screen
        {
            get
            {
                Widget widget = this;
                while (true)
                {
                    if (widget == null) return null;
                    if (widget is Screen screen) return screen;
                    widget = widget.Parent;
                }
            }
        }
        public Layout Layout
        {
            get { return layout; }
            set { layout = value; }
        }

        protected internal static Vector2 GetValidSize(Vector2 pref, Vector2 fix)
        {
            return new Vector2(fix.X != 0 ? fix.X : pref.X, fix.Y != 0 ? fix.Y : pref.Y);
        }

        public void RequestFocus()
        {
            Screen?.UpdateFocus(this);
        }

        public void LooseFocus()
        {
            Screen?.UpdateFocus(null);
        }
        public void Invalidate()
        {
            Screen?.InvalidateWindow(Window);
        }

        public virtual void PerformLayout(IGraphics gfx)
        {
            if (layout != null)
            {
                layout.PerformLayout(gfx, this);
            }
            else
            {
                foreach (var c in children)
                {
                    c.Size = GetValidSize(c.GetPreferredSize(gfx), c.fixedSize);
                    c.PerformLayout(gfx);
                }
            }
        }

        public virtual Vector2 GetPreferredSize(IGraphics gfx)
        {
            if (layout != null) return layout.GetPreferredSize(gfx, this);
            return GetValidSize(size, fixedSize);
        }

        public virtual void Render(IGraphics gfx)
        {
            if (children.Count == 0) return;
            gfx.SaveState();
            gfx.Translate(position.X, position.Y);
            foreach (var child in children)
            {
                if (child.visible)
                    child.Render(gfx);

            }
            gfx.RestoreState();
        }

        public virtual bool KeyboardEvent(Key key)
        {
            return false;
        }

        public virtual bool ScrollEvent(Vector2 p, Vector2 rel)
        {
            foreach (var child in children.Reverse<Widget>())
            {
                if (!child.Visible) continue;
                if (child.Contains(p) && child.ScrollEvent(p - position, rel))
                {
                    return true;
                }
            }
            return false;
        }
        public virtual bool MouseDragEvent(Vector2 p, Vector2 rel, MouseButton button)
        {
            return false;
        }
        public virtual bool MouseMoveEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            foreach (var child in children.Reverse<Widget>())
            {
                if (child.Visible && child.Contains(p - position))
                {
                    if (child.MouseMoveEvent(p - position, button, ref widget))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool MouseButtonDownEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            foreach (var child in children.Reverse<Widget>())
            {
                if (child.Visible && child.Contains(p - position))
                {
                    if (child.MouseButtonDownEvent(p - position, button, ref widget))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool MouseButtonUpEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            foreach (var child in children.Reverse<Widget>())
            {
                if (child.Visible && child.Contains(p - position))
                {
                    if (child.MouseButtonUpEvent(p - position, button, ref widget))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool TimerEvent(Vector2 p, MouseButton button)
        {
            return false;
        }

    }
}
