using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Input;

namespace TileEngine.IGUI
{
    public class ScrollerGadget : Gadget
    {
        private PropGadget propGadget;
        private ArrowButton arrowIncGadget;
        private ArrowButton arrowDecGadget;
        private Orientation orientation;
        private bool arrows;
        private int scrollerTotal;
        private int scrollerVisible;
        private int scrollerTop;
        private int arrowDelta;

        private class ArrowButton : ToolButton
        {
            private ScrollerGadget scroller;
            private int delta;
            public ArrowButton(ScrollerGadget scroller, int delta)
            {
                this.scroller = scroller;
                this.delta = delta;
            }

            private void UpdateScroller()
            {
                scroller.ScrollerTop += delta;
            }

            public override GoActiveResult HandleInput(IDCMPFlags idcmp, int x, int y, MouseButton button, InputCode code)
            {
                var res = base.HandleInput(idcmp, x, y, button, code);
                if (res == GoActiveResult.MeActive)
                {
                    switch (idcmp)
                    {
                        case IDCMPFlags.GadgetDown:
                            UpdateScroller();
                            break;
                    }
                }
                return res;
            }
        }

        public ScrollerGadget() : this(TagItems.Empty)
        {
        }

        public ScrollerGadget(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            arrowDelta = 1;
            orientation = Orientation.Vertical;
            arrows = true;
            propGadget = new PropGadget();
            arrowIncGadget = new ArrowButton(this, arrowDelta);
            arrowDecGadget = new ArrowButton(this, -arrowDelta);
            AddMember(propGadget);
            AddMember(arrowIncGadget);
            AddMember(arrowDecGadget);
            New(tags);
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
                    //arrowIncGadget.Delta = arrowDelta;
                    //arrowDecGadget.Delta = -arrowDelta;
                }
            }
        }

        public override void Layout(bool initial)
        {
            base.Layout(initial);
            WriteChanges();
            propGadget.Layout(initial);
            arrowDecGadget.Layout(initial);
            arrowIncGadget.Layout(initial);
        }

        protected override void OnNotifyClick(int id)
        {
            base.OnNotifyClick(id);
            Notify(UpdateFlags.Final, (Tags.SCROLLER_Top, ScrollerTop));
        }

        private void ReadChanges()
        {
            //if (IsUpdating) return;
            scrollerVisible = propGadget.FreeVisible;
            scrollerTotal = propGadget.FreeTotal;
            scrollerTop = propGadget.FreeTop;
        }

        private void WriteChanges()
        {
            //if (IsUpdating) return;
            //if (Parent == null) return;
            //IBox box = new Box(WinBox);
            //if (box == null) return;
            //BeginUpdate();
            var frame = Bounds;
            frame.Offset(-Window.BorderLeft, -Window.BorderTop);
            switch (orientation)
            {
                case Orientation.Vertical:
                    propGadget.LeftEdge = frame.Left;
                    propGadget.TopEdge = frame.Top;
                    propGadget.Width = frame.Width;
                    propGadget.Height = frame.Height;
                    propGadget.PropFlags &= ~PropFlags.FreeHoriz;
                    propGadget.PropFlags |= PropFlags.AutoKnob | PropFlags.FreeVert;
                    propGadget.FreeTotal = scrollerTotal;
                    propGadget.FreeVisible = scrollerVisible;
                    propGadget.FreeTop = scrollerTop;
                    if (arrows)
                    {
                        arrowDecGadget.Icon = Icons.ENTYPO_ICON_ARROW_UP;
                        arrowIncGadget.Icon = Icons.ENTYPO_ICON_ARROW_DOWN;
                        arrowDecGadget.LeftEdge = frame.Left;
                        arrowIncGadget.LeftEdge = frame.Left;
                        arrowDecGadget.TopEdge = frame.Bottom - 40;
                        arrowIncGadget.TopEdge = frame.Bottom - 20;
                        arrowIncGadget.Height = 20;
                        arrowDecGadget.Height = 20;
                        arrowIncGadget.Width = frame.Width;
                        arrowDecGadget.Width = frame.Width;
                        propGadget.Height -= 40;
                    }
                    else
                    {
                        //arrowDecGadget.Visible = false;
                        //arrowIncGadget.Visible = false;
                    }
                    break;
                case Orientation.Horizontal:
                    propGadget.LeftEdge = frame.Left + 1;
                    propGadget.TopEdge = frame.Top + 1;
                    propGadget.Width = frame.Width - 2;
                    propGadget.Height = frame.Height - 2;
                    propGadget.PropFlags &= ~PropFlags.FreeVert;
                    propGadget.PropFlags |= PropFlags.AutoKnob | PropFlags.FreeHoriz | PropFlags.Borderless;
                    propGadget.FreeTotal = scrollerTotal;
                    propGadget.FreeVisible = scrollerVisible;
                    propGadget.FreeTop = scrollerTop;
                    if (arrows)
                    {
                        arrowDecGadget.Icon = Icons.ENTYPO_ICON_ARROW_LEFT;
                        arrowIncGadget.Icon = Icons.ENTYPO_ICON_ARROW_RIGHT;
                        arrowDecGadget.TopEdge = frame.Top + 1;
                        arrowIncGadget.TopEdge = frame.Top + 1;
                        arrowDecGadget.LeftEdge = frame.Right  - 40;
                        arrowIncGadget.LeftEdge = frame.Right  - 20;
                        arrowDecGadget.Width = 20;
                        arrowIncGadget.Width = 20;
                        arrowDecGadget.Height = frame.Height - 2;
                        arrowIncGadget.Height = frame.Height - 2;
                        propGadget.Width -= 41;
                    }
                    else
                    {
                        //arrowDecGadget.Visible = false;
                        //arrowIncGadget.Visible = false;
                    }
                    break;
            }
            //EndUpdate();
        }

        protected override int SetTag(SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.SCROLLER_Orientation:
                    orientation = tag.GetTagData(Orientation.Horizontal);
                    return 1;
                case Tags.SCROLLER_Top:
                    scrollerTop = tag.GetTagData(0);
                    return 1;
                case Tags.SCROLLER_Total:
                    scrollerTotal = tag.GetTagData(0);
                    return 1;
                case Tags.SCROLLER_Visible:
                    scrollerVisible = tag.GetTagData(0);
                    return 1;
            }
            return base.SetTag(set, update, tag);
        }

    }
}
