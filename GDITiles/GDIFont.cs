using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;

namespace GDITiles
{
    public class GDIFont : Font
    {
        private System.Drawing.Font font;
        public GDIFont(string name, int size, System.Drawing.Font font)
            :base(name, size)
        {
            this.font = font;
        }

        public System.Drawing.Font Font
        {
            get { return font; }
        }
    }
}
