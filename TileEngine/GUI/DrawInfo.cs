using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI
{
    public class DrawInfo
    {
        public const int DETAILPEN = 0x0000;
        public const int BLOCKPEN = 0x0001;
        public const int TEXTPEN = 0x0002;
        public const int SHINEPEN = 0x0003;
        public const int SHADOWPEN = 0x0004;
        public const int FILLPEN = 0x0005;
        public const int FILLTEXTPEN = 0x0006;
        public const int BACKGROUNDPEN = 0x0007;
        public const int HIGHLIGHTTEXTPEN = 0x0008;
        public const int HOVERSHINEPEN = 0x0009;
        public const int HOVERSHADOWPEN = 0x000A;
        public const int HOVERBACKGROUNDPEN = 0x000B;
        public const int INACTIVEHOVERBACKGROUNDPEN = 0x000C;
        public const int DISABLEDTEXTPEN = 0x000D;
        public const int PROPCLEARPEN = 0x000E;

        public IList<Color> Pens { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Color DetailPen { get { return GetPen(DETAILPEN); } set { SetPen(DETAILPEN, value); } }
        public Color BlockPen { get { return GetPen(BLOCKPEN); } set { SetPen(BLOCKPEN, value); } }
        public Color TextPen { get { return GetPen(TEXTPEN); } set { SetPen(TEXTPEN, value); } }
        public Color ShinePen { get { return GetPen(SHINEPEN); } set { SetPen(SHINEPEN, value); } }
        public Color ShadowPen { get { return GetPen(SHADOWPEN); } set { SetPen(SHADOWPEN, value); } }
        public Color FillPen { get { return GetPen(FILLPEN); } set { SetPen(FILLPEN, value); } }
        public Color FillTextPen { get { return GetPen(FILLTEXTPEN); } set { SetPen(FILLTEXTPEN, value); } }
        public Color BackgoundPen { get { return GetPen(BACKGROUNDPEN); } set { SetPen(BACKGROUNDPEN, value); } }
        public Color HighlightTextPen { get { return GetPen(HIGHLIGHTTEXTPEN); } set { SetPen(HIGHLIGHTTEXTPEN, value); } }
        public Color HoverShinePen { get { return GetPen(HOVERSHINEPEN); } set { SetPen(HOVERSHINEPEN, value); } }
        public Color HoverShadowPen { get { return GetPen(HOVERSHADOWPEN); } set { SetPen(HOVERSHADOWPEN, value); } }
        public Color HoverBackgroundPen { get { return GetPen(HOVERBACKGROUNDPEN); } set { SetPen(HOVERBACKGROUNDPEN, value); } }
        public Color InactiveHoverBackgroundPen { get { return GetPen(INACTIVEHOVERBACKGROUNDPEN); } set { SetPen(INACTIVEHOVERBACKGROUNDPEN, value); } }
        public Color DisabledTextPen { get { return GetPen(DISABLEDTEXTPEN); } set { SetPen(DISABLEDTEXTPEN, value); } }
        public Color PropClearPen { get { return GetPen(PROPCLEARPEN); } set { SetPen(PROPCLEARPEN, value); } }

        public Color GetPen(int pen)
        {
            if (pen >= 0 && Pens != null && pen < Pens.Count)
            {
                return Pens[pen];
            }
            return Color.Black;
        }

        public void SetPen(int pen, Color value)
        {
            if (pen >= 0)
            {
                if (Pens == null) Pens = new List<Color>();
                int diff = pen + 1 - Pens.Count;
                for (int i = 0; i < diff; i++)
                {
                    Pens.Add(new Color());
                }
                Pens[pen] = value;
            }
        }
    }
}
