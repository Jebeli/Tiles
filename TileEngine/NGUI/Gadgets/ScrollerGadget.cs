using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI.Gadgets
{
    public class ScrollerGadget : GroupGadget
    {
        private enum ArrowKind
        {
            Left,
            Right,
            Up,
            Down
        }
        private class ArrowButton : ButtonGadget
        {

            private ScrollerGadget scroller;
            public int Delta { get; set; }
            public ArrowKind Kind { get; set; }
            public ArrowButton(ScrollerGadget scroller, int delta)
            {
                this.scroller = scroller;
                Delta = delta;
            }

            public override void HandleInput(InputEvent inputEvent)
            {
                base.HandleInput(inputEvent);
                switch (inputEvent.InputClass)
                {
                    case InputClass.GadgetDown:
                        UpdateScroller();
                        break;
                    case InputClass.Timer:
                        if (Selected) UpdateScroller();
                        break;
                }
            }

            private void UpdateScroller()
            {
                scroller.ScrollerTop += Delta;
            }

            public override void RenderBack(IGraphics graphics)
            {
                base.RenderBack(graphics);
                IBox box = new Box(WinBox);
                if (box.IsEmpty) return;
                var di = DrawInfo;
                //int h_spacing;
                //int v_spacing;
                //h_spacing = box.Width / 4;
                //v_spacing = box.Height / 4;
                //box = box.Shrink(h_spacing, v_spacing);
                int diff = 6;
                box = box.Shrink(diff, diff);
                int x;
                int w;
                int y;
                int h;
                switch (Kind)
                {
                    case ArrowKind.Up:
                        x = box.CenterX;
                        w = 0;
                        for (int dy = box.Top - 1; dy <= box.Bottom; dy += 2)
                        {
                            ClearRect(graphics, x - w, dy, x + w, dy + 2, di.ShinePen);
                            ClearRect(graphics, x - w - 1, dy, x - w, dy + 1, di.DarkEdgePen);
                            ClearRect(graphics, x - w - 1, dy + 1, x - w, dy + 2, di.DarkEdgePen);
                            ClearRect(graphics, x + w, dy, x + w + 1, dy + 1, di.DarkEdgePen);
                            ClearRect(graphics, x + w, dy + 1, x + w + 1, dy + 2, di.DarkEdgePen);
                            w += 1;
                        }
                        ClearRect(graphics, x - w - 1, box.Bottom + 2, x + w + 1, box.Bottom + 3, di.DarkEdgePen);

                        break;
                    case ArrowKind.Down:
                        x = box.CenterX;
                        w = 0;
                        for (int dy = box.Bottom + 1; dy >= box.Top; dy -= 2)
                        {
                            ClearRect(graphics, x - w, dy, x + w, dy + 2, di.ShinePen);
                            ClearRect(graphics, x - w - 1, dy, x - w, dy + 1, di.DarkEdgePen);
                            ClearRect(graphics, x - w - 1, dy + 1, x - w, dy + 2, di.DarkEdgePen);
                            ClearRect(graphics, x + w, dy, x + w + 1, dy + 1, di.DarkEdgePen);
                            ClearRect(graphics, x + w, dy + 1, x + w + 1, dy + 2, di.DarkEdgePen);
                            w += 1;
                        }
                        ClearRect(graphics, x - w - 1, box.Top - 1, x + w + 1, box.Top, di.DarkEdgePen);

                        break;
                    case ArrowKind.Left:
                        y = box.CenterY;
                        h = 0;
                        for (int dx = box.Left - 1; dx <= box.Right; dx += 2)
                        {
                            ClearRect(graphics, dx, y - h, dx + 2, y + h, di.ShinePen);
                            ClearRect(graphics, dx, y - h - 1, dx + 1, y - h, di.DarkEdgePen);
                            ClearRect(graphics, dx + 1, y - h - 1, dx + 2, y - h, di.DarkEdgePen);
                            ClearRect(graphics, dx, y + h, dx + 1, y + h + 1, di.DarkEdgePen);
                            ClearRect(graphics, dx + 1, y + h, dx + 2, y + h + 1, di.DarkEdgePen);
                            h += 1;
                        }
                        ClearRect(graphics, box.Right + 2, y - h - 1, box.Right + 3, y + h + 1, di.DarkEdgePen);
                        break;
                    case ArrowKind.Right:
                        y = box.CenterY;
                        h = 0;
                        for (int dx = box.Right + 1; dx >= box.Left; dx -= 2)
                        {
                            ClearRect(graphics, dx, y - h, dx + 2, y + h, di.ShinePen);
                            ClearRect(graphics, dx, y - h - 1, dx + 1, y - h, di.DarkEdgePen);
                            ClearRect(graphics, dx + 1, y - h - 1, dx + 2, y - h, di.DarkEdgePen);
                            ClearRect(graphics, dx, y + h, dx + 1, y + h + 1, di.DarkEdgePen);
                            ClearRect(graphics, dx + 1, y + h, dx + 2, y + h + 1, di.DarkEdgePen);
                            h += 1;
                        }
                        ClearRect(graphics, box.Left - 1, y - h - 1, box.Left, y + h + 1, di.DarkEdgePen);
                        break;
                }
            }
        }

        private Orientation orientation;
        private bool arrows;
        private PropGadget propGadget;
        private ArrowButton arrowIncGadget;
        private ArrowButton arrowDecGadget;
        private int scrollerTotal;
        private int scrollerVisible;
        private int scrollerTop;
        private int arrowDelta;
        public ScrollerGadget()
        {
            BeginUpdate();
            arrowDelta = 1;
            orientation = Orientation.Vertical;
            arrows = true;
            propGadget = new PropGadget();
            propGadget.Target = this;
            arrowIncGadget = new ArrowButton(this, arrowDelta);
            arrowDecGadget = new ArrowButton(this, -arrowDelta);
            AddChild(propGadget);
            AddChild(arrowIncGadget);
            AddChild(arrowDecGadget);
            EndUpdate();
        }

        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    WriteChanges();
                }
            }
        }

        public int ScrollerTotal
        {
            get
            {
                ReadChanges();
                return scrollerTotal;
            }
            set
            {
                if (scrollerTotal != value)
                {
                    scrollerTotal = value;
                    WriteChanges();
                }
            }
        }

        public int ScrollerTop
        {
            get
            {
                ReadChanges();
                return scrollerTop;
            }
            set
            {
                if (value > (scrollerTotal - scrollerVisible)) { value = scrollerTotal - scrollerVisible; }
                if (value < 0) value = 0;
                if (scrollerTop != value)
                {
                    scrollerTop = value;
                    WriteChanges();
                }
            }
        }
        public int ScrollerVisible
        {
            get
            {
                ReadChanges();
                return scrollerVisible;
            }
            set
            {
                if (scrollerVisible != value)
                {
                    scrollerVisible = value;
                    WriteChanges();
                }
            }
        }

        public bool Arrows
        {
            get { return arrows; }
            set
            {
                if (arrows != value)
                {
                    arrows = value;
                    WriteChanges();
                }
            }
        }

        public int ArrowDelta
        {
            get { return arrowDelta; }
            set
            {
                if (arrowDelta != Math.Abs(value))
                {
                    arrowDelta = Math.Abs(value);
                    arrowIncGadget.Delta = arrowDelta;
                    arrowDecGadget.Delta = -arrowDelta;
                }
            }
        }
        private void ReadChanges()
        {
            if (IsUpdating) return;
            scrollerVisible = propGadget.FreeVisible;
            scrollerTotal = propGadget.FreeTotal;
            scrollerTop = propGadget.FreeTop;
        }

        private void WriteChanges()
        {
            if (IsUpdating) return;
            if (Parent == null) return;
            IBox box = new Box(WinBox);
            if (box == null) return;
            BeginUpdate();
            switch (orientation)
            {
                case Orientation.Vertical:
                    propGadget.Width = box.Width;
                    propGadget.Height = box.Height;
                    propGadget.PropFlags &= ~PropFlags.FreeHoriz;
                    propGadget.PropFlags |= PropFlags.AutoKnob | PropFlags.FreeVert;
                    propGadget.VertTotal = scrollerTotal;
                    propGadget.VertVisible = scrollerVisible;
                    propGadget.VertTop = scrollerTop;
                    if (arrows)
                    {
                        arrowDecGadget.Left = 0;
                        arrowIncGadget.Left = 0;
                        arrowDecGadget.Top = box.Height - 39;
                        arrowIncGadget.Top = box.Height - 20;
                        arrowIncGadget.Height = 20;
                        arrowDecGadget.Height = 20;
                        arrowIncGadget.Width = box.Width;
                        arrowDecGadget.Width = box.Width;
                        arrowDecGadget.Visible = true;
                        arrowIncGadget.Visible = true;
                        arrowDecGadget.Kind = ArrowKind.Up;
                        arrowIncGadget.Kind = ArrowKind.Down;
                        propGadget.Height -= 38;
                    }
                    else
                    {
                        arrowDecGadget.Visible = false;
                        arrowIncGadget.Visible = false;
                    }
                    break;
                case Orientation.Horizontal:
                    propGadget.Width = box.Width;
                    propGadget.Height = box.Height;
                    propGadget.PropFlags &= ~PropFlags.FreeVert;
                    propGadget.PropFlags |= PropFlags.AutoKnob | PropFlags.FreeHoriz;
                    propGadget.HorizTotal = scrollerTotal;
                    propGadget.HorizVisible = scrollerVisible;
                    propGadget.HorizTop = scrollerTop;
                    if (arrows)
                    {
                        arrowDecGadget.Top = 0;
                        arrowIncGadget.Top = 0;
                        arrowDecGadget.Left = box.Width - 39;
                        arrowIncGadget.Left = box.Width - 20;
                        arrowIncGadget.Width = 20;
                        arrowDecGadget.Width = 20;
                        arrowIncGadget.Height = box.Height;
                        arrowDecGadget.Height = box.Height;
                        arrowDecGadget.Visible = true;
                        arrowIncGadget.Visible = true;
                        arrowDecGadget.Kind = ArrowKind.Left;
                        arrowIncGadget.Kind = ArrowKind.Right;
                        propGadget.Width -= 38;
                    }
                    else
                    {
                        arrowDecGadget.Visible = false;
                        arrowIncGadget.Visible = false;
                    }
                    break;
            }
            EndUpdate();
        }

        public override void Layout()
        {
            base.Layout();
            WriteChanges();
        }

        public override void Update()
        {
            base.Update();
            ReadChanges();
        }
    }
}
