using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI
{
    public class Text : GUIElement
    {
        private string value;
        public Text()
        {
            Selectable = false;
        }
        public override void HandleInput(InputEvent inputEvent)
        {

        }

        public override void RenderBack(IGraphics graphics)
        {
            RenderText(graphics, WinBox, DrawInfo.TextPen, Font, value);
        }
        public override void RenderFront(IGraphics graphics)
        {

        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
