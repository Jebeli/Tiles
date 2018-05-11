using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.GUI
{
    public class StringGadget : Gadget
    {
        public StringGadget()
        {
            GadgetType &= ~GadgetType.GTYPEMASK;
            GadgetType |= GadgetType.STRGADGET;
            Flags = GadgetFlags.BOOPSIGADGET;
            StringInfo = new StringInfo()
            {
                MaxChars = 128
            };
        }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.STRINGA_LongVal:
                    StringInfo.LongInt = tag.GetTagData(0);
                    StringInfo.Buffer = StringInfo.LongInt.ToString();
                    StringInfo.BufferPos = StringInfo.Buffer.Length;
                    Activation |= GadgetActivation.LONGINT;
                    return 1;
                case Tags.STRINGA_TextVal:
                    StringInfo.Buffer = tag.GetTagData("");
                    StringInfo.BufferPos = StringInfo.Buffer.Length;
                    Activation &= ~GadgetActivation.LONGINT;
                    return 1;
                case Tags.STRINGA_MaxChars:
                    StringInfo.MaxChars = tag.GetTagData(128);
                    return 0;
                default:
                    return base.SetTag(gadgetInfo, set, update, tag);
            }

        }

        public override GadgetActive HandleInput(GadgetInfo gadgetInfo, InputEvent inputEvent, ref int termination, int mouseX, int mouseY)
        {
            if (inputEvent.InputClass == InputClass.MOUSEDOWN)
            {

            }
            if (inputEvent.InputClass == InputClass.KEYDOWN)
            {

            }
            if (inputEvent.InputClass == InputClass.KEYUP)
            {
                string str = StringInfo.Buffer;
                int pos = StringInfo.BufferPos;
                switch (inputEvent.Key)
                {
                    case Key.Enter:
                    case Key.Enter | Key.Shift:
                        NotifyVal(gadgetInfo, true);
                        return GadgetActive.NextActive;
                    case Key.Tab:
                        NotifyVal(gadgetInfo, true);
                        return GadgetActive.NextActive;
                    case Key.Tab | Key.Shift:
                        NotifyVal(gadgetInfo, true);
                        return GadgetActive.PrevActive;
                    case Key.Left:
                        StringInfo.BufferPos--;
                        break;
                    case Key.Left | Key.Shift:
                    case Key.Home:
                        StringInfo.BufferPos = 0;
                        break;
                    case Key.Right:
                        StringInfo.BufferPos++;
                        break;
                    case Key.Right | Key.Shift:
                    case Key.End:
                        StringInfo.BufferPos = StringInfo.NumChars;
                        break;
                    case Key.Delete:
                        if (pos < str.Length)
                        {
                            str = str.Remove(pos, 1);
                            StringInfo.Buffer = str;
                        }
                        break;
                    case Key.Delete | Key.Shift:
                        if (pos < str.Length)
                        {
                            str = str.Remove(pos, str.Length - pos);
                            StringInfo.Buffer = str;
                        }
                        break;
                    case Key.Back:
                        if (pos > 0)
                        {
                            str = str.Remove(pos - 1, 1);
                            StringInfo.Buffer = str;
                            StringInfo.BufferPos--;
                        }
                        break;
                    case Key.Back | Key.Shift:
                        if (pos > 0)
                        {
                            str = str.Remove(0, pos);
                            StringInfo.Buffer = str;
                            StringInfo.BufferPos = 0;
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
                            if (str.Length < StringInfo.MaxChars)
                            {
                                StringInfo.Buffer = str;
                                StringInfo.BufferPos++;
                                UpdateStringInfo(this);
                            }
                        }
                        break;
                }
            }
            return GadgetActive.MeActive;
        }

        private void NotifyVal(GadgetInfo gadgetInfo, bool final)
        {
            UpdateFlags flags = final ? UpdateFlags.Final : UpdateFlags.Interim;
            if ((Activation & GadgetActivation.LONGINT) == GadgetActivation.LONGINT)
            {
                Notify(gadgetInfo, flags, (Tags.STRINGA_LongVal, StringInfo.LongInt), (Tags.GA_ID, GadgetId));
            }
            else
            {
                Notify(gadgetInfo, flags, (Tags.STRINGA_TextVal, StringInfo.Buffer), (Tags.GA_ID, GadgetId));
            }
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

        public override void Render(GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw)
        {
            Render(this, gadgetInfo, graphics, redraw);
        }

        private static void Render(Gadget gadget, GadgetInfo gadgetInfo, IGraphics graphics, GadgetRedraw redraw)
        {
            if (gadget != null)
            {
                IBox container = null;
                gadget.GetGadgetIBox(gadgetInfo, ref container);
                if ((container.Width <= 1) || (container.Height <= 1))
                    return;
                if (gadget.GadgetImage != null)
                {
                    gadget.RenderBaseFrame(gadgetInfo, graphics, redraw, container);
                    container.LeftEdge += 2;
                    container.TopEdge += 2;
                    container.Width -= 4;
                    container.Height -= 4;
                }
                StringInfo si = gadget.StringInfo;
                if (si != null)
                {
                    UpdateStringInfo(gadget);
                    UpdateDisp(gadget);
                    graphics.DrawRect(container, gadgetInfo.DrawInfo.ShadowPen);
                    container.LeftEdge++;
                    container.TopEdge++;
                    container.Width -= 2;
                    container.Height -= 2;
                    graphics.RectFill(container, gadgetInfo.DrawInfo.PropClearPen);

                    string dispstr = si.Buffer.Substring(si.DispPos);

                    int textleft = container.LeftEdge;
                    int texttop = container.TopEdge + container.Height / 2;
                    IBox textextent = null;
                    int textLen = graphics.TextFit(gadget.Font, dispstr, ref textextent, container, 0, 0);
                    graphics.RenderText(gadget.Font, dispstr.Substring(0, textLen), textleft, texttop, gadgetInfo.DrawInfo.TextPen, HorizontalTextAlign.Left, VerticalTextAlign.Center);
                    if ((gadget.Activation & GadgetActivation.ACTIVEGADGET) == GadgetActivation.ACTIVEGADGET)
                    {
                        dispstr += " ";
                        int curserpos = si.BufferPos - si.DispPos;
                        textleft += TextLength(graphics, gadget.Font, dispstr, curserpos);
                        string cursorText = dispstr.Substring(curserpos, 1);
                        int cursorLength = TextLength(graphics, gadget.Font, cursorText, 1) - 1;
                        Color sp = new Color(128, 128, 128, 64);
                        graphics.RectFill(textleft, container.TopEdge, textleft + cursorLength, container.BottomEdge, sp);
                    }
                }
                Intuition.DrawIntuiText(graphics, gadget.GadgetText, container.LeftEdge, container.TopEdge);
            }

        }

        private static int TextLength(IGraphics rp, Font font, string text, int length)
        {
            if (length == 0) return 0;
            text = text.Substring(0, length);
            return rp.MeasureTextWidth(font, text);
        }

        private static void UpdateStringInfo(Gadget gadget)
        {
            if (gadget != null)
            {
                StringInfo si = gadget.StringInfo;
                if (si != null)
                {
                    if (si.BufferPos > si.NumChars)
                    {
                        si.BufferPos = si.NumChars;
                    }
                    bool intGad = ((gadget.Activation & GadgetActivation.LONGINT) == GadgetActivation.LONGINT);
                    if (intGad)
                    {
                        int num;
                        if (int.TryParse(si.Buffer, out num))
                        {
                            si.LongInt = num;
                        }
                    }

                }
            }
        }

        private static void UpdateDisp(Gadget gadget)
        {
            if (gadget != null)
            {
                StringInfo si = gadget.StringInfo;
                if (si != null)
                {

                }
            }
        }

    }
}
