using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Resources;

namespace TileEngine.Fonts
{
    public class Font : Resource
    {
        private string fontName;
        private int fontSize;
        public Font(string name, int size)
            : base(GetFontName(name, size))
        {
            fontName = name;
            fontSize = size;
        }

        public string FontName
        {
            get { return fontName; }
        }

        public int FontSize
        {
            get { return fontSize; }
        }

        public static string GetFontName(string name, int size)
        {
            return name + "_" + size;
        }
    }
}
