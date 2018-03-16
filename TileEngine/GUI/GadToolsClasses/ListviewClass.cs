using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.GUI.GadToolsClasses
{
    public class ListviewClass : FrameButtonGadget, IGadToolsGadget
    {
        private const int LV_BORDER_Y = 1;
        private const int LV_BORDER_X = 1;
        public ListviewClass()
        {
            ItemHeight = 18;
            Spacing = 0;
            GadgetImage = Intuition.NewObject(Intuition.FRAMEICLASS, (Tags.IA_FrameType, FrameType.Button), (Tags.IA_EdgesOnly, true), (Tags.IA_Recessed, true), (Tags.IA_ReadOnly, true)) as Image;
            GadgetKind = GadKind.ListView;
            HoverIndex = -1;
            SelectedIndex = -1;
        }
        public GadKind GadgetKind { get; private set; }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.GTLV_Labels:
                    Items = tag.GetTagData<IList<object>>();
                    NumEntries = Items.Count;
                    return 1;
                case Tags.GTLV_Selected:
                    SelectedIndex = tag.GetTagData(0);
                    DoShowSelected();
                    return 1;
                case Tags.GTLV_Top:
                case Tags.GTSC_Top:
                case Tags.PGA_Top:
                    Top = tag.GetTagData(0);
                    return 1;
                case Tags.GTLV_ShowSelected:
                    ShowSelected = tag.GetTagData<Gadget>();
                    DoShowSelected();
                    return 1;
                default:
                    return base.SetTag(gadgetInfo, set, update, tag);
            }
        }

        public override void Render(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw)
        {
            base.Render(gadgetInfo, graphics, redraw);
            RenderEntries(gadgetInfo, graphics, Top, ShownEntries());
        }

        private void RenderEntries(GadgetInfo gadgetInfo, IGraphics graphics, int entryoffset, int numentries)
        {
            int totalitemheight = TotalItemHeigh();
            int left = LeftEdge + LV_BORDER_X;
            int top = TopEdge + LV_BORDER_Y;
            int width = Width - 1 - LV_BORDER_X * 2;
            for (int i = entryoffset; i < Items.Count && numentries > 0; i++)
            {
                string txt = Items[i].ToString();
                bool selected = i == SelectedIndex;
                bool hover = i == HoverIndex;
                RenderEntry(gadgetInfo, graphics, txt, left, top, width, totalitemheight, selected, hover);
                top += totalitemheight;
                numentries--;
            }
        }

        private void RenderEntry(GadgetInfo gadgetInfo, IGraphics graphics, string text, int left, int top, int width, int height, bool selected, bool hover)
        {
            if (selected)
            {
                if (hover)
                {
                    graphics.FillRectangle(left, top, width, height, gadgetInfo.DrawInfo.HoverBackgroundPen);
                }
                else
                {
                    graphics.FillRectangle(left, top, width, height, gadgetInfo.DrawInfo.FillPen);
                }
            }
            else if (hover)
            {
                graphics.FillRectangle(left, top, width, height, gadgetInfo.DrawInfo.InactiveHoverBackgroundPen);
            }
            IBox extend = null;
            int textlen = graphics.TextFit(text, ref extend, null, width, height);
            graphics.RenderText(text.Substring(0, textlen), left, top + height / 2, gadgetInfo.DrawInfo.TextPen, HorizontalTextAlign.Left, VerticalTextAlign.Center);
        }

        public override GadgetActive HandleInput(GadgetInfo gadgetInfo, InputEvent inputEvent, ref int termination, int mouseX, int mouseY)
        {
            GadgetActive ga = base.HandleInput(gadgetInfo, inputEvent, ref termination, mouseX, mouseY);
            int shown = ShownEntries();
            int totalItemHeight = TotalItemHeigh();
            int clickPos = (mouseY - TopEdge - LV_BORDER_Y) / totalItemHeight;
            int newIndex = SelectedIndex;
            if (clickPos < 0)
            {

            }
            else if (clickPos > shown)
            {

            }
            if ((clickPos >= 0) && (clickPos < shown))
            {
                if ((clickPos + Top) != SelectedIndex)
                {
                    newIndex = clickPos + Top;
                }
            }
            switch (inputEvent.InputClass)
            {
                case InputClass.MOUSEDOWN:
                    SetSelectedIndex(newIndex);
                    break;
                case InputClass.MOUSEUP:
                    break;
                case InputClass.MOUSEMOVE:
                    if (newIndex != HoverIndex)
                    {
                        HoverIndex = newIndex;
                    }
                    break;
                case InputClass.KEYUP:
                    switch (inputEvent.Key)
                    {
                        case Key.Up:
                            SetSelectedIndex(newIndex - 1);
                            break;
                        case Key.Up | Key.Shift:
                            SetSelectedIndex(newIndex - NumItemsFit());
                            break;
                        case Key.Down:
                            SetSelectedIndex(newIndex + 1);
                            break;
                        case Key.Down | Key.Shift:
                            SetSelectedIndex(newIndex + NumItemsFit());
                            break;
                        case Key.Home:
                            SetSelectedIndex(0);
                            break;
                        case Key.End:
                            SetSelectedIndex(NumEntries - 1);
                            break;
                    }
                    break;
            }
            return ga;
        }

        private void DoShowSelected()
        {
            if ((SelectedIndex >= 0) && (NumEntries > 0) && (ShowSelected != null))
            {
                ShowSelected.Set((Tags.GTST_String, Items[SelectedIndex].ToString()));
            }
        }

        private void SetSelectedIndex(int newIndex)
        {
            if (newIndex < 0)
            {
                newIndex = 0;
            }
            else if (newIndex >= NumEntries)
            {
                newIndex = NumEntries - 1;
            }
            if (newIndex != SelectedIndex)
            {
                SelectedIndex = newIndex;
                AdjustTop();
                DoShowSelected();
            }
        }

        private void AdjustTop()
        {
            int showEntries = ShownEntries();
            while (SelectedIndex >= Top + showEntries)
            {
                Top++;
                HoverIndex++;
            };
            while (SelectedIndex < Top)
            {
                Top--;
                HoverIndex--;
            };
            UpdateScroller();
        }

        private void UpdateScroller()
        {
            if (Scroller != null)
            {
                SetLoop();
                Scroller.Set((Tags.GTSC_Top, Top),
                    (Tags.GTSC_Total, NumEntries),
                    (Tags.GTSC_Visible, NumItemsFit()));
                ClearLoop();
            }
        }

        private void MakeVisible(int index)
        {

        }

        public int ShownEntries()
        {
            return Math.Min(NumItemsFit(), Items.Count);
        }

        public int NumItemsFit()
        {
            int numfit = (Height - 2 * LV_BORDER_Y) / TotalItemHeigh();
            return numfit;

        }

        public int TotalItemHeigh()
        {
            return ItemHeight + Spacing;
        }

        public Gadget Scroller { get; set; }
        public Gadget ShowSelected { get; set; }
        public int Top { get; set; }
        public int SelectedIndex { get; set; }
        public int HoverIndex { get; set; }
        public int Spacing { get; set; }
        public int ItemHeight { get; set; }
        public int NumEntries { get; set; }
        public IList<object> Items { get; set; }



    }
}
