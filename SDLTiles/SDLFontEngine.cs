using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;
using SDL2;

namespace SDLTiles
{
    public class SDLFontEngine : AbstractFontEngine
    {
        public SDLFontEngine()
        {
            SDL_ttf.TTF_Init();
        }
        protected override Font MakeFont(string name, int size)
        {
            IntPtr ft = SDL_ttf.TTF_OpenFont(name, size);
            if (ft != IntPtr.Zero)
            {
                return new SDLFont(name, size, ft);
            }
            return null;
        }

        protected override void DestroyFont(Font font)
        {
            IntPtr ft = font.GetFont();
            SDL_ttf.TTF_CloseFont(ft);
        }
    }
}
