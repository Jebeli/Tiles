using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Graphics
{
    public abstract class AbstractGraphics : IGraphics
    {
        public abstract Texture CreateTexture(int width, int height);

        public abstract Texture GetTexture(string textureId);
    }
}
