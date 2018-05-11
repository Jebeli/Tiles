using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.NGUI
{
    public class DrawInfo
    {
        public Color BackPen { get; set; }
        public Color FrontPen { get; set; }
        public Color HoverBackPen { get; set; }
        public Color HoverFrontPen { get; set; }
        public Color SelectedBackPen { get; set; }
        public Color SelectedFrontPen { get; set; }
        public Color ActiveBorderPen { get; set; }
        public Color HoverActiveBorderPen { get; set; }
        public Color ShinePen { get; set; }
        public Color ShadowPen { get; set; }
        public Color DarkEdgePen { get; set; }

        public Color TextPen { get; set; }
    }
}
