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
        private long frameId;
        private bool inFrame;
        protected int width;
        protected int height;
        protected int viewWidth;
        protected int viewHeight;
        protected float viewScale;

        public AbstractGraphics(int width, int height)
        {
            this.width = width;
            this.height = height;
            viewWidth = width;
            viewHeight = height;
            viewScale = 1.0f;
        }
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }
        public int ViewWidth
        {
            get { return viewWidth; }
        }
        public int ViewHeight
        {
            get { return viewHeight; }
        }
        public float ViewScale
        {
            get { return viewScale; }
        }
        public long FrameId
        {
            get { return frameId; }
        }

        public bool InFrame
        {
            get { return inFrame; }
        }
        public void BeginFrame()
        {
            frameId++;
            inFrame = true;
            InitGraphicsIfNeeded();
        }
        public void EndFrame()
        {
            inFrame = false;
        }

        public void SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            viewWidth = (int)(width * viewScale);
            viewHeight = (int)(height * viewScale);
        }
        public void SetScale(float scale)
        {
            viewScale = scale;
            viewWidth = (int)(width * viewScale);
            viewHeight = (int)(height * viewScale);
        }
        public abstract void ClearScreen();
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

        protected abstract void InitGraphics();
        protected abstract bool NeedsInitGraphics { get; }
        protected void InitGraphicsIfNeeded()
        {
            if (NeedsInitGraphics) InitGraphics();
        }

        protected bool CheckInFrame(bool throwException = true)
        {
            if (!inFrame && throwException) { throw new InvalidOperationException("Render Operation while not in frame"); }
            return inFrame;
        }
    }
}
