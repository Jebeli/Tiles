using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Resources;

namespace TileEngine.Graphics
{
    public abstract class Texture : Resource
    {
        public Texture(string name)
            : base(name)
        {

        }
        public abstract int Width { get; }
        public abstract int Height { get; }

        public TextureRegion GetRegion(int clipX, int clipY, int clipW, int clipH, int offsetX, int offsetY)
        {
            return new TextureRegion(this, clipX, clipY, clipW, clipH, offsetX, offsetY);
        }

    }
}
