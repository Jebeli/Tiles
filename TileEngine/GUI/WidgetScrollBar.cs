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

    public class WidgetScrollBar : Widget
    {
        private WidgetButton buttonUp;
        private WidgetButton buttonDown;
        private int minValue;
        private int maxValue;
        private int value;
        private double knobSize = 1.0;
        private double knobPosition = 0.0;
        private NinePatch knobPatch;
        private NinePatch knobPatchPressed;
        private bool knobPressed;
        private int knobStartY;
        public WidgetScrollBar()
            : base(WidgetFactory.Window9P)
        {
            knobPatch = WidgetFactory.Button9P;
            knobPatchPressed = WidgetFactory.Button9PPressed;
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

        public double KnobSize
        {
            get { return knobSize; }
            set { knobSize = value; }
        }

        public double KnobPosition
        {
            get { return knobPosition; }
            set { knobPosition = value; }
        }

        public NinePatch KnobPatch
        {
            get { return knobPatch; }
            set { knobPatch = value; }
        }
        public int MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                AdjustKnobSize();
                AdjustKnobPosition();
            }
        }

        public int MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                AdjustKnobSize();
                AdjustKnobPosition();
            }
        }

        private void AdjustKnobSize()
        {
            int range = maxValue - minValue;
            knobSize = 1.0 / range;
        }

        private void AdjustKnobPosition()
        {
            int range = maxValue - minValue;
            int pos = value - minValue;
            knobPosition = pos;
            knobPosition /= range;
        }

        private void AdjustValue()
        {
            int range = maxValue - minValue;
            int pos = (int)(knobPosition * range);
            Value = pos - minValue;
        }

        private void MoveKnob(int mouseX, int mouseY)
        {
            int x;
            int y;
            int width;
            int height;
            CalcBounds(out x, out y, out width, out height);
            int range = maxValue - minValue;
            int kx;
            int ky;
            int kw;
            int kh;
            GetKnobRect(out kx, out ky, out kw, out kh);
            int innerHeight = height - buttonDown.Height - buttonUp.Height - kh;
            double kPos = mouseY - y - buttonDown.Height - knobStartY;
            kPos /= innerHeight;
            if (kPos > 1.0) kPos = 1.0;
            if (kPos < 0.0) kPos = 0.0;
            knobPosition = kPos;
            AdjustValue();
        }

        public int Value
        {
            get { return value; }
            set
            {
                if (value > maxValue) value = maxValue;
                if (value < minValue) value = minValue;
                if (this.value != value)
                {
                    this.value = value;
                    AdjustKnobSize();
                    AdjustKnobPosition();
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

        protected override void OnMouseDown(int x, int y, MouseButton button)
        {
            knobPressed = false;
            if (KnobContains(x, y))
            {
                int kx;
                int ky;
                int kw;
                int kh;
                GetKnobRect(out kx, out ky, out kw, out kh);
                knobStartY = y - ky;
                knobPressed = true;
            }
        }

        protected override void OnMouseUp(int x, int y, MouseButton button)
        {
            if (knobPressed)
            {
                MoveKnob(x, y);
            }
            knobPressed = false;
        }

        protected override void OnMouseMove(int x, int y, MouseButton button)
        {
            if (knobPressed)
            {
                MoveKnob(x, y);
            }
        }

        private bool KnobContains(int mouseX, int mouseY)
        {
            int x;
            int y;
            int width;
            int height;
            GetKnobRect(out x, out y, out width, out height);
            return (mouseX >= x && mouseY >= y && mouseX < x + width && mouseY < y + height);
        }

        protected void GetKnobRect(out int x, out int y, out int width, out int height)
        {
            CalcBounds(out x, out y, out width, out height);
            int innerHeight = Height - buttonDown.Height - buttonUp.Height;
            int kh = (int)(innerHeight * knobSize);
            if (kh < 30) kh = 30;
            int ky = y + buttonUp.Height + (int)((innerHeight - kh) * KnobPosition);
            y = ky;
            height = kh;
        }


        protected override void Draw(IGraphics graphics, int x, int y, int width, int height)
        {
            if (!DrawNinePatch(graphics, x, y, width, height))
            {
                graphics.RenderWidget(x, y, width, height, Enabled, Hover, Pressed);
            }
            if (knobPressed && knobPatchPressed != null)
            {
                GetKnobRect(out x, out y, out width, out height);
                knobPatchPressed.Draw(graphics, x, y, width, height);
            }
            else if (knobPatch != null)
            {
                GetKnobRect(out x, out y, out width, out height);
                knobPatch.Draw(graphics, x, y, width, height);
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
