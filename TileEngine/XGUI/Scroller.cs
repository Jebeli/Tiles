using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;

namespace TileEngine.XGUI
{
    public class Scroller : Widget
    {
        public const int SCROLLER_SIZE = 20;
        private PropGadget prop;
        private ToolButton arrowInc;
        private ToolButton arrowDec;
        private Orientation orientation;
        private int total;
        private int level;
        private int amount;
        private int overlap;
        private BoxLayout box;
        private int arrowDelta;
        public Scroller(Widget parent, Orientation orientation = Orientation.Horizontal)
            : base(parent)
        {
            BorderLeft = 2;
            BorderTop = 2;
            BorderRight = 2;
            BorderBottom = 2;
            arrowDelta = 1;
            total = 100;
            level = 0;
            amount = 50;
            overlap = 1;
            this.orientation = orientation;
            box = new BoxLayout(orientation);
            Layout = box;
            prop = new PropGadget(this);
            prop.PropChanged += Prop_PropChanged;
            arrowInc = new ToolButton(this, Icons.NONE)
            {
                ClickEvent = ArrowInc_Click,
                FixedSize = new Vector2(SCROLLER_SIZE,SCROLLER_SIZE)
            };
            arrowDec = new ToolButton(this, Icons.NONE)
            {
                ClickEvent = ArrowDec_Click,
                FixedSize = new Vector2(SCROLLER_SIZE,SCROLLER_SIZE)
            };
            UpdateOrientation();
            ChangeValues();
        }

        public event EventHandler<IndexEventArgs> LevelChanged;

        public EventHandler<IndexEventArgs> LevelChangedEvent
        {
            set { LevelChanged += value; }
        }

        private void Prop_PropChanged(object sender, EventArgs e)
        {
            UpdateValues();
        }

        private void ArrowDec_Click(object sender, EventArgs e)
        {
            Level = Math.Min(Total, Level + arrowDelta);
        }

        private void ArrowInc_Click(object sender, EventArgs e)
        {
            Level = Math.Max(0, Level - arrowDelta);
        }

        public int Total
        {
            get { return total; }
            set
            {
                if (total != value)
                {
                    total = value;
                    ChangeValues();
                }
            }
        }

        public int Level
        {
            get { return level; }
            set
            {
                if (level != value)
                {
                    level = value;
                    ChangeValues();
                    OnLevelChanged(level);
                }
            }
        }

        public int Amount
        {
            get { return amount; }
            set
            {
                if (amount != value)
                {
                    amount = value;
                    ChangeValues();
                }
            }
        }

        public int ArrowDelta
        {
            get { return arrowDelta; }
            set { arrowDelta = value; }
        }

        protected virtual void OnLevelChanged(int index)
        {
            LevelChanged?.Invoke(this, new IndexEventArgs(index));
        }

        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    UpdateOrientation();
                    ChangeValues();
                }
            }
        }

        private void UpdateOrientation()
        {
            box.Orientation = orientation;
            arrowInc.Icon = orientation == Orientation.Horizontal ? Icons.ENTYPO_ICON_ARROW_LEFT : Icons.ENTYPO_ICON_ARROW_UP;
            arrowDec.Icon = orientation == Orientation.Horizontal ? Icons.ENTYPO_ICON_ARROW_RIGHT : Icons.ENTYPO_ICON_ARROW_DOWN;
            if (orientation == Orientation.Horizontal)
            {
                arrowInc.FixedWidth = SCROLLER_SIZE;
                arrowInc.FixedHeight = SCROLLER_SIZE;
                arrowInc.Width = SCROLLER_SIZE;
                arrowInc.Height = SCROLLER_SIZE;
                arrowDec.FixedWidth = SCROLLER_SIZE;
                arrowDec.FixedHeight = SCROLLER_SIZE;
                arrowDec.Width = SCROLLER_SIZE;
                arrowDec.Height = SCROLLER_SIZE;

                FixedHeight = SCROLLER_SIZE + 4;
                FixedWidth = 0;
            }
            else
            {
                arrowInc.FixedWidth = SCROLLER_SIZE;
                arrowInc.FixedHeight = SCROLLER_SIZE;
                arrowInc.Width = SCROLLER_SIZE;
                arrowInc.Height = SCROLLER_SIZE;
                arrowDec.FixedWidth = SCROLLER_SIZE;
                arrowDec.FixedHeight = SCROLLER_SIZE;
                arrowDec.Width = SCROLLER_SIZE;
                arrowDec.Height = SCROLLER_SIZE;

                FixedWidth = SCROLLER_SIZE + 4;
                FixedHeight = 0;
            }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            if (orientation == Orientation.Horizontal)
            {
                return base.GetPreferredSize(gfx);
            }
            else
            {
                return base.GetPreferredSize(gfx);
            }
        }

        public override void PerformLayout(IGraphics gfx)
        {
            base.PerformLayout(gfx);
            if (orientation == Orientation.Horizontal)
            {
                int width = ClientWidth - arrowDec.Width - arrowInc.Width;
                int diff = width - prop.Width;
                prop.Width = width;
                arrowDec.Left += diff;
                arrowInc.Left += diff;
            }
            else
            {
                int height = ClientHeight - arrowDec.Height - arrowInc.Height;
                int diff = height - prop.Height;
                prop.Height = height;
                arrowDec.Top += diff;
                arrowInc.Top += diff;
            }
        }

        private void UpdateValues()
        {
            int pot = orientation == Orientation.Horizontal ? prop.HorizPot : prop.VertPot;
            int lev = FindScrollerTop(total, amount, pot);
            if (lev != level)
            {
                level = lev;
                OnLevelChanged(level);
            }
        }

        private void ChangeValues()
        {
            FindScrollerValues(total, amount, level, overlap, out int body, out int pot);
            if (orientation == Orientation.Horizontal)
            {
                prop.ModifyProp(PropFlags.AutoKnob | PropFlags.FreeHoriz | PropFlags.Borderless, pot, prop.VertPot, body, prop.VertBody);
                //arrowDelta = prop.HPotRes;
            }
            else
            {
                prop.ModifyProp(PropFlags.AutoKnob | PropFlags.FreeVert | PropFlags.Borderless, prop.HorizPot, pot, prop.HorizBody, body);
                //arrowDelta = prop.VPotRes;
            }
        }

        private static void FindScrollerValues(int total, int visible, int top, int overlap, out int body, out int pot)
        {
            int hidden = total > visible ? total - visible : 0;
            if (top > hidden) top = hidden;
            body = (hidden > 0) ? ((visible - overlap) * PropGadget.MAXBODY) / (total - overlap) : PropGadget.MAXBODY;
            pot = (hidden > 0) ? (top * PropGadget.MAXPOT) / hidden : 0;
        }

        private static int FindScrollerTop(int total, int visible, int pot)
        {
            int hidden = total > visible ? total - visible : 0;
            return ((hidden * pot) + (PropGadget.MAXPOT / 2)) / PropGadget.MAXPOT;
        }

        public override void Render(IGraphics gfx)
        {
            Theme.RenderScroller(gfx, this);
            base.Render(gfx);
        }

    }
}
