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

    public class WidgetList : Widget
    {
        private List<WidgetListItem> items;
        private int itemHeight;
        private int topItemIndex;
        private WidgetScrollBar scrollBar;
        private int selectedIndex;
        private WidgetListItem selectedItem;
        public WidgetList()
            : base(null, null, null)
        {
            items = new List<WidgetListItem>();
            itemHeight = 30;
            scrollBar = new WidgetScrollBar();
            scrollBar.ValueChanged += ScrollBar_ValueChanged;
            AddWidget(scrollBar);
        }

        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            SelectedIndex = scrollBar.Value;
        }

        public int ItemHeight
        {
            get { return itemHeight; }
            set { itemHeight = value; }
        }

        public void Clear()
        {
            items.Clear();
            topItemIndex = 0;
            selectedIndex = -1;
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
                        selectedItem = items[value];
                        topItemIndex = value;
                        scrollBar.Value = value;
                    }
                    else
                    {
                        selectedIndex = -1;
                        selectedItem = null;
                        scrollBar.Value = 0;
                    }
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

        public WidgetListItem Add(object item)
        {
            WidgetListItem listItem = new WidgetListItem();
            listItem.Value = item;
            listItem.Text = item.ToString();
            listItem.Index = items.Count;
            items.Add(listItem);
            scrollBar.MaxValue = items.Count - 1;
            return listItem;
        }

        protected override void Draw(IGraphics graphics, int x, int y, int width, int height)
        {
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
            if (item.Image != null)
            {                
                graphics.Render(item.Image.Texture, x, y, Math.Min(itemHeight, item.Image.Width), Math.Min(itemHeight, item.Image.Height),item.Image.X,item.Image.Y,item.Image.Width,item.Image.Height);
            }
            graphics.RenderText(item.Text, x + 2 + itemHeight, y + height / 2, HorizontalTextAlign.Left);
        }

        protected override void BoundsChanged()
        {
            scrollBar.SetBounds(Width - 30, 0, 30, Height);
        }

        public class WidgetListItem
        {
            private string text;
            private object value;
            private int index;
            private TextureRegion image;

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
        }
    }
}
