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
    using TileEngine.Input;

    public class StrGadget : Gadget
    {
        private string buffer;
        private int bufferPos;
        private int labelOffset;

        public StrGadget(Gadget parent, string label = "")
            : base(parent, label)
        {

        }

        public string Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public int BufferPos
        {
            get { return bufferPos; }
            set
            {
                if (value > buffer.Length) value = buffer.Length;
                if (value < 0) value = 0;
                bufferPos = value;
            }
        }

        public Rect LabelBounds
        {
            get
            {
                Rect res = Bounds;
                if (labelOffset > 0)
                {
                    res.Width = labelOffset;
                }
                return res;
            }
        }

        public Rect ContainerBounds
        {
            get
            {
                Rect res = Bounds;
                if (labelOffset > 0)
                {
                    res.Offset(labelOffset, 0);
                    res.Width -= labelOffset;
                }
                return res;
            }
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            int labelW = 0;
            if (!string.IsNullOrEmpty(Label))
            {
                labelW = gfx.MeasureTextWidth(Font, Label);
                labelW += 4;
                labelOffset = labelW;
            }
            int buffW = gfx.MeasureTextWidth(Font, buffer);
            buffW += 4;
            return new Vector2(labelW + buffW, 24);
        }

        protected override void RenderGadget(IGraphics gfx)
        {
            theme.RenderGadget(gfx, this);
        }

        public override void HandleKeyDown(Key keyData, Key keyCode, char code)
        {
            switch (keyData)
            {
                case Key.Left:
                    BufferPos--;
                    break;
                case Key.Right:
                    BufferPos++;
                    break;
                case Key.Home:
                    BufferPos = 0;
                    break;
                case Key.End:
                    BufferPos = buffer.Length;
                    break;
                case Key.Delete:
                    if (bufferPos < buffer.Length)
                    {
                        buffer = buffer.Remove(bufferPos, 1);
                    }
                    break;
                case Key.Back:
                    if (bufferPos > 0)
                    {
                        buffer = buffer.Remove(bufferPos - 1, 1);
                        bufferPos--;
                    }
                    break;
            }
            if (char.IsLetterOrDigit(code) || char.IsPunctuation(code) || code == ' ')
            {
                buffer = buffer.Insert(bufferPos, "" + code);
                bufferPos++;
            }
        }

        public override void HandleKeyUp(Key keyData, Key keyCode, char code)
        {
            if (keyData == Key.Return)
            {
                OnGadgetUp();
                UnFocus();
            }
        }
    }
}
