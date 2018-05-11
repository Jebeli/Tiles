using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.YGUI
{
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
