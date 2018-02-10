using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Files;

namespace TileEngine.Graphics
{
    public abstract class AbstractGraphics : IGraphics, IDisposable
    {
        public abstract Texture CreateTexture(string textureId, int width, int height);

        public abstract Texture GetTexture(string textureId, IFileResolver fileResolver);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {

        }
    }
}
