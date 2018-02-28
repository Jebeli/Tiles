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
    using Input;

    public class WidgetList : Widget
    {
        private List<WidgetListItem> items;
        private int itemHeight;
        private int topItemIndex;
        private WidgetScrollBar scrollBar;
        private int selectedIndex;
        private WidgetListItem selectedItem;
        public WidgetList()
            : base(WidgetFactory.Window9P)
        {
            items = new List<WidgetListItem>();
            itemHeight = 30;
            scrollBar = new WidgetScrollBar();
            scrollBar.ValueChanged += ScrollBar_ValueChanged;
            AddWidget(scrollBar);
        }

        public event EventHandler<EventArgs> SelectedIndexChanged;

        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            TopItemIndex = scrollBar.Value;
        }

        public int ItemHeight
        {
            get { return itemHeight; }
            set { itemHeight = value; }
        }

        public void Clear()
        {
            items.Clear();
            selectedIndex = -1;
            TopItemIndex = 0;
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (selectedIndex != value)
                {
                    if (value >= 0 && value < items.Count)
                    {
                        selectedIndex = value;
                        if (selectedItem != null)
                        {
                            selectedItem.Selected = false;
                        }
                        selectedItem = items[value];
                        selectedItem.Selected = true;
                        EnsureVisible(value);
                    }
                    else
                    {
                        selectedIndex = -1;
                        selectedItem = null;
                        TopItemIndex = 0;
                    }
                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void EnsureVisible(int index, int pad = 1)
        {
            if (index - pad < topItemIndex)
            {
                TopItemIndex = index - pad;
            }
            int numItems = VisibleItems;
            if (index + pad >= topItemIndex + numItems)
            {
                TopItemIndex = index + pad - (numItems - 1);
            }
        }

        public int TopItemIndex
        {
            get { return topItemIndex; }
            set
            {
                if (value > items.Count - VisibleItems) value = items.Count - VisibleItems;
                if (value < 0) value = 0;
                if (topItemIndex != value)
                {
                    topItemIndex = value;
                    scrollBar.Value = value;
                }
            }
        }

        public WidgetListItem SelectedItem
        {
            get { return selectedItem; }
        }

        public IList<WidgetListItem> Items
        {
            get { return items; }
        }

        public int VisibleItems
        {
            get { return Height / itemHeight; }
        }

        public WidgetListItem Add(object item)
        {
            WidgetListItem listItem = new WidgetListItem();
            listItem.Value = item;
            listItem.Text = item.ToString();
            listItem.Index = items.Count;
            items.Add(listItem);
            scrollBar.MaxValue = Math.Max(0, items.Count - VisibleItems);
            return listItem;
        }

        protected override void Draw(IGraphics graphics, int x, int y, int width, int height)
        {
            if (!DrawNinePatch(graphics, x, y, width, height))
            {
                graphics.RenderWidget(x, y, width, height, Enabled, Hover, Pressed);
            }
            int numItems = height / itemHeight;
            int index = topItemIndex;
            int itemCount = items.Count;
            for (int i = 0; i < numItems && index < itemCount; i++)
            {
                var item = items[index];
                DrawItem(item, graphics, x, y, width, itemHeight);
                index++;
                y += itemHeight;
            }
        }

        private void DrawItem(WidgetListItem item, IGraphics graphics, int x, int y, int width, int height)
        {
            if (item.Selected)
            {
                if (PatchPressed != null)
                {
                    PatchPressed.Draw(graphics, x, y, width, height);
                }
            }
            if (item.Image != null)
            {
                graphics.Render(item.Image.Texture, x, y, Math.Min(itemHeight, item.Image.Width), Math.Min(itemHeight, item.Image.Height), item.Image.X, item.Image.Y, item.Image.Width, item.Image.Height);
            }
            graphics.RenderText(item.Text, x + 2 + itemHeight, y + height / 2, GetTextColor(), HorizontalTextAlign.Left);
        }

        protected override void BoundsChanged()
        {
            scrollBar.SetBounds(Width - 30, 0, 30, Height);
        }

        protected override void OnMouseUp(int x, int y, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                int left;
                int top;
                int width;
                int height;
                CalcBounds(out left, out top, out width, out height);

                y -= top;
                int index = y / itemHeight;
                index += topItemIndex;
                SelectedIndex = index;
            }
        }

        public class WidgetListItem
        {
            private string text;
            private object value;
            private int index;
            private TextureRegion image;
            private bool selected;

            public int Index
            {
                get { return index; }
                set { index = value; }
            }
            public string Text
            {
                get { return text; }
                set { text = value; }
            }

            public TextureRegion Image
            {
                get { return image; }
                set { image = value; }
            }

            public object Value
            {
                get { return value; }
                set { this.value = value; }
            }

            public bool Selected
            {
                get { return selected; }
                set { selected = value; }
            }
        }
    }
}
