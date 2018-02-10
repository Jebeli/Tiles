using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Core;
using TileEngine.Files;
using TileEngine.Graphics;
using TileEngine.Resources;

namespace TileEngine
{
    public class Engine : ITimeInfoProvider
    {
        private int maxFramesPerSecond = 60;
        private IFileResolver fileResolver;
        private IGraphics graphics;
        private ITimeInfoProvider timeProvider;
        private ResourceManager<Texture> textureManager;


        public Engine(IFileResolver fileResolver, IGraphics graphics)
        {
            this.fileResolver = fileResolver;
            this.graphics = graphics;
            timeProvider = new StopWatchTimeInfoProvider();
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

        public ITimeInfoProvider TimeProvider
        {
            get { return timeProvider; }
        }

        public int MaxFramesPerSecond
        {
            get { return maxFramesPerSecond; }
            set { maxFramesPerSecond = value; }
        }

        public double FrameRate
        {
            get { return 1.0 / maxFramesPerSecond; }
        }
        public bool Update()
        {
            return Update(GetUpdateTimeInfo());
        }

        public bool Update(TimeInfo time)
        {
            if (time.ElapsedGameTime.TotalSeconds >= FrameRate)
            {
                return true;
            }
            return false;
        }
        public TimeInfo GetRenderTimeInfo()
        {
            return timeProvider.GetRenderTimeInfo();
        }

        public TimeInfo GetUpdateTimeInfo()
        {
            return timeProvider.GetUpdateTimeInfo();
        }
        public TimeSpan GetCurrentTime()
        {
            return timeProvider.GetCurrentTime();
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
