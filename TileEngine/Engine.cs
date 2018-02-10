using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Files;
using TileEngine.Graphics;

namespace TileEngine
{
    public class Engine
    {
        private IFileResolver fileResolver;
        private IGraphics graphics;


        public Engine(IFileResolver fileResolver, IGraphics graphics)
        {
            this.fileResolver = fileResolver;
            this.graphics = graphics;
        }
        public IFileResolver FileResolver
        {
            get { return fileResolver; }
        }

        public IGraphics Graphics
        {
            get { return graphics; }
        }

    }
}
