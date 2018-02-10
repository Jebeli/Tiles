using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.Graphics
{
    public interface IGraphics
    {
        Texture CreateTexture(int width, int height);
        Texture GetTexture(string textureId);
    }
}
