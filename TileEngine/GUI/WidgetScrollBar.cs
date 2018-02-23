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

    public class WidgetScrollBar : Widget
    {
        private WidgetButton buttonUp;
        private WidgetButton buttonDown;
        private int minValue;
        private int maxValue;
        private int value;
        public WidgetScrollBar()
            : base(WidgetFactory.Window9P)
        {
            maxValue = 100;
            buttonUp = new WidgetButton("^");
            buttonUp.Repeat = true;
            buttonUp.Click += ButtonUp_Click;
            buttonDown = new WidgetButton("v");
            buttonDown.Repeat = true;
            buttonDown.Click += ButtonDown_Click;
            AddWidget(buttonUp);
            AddWidget(buttonDown);
        }

        public event EventHandler<EventArgs> ValueChanged;

        public int MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        public int Value
        {
            get { return value; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnValueChanged();
                }
            }
        }

        private void ButtonDown_Click(object sender, EventArgs e)
        {
            Value = Math.Min(value + 1, maxValue);
        }

        private void ButtonUp_Click(object sender, EventArgs e)
        {
            Value = Math.Max(value - 1, minValue);
        }

        protected override void Draw(IGraphics graphics, int x, int y, int width, int height)
        {
            if (!DrawNinePatch(graphics, x, y, width, height))
            {
                graphics.RenderWidget(x, y, width, height, Enabled, Hover, Pressed);
            }
        }

        protected override void BoundsChanged()
        {
            buttonUp.SetBounds(0, 0, 30, 30);
            buttonDown.SetBounds(0, Height - 30, 30, 30);
        }

        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
