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
    public class ToolButton : Button
    {
        public const int TOOLBUTTON_SIZE = 22;
        public ToolButton(Widget parent, Icons icon, EventHandler<EventArgs> eventHandler = null)
            : base(parent, "", icon, eventHandler)
        {            
            IconPlacement = IconPlacement.Center;
            FixedSize = new Vector2(TOOLBUTTON_SIZE, TOOLBUTTON_SIZE);
            Size = new Vector2(TOOLBUTTON_SIZE, TOOLBUTTON_SIZE);
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            return new Vector2(TOOLBUTTON_SIZE, TOOLBUTTON_SIZE);
        }

        public override bool TimerEvent(Vector2 p, MouseButton button)
        {
            if (Selected && button == MouseButton.Left)
            {
                OnClick(EventArgs.Empty);
            }
            return false;
        }
    }
}
