using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;

namespace TileEngine.Nano
{
    public class ToolButton : Button
    {
        public ToolButton(Widget parent, Icons icon)
            : base(parent, "", icon)
        {
            Flags = ButtonFlags.RadioButton | ButtonFlags.ToggleButton;
            FixedSize = new Vector2(25, 25);            
        }
    }
}
