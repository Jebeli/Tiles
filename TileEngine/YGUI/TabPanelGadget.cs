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

    public class TabPanelGadget : Gadget
    {
        private List<TabGadget> tabs;
        private List<TabHeaderGadget> tabHeaders;
        private int selectedIndex;

        public TabPanelGadget(Gadget parent)
            : base(parent)
        {
            tabs = new List<TabGadget>();
            tabHeaders = new List<TabHeaderGadget>();
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value < 0) value = 0;
                if (value >= tabs.Count) value = tabs.Count - 1;
                if (selectedIndex != value)
                {
                    selectedIndex = value;
                }
            }
        }

        private void AdjustTab(IGraphics gfx)
        {
            int index = 0;
            int x = 0;
            int hw = Width / tabs.Count;
            int hh = 24;
            foreach (var tab in tabs)
            {
                var head = tabHeaders[index];
                head.LeftEdge = x;
                head.TopEdge = 0;
                head.Width = hw;
                if (index == tabs.Count - 1)
                {
                    while (head.LeftEdge + head.Width < Width)
                    {
                        head.Width++;
                    }
                }
                head.Height = hh;
                head.Id = index;
                if (index == selectedIndex)
                {
                    head.Selected = true;
                    tab.Visible = true;
                    tab.LeftEdge = 0;
                    tab.TopEdge = 24;
                    tab.Width = Width;
                    tab.Height = Height - 24;
                    tab.PerformLayout(gfx);
                }
                else
                {
                    head.Selected = false;
                    tab.Visible = false;
                }
                index++;
                x += hw;
            }
        }

        public TabGadget SelectedTab
        {
            get
            {
                if (selectedIndex >= 0)
                    return tabs[selectedIndex];
                return null;
            }
        }

        public TabGadget AddTab(string label, Orientation orientation)
        {
            TabGadget tab = new TabGadget(this, label, orientation, Alignment.Fill);
            tabs.Add(tab);
            TabHeaderGadget head = new TabHeaderGadget(this, label);
            head.GadgetUp += Head_GadgetUp;
            tabHeaders.Add(head);
            return tab;
        }

        private void Head_GadgetUp(object sender, EventArgs e)
        {
            int index = ((Gadget)sender).Id;
            SelectedIndex = index;
            Window.Invalidate();
        }

        public void RemTab(TabGadget tab)
        {
            int index = tabs.IndexOf(tab);
            if (index >= 0)
            {
                tabs.RemoveAt(index);
                tabHeaders.RemoveAt(index);
            }
        }

        public override Point GetPreferredSize(IGraphics gfx)
        {
            Point pSize = new Point(tabs.Count * 64, 0);
            foreach (var tab in tabs)
            {
                Point tSize = tab.GetPreferredSize(gfx);
                pSize.X = Math.Max(pSize.X, tSize.X);
                pSize.Y = Math.Max(pSize.Y, tSize.Y);
            }
            pSize.Y += 24;
            return pSize;
        }

        public override void PerformLayout(IGraphics gfx)
        {
            base.PerformLayout(gfx);
            AdjustTab(gfx);
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }
    }
}
