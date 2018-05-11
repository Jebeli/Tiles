using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace SDLTiles
{
    using SDL2;
    public class SDLTexture : Texture
    {
        private IntPtr texture;
        private int width;
        private int height;
        public SDLTexture(string name, IntPtr texture)
          : base(name)
        {
            this.texture = texture;
            SDL.SDL_QueryTexture(texture, out uint format, out int access, out width, out height);
        }


        public override int Width => width;

        public override int Height => height;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SDL.SDL_DestroyTexture(texture);
            }
        }

        public IntPtr Texture
        {
            get { return texture; }
        }
    }
}
