using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.XGUI
{
    public class StrGadget : Widget
    {
        public const int STR_SIZE = 20;
        private string buffer;
        private Vector2 mouseDownPos;
        private Vector2 mousePos;
        private Vector2 mouseDragPos;
        private int cursorPos;
        private int selectionPos;
        private int glyphWidth;

        public StrGadget(Widget parent, string buffer = "")
            : base(parent)
        {
            this.buffer = buffer;
            cursorPos = -1;
            selectionPos = -1;
            mouseDownPos = new Vector2(-1, -1);
            mouseDragPos = new Vector2(-1, -1);
            mousePos = new Vector2(-1, -1);
        }

        public event EventHandler<EventArgs> Click;

        public EventHandler<EventArgs> ClickEvent
        {
            set { Click += value; }
        }
        public string Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public int LongInt
        {
            get
            {
                if (int.TryParse(buffer, out int result))
                {
                    return result;
                }
                return 0;
            }
            set
            {
                buffer = value.ToString();
            }
        }

        protected virtual void OnClick(EventArgs e)
        {
            Click?.Invoke(this, e);
        }
        private void UpdateCursor()
        {
            if (mouseDownPos.X != -1)
            {
                cursorPos = Position2CursorIndex(mouseDownPos.X);
                mouseDownPos = new Vector2(-1, -1);
            }
            else if (mouseDragPos.X != -1)
            {
                if (selectionPos == -1)
                    selectionPos = cursorPos;
                cursorPos = Position2CursorIndex(mouseDragPos.X);
            }
            else
            {
                if (cursorPos == -1)
                {
                    cursorPos = buffer.Length;
                }
            }
            if (cursorPos == selectionPos)
            {
                selectionPos = -1;
            }
        }

        private int Position2CursorIndex(int x)
        {
            x -= Left;
            x -= 3;
            x /= glyphWidth;
            if (x > buffer.Length)
            {
                x = buffer.Length;
            }
            return x;
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            glyphWidth = gfx.MeasureTextWidth(TopazFont, "W");
            Vector2 sz = new Vector2(STR_SIZE, STR_SIZE);
            sz.X += 75;
            return sz;
        }

        public override void Render(IGraphics gfx)
        {
            Theme.RenderStr(gfx, this);
            if (Active)
            {
                UpdateCursor();
                gfx.SaveState();
                gfx.Translate(Left, Top);

                if (cursorPos > -1)
                {
                    int caretX = 3 + cursorPos * glyphWidth;
                    if (selectionPos > -1)
                    {
                        int selX = 3 + selectionPos * glyphWidth;
                        if (caretX > selX)
                        {
                            int temp = selX;
                            selX = caretX;
                            caretX = temp;
                        }
                        gfx.FillRectangle(caretX, 3, selX - caretX, ClientHeight - 6, new Color(255, 255, 255, 80));
                    }
                    caretX = 3 + cursorPos * glyphWidth;
                    gfx.DrawLine(caretX, 3, caretX, ClientHeight - 3, new Color(255, 192, 0, 255));
                }
                gfx.RestoreState();
            }
            base.Render(gfx);
        }

        public override bool MouseButtonDownEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            if (button == MouseButton.Left)
            {
                widget = this;
                mouseDownPos = p;
                mouseDragPos = new Vector2(-1, -1);
                selectionPos = -1;
                return true;
            }
            else
            {
                mouseDownPos = new Vector2(-1, -1);
                mouseDragPos = new Vector2(-1, -1);
                selectionPos = -1;
                return false;
            }
        }

        public override bool MouseButtonUpEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            if (button == MouseButton.Left && Selected)
            {
                widget = this;
                mouseDownPos = new Vector2(-1, -1);
                mouseDragPos = new Vector2(-1, -1);
                return true;
            }
            else
            {
                mouseDownPos = new Vector2(-1, -1);
                mouseDragPos = new Vector2(-1, -1);
                return false;
            }
        }

        public override bool MouseMoveEvent(Vector2 p, MouseButton button, ref Widget widget)
        {
            if (Selected)
            {
                mousePos = p;
                return true;
            }
            return false;
        }

        public override bool MouseDragEvent(Vector2 p, Vector2 rel, MouseButton button)
        {
            if (Active)
            {
                mousePos = p;
                mouseDragPos = p;
                return true;
            }
            return false;
        }

        public override bool KeyboardEvent(Key key)
        {
            if (Active && Enabled)
            {
                switch (key)
                {
                    case Key.Left:
                        if (cursorPos > 0)
                        {
                            cursorPos--;
                        }
                        break;
                    case Key.Right:
                        if (cursorPos < buffer.Length)
                        {
                            cursorPos++;
                        }
                        break;
                    case Key.Home:
                        cursorPos = 0;
                        break;
                    case Key.End:
                        cursorPos = buffer.Length;
                        break;
                    case Key.Back:
                        if (!DeleteSelection())
                        {
                            if (cursorPos > 0)
                            {
                                buffer = buffer.Remove(cursorPos-1, 1);
                                cursorPos--;
                            }
                        }
                        break;
                    case Key.Delete:
                        if (!DeleteSelection())
                        {
                            if (cursorPos < buffer.Length)
                            {
                                buffer = buffer.Remove(cursorPos, 1);                                
                            }
                        }
                        break;
                    case Key.Return:
                        OnClick(EventArgs.Empty);
                        LooseFocus();
                        break;
                    default:
                        DeleteSelection();
                        buffer = buffer.Insert(cursorPos, "" + (char)key);
                        cursorPos++;
                        break;
                }
                return true;
            }
            return false;
        }

        private bool DeleteSelection()
        {
            if (selectionPos > -1)
            {
                int begin = cursorPos;
                int end = selectionPos;
                if (begin > end)
                {
                    int temp = begin;
                    begin = end;
                    end = temp;
                }
                buffer = buffer.Remove(begin, end - begin);
                cursorPos = begin;
                selectionPos = -1;
                return true;
            }
            return false;
        }
    }
}
