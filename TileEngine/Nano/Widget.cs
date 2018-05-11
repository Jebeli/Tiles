namespace TileEngine.Nano
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;
    using TileEngine.Graphics;
    using Input;
    using TileEngine.Fonts;

    public class Widget
    {

        private static bool showWidgetBounds = false;

        protected Widget parent;
        protected Theme theme;
        protected Layout layout;
        protected string id;
        protected Vector2 pos;
        protected Vector2 size;
        protected Vector2 fixedSize;
        protected List<Widget> children;
        protected bool visible;
        protected bool enabled;
        protected bool focused;
        protected bool mouseFocused;
        protected string tooltip;
        protected int fontSize;
        protected float iconExtraScale;
        protected Font font;

        public Widget(Widget parent)
        {
            visible = true;
            enabled = true;
            focused = false;
            mouseFocused = false;
            tooltip = "";
            fontSize = -1;
            iconExtraScale = 1.0f;
            children = new List<Widget>();
            if (parent != null)
            {
                parent.AddChild(this);
            }
        }

        public static bool ShowWidgetBounds
        {
            get { return showWidgetBounds; }
            set { showWidgetBounds = value; }
        }

        public Widget Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Layout Layout
        {
            get { return layout; }
            set { layout = value; }
        }

        public virtual Theme Theme
        {
            get { return theme; }
            set
            {
                if (theme == value) return;
                theme = value;
                foreach (var child in children) child.Theme = value;
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
            set { font = value; }
        }

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public Vector2 AbsolutePosition
        {
            get { return GetAbsolutePosition(); }
        }

        private Vector2 GetAbsolutePosition()
        {
            return parent != null ? parent.AbsolutePosition + pos : pos;
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public int Left
        {
            get { return pos.X; }
            set { pos.X = value; }
        }

        public int Top
        {
            get { return pos.Y; }
            set { pos.Y = value; }
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

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool VisibleRecursive
        {
            get { return GetVisibleRecursive(); }
        }

        private bool GetVisibleRecursive()
        {
            bool result = true;
            Widget w = this;
            while (w != null)
            {
                result &= w.Visible;
                w = w.Parent;
            }
            return result;
        }

        public int ChildCount
        {
            get { return children.Count; }
        }

        public IList<Widget> Children
        {
            get { return children; }
        }

        public virtual void AddChild(int index, Widget widget)
        {
            children.Insert(index, widget);
            widget.Parent = this;
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
                    //if (widget == null) throw new InvalidOperationException("Widget:internal error (could not find parent window)");
                    if (widget == null) return null;
                    Window window = widget as Window;
                    if (window != null) return window;
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
                    //if (widget == null) throw new InvalidOperationException("Widget:internal error (could not find parent screen)");
                    if (widget == null) return null;
                    Screen screen = widget as Screen;
                    if (screen != null) return screen;
                    widget = widget.Parent;
                }
            }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool Focused
        {
            get { return focused; }
            set { focused = value; }
        }

        public void RequestFocus()
        {
            Screen.UpdateFocus(this);
        }

        public string ToolTip
        {
            get { return tooltip; }
            set { tooltip = value; }
        }

        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        public bool HasFontSize
        {
            get { return fontSize > 0; }
        }

        public float IconExtraScale
        {
            get { return iconExtraScale; }
            set { iconExtraScale = value; }
        }

        public bool Contains(Vector2 p)
        {
            var d = p - pos;
            return d.X >= 0 && d.Y >= 0 && d.X < size.X && d.Y < size.Y;
        }

        public Widget FindWidget(Vector2 p)
        {
            foreach (var child in children.Reverse<Widget>())
            {
                if (child.Visible && child.Contains(p - pos))
                {
                    return child.FindWidget(p - pos);
                }
            }
            return Contains(p) ? this : null;
        }

        public virtual Vector2 GetPreferredSize(IGraphics gfx)
        {
            if (layout != null) return layout.GetPreferredSize(gfx, this);
            return size;
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
                    Vector2 pref = c.GetPreferredSize(gfx);
                    Vector2 fix = c.FixedSize;
                    c.Size = new Vector2(fix.X != 0 ? fix.X : pref.X, fix.Y != 0 ? fix.Y : pref.Y);
                    c.PerformLayout(gfx);
                }
            }
        }

        public virtual void Draw(IGraphics gfx)
        {
            if (showWidgetBounds)
            {
                gfx.DrawRectangle(pos.X, pos.Y, size.X, size.Y, new Color(255, 0, 0));
            }
            if (children.Count == 0) return;
            gfx.SaveState();
            gfx.Translate(pos.X, pos.Y);
            foreach (var child in children)
            {
                if (child.Visible)
                {
                    child.Draw(gfx);
                }
            }
            gfx.RestoreState();
        }

        public virtual bool ScrollEvent(Vector2 p, Vector2 rel)
        {
            foreach (var child in children.Reverse<Widget>())
            {
                if (!child.Visible) continue;
                if (child.Contains(p) && child.ScrollEvent(p - pos, rel))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool FocusEvent(bool focused)
        {
            this.focused = focused;
            return false;
        }

        public virtual bool MouseEnterEvent(Vector2 i, bool enter)
        {
            mouseFocused = enter;
            return false;
        }
        public virtual bool MouseMotionEvent(Vector2 p, Vector2 rel, MouseButton button)
        {
            foreach (var child in children.Reverse<Widget>())
            {
                if (!child.Visible) continue;
                bool contained = child.Contains(p - pos);
                bool prevContained = child.Contains(p - pos - rel);
                if (contained != prevContained)
                {
                    child.MouseEnterEvent(p, contained);
                }
                if ((contained || prevContained) && child.MouseMotionEvent(p - pos, rel, button))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool MouseButtonEvent(Vector2 p, MouseButton button, bool down)
        {
            foreach (var child in children.Reverse<Widget>())
            {
                if (child.Visible && child.Contains(p - pos) &&
                    child.MouseButtonEvent(p - pos, button, down))
                    return true;
            }
            if (button == MouseButton.Left && down && !focused)
                RequestFocus();
            return false;
        }

        public virtual bool MouseDragEvent(Vector2 p, Vector2 rel, MouseButton button)
        {
            return false;
        }
    }
}
