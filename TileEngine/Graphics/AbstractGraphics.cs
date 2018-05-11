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
    using Core;
    using System.Collections.Generic;
    using TileEngine.Fonts;

    public abstract class AbstractGraphics : IGraphics, IDisposable
    {

        private class GraphicsState
        {
            public int TransX;
            public int TransY;
            public int ClipX;
            public int ClipY;
            public int ClipW;
            public int ClipH;
        }

        private long frameId;
        private bool inFrame;
        private DebugOptions debugOptions;
        protected int width;
        protected int height;
        protected int viewWidth;
        protected int viewHeight;
        protected float viewScale;
        protected const int NUM_VERTICES = 10;
        protected int transX;
        protected int transY;
        protected int clipX;
        protected int clipY;
        protected int clipW;
        protected int clipH;
        private List<GraphicsState> states;
        private IFontEngine fontEngine;

        public AbstractGraphics(int width, int height, IFontEngine fontEngine, DebugOptions debugOptions = null)
        {
            this.width = width;
            this.height = height;
            this.fontEngine = fontEngine;
            viewWidth = width;
            viewHeight = height;
            viewScale = 1.0f;
            this.debugOptions = debugOptions ?? new DebugOptions();
            states = new List<GraphicsState>();
        }

        public IFontEngine FontEngine
        {
            get { return fontEngine; }
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

        public DebugOptions DebugOptions
        {
            get { return debugOptions; }
            set { debugOptions = value; }
        }

        public abstract void SetTarget(Texture tex);
        public abstract void ClearTarget();

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

        public void Translate(int x, int y)
        {
            transX += x;
            transY += y;
        }

        public string GetStateDescription()
        {
            var s = GetGraphicsState();
            return $"{s.TransX}/{s.TransY} {s.ClipX}/{s.ClipY} {s.ClipW}/{s.ClipH}";
        }


        private GraphicsState GetGraphicsState()
        {
            GraphicsState state = new GraphicsState();
            state.TransX = transX;
            state.TransY = transY;
            state.ClipX = clipX;
            state.ClipY = clipY;
            state.ClipW = clipW;
            state.ClipH = clipH;
            return state;
        }

        public void SaveState()
        {
            var state = GetGraphicsState();
            states.Add(state);
        }

        public void RestoreState()
        {
            if (states.Count > 0)
            {
                var state = states[states.Count - 1];
                states.RemoveAt(states.Count - 1);
                transX = state.TransX;
                transY = state.TransY;
                clipX = state.ClipX;
                clipY = state.ClipY;
                clipW = state.ClipW;
                clipH = state.ClipH;
                SetClip(clipX, clipY, clipW, clipH);
            }
            else
            {
                transX = 0;
                transY = 0;
                clipX = 0;
                clipY = 0;
                clipW = 0;
                clipH = 0;
                ClearClip();
            }
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
        public abstract void SetClip(int x, int y, int width, int height);
        public abstract void ClearClip();
        public abstract void ClearScreen();
        public abstract void ClearScreen(Color color);
        public abstract void DrawTextures(Texture texture, int[] vertices, int offset, int count);
        public void Render(TextureRegion textureRegion, int x, int y)
        {
            if (textureRegion != null)
                Render(textureRegion.Texture,
                    x + textureRegion.OffsetX,
                    y + textureRegion.OffsetY,
                    textureRegion.Width,
                    textureRegion.Height,
                    textureRegion.X,
                    textureRegion.Y,
                    textureRegion.Width,
                    textureRegion.Height);
        }

        public void Render(Texture texture, int x, int y, int trans)
        {
            if (texture != null)
                Render(texture,
                    x,
                    y,
                    texture.Width,
                    texture.Height,
                    0,
                    0,
                    texture.Width,
                    texture.Height,
                    trans);
        }

        public void Render(TextureRegion textureRegion, int x, int y, int width, int height)
        {
            if (textureRegion != null)
                Render(textureRegion.Texture,
                    x + textureRegion.OffsetX,
                    y + textureRegion.OffsetY,
                    width,
                    height,
                    textureRegion.X,
                    textureRegion.Y,
                    textureRegion.Width,
                    textureRegion.Height);
        }

        public abstract void Render(Texture texture, int x, int y, int width, int height, int srcX, int srcY, int srcWidth, int srcHeight, int trans = 0);

        public abstract void RenderText(Font font, string text, int x, int y, Color color, HorizontalTextAlign hAlign = HorizontalTextAlign.Center, VerticalTextAlign vAlign = VerticalTextAlign.Center);
        public abstract void RenderText(Font font, string text, int x, int y, Color color, Color bg, HorizontalTextAlign hAlign = HorizontalTextAlign.Center, VerticalTextAlign vAlign = VerticalTextAlign.Center);
        public abstract void RenderIcon(int icon, int x, int y);
        public abstract void RenderIcon(int icon, int x, int y, Color color, HorizontalTextAlign hAlign = HorizontalTextAlign.Center, VerticalTextAlign vAlign = VerticalTextAlign.Center);

        public abstract int MeasureTextWidth(Font font, string text);
        public abstract void DrawRectangle(int x, int y, int width, int height, Color color);
        public abstract void FillRectangle(int x, int y, int width, int height, Color color);
        public abstract void FillRectangle(int x, int y, int width, int height, Color color, Color color2);
        public abstract void DrawLine(int x1, int y1, int x2, int y2, Color color);
        public abstract void DrawText(Font font, string text, int x, int y);
        public abstract void DrawTileGrid(int x, int y, int width, int height, MapOrientation oriention = MapOrientation.Isometric);
        public abstract void DrawTileSelected(int x, int y, int width, int height, MapOrientation oriention = MapOrientation.Isometric);
        public abstract Texture CreateTexture(string textureId, int width, int height);

        public abstract Texture GetTexture(string textureId, IFileResolver fileResolver);
        public abstract void ExitRequested();
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
