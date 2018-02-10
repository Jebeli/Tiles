using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Files;

namespace TileEngine.Graphics
{
    public interface IGraphics
    {
        Texture CreateTexture(string textureId, int width, int height);
        Texture GetTexture(string textureId, IFileResolver fileResolver);
    }
}
