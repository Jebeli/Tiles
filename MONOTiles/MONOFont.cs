using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;

namespace MONOTiles
{
    public class MONOFont : Font
    {
        private MonoGame.Extended.BitmapFonts.BitmapFont font;
        public MONOFont(string name, int size, MonoGame.Extended.BitmapFonts.BitmapFont font)
            : base(name, size)
        {
            this.font = font;
        }

        public MonoGame.Extended.BitmapFonts.BitmapFont Font
        {
            get { return font; }
        }
    }
}
