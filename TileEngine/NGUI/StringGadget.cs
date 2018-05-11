using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.NGUI
{
    public class StringGadget : Gadget
    {
        private int bufferPos;
        private int longInt;

        public string Buffer { get; set; }
        public string UndoBuffer { get; set; }
        public int BufferPos
        {
            get { return bufferPos; }
            set
            {
                if (value < 0) value = 0;
                if (value >= NumChars) value = NumChars;
                bufferPos = value;
            }
        }
        public int MaxChars { get; set; }
        public int DispPos { get; set; }
        public int UndoPos { get; set; }
        public int NumChars
        {
            get { return Buffer.Length; }
        }

        public int DispCount { get; set; }
        public int CLeft { get; set; }
        public int CTop { get; set; }
        public StringFlags StringFlags { get; set; }
        public int LongInt
        {
            get { return longInt; }
            set
            {
                longInt = value;
                Buffer = value.ToString();
            }
        }

        public StringGadget()
        {
            GadgetFlags &= ~GadgetFlags.RelVerify;
            MaxChars = 128;
        }

        public override void RenderBack(IGraphics graphics)
        {
            IBox box = new Box(WinBox);
            if (box.IsEmpty) return;
            var di = DrawInfo;
            RenderBox(graphics, box, di.DarkEdgePen);
            box = box.Shrink(1, 1);
            ClearBox(graphics, box, di.BackPen);
            UpdateStringInfo(this);
            UpdateDisp(this);
            string dispstr = Buffer.Substring(DispPos);

            int textleft = box.Left;
            int texttop = box.Top + box.Height / 2;
            IBox textextent = null;
            int textLen = TextFit(graphics, Font, dispstr, ref textextent, box, 0, 0);
            graphics.RenderText(Font, dispstr.Substring(0, textLen), textleft, texttop, di.TextPen, HorizontalTextAlign.Left, VerticalTextAlign.Center);
            if (Active)
            {
                dispstr += " ";
                int curserpos = BufferPos - DispPos;
                textleft += TextLength(graphics, Font, dispstr, curserpos);
                string cursorText = dispstr.Substring(curserpos, 1);
                int cursorLength = TextLength(graphics, Font, cursorText, 1) - 1;
                Color sp = new Color(128, 128, 128, 64);
                ClearRect(graphics, textleft, box.Top, textleft + cursorLength, box.Bottom, sp);
            }
        }

        public override void HandleInput(InputEvent inputEvent)
        {
            base.HandleInput(inputEvent);
            switch (inputEvent.InputClass)
            {
                case InputClass.KeyUp:
                    string str = Buffer;
                    int pos = BufferPos;
                    switch (inputEvent.Key)
                    {
                        case Key.Enter:
                        case Key.Enter | Key.Shift:
                            HandleInputDone(inputEvent);
                            break;
                        case Key.Tab:
                            HandleInputDone(inputEvent);
                            break;
                        case Key.Tab | Key.Shift:
                            HandleInputDone(inputEvent);
                            break;
                        case Key.Left:
                            BufferPos--;
                            break;
                        case Key.Left | Key.Shift:
                        case Key.Home:
                            BufferPos = 0;
                            break;
                        case Key.Right:
                            BufferPos++;
                            break;
                        case Key.Right | Key.Shift:
                        case Key.End:
                            BufferPos = NumChars;
                            break;
                        case Key.Delete:
                            if (pos < str.Length)
                            {
                                str = str.Remove(pos, 1);
                                Buffer = str;
                            }
                            break;
                        case Key.Delete | Key.Shift:
                            if (pos < str.Length)
                            {
                                str = str.Remove(pos, str.Length - pos);
                                Buffer = str;
                            }
                            break;
                        case Key.Back:
                            if (pos > 0)
                            {
                                str = str.Remove(pos - 1, 1);
                                Buffer = str;
                                BufferPos--;
                            }
                            break;
                        case Key.Back | Key.Shift:
                            if (pos > 0)
                            {
                                str = str.Remove(0, pos);
                                Buffer = str;
                                BufferPos = 0;
                            }
                            break;
                        case Key.Up:
                        case Key.Down:
                            break;
                        default:
                            char c = KeyToChar(inputEvent.Key);
                            if (c != (char)0)
                            {
                                str = str.Insert(pos, c.ToString());
                                if (str.Length < MaxChars)
                                {
                                    Buffer = str;
                                    BufferPos++;
                                    UpdateStringInfo(this);
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        private void HandleInputDone(InputEvent inputEvent)
        {
            GUISystem.OnInput(new InputEvent(inputEvent)
            {
                InputClass = InputClass.GadgetUp,
                Gadget = this
            });
            Active = false;
        }

        private static char KeyToChar(Key key)
        {
            bool shift = (key & Key.Shift) == Key.Shift;
            Key rk = (key & Key.KeyCode);
            if ((rk >= Key.A) && (rk <= Key.Z))
            {
                return shift ? char.ToUpperInvariant((char)rk) : char.ToLowerInvariant((char)rk);
            }
            if ((rk >= Key.D0) && (rk <= Key.D9))
            {
                return (char)rk;
            }
            if (rk == Key.Space)
            {
                return (char)rk;
            }
            return (char)0;
        }

        private static void UpdateStringInfo(StringGadget gadget)
        {
            if (gadget != null)
            {
                if (gadget.BufferPos > gadget.NumChars)
                {
                    gadget.BufferPos = gadget.NumChars;
                }
                bool intGad = gadget.StringFlags.HasFlag(StringFlags.LongInt);
                if (intGad)
                {
                    if (int.TryParse(gadget.Buffer, out int num))
                    {
                        gadget.LongInt = num;
                    }
                    else if (!string.IsNullOrEmpty(gadget.Buffer))
                    {
                        gadget.LongInt = gadget.longInt;
                    }
                }
            }
        }

        private static void UpdateDisp(StringGadget gadget)
        {
            if (gadget != null)
            {

            }
        }


        private static int TextLength(IGraphics graphics, Font font, string text, int length)
        {
            if (length == 0) return 0;
            text = text.Substring(0, length);
            return graphics.MeasureTextWidth(font, text);
        }

        private static int TextFit(IGraphics graphics, Font font, string text, ref IBox textExtent, IBox constrainingExtent, int constrainingBitWidth, int constrainingBitHeight)
        {
            if (textExtent == null) textExtent = new Box();
            int tw = graphics.MeasureTextWidth(font, text);
            textExtent.Width = tw;
            int retVal = text.Length;
            if (constrainingExtent != null)
            {
                constrainingBitWidth = constrainingExtent.Width;
                constrainingBitHeight = constrainingExtent.Height;
            }
            if (tw <= constrainingBitWidth)
            {
                return retVal;
            }
            while (retVal > 0)
            {
                retVal--;
                string txt = text.Substring(0, retVal);
                tw = graphics.MeasureTextWidth(font, txt);
                if (tw <= constrainingBitWidth)
                {
                    return retVal;
                }
            }
            return 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetType().Name);
            sb.Append(": ");
            sb.Append(Text);
            sb.Append(" B");
            sb.Append(Buffer);
            return sb.ToString();
        }
    }
}
