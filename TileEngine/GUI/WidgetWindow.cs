using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI
{
    public class WidgetWindow : Widget
    {

        protected override void Draw(IGraphics graphics, int x, int y, int width, int height)
        {
            if (!DrawNinePatch(graphics, x, y, width, height))
            {
                graphics.RenderWidget(x, y, width, height, Enabled, Hover, Pressed);
            }
        }

        protected override void BoundsChanged()
        {

        }
    }
}
