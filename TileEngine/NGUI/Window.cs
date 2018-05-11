using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI
{
    public class Window : GUIElement
    {
        private bool zipped;
        private int zipLeft;
        private int zipTop;
        private int zipWidth;
        private int zipHeight;
        private int normalLeft;
        private int normalTop;
        private int normalWidth;
        private int normalHeight;
        private Texture bitmap;
        private int reqCount;
        private Window reqWindow;
        public Window()
        {
            MinWidth = 100;
            MinHeight = 100;
            Opacity = 255;
            HoverOpacity = 255;
            BorderLeft = 4;
            BorderRight = 4;
            BorderBottom = 4;
            BorderTop = 19;
        }

        public WindowFlags WindowFlags { get; set; }
        public string Title { get; set; }
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }
        public int Opacity { get; set; }
        public int HoverOpacity { get; set; }
        public int BorderLeft { get; private set; }
        public int BorderTop { get; private set; }
        public int BorderRight { get; private set; }
        public int BorderBottom { get; private set; }
        public bool InRequest { get { return reqCount > 0; } }
        public int RenderTransparency => Hover ? 255 - HoverOpacity : 255 - Opacity;
        public Window ReqWindow { get { return reqWindow; } }
        public override string Text { get => Title; set => Title = value; }
        protected override void Initialize()
        {
            if (WindowFlags.HasFlag(WindowFlags.Borderless))
            {
                BorderLeft = 1;
                BorderTop = 1;
                BorderBottom = 1;
                BorderRight = 1;
            }
            else
            {
                BorderLeft = 4;
                BorderTop = 4;
                BorderBottom = 4;
                BorderRight = 4;
                if (WindowFlags.HasFlag(WindowFlags.CloseGadget))
                {
                    if (!WindowFlags.HasFlag(WindowFlags.SizeBRight) && !WindowFlags.HasFlag(WindowFlags.SizeBBottom))
                    {
                        WindowFlags |= WindowFlags.SizeBRight;
                    }
                }
                else
                {
                    WindowFlags &= ~(WindowFlags.SizeBRight | WindowFlags.SizeBBottom);
                }
                if (WindowFlags.HasFlag(WindowFlags.SizeBBottom))
                {
                    BorderBottom = 19;
                }
                if (WindowFlags.HasFlag(WindowFlags.SizeBRight))
                {
                    BorderRight = 19;
                }
                if (WindowFlags.HasFlag(WindowFlags.CloseGadget) | WindowFlags.HasFlag(WindowFlags.ZoomGadget) | WindowFlags.HasFlag(WindowFlags.DepthGadget) | WindowFlags.HasFlag(WindowFlags.DragGadget) | !string.IsNullOrEmpty(Title))
                {
                    BorderTop = 19;
                }
            }
            GUISystem.AddSysGadgets(this);
            normalLeft = Left;
            normalTop = Top;
            normalWidth = Width;
            normalHeight = Height;
            zipLeft = Left;
            zipTop = Top;
            if (MaxWidth > MinWidth)
            {
                zipWidth = MaxWidth;
            }
            else
            {
                zipWidth = MinWidth;
            }
            if (MaxHeight > MinHeight)
            {
                zipHeight = MaxHeight;
            }
            else
            {
                zipHeight = MinHeight;
            }
            if (WindowFlags.HasFlag(WindowFlags.Activate))
            {
                GUISystem.ActivateWindow(this);
            }
        }

        public void Move(int deltaX, int deltaY)
        {
            int l = Left + deltaX;
            int t = Top + deltaY;
            if (l + Width > Parent.Width) l = Parent.Width - Width;
            if (l < 0) l = 0;
            if (t + Height > Parent.Height) t = Parent.Height - Height;
            if (t < 0) t = 0;
            Left = l;
            Top = t;
        }

        public void Size(int deltaX, int deltaY)
        {
            int w = Width + deltaX;
            int h = Height + deltaY;
            if (w > MaxWidth && MaxWidth > MinWidth) w = MaxWidth;
            if (Left + w > Parent.Width) w = Parent.Width - Left;
            if (w < MinWidth) w = MinWidth;
            if (h > MaxHeight && MaxHeight > MinHeight) h = MaxHeight;
            if (Top + h > Parent.Height) h = Parent.Height - Top;
            if (h < MinHeight) h = MinHeight;
            Width = w;
            Height = h;
        }

        public void Zip()
        {
            if (zipped)
            {
                zipLeft = Left;
                zipTop = Top;
                zipWidth = Width;
                zipHeight = Height;
                Left = normalLeft;
                Top = normalTop;
                Width = normalWidth;
                Height = normalHeight;
                zipped = false;
            }
            else
            {
                normalLeft = Left;
                normalTop = Top;
                normalWidth = Width;
                normalHeight = Height;
                Left = zipLeft;
                Top = zipTop;
                Width = zipWidth;
                Height = zipHeight;
                zipped = true;
            }
        }

        public void ToggleDepth()
        {
            if (IsTopMostWindow)
            {
                MoveToBack();
            }
            else
            {
                MoveToFront();
            }
        }

        internal Window Close()
        {
            Window next = null;
            IGUIElement parent = Parent;
            if (parent != null)
            {

                parent.RemChild(this);
                foreach (var c in parent.Children)
                {
                    if (c is Window)
                    {
                        next = (Window)c;
                    }
                }
            }
            bitmap?.Dispose();
            bitmap = null;
            return next;
        }

        internal void BeginRequest(Window window)
        {
            if (window != null)
            {
                reqWindow = window;
                reqCount++;
            }
        }

        internal void EndRequest(Window window)
        {
            if ((window != null) && (window == reqWindow))
            {
                reqWindow = null;
                if (reqCount > 0)
                    reqCount--;
            }
        }

        internal void BeginRequest(Requester req)
        {
            if (req != null)
            {
                AddChild(req);
                reqCount++;
            }
        }

        internal void EndRequest(Requester req)
        {
            if (req != null)
            {
                RemChild(req);
                if (reqCount > 0)
                    reqCount--;
            }
        }

        public override void HandleInput(InputEvent inputEvent)
        {

        }
        public override void RenderBack(IGraphics graphics)
        {
            SimpleWindowRender(graphics);
        }

        public override void RenderFront(IGraphics graphics)
        {
            SimpleTitleRender(graphics);
            SimpleDisableRender(graphics);
        }

        private void SimpleWindowRender(IGraphics graphics)
        {
            IBox box = new Box(WinBox);
            if (box.IsEmpty) return;
            var di = DrawInfo;
            Color bgColor = di.BackPen;
            if (!WindowFlags.HasFlag(WindowFlags.Borderless))
            {
                RenderBox3D(graphics, box, di.ShinePen, di.ShadowPen);
                Color fc = GetBorderFillColor();
                graphics.FillRectangle(box.Left + 1, box.Top + 1, BorderLeft - 1, box.Height - 2, fc);
                graphics.FillRectangle(box.Right - BorderRight + 1, box.Top + 1, BorderRight - 1, box.Height - 2, fc);
                graphics.FillRectangle(box.Left + BorderLeft, box.Bottom - BorderBottom + 1, box.Width - (BorderLeft + BorderRight), BorderBottom - 1, fc);
                box.Left += BorderLeft;
                box.Top += BorderTop;
                box.Width -= (BorderRight + BorderLeft);
                box.Height -= (BorderBottom + BorderTop);
                RenderBox3D(graphics, box, di.ShadowPen, di.ShinePen);
                ClearBox(graphics, box.Shrink(1, 1), bgColor);
            }
            else
            {
                ClearBox(graphics, box, bgColor);
            }
        }

        private Color GetBorderFillColor()
        {
            bool active = State.HasFlag(StateFlags.Active);
            return active ? DrawInfo.ActiveBorderPen : DrawInfo.BackPen;
        }


        private void SimpleTitleRender(IGraphics graphics)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                IBox box = new Box(WinBox);
                if (box.IsEmpty) return;
                var di = DrawInfo;
                box.Left += 3;
                if (WindowFlags.HasFlag(WindowFlags.CloseGadget)) { box.Left += 20; }
                graphics.RenderText(Font, Title, box.Left, 10, di.TextPen, HorizontalTextAlign.Left, VerticalTextAlign.Center);
            }
        }


        internal bool IsTopMostWindow
        {
            get
            {
                if (Parent != null)
                {
                    return Parent.Children.LastOrDefault() == this;
                }
                return false;
            }
        }

        protected override IBox GetWinBox()
        {
            IBox box = base.GetWinBox();
            box.Left = 0;
            box.Top = 0;
            return box;
        }

        public override void BeginRender(IGraphics graphics)
        {
            var box = ScrBox;
            if (!box.IsEmpty)
            {
                if (bitmap == null || bitmap.Width != box.Width || bitmap.Height != box.Height)
                {
                    bitmap?.Dispose();
                    bitmap = GUISystem.GetWindowBitmap(this);
                }
                graphics.SetTarget(bitmap);
            }
        }

        public override void EndRender(IGraphics graphics)
        {
            var box = ScrBox;
            if (!box.IsEmpty)
            {
                graphics.ClearTarget();
                graphics.Render(bitmap, box.Left, box.Top, RenderTransparency);
            }
        }

    }
}
