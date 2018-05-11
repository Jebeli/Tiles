using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Fonts;
using TileEngine.Graphics;
using TileEngine.Input;

namespace SDLTiles
{
    public static class SDLExtensions
    {
        public static IntPtr GetTexture(this Texture tex)
        {
            if (tex != null) { return ((SDLTexture)tex).Texture; }
            return IntPtr.Zero;
        }

        public static IntPtr GetFont(this Font fnt)
        {
            if (fnt != null) { return ((SDLFont)fnt).Font; }
            return IntPtr.Zero;
        }

        public static MouseButton GetMouseButton(this SDL2.SDL.SDL_MouseButtonEvent evt)
        {
            MouseButton res = MouseButton.None;
            if (evt.button == SDL2.SDL.SDL_BUTTON_LEFT) { res |= MouseButton.Left; }
            if (evt.button == SDL2.SDL.SDL_BUTTON_RIGHT) { res |= MouseButton.Right; }
            return res;
        }
    }
}
