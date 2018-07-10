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
    using TileEngine.Graphics;
    using TileEngine.Input;
    using TileEngine.Logging;

    public class Window : Gadget
    {
        private bool dragging;
        private bool sizing;
        private bool thinBorder;
        private bool closeGadget;
        private ButtonGadget closeButton;
        private bool sizeGadget;
        private ButtonGadget sizeButton;
        private bool sizeBRight;
        private bool sizeBBottom;
        private bool depthGadget;
        private ButtonGadget depthButton;


        public Window(Screen screen, string title = "", Orientation orientation = Orientation.Vertical)
            : base(screen, title)
        {
            Clipped = true;
            layout = new BoxLayout(orientation, Alignment.Fill, 10, 10);
            CalcBorders();
            InitBorderPanels();
        }

        public event EventHandler<EventArgs> WindowClose;
        public EventHandler<EventArgs> WindowCloseEvent { set { WindowClose += value; } }

        protected virtual void OnWindowClose()
        {
            WindowClose?.Invoke(this, EventArgs.Empty);
        }


        public override bool Borderless
        {
            get { return base.Borderless; }
            set
            {
                if (base.Borderless != value)
                {
                    base.Borderless = value;
                    CalcBorders();
                }
            }
        }

        public bool ThinBorder
        {
            get { return thinBorder; }
            set
            {
                if (thinBorder != value)
                {
                    thinBorder = value;
                    CalcBorders();
                }
            }
        }

        public bool CloseGadget
        {
            get { return closeGadget; }
            set
            {
                if (closeGadget != value)
                {
                    closeGadget = value;
                    CalcBorders();
                }
            }
        }

        public bool SizeGadget
        {
            get { return sizeGadget; }
            set
            {
                if (sizeGadget != value)
                {
                    sizeGadget = value;
                    CalcBorders();
                }
            }
        }

        public bool SizeBRight
        {
            get { return sizeBRight; }
            set
            {
                if (sizeBRight != value)
                {
                    sizeBRight = value;
                    CalcBorders();
                }
            }
        }

        public bool SizeBBottom
        {
            get { return sizeBBottom; }
            set
            {
                if (sizeBBottom != value)
                {
                    sizeBBottom = value;
                    CalcBorders();
                }
            }
        }

        public bool DepthGadget
        {
            get { return depthGadget; }
            set { depthGadget = value; }
        }

        private void CalcBorders()
        {
            if (Borderless)
            {
                BorderLeft = 0;
                BorderRight = 0;
                BorderBottom = 0;
                BorderTop = 0;
            }
            else
            {
                if (thinBorder)
                {
                    BorderLeft = 2;
                    BorderRight = CalcBorderRight();
                    BorderBottom = CalcBorderBottom();
                    BorderTop = CalcBorderTop();
                }
                else
                {
                    BorderLeft = 4;
                    BorderRight = CalcBorderRight();
                    BorderBottom = CalcBorderBottom();
                    BorderTop = CalcBorderTop();
                }
            }
        }

        private int CalcBorderRight()
        {
            if (sizeGadget)
            {
                if (sizeBRight) { return 24; }
            }
            if (Borderless) return 0;
            if (thinBorder) return 2;
            return 4;
        }

        private int CalcBorderTop()
        {
            if (!string.IsNullOrEmpty(Label)) return 24;
            if (closeGadget) return 24;
            if (depthGadget) return 24;
            if (Borderless) return 0;
            if (thinBorder) return 2;
            return 4;
        }

        private int CalcBorderBottom()
        {
            if (sizeGadget)
            {
                if (sizeBBottom) { return 24; }
                if (!sizeBRight) { return 24; }
            }
            if (Borderless) return 0;
            if (thinBorder) return 2;
            return 4;
        }

        private void HideBorder()
        {
            closeButton.Visible = false;
            sizeButton.Visible = false;
            depthButton.Visible = false;
            RemChild(closeButton);
            RemChild(sizeButton);
            RemChild(depthButton);
        }

        private void ShowBorder()
        {
            if (closeGadget)
            {
                AddChild(0, closeButton);
                closeButton.Visible = closeGadget;
                closeButton.Size = new Point(25, 25);
                closeButton.Position = new Point(0, 0);
            }
            if (sizeGadget)
            {
                AddChild(0, sizeButton);
                sizeButton.Visible = sizeGadget;
                sizeButton.Size = new Point(25, 25);
                sizeButton.Position = new Point(Width - 25, Height - 25);
            }
            if (depthGadget)
            {
                AddChild(0, depthButton);
                depthButton.Visible = depthGadget;
                depthButton.Size = new Point(25, 25);
                depthButton.Position = new Point(Width - 25, 0);
            }
        }

        private void InitBorderPanels()
        {
            closeButton = new ButtonGadget(this, theme.CloseIcon);
            closeButton.FixedSize = new Point(25, 25);
            closeButton.TransparentBackground = true;
            closeButton.Visible = closeGadget;
            closeButton.Borderless = true;
            closeButton.GadgetUp += (o, i) => { OnWindowClose(); };

            sizeButton = new ButtonGadget(this, theme.SizeIcon);
            sizeButton.FixedSize = new Point(25, 25);
            sizeButton.TransparentBackground = true;
            sizeButton.Visible = sizeGadget;
            sizeButton.Borderless = true;
            sizeButton.GadgetDown += SizeButton_GadgetDown;
            sizeButton.GadgetUp += SizeButton_GadgetUp;

            depthButton = new ButtonGadget(this, theme.DepthIcon);
            depthButton.FixedSize = new Point(25, 25);
            depthButton.TransparentBackground = true;
            depthButton.Visible = depthGadget;
            depthButton.Borderless = true;
            depthButton.GadgetUp += (o, i) => { ToggleDepth(); };

        }

        private void SizeButton_GadgetUp(object sender, EventArgs e)
        {
            sizing = false;
        }

        private void SizeButton_GadgetDown(object sender, EventArgs e)
        {
            sizing = true;
        }

        public override Point GetPreferredSize(IGraphics gfx)
        {
            HideBorder();
            Point result = base.GetPreferredSize(gfx);
            int minX = gfx.MeasureTextWidth(Font, Label);
            if (closeGadget) minX += 25;
            if (depthGadget) minX += 25;
            result.X = Math.Max(result.X, minX);
            ShowBorder();
            return result;
        }

        public void ToggleDepth()
        {
            Screen?.WindowToggleDepth(this);
        }

        public void Invalidate()
        {
            Screen?.InvalidateWindow(this);
        }

        public void CloseWindow()
        {
            Screen?.CloseWindow(this);
        }

        internal void ClearDraggingAndSizing()
        {
            dragging = false;
            sizing = false;
        }

        public override void PerformLayout(IGraphics gfx)
        {
            HideBorder();
            base.PerformLayout(gfx);
            ShowBorder();
        }

        public void Move(Point rel)
        {
            Screen.MoveWindow(this, rel);
        }

        public override bool HandleMouseDrag(Point p, Point rel, MouseButton button)
        {
            if (Enabled && dragging && button == MouseButton.Left)
            {
                Move(rel);
                return true;
            }
            else if (Enabled && sizing && button == MouseButton.Left)
            {
                Size += rel;
                FixedSize = Size;
                Invalidate();
                return true;
            }
            return false;
        }

        protected override void HandleSelectDown(Point p)
        {
            dragging = false;
            sizing = false;
            base.HandleSelectDown(p);
            if ((p.Y >= 0) && (p.Y < BorderTop))
            {
                dragging = true;
            }
        }

        protected override void HandleSelectUp(Point p)
        {
            dragging = false;
            sizing = false;
            base.HandleSelectUp(p);
        }

        //public override bool HandleMouseButtonDown(Vector2 p, MouseButton button)
        //{
        //    dragging = false;
        //    sizing = false;
        //    if (base.HandleMouseButtonDown(p, button)) return true;
        //    if (Enabled && (button == MouseButton.Left))
        //    {
        //        if ((p.Y - Position.Y) < BorderTop)
        //        {
        //            dragging = true;
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public override bool HandleMouseButtonUp(Vector2 p, MouseButton button)
        //{
        //    dragging = false;
        //    sizing = false;
        //    if (base.HandleMouseButtonUp(p, button)) return true;
        //    if (Enabled && (button == MouseButton.Left))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderWindow(gfx, this);
        }

        public override string ToString()
        {
            return $"Window {Label}";
        }


    }
}
