﻿/*
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
    using TileEngine.Input;

    public class ChooserGadget : Gadget
    {
        private List<object> items;
        private int selectedIndex;
        private PopupWindow popup;

        public ChooserGadget(Gadget parent)
            : base(parent)
        {
            RelVerify = false;
            selectedIndex = -1;
            items = new List<object>();
            Size = new Point(64 + 24, 24);
        }

        public event EventHandler<EventArgs> SelectedIndexChanged;
        public EventHandler<EventArgs> SelectedIndexChangedEvent
        {
            set { SelectedIndexChanged += value; }
        }

        public IList<object> Items
        {
            get { return items; }
            set
            {
                items = new List<object>(value);
                SelectedIndex = selectedIndex;
            }
        }

        protected virtual void OnSelectedIndexChanged()
        {
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value < 0) value = 0;
                if (value >= items.Count) value = items.Count - 1;
                if (value != selectedIndex)
                {
                    selectedIndex = value;
                    if (selectedIndex >= 0)
                    {
                        Label = items[selectedIndex].ToString();
                    }
                    else
                    {
                        Label = "";
                    }
                    OnSelectedIndexChanged();
                }
            }
        }

        private void TogglePopup()
        {
            if (popup != null && popup.Visible)
            {
                HidePopup();
            }
            else
            {
                ShowPopup();
            }
        }

        private void ShowPopup()
        {
            HidePopup();
            popup = new PopupWindow(this)
            {
                Position = AbsolutePosition
            };
            popup.TopEdge += Height;
            popup.FixedWidth = Width;
            List<ItemGadget> list = new List<ItemGadget>();
            int index = 0;
            foreach (var item in items)
            {
                list.Add(new ItemGadget(popup, item.ToString())
                {
                    Id = index,
                    Selected = index == selectedIndex,
                    Items = list,
                    GadgetUpEvent = Item_GadgetUp
                });

                index++;
            }
            Screen.ShowWindow(popup);
        }

        private void HidePopup()
        {
            if (popup != null)
            {
                Screen.CloseWindow(popup);
                popup = null;
            }
        }

        private void Item_GadgetUp(object sender, EventArgs e)
        {
            int index = ((Gadget)sender).Id;
            HidePopup();
            SelectedIndex = index;
        }

        protected override void HandleSelectUp(Point p)
        {
            base.HandleSelectUp(p);
            TogglePopup();
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }
    }
}
