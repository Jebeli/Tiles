using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Resources;

namespace TileEngine.Fonts
{
    public abstract class AbstractFontEngine : IFontEngine
    {
        private ResourceManager<Font> fontManager;
        private Font defaultFont;
        private Font iconFont;
        private Font topazFont;
        public AbstractFontEngine()
        {
            fontManager = new ResourceManager<Font>();
        }

        public Font DefaultFont
        {
            get { return defaultFont; }
        }

        public Font IconFont
        {
            get { return iconFont; }
        }

        public Font TopazFont
        {
            get { return topazFont; }
        }
        public Font OpenFont(string name, int size)
        {
            string fn = Font.GetFontName(name, size);
            Font font = fontManager.Get(fn);
            if (font == null)
            {
                font = MakeFont(name, size);
                if (font != null)
                {
                    fontManager.Add(font);
                }
            }
            return font;
        }

        public void CloseFont(Font font)
        {
            DestroyFont(font);
            fontManager.Free(font);
        }

        public void Init(string defaultFontName, int defaultFontSize, string topazFontName, int topazFontSize, string iconFontName, int iconFontSize)
        {
            defaultFont = OpenFont(defaultFontName, defaultFontSize);
            iconFont = OpenFont(iconFontName, iconFontSize);
            topazFont = OpenFont(topazFontName, topazFontSize);
        }

        protected abstract Font MakeFont(string name, int size);
        protected abstract void DestroyFont(Font font);
    }
}
