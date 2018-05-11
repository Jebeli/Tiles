using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;

namespace SDLTiles
{
    public class SDLFont : Font
    {
        private IntPtr font;
        public SDLFont(string name, int size, IntPtr font)
            : base(name, size)
        {
            this.font = font;
        }

        public IntPtr Font
        {
            get { return font; }
        }
    }
}
