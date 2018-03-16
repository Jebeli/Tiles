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
    using TileEngine.Screens;

    public class Window
    {
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private int zoomLeftEdge;
        private int zoomTopEdge;
        private int zoomWidth;
        private int zoomHeight;
        private int normalLeftEdge;
        private int normalTopEdge;
        private int normalWidth;
        private int normalHeight;
        private int borderLeft;
        private int borderTop;
        private int borderRight;
        private int borderBottom;
        private WindowFlags flags;
        private MoreWindowFlags moreFlags;
        private IDCMPFlags idcmpflags;
        private string title;
        private List<Gadget> gadgets = new List<Gadget>();
        private List<Requester> requesters = new List<Requester>();
        private int minWidth;
        private int minHeight;
        private int maxWidth;
        private int maxHeight;
        private bool zoomed;
        private IScreen screen;
        private Texture bitmap;
        private Engine engine;
        private bool valid;
        private int opacity;
        private Color bgColor;
        private Color fgColor;
        public Window(Engine engine, NewWindow nw)
        {
            this.engine = engine;
            opacity = 255;
            HoverOpacity = 255;
            screen = nw.Screen;
            leftEdge = nw.LeftEdge;
            topEdge = nw.TopEdge;
            width = nw.Width;
            height = nw.Height;
            minWidth = nw.MinWidth;
            maxWidth = nw.MaxWidth;
            minHeight = nw.MinHeight;
            maxHeight = nw.MaxHeight;
            zoomLeftEdge = leftEdge;
            zoomTopEdge = topEdge;
            zoomWidth = nw.MaxWidth;
            zoomHeight = nw.MaxHeight;
            idcmpflags = nw.IDCMPFlags;
            flags = nw.Flags;
            title = nw.Title;
            if (!HasFlag(WindowFlags.WFLG_BORDERLESS))
            {
                borderLeft = 4;
                borderTop = 4;
                borderRight = 4;
                borderBottom = 4;
                if (HasFlag(WindowFlags.WFLG_SIZEGADGET))
                {
                    if (HasFlag(WindowFlags.WFLG_SIZEBRIGHT))
                    {
                        if (borderRight < 18)
                            borderRight = 18;
                    }
                    if (HasFlag(WindowFlags.WFLG_SIZEBBOTTOM))
                    {
                        if (borderBottom < 18)
                            borderBottom = 18;
                    }
                }
                if (HasFlag(WindowFlags.WFLG_CLOSEGADGET | WindowFlags.WFLG_DRAGBAR | WindowFlags.WFLG_DEPTHGADGET | WindowFlags.WFLG_HASZOOM))
                {
                    borderTop += 16;
                }
            }
            InitBitmap();
        }

        public int Opacity
        {
            get { return opacity; }
            set { opacity = value; }
        }

        public int HoverOpacity { get; set; }

        public int RenderTransparency
        {
            get
            {
                return (moreFlags & MoreWindowFlags.WFLG_HOVER) == MoreWindowFlags.WFLG_HOVER ? 255 - HoverOpacity : 255 - opacity;
            }
        }

        public Texture Bitmap
        {
            get { return bitmap; }
        }

        public Color BgColor
        {
            get { return bgColor; }
            set { bgColor = value; }
        }

        public Color FgColor
        {
            get { return fgColor; }
            set { fgColor = value; }
        }

        internal bool IsTopMostWindow
        {
            get
            {
                if (screen != null)
                {
                    return screen.Windows.LastOrDefault() == this;
                }
                return false;
            }
        }

        internal void Close()
        {
            if (screen != null) screen.RemoveWindow(this);
            bitmap?.Dispose();
            bitmap = null;
            valid = true;
        }
        private void InitBitmap()
        {
            bitmap?.Dispose();
            bitmap = engine.Graphics.CreateTexture("Window_" + title, width, height);
            Invalidate();
        }

        private void CheckBitmap()
        {
            if (bitmap == null || bitmap.Width != width || bitmap.Height != height)
            {
                InitBitmap();
            }
        }

        public bool HasFlag(WindowFlags flg)
        {
            return (flags & flg) != 0;
        }

        public IScreen Screen
        {
            get { return screen; }
        }

        public int LeftEdge
        {
            get { return leftEdge; }
            internal set
            {
                if (value > engine.Graphics.ViewWidth - width) value = engine.Graphics.ViewWidth - width;
                if (value < 0) value = 0;
                leftEdge = value;
            }
        }

        public int TopEdge
        {
            get { return topEdge; }
            internal set
            {
                if (value > engine.Graphics.ViewHeight - height) value = engine.Graphics.ViewHeight - height;
                if (value < 0) value = 0;
                topEdge = value;
            }
        }

        public int Width
        {
            get { return width; }
            internal set
            {
                if (width != value)
                {
                    if (value < minWidth) value = minWidth;
                    if (value > maxWidth) value = maxWidth;
                    width = value;
                    CheckBitmap();
                }
            }
        }

        public int Height
        {
            get { return height; }
            internal set
            {
                if (height != value)
                {
                    if (value < minHeight) value = minHeight;
                    if (value > maxHeight) value = maxHeight;
                    height = value;
                    CheckBitmap();
                }
            }
        }

        public WindowFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public MoreWindowFlags MoreFlags
        {
            get { return moreFlags; }
            set { moreFlags = value; }
        }

        public IDCMPFlags IDCMPFlags
        {
            get { return idcmpflags; }
            set { idcmpflags = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public IEnumerable<Gadget> Gadgets
        {
            get { return gadgets; }
        }

        public IEnumerable<Requester> Requesters
        {
            get { return requesters; }
        }

        public int MinWidth
        {
            get { return minWidth; }
            internal set
            {
                if (value == 0)
                {
                    minWidth = width;
                }
                else if (value <= width)
                {
                    minWidth = value;
                }
            }
        }
        public int MinHeight
        {
            get { return minHeight; }
            internal set
            {
                if (value == 0)
                {
                    minHeight = height;
                }
                else if (value <= height)
                {
                    minHeight = value;
                }
            }
        }
        public int MaxWidth
        {
            get { return maxWidth; }
            internal set
            {
                if (value == 0)
                {
                    maxWidth = width;
                }
                else if (value >= width)
                {
                    maxWidth = value;
                }
            }
        }
        public int MaxHeight
        {
            get { return maxHeight; }
            internal set
            {
                if (value == 0)
                {
                    maxHeight = height;
                }
                else if (value >= height)
                {
                    maxHeight = value;
                }
            }
        }

        public int BorderLeft
        {
            get { return borderLeft; }
        }

        public int BorderTop
        {
            get { return borderTop; }
        }

        public int BorderRight
        {
            get { return borderRight; }
        }

        public int BorderBottom
        {
            get { return borderBottom; }
        }

        public int InnerWidth
        {
            get { return Width - BorderLeft - BorderRight; }
        }

        public int InnerHeight
        {
            get { return Height - BorderTop - BorderBottom; }
        }

        public bool Valid
        {
            get { return valid; }
        }

        public void Validate()
        {
            valid = true;
        }

        public void Invalidate()
        {
            valid = false;
        }

        internal void SetWindowBox(int left, int top, int width, int height)
        {
            leftEdge = left;
            topEdge = top;
            Width = width;
            Height = height;
        }

        internal void SetLimits(int minWidth, int minHeight, int maxWidth, int maxHeight)
        {
            if (minWidth > 0)
            {
                this.minWidth = minWidth;
            }
            if (maxWidth > 0)
            {
                this.maxWidth = maxWidth;
            }
            if (minHeight > 0)
            {
                this.minHeight = minHeight;
            }
            if (maxHeight > 0)
            {
                this.maxHeight = maxHeight;
            }
        }

        internal void Zip()
        {
            if (zoomed)
            {
                zoomHeight = height;
                zoomWidth = width;
                zoomTopEdge = topEdge;
                zoomLeftEdge = leftEdge;
                leftEdge = normalLeftEdge;
                topEdge = normalTopEdge;
                width = normalWidth;
                height = normalHeight;
                zoomed = false;
                CheckBitmap();
            }
            else
            {
                normalHeight = height;
                normalWidth = width;
                normalLeftEdge = leftEdge;
                normalTopEdge = topEdge;
                leftEdge = zoomLeftEdge;
                topEdge = zoomTopEdge;
                width = zoomWidth;
                height = zoomHeight;
                zoomed = true;
                CheckBitmap();
            }
        }

        internal bool Request(Requester req)
        {
            foreach (var gad in req.ReqGadgets)
            {
                gad.Window = this;
                gad.Requester = req;
            }
            requesters.Add(req);
            flags |= WindowFlags.WFLG_INREQUEST;
            return true;
        }

        internal bool EndRequest(Requester req)
        {
            bool ok = requesters.Remove(req);
            if (requesters.Count == 0)
            {
                flags &= ~WindowFlags.WFLG_INREQUEST;
            }
            return ok;
        }

        internal int AddGadget(Gadget gadget, int position)
        {
            gadget.Window = this;
            if (position >= 0 && position < gadgets.Count)
            {
                gadgets.Insert(position, gadget);
            }
            else
            {
                gadgets.Add(gadget);
            }
            return gadgets.IndexOf(gadget);
        }

        internal int AddGList(IEnumerable<Gadget> gadgets, int position, int numgad)
        {
            int result = -1;
            int count = 0;
            foreach (var gad in gadgets)
            {
                if ((numgad >= 0) && (count >= numgad)) break;
                int realPos = AddGadget(gad, position);
                if ((count == 0) || (result == -1)) result = realPos;
                count++;
            }
            return result;
        }

        internal int RemoveGadget(Gadget gadget)
        {
            int index = gadgets.IndexOf(gadget);
            if (index >= 0)
            {
                gadgets.RemoveAt(index);
            }
            return index;
        }

        internal int RemoveGList(IEnumerable<Gadget> gadgets, int numgad)
        {
            int result = -1;
            int count = 0;
            foreach (var gad in gadgets)
            {
                if ((numgad >= 0) && (count >= numgad)) break;
                int pos = RemoveGadget(gad);
                if (count == 0 || result == -1) result = pos;
                count++;
            }
            return result;
        }

        public override string ToString()
        {
            return $"Window {title}";
        }
    }
}
