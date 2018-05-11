using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;
using TileEngine.Graphics;
using TileEngine.Logging;

namespace TileEngine.NGUI
{
    public abstract class GUIElement : GUIDimensions, IGUIElement
    {
        private bool visible;
        private bool selectable;
        private bool enabled;
        private StateFlags state;
        private IGUIElement parent;
        private List<IGUIElement> children;
        private Text textElement;
        private int id;
        private DrawInfo drawInfo;
        private bool initDone;
        private IBox winBox;
        private IBox scrBox;
        private IGUIElement target;
        private int updateCount;
        private int preferredWidth;
        private int preferredHeight;
        private int fixedWidth;
        private int fixedHeight;
        private Font font;

        public GUIElement()
        {
            fixedWidth = 0;
            fixedHeight = 0;
            preferredWidth = 0;
            preferredHeight = 0;
            visible = true;
            selectable = true;
            enabled = true;
            children = new List<IGUIElement>();
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool Selectable
        {
            get { return selectable; }
            set { selectable = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        public DrawInfo DrawInfo
        {
            get
            {
                if (drawInfo != null)
                {
                    return drawInfo;
                }
                return parent?.DrawInfo;
            }
            set { drawInfo = value; }
        }

        public bool Hover
        {
            get { return state.HasFlag(StateFlags.Hover); }
            set
            {
                if (value)
                {
                    state |= StateFlags.Hover;
                }
                else
                {
                    state &= ~StateFlags.Hover;
                }
                IGUIElement p = parent;
                while (p != null)
                {
                    p.Hover = value;
                    p = p.Parent;
                }
            }
        }
        public bool Selected
        {
            get { return state.HasFlag(StateFlags.Selected); }
            set
            {
                if (value)
                {
                    state |= StateFlags.Selected;
                }
                else
                {
                    state &= ~StateFlags.Selected;
                }
                //IGUIElement p = parent;
                //while (p != null)
                //{
                //    p.Selected = value;
                //    p = p.Parent;
                //}
            }
        }
        public bool Active
        {
            get { return state.HasFlag(StateFlags.Active); }
            set
            {
                if (value)
                {
                    state |= StateFlags.Active;
                }
                else
                {
                    state &= ~StateFlags.Active;
                }
                //IGUIElement p = parent;
                //while (p != null)
                //{
                //    p.Active = value;
                //    p = p.Parent;
                //}
            }
        }

        public StateFlags State
        {
            get { return state; }
            set { state = value; }
        }

        public IBox WinBox
        {
            get { return winBox; }
            set
            {
                if (winBox == null || !winBox.Equals(value))
                {
                    winBox = value;
                }
            }
        }

        public IBox ScrBox
        {
            get { return scrBox; }
            set
            {
                if (scrBox == null || !scrBox.Equals(value))
                {
                    scrBox = value;
                }
            }
        }

        public IGUIElement Target
        {
            get { return target; }
            set { target = value; }
        }

        public IGUIElement Parent
        {
            get { return parent; }
            set
            {
                if (parent != value)
                {
                    parent = value;
                }
            }
        }

        public IEnumerable<IGUIElement> Children
        {
            get { return children; }
        }

        protected virtual void ChildAdded(IGUIElement element)
        {
            Layout();
        }

        protected virtual void ChildRemoved(IGUIElement element)
        {
            Layout();
        }

        public void AddChild(IGUIElement element)
        {
            children.Add(element);
            element.Parent = this;
            element.Init();
            ChildAdded(element);
        }

        public void RemChild(IGUIElement element)
        {
            children.Remove(element);
            element.Parent = null;
            ChildRemoved(element);
        }

        public void MoveToBack()
        {
            parent?.MoveChildToBack(this);
        }

        public void MoveToFront()
        {
            parent?.MoveChildToFront(this);
        }

        public void MoveChildToBack(IGUIElement element)
        {

        }
        public void MoveChildToFront(IGUIElement element)
        {

        }

        public virtual string Text
        {
            get { return textElement?.Value; }
            set
            {
                if (textElement == null)
                {
                    TextElement = new Text()
                    {
                        Value = value,
                        DimFlags = DimFlags.RelWidth | DimFlags.RelHeight
                    };
                }
                else
                {
                    textElement.Value = value;
                }
            }
        }

        public Text TextElement
        {
            get { return textElement; }
            set
            {
                if (textElement != value)
                {
                    if (textElement != null)
                    {
                        children.Remove(textElement);
                        textElement.parent = null;
                    }
                    textElement = value;
                    if (textElement != null)
                    {
                        AddChild(textElement);
                    }

                }
            }
        }

        public void Init()
        {
            if (!initDone)
            {
                Initialize();
                initDone = true;
            }
        }

        protected virtual void Initialize()
        {
        }

        public virtual bool HitTest(int x, int y)
        {
            return ScrBox.ContainsPoint(x, y);
        }

        public void SetPreferredSize(int w, int h)
        {
            preferredWidth = w;
            preferredHeight = h;
        }
        public void SetFixedSize(int w, int h)
        {
            fixedWidth = w;
            fixedHeight = h;
        }
        public virtual IBox GetPreferredSize()
        {
            return new Box() { Width = preferredWidth, Height = preferredHeight };
        }

        public virtual IBox GetFixedSize()
        {
            return new Box() { Width = fixedWidth, Height = fixedHeight };
        }

        public override void Layout()
        {
            if (preferredWidth == 0) preferredWidth = Width;
            if (preferredHeight == 0) preferredHeight = Height;
            if (parent == null) return;
            WinBox = GetWinBox();
            ScrBox = GetScrBox();
            Logger.Detail("GUI", $"Layout {this} Win {WinBox} Scr {ScrBox}");
            foreach (var c in children)
            {
                c.Layout();
            }
        }

        public virtual void Notify()
        {
            if (target != null)
            {
                Logger.Info("GUI", $"Notify from {this} to {target}");
                target.Update();

            }
        }

        public virtual void Update()
        {

        }

        public void BeginUpdate()
        {
            updateCount++;
        }
        public void EndUpdate()
        {
            updateCount--;
        }
        public bool IsUpdating { get { return updateCount > 0; } }

        public abstract void HandleInput(InputEvent inputEvent);

        public abstract void RenderBack(IGraphics graphics);
        public abstract void RenderFront(IGraphics graphics);
        public virtual void BeginRender(IGraphics graphics)
        {

        }
        public virtual void EndRender(IGraphics graphics)
        {

        }

        internal static void DisableBox(IGraphics graphics, IBox box)
        {
            graphics.FillRectangle(box.Left, box.Top, box.Width, box.Height, new Color(128, 128, 128, 128));
        }

        internal static void ClearBox(IGraphics graphics, IBox box, Color color)
        {
            graphics.FillRectangle(box.Left, box.Top, box.Width, box.Height, color);
        }

        internal static void RenderBox(IGraphics graphics, IBox box, Color color)
        {
            graphics.DrawRectangle(box.Left, box.Top, box.Width, box.Height, color);
        }
        internal static void RenderRect(IGraphics graphics, int left, int top, int right, int bottom, Color color)
        {
            graphics.DrawRectangle(left, top, right - left, bottom - top, color);
        }
        internal static void ClearRect(IGraphics graphics, int left, int top, int right, int bottom, Color color)
        {
            graphics.FillRectangle(left, top, right - left, bottom - top, color);
        }

        internal static void RenderBox3D(IGraphics graphics, IBox box, Color shine, Color shadow)
        {
            graphics.FillRectangle(box.Left, box.Top, box.Width - 1, 1, shine);
            graphics.FillRectangle(box.Left, box.Top, 1, box.Height, shine);
            graphics.FillRectangle(box.Left + 1, box.Bottom, box.Width - 1, 1, shadow);
            graphics.FillRectangle(box.Right, box.Top, 1, box.Height, shadow);
        }

        internal static void RenderText(IGraphics graphics, IBox box, Color color, Font font, string text)
        {
            graphics.RenderText(font, text, box.CenterX, box.CenterY, color);
        }
        public void DebugRender(IGraphics graphics, Font font)
        {
            IBox box = new Box(WinBox);
            string txt = $"{State} {box.Left}/{box.Top}";
            graphics.RenderText(font, txt, box.Left + 1, box.Top + 1, Color.Black, HorizontalTextAlign.Left, VerticalTextAlign.Top);
        }
        internal void SimpleImageRender(IGraphics graphics)
        {
            IBox box = new Box(WinBox);
            if (box.IsEmpty) return;
            DrawInfo di = DrawInfo;
            Color fgColor = di.FrontPen;
            RenderBox(graphics, box, fgColor);
        }

        internal void SimpleGadgetRender(IGraphics graphics)
        {
            IBox box = new Box(WinBox);
            if (box.IsEmpty) return;
            RenderBox(graphics, box, DrawInfo.DarkEdgePen);
            RenderBox3D(graphics, box.Shrink(1, 1), GetShineColor(), GetShadowColor());
            ClearBox(graphics, box.Shrink(2, 2), GetBgColor());
        }
        protected virtual Color GetShineColor()
        {
            bool selected = State.HasFlag(StateFlags.Selected);
            return selected ? DrawInfo.ShadowPen : DrawInfo.ShinePen;
        }

        protected virtual Color GetShadowColor()
        {
            bool selected = State.HasFlag(StateFlags.Selected);
            return selected ? DrawInfo.ShinePen : DrawInfo.ShadowPen;
        }

        protected virtual Color GetFgColor()
        {
            bool selected = State.HasFlag(StateFlags.Selected);
            return selected ? DrawInfo.SelectedFrontPen : DrawInfo.FrontPen;
        }
        protected virtual Color GetBgColor()
        {
            bool hover = State.HasFlag(StateFlags.Hover);
            return hover ? DrawInfo.HoverBackPen : DrawInfo.BackPen;
        }

        internal void SimpleDisableRender(IGraphics graphics)
        {
            IBox box = new Box(WinBox);
            if (box.IsEmpty) return;
            if (!enabled)
            {
                DisableBox(graphics, box);
            }
        }

        protected virtual IBox GetScrBox()
        {
            return AdjustBox(Parent?.ScrBox);
        }
        protected virtual IBox GetWinBox()
        {
            return AdjustBox(Parent?.WinBox);
        }

        private IBox AdjustBox(IBox pbox)
        {
            IBox box = new Box();
            if (pbox != null)
            {
                if (DimFlags.HasFlag(DimFlags.RelRight))
                {
                    box.Left = pbox.Right + Left;
                }
                else if (DimFlags.HasFlag(DimFlags.RelCenterX))
                {
                    box.Left = pbox.CenterX + Left;
                }
                else
                {
                    box.Left = pbox.Left + Left;
                }
                if (DimFlags.HasFlag(DimFlags.RelBottom))
                {
                    box.Top = pbox.Bottom + Top;
                }
                else if (DimFlags.HasFlag(DimFlags.RelCenterY))
                {
                    box.Top = pbox.CenterY + Top;
                }
                else
                {
                    box.Top = pbox.Top + Top;
                }
                if (DimFlags.HasFlag(DimFlags.RelWidth))
                {
                    box.Width = pbox.Width + Width;
                }
                else
                {
                    box.Width = Width;
                }
                if (DimFlags.HasFlag(DimFlags.RelHeight))
                {
                    box.Height = pbox.Height + Height;
                }
                else
                {
                    box.Height = Height;
                }
            }
            else
            {
                box.Width = Math.Abs(Width);
                box.Height = Math.Abs(Height);
            }
            return box;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetType().Name);
            sb.Append(": ");
            sb.Append(Text);
            return sb.ToString();
        }

    }
}
