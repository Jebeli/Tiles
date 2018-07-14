using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Audio;

namespace SDLTiles
{
    class SDLMusic : Music
    {
        private IntPtr chunk;

        public SDLMusic(string name, IntPtr chunk)
            : base(name)
        {
            this.chunk = chunk;
        }

        public IntPtr Chunk
        {
            get { return chunk; }
        }

        protected override void Dispose(bool disposing)
        {
            if (!chunk.Equals(IntPtr.Zero))
            {
                SDL_mixer.Mix_FreeMusic(chunk);
            }
            chunk = IntPtr.Zero;
        }
    }
}
