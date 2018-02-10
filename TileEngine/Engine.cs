using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Files;
using TileEngine.Graphics;
using TileEngine.Resources;

namespace TileEngine
{
    public class Engine
    {
        private IFileResolver fileResolver;
        private IGraphics graphics;
        private ResourceManager<Texture> textureManager;


        public Engine(IFileResolver fileResolver, IGraphics graphics)
        {
            this.fileResolver = fileResolver;
            this.graphics = graphics;
            textureManager = new ResourceManager<Texture>();
        }
        public IFileResolver FileResolver
        {
            get { return fileResolver; }
        }

        public IGraphics Graphics
        {
            get { return graphics; }
        }


        public Texture GetTexture(string textureId)
        {
            Texture tex = null;
            if (textureManager.Exists(textureId))
            {
                tex = textureManager.Get(textureId);
            }
            else
            {
                tex = graphics.GetTexture(textureId, fileResolver);
                if (tex != null) textureManager.Add(tex);
            }
            return tex;
        }

    }
}
