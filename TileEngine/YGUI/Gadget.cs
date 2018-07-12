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
    using TileEngine.Fonts;
    using TileEngine.Graphics;
    using TileEngine.Input;
    using TileEngine.Logging;

    public class Gadget
    {
        private static bool showBounds = false;

        private Gadget parent;
        private List<Gadget> children;
        private Point position;
        private Point size;
        private Point fixedSize;
        private Point minSize;
        private bool transparentBackground;
        private bool visible;
        private bool hover;
        private bool mouseSelected;
        private bool selected;
        private bool focused;
        private bool enabled;
        private bool sticky;
        private bool borderless;
        private bool relVerify;
        private bool clipped;
        protected Layout layout;
        protected ITheme theme;
        private Font font;
        private int borderTop;
        private int borderLeft;
        private int borderRight;
        private int borderBottom;
        private string label;
        private Icons icon;
        private int id;
        private object tag;
        private int updateCount;
        private string tooltipText;
        private Point tooltipPos;
        private bool showTooltip;


        public Gadget(Gadget parent, string label = "", Icons icon = Icons.NONE)
        {
            visible = true;
            enabled = true;
            relVerify = true;
            this.label = label;
            this.icon = icon;
            children = new List<Gadget>();
            if (parent != null)
            {
                parent.AddChild(this);
            }
        }

        public event EventHandler<EventArgs> GadgetDown;
        public EventHandler<EventArgs> GadgetDownEvent { set { GadgetDown += value; } }

        public event EventHandler<EventArgs> GadgetUp;
        public EventHandler<EventArgs> GadgetUpEvent { set { GadgetUp += value; } }

        protected virtual void OnGadgetDown()
        {
            GadgetDown?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnGadgetUp()
        {
            GadgetUp?.Invoke(this, EventArgs.Empty);
        }

        public void BeginUpdate()
        {
            updateCount++;
        }

        public void EndUpdate()
        {
            updateCount--;
            if (updateCount <= 0)
            {
                OnEndUpdate();
            }
        }

        public bool IsUpdating
        {
            get { return updateCount > 0; }
        }

        protected virtual void OnEndUpdate()
        {            
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public bool TransparentBackground
        {
            get { return transparentBackground; }
            set { transparentBackground = value; }
        }

        public bool Clipped
        {
            get { return clipped; }
            set { clipped = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool Hover
        {
            get { return hover; }
            set { hover = value; }
        }

        public bool MouseSelected
        {
            get { return mouseSelected; }
            set { mouseSelected = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public bool DrawSelected
        {
            get { return mouseSelected || selected; }
        }

        public bool Focused
        {
            get { return focused; }
            set { focused = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool Sticky
        {
            get { return sticky; }
            set { sticky = value; }
        }

        public virtual bool Borderless
        {
            get { return borderless; }
            set { borderless = value; }
        }

        public bool RelVerify
        {
            get { return relVerify; }
            set { relVerify = value; }
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

        public IList<Gadget> Children
        {
            get { return children; }
        }

        public Gadget Parent
        {
            get { return parent; }
        }

        public Layout Layout
        {
            get { return layout; }
            set { layout = value; }
        }

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        public string TooltipText
        {
            get { return tooltipText; }
            set { tooltipText = value; }
        }

        public Point TooltipPos
        {
            get { return tooltipPos; }
            set { tooltipPos = value; }
        }

        public bool ShowTooltip
        {
            get { return showTooltip; }
            set { showTooltip = value; }
        }

        public void SetTooltip(string text, Point pos)
        {
            tooltipText = text;
            tooltipPos = pos;
            showTooltip = !string.IsNullOrEmpty(tooltipText);
        }

        public void HideTooltip()
        {
            showTooltip = false;
        }

        public Icons Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public Window Window
        {
            get
            {
                Gadget gadget = this;
                while (true)
                {
                    if (gadget == null) return null;
                    if (gadget is Window window) return window;
                    gadget = gadget.parent;
                }
            }
        }

        public Screen Screen
        {
            get
            {
                Gadget gadget = this;
                while (true)
                {
                    if (gadget == null) return null;
                    if (gadget is Screen screen) return screen;
                    gadget = gadget.parent;
                }
            }
        }

        public Rect Bounds
        {
            get { return new Rect(position, size); }
        }

        public Rect ClientBounds
        {
            get { return new Rect(LeftEdge + BorderLeft, TopEdge + BorderTop, Width - BorderLeft - BorderRight, Height - BorderTop - BorderBottom); }
        }

        public Rect ClipRect
        {
            get { return new Rect(BorderLeft, BorderTop, Width - BorderLeft - BorderRight, Height - BorderTop - BorderBottom); }
        }

        public Point AbsolutePosition
        {
            get { return parent != null ? parent.AbsolutePosition + position : position; }
        }

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public Point ScreenToGadget(Point p)
        {
            return p - AbsolutePosition;
        }

        public Point GadgetToScreen(Point p)
        {
            return p + AbsolutePosition;
        }

        public Point Size
        {
            get { return size; }
            set
            {
                if (value.X < minSize.X) value.X = minSize.X;
                if (value.Y < minSize.Y) value.Y = minSize.Y;
                size = value;
            }
        }

        public Point FixedSize
        {
            get { return fixedSize; }
            set { fixedSize = value; }
        }

        public Point MinSize
        {
            get { return minSize; }
            set { minSize = value; }
        }

        public int LeftEdge
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public int TopEdge
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public int Width
        {
            get { return size.X; }
            set
            {
                if (value < minSize.X) value = minSize.X;
                size.X = value;
            }
        }

        public int Height
        {
            get { return size.Y; }
            set
            {
                if (value < minSize.Y) value = minSize.Y;
                size.Y = value;
            }
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

        public bool Contains(Point p)
        {
            
            var d = p - position;
            return d.X >= 0 && d.Y >= 0 && d.X < size.X && d.Y < size.Y;
        }

        public bool ContainsRel(Point p)
        {
            return p.X >= 0 && p.Y >= 0 && p.X < size.X && p.Y < size.Y;
        }

        public Gadget FindGadget(Point pos)
        {            
            foreach (var g in children)
            {
                if (g.visible && g.Contains(pos - position))
                {
                    return g.FindGadget(pos - position);
                }
            }
            if (visible && Contains(pos))
            {
                return this;
            }
            return null;
        }

        public void AddChild(Gadget gadget)
        {
            AddChild(children.Count, gadget);
        }

        public virtual void AddChild(int index, Gadget gadget)
        {
            children.Insert(index, gadget);
            gadget.parent = this;
            gadget.theme = theme;
        }

        public void RemChild(Gadget gadget)
        {
            if (children.Remove(gadget))
            {
                gadget.parent = null;
            }
        }

        public void Clear()
        {
            while (children.Count > 0)
            {
                RemChild(children[0]);
            }
            children.Clear();
        }

        public void UnFocus()
        {
            if (focused)
            {
                Screen.SetFocusedGadget(null);
            }
        }

        public void Focus()
        {
            if (!focused)
            {
                Screen.SetFocusedGadget(this);
            }
        }

        internal void InitMinSize()
        {
            if (minSize.IsEmpty)
            {
                minSize = size;
            }
        }

        public virtual void HandleKeyDown(Key keyData, Key keyCode, char code)
        {

        }

        public virtual void HandleKeyUp(Key keyData, Key keyCode, char code)
        {

        }

        public virtual void HandleTimer(Point p, MouseButton button)
        {

        }

        public virtual bool HandleMouseDrag(Point p, Point rel, MouseButton button)
        {
            return false;
        }

        protected virtual void HandleSelectDown(Point p)
        {
            OnGadgetDown();

        }

        protected virtual void HandleSelectUp(Point p)
        {
            OnGadgetUp();
        }

        protected virtual void HandleSelectMove(Point p)
        {

        }

        public bool HandleMouseButtonDown(Point p, MouseButton button)
        {
            Logger.Detail("GUI", $"Mouse Down @{p} {this}");
            if (visible && enabled && button == MouseButton.Left && ContainsRel(p))
            {
                Logger.Info("GUI", $"Select Down @{p} {this}");
                HandleSelectDown(p);
                return true;
            }
            return false;
        }

        public bool HandleMouseButtonUp(Point p, MouseButton button)
        {
            Logger.Detail("GUI", $"Mouse Up @{p} {this}");
            if (visible && enabled && button == MouseButton.Left)
            {
                Logger.Info("GUI", $"Select Up @{p} {this}");
                HandleSelectUp(p);
                return true;
            }
            return false;
        }

        public bool HandleMouseMove(Point p, MouseButton button)
        {
            Logger.Detail("GUI", $"Mouse Move @{p} {this}");
            if (visible && enabled && button == MouseButton.Left)
            {
                Logger.Detail("GUI", $"Select Move @{p} {this}");
                HandleSelectMove(p);
                return true;
            }
            return false;
        }

        public virtual Point GetPreferredSize(IGraphics gfx)
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
                    if (c.Visible)
                    {
                        c.Size = Layout.GetValidSize(c.GetPreferredSize(gfx), c.FixedSize);
                        c.PerformLayout(gfx);
                    }
                }
            }
        }

        public virtual void Render(IGraphics gfx)
        {
            RenderGadget(gfx);
            if (showTooltip)
            {
                RenderTooltip(gfx);
            }
            if (showBounds)
            {
                gfx.DrawRectangle(position.X - 1, position.Y - 1, size.X + 2, size.Y + 2, new Color(255, 0, 0));
            }
            if (children.Count == 0) return;
            gfx.SaveState();
            gfx.Translate(position.X, position.Y);
            if (clipped)
            {
                gfx.SetClip(0, 0, size.X, size.Y);
            }
            foreach (var child in children)
            {
                if (child.visible)
                {
                    child.Render(gfx);
                }
            }
            gfx.RestoreState();
        }

        protected virtual void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

        protected virtual void RenderTooltip(IGraphics gfx)
        {
            theme.RenderTooltip(gfx, this);
        }

        public override string ToString()
        {
            return $"Gaget {label}";
        }
    }
}
