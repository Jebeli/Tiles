using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Input;

namespace TileEngine.IGUI
{
    public class ToolButton : Gadget
    {
        private int skipTicks;
        public ToolButton()
            : this(TagItems.Empty)
        {
        }

        public ToolButton(params (Tags, object)[] tags)
            : base(TagItems.Empty)
        {
            Type = GadgetType.CustomGadget;
            PreferredWidth = 24;
            PreferredHeight = 24;
            New(tags);
        }

        public override GoActiveResult HandleInput(IDCMPFlags idcmp, int x, int y, MouseButton button, InputCode code)
        {
            var res = base.HandleInput(idcmp, x, y, button, code);
            if (res == GoActiveResult.MeActive)
            {
                switch (idcmp)
                {
                    case IDCMPFlags.MouseButtons:
                        if (code == InputCode.Pressed)
                        {
                            skipTicks = 2;
                        }
                        break;
                    case IDCMPFlags.IntuiTicks:
                        if (Selected && Active)
                        {
                            skipTicks--;
                            if (skipTicks <= 0)
                            {
                                skipTicks = 0;
                                Window?.Screen?.InputEvent(IDCMPFlags.GadgetDown, this, Window, x, y, button, InputCode.Repeat);
                            }
                        }
                        break;
                }
            }
            return res;
        }
    }
}
