/*
Copyright © 2018 Jean Pascal Bellot

This file is part of Tiles.

Tiles is free software: you can redistribute it and/or modify it under the terms
of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

Tiles is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Tiles.  If not, see http://www.gnu.org/licenses/
*/

namespace TileEngine.Graphics
{
    using System;
    using Files;

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
