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
        private int textTabWidth;
        private int lineSkip;
        private int bufferSelStart;
        private int bufferSelEnd;

        public StrGadget(Gadget parent, string label = "")
            : base(parent, label)
        {
            textTabWidth = 4 * 24;
            lineSkip = 24;
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
                if (value == -1) value = buffer.Length;
                if (value > buffer.Length) value = buffer.Length;
                if (value < 0) value = 0;
                bufferSelStart = 0;
                bufferSelEnd = 0;
                bufferPos = value;
                NormSelection();
            }
        }

        public int BufferSelStart
        {
            get { return bufferSelStart; }
        }

        public int BufferSelEnd
        {
            get { return bufferSelEnd; }
        }

        private void NormSelection()
        {
            if (bufferSelStart > bufferSelEnd)
            {
                int temp = bufferSelEnd;
                bufferSelEnd = bufferSelStart;
                bufferSelStart = temp;
            }
        }

        private void SetBufferSel(int start, int end)
        {
            bufferSelStart = start;
            bufferSelEnd = end;
            NormSelection();
        }

        private void SetBufferSel(int pos)
        {
            if (pos < bufferPos)
            {
                SetBufferSel(pos, bufferPos);
            }
            else if (pos > bufferPos)
            {
                SetBufferSel(bufferSelStart, pos);
            }
            else
            {
                SetBufferSel(pos, pos);
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

        protected bool WrapAtChar(int x, char s)
        {
            return false;
        }

        protected bool MapPosition(IGraphics gfx, int mx, int my, out int pos)
        {
            pos = -1;
            int nLines = 0;
            int x = 0;
            int y = 0;
            if (my < y) return false;
            if (mx < x) return false;
            for (int i = 0; i < buffer.Length; i++)
            {
                char ch = buffer[i];
                if (WrapAtChar(x, ch))
                {
                    x = 0;
                    nLines++;
                }
                if (ch == '\n')
                {
                    x = 0;
                    nLines++;
                }
                else if (ch == '\t')
                {
                    x += textTabWidth;
                }
                else
                {
                    int gx = gfx.MeasureTextWidth(Font, "" + ch);
                    if ((my >= y && my <= (y + lineSkip)) && (mx >= x && mx <= x + gx))
                    {
                        pos = i;
                        return true;
                    }
                    x += gx;
                }
            }
            if ((my >= y && my <= (y + lineSkip)) && (mx >= x))
            {
                pos = buffer.Length;
                return true;
            }
            return false;
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

        protected override void HandleSelectDown(Vector2 p)
        {
            var gfx = Screen?.Engine?.Graphics;
            if (gfx != null)
            {
                if (MapPosition(gfx, p.X, p.Y, out int pos))
                {
                    BufferPos = pos;
                }
            }
            base.HandleSelectDown(p);
        }

        protected override void HandleSelectUp(Vector2 p)
        {
            var gfx = Screen?.Engine?.Graphics;
            if (gfx != null)
            {
                if (MapPosition(gfx, p.X, p.Y, out int pos))
                {
                    SetBufferSel(pos);
                }
            }
            base.HandleSelectUp(p);
        }

        protected override void HandleSelectMove(Vector2 p)
        {
            var gfx = Screen?.Engine?.Graphics;
            if (gfx != null)
            {
                if (MapPosition(gfx, p.X, p.Y, out int pos))
                {
                    SetBufferSel(pos);
                }
            }
            base.HandleSelectMove(p);
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
