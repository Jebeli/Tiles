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

namespace MONOTiles
{
    using TileEngine.Files;
    using TileEngine.Graphics;
    using TileEngine.Logging;

    public class MONOGraphics : AbstractGraphics
    {
        private Game1 game;
        private Microsoft.Xna.Framework.Graphics.RenderTarget2D view;
        private ExtendedSpriteBatch batch;
        public MONOGraphics(Game1 game, int width, int height, DebugOptions debugOptions = null)
            : base(width, height, debugOptions)
        {
            this.game = game;
        }
        public void BeginFrame(ExtendedSpriteBatch batch)
        {
            BeginFrame();
            this.batch = batch;
            game.GraphicsDevice.SetRenderTarget(view);
            this.batch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred,
                Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend,
                Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp,
                Microsoft.Xna.Framework.Graphics.DepthStencilState.None,
                Microsoft.Xna.Framework.Graphics.RasterizerState.CullCounterClockwise);
        }

        public void EndFrame(ExtendedSpriteBatch batch)
        {
            batch.End();
            Microsoft.Xna.Framework.Rectangle dst = new Microsoft.Xna.Framework.Rectangle(0, 0, Width, Height);
            Microsoft.Xna.Framework.Rectangle src = new Microsoft.Xna.Framework.Rectangle(0, 0, view.Width, view.Height);
            game.GraphicsDevice.SetRenderTarget(null);
            batch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Immediate, Microsoft.Xna.Framework.Graphics.BlendState.Opaque);
            batch.Draw(view, dst, src, Microsoft.Xna.Framework.Color.White);
            batch.End();
            this.batch = null;
            EndFrame();
        }
        public override void ClearScreen()
        {
            game.GraphicsDevice.Clear(Microsoft.Xna.Framework.Graphics.ClearOptions.Target, Microsoft.Xna.Framework.Color.Transparent, 1.0f, 0);
        }

        public override void Render(Texture texture, int x, int y, int width, int height, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            var bmp = texture.GetTexture();
            if (bmp != null)
            {
                var dstRect = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
                var srcRect = new Microsoft.Xna.Framework.Rectangle(srcX, srcY, srcWidth, srcHeight);
                var col = Microsoft.Xna.Framework.Color.White;
                batch.Draw(bmp, dstRect, srcRect, col, 0.0f, Microsoft.Xna.Framework.Vector2.Zero, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);
            }
        }

        public override void DrawText(string text, int x, int y)
        {
            
        }

        public override void DrawTileGrid(int x, int y, int width, int height)
        {
        }

        public override void DrawTileSelected(int x, int y, int width, int height)
        {
        }

        public override Texture CreateTexture(string textureId, int width, int height)
        {
            var bmp = new Microsoft.Xna.Framework.Graphics.RenderTarget2D(game.GraphicsDevice,
                                width,
                                height,
                                false,
                                Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color,
                                Microsoft.Xna.Framework.Graphics.DepthFormat.None,
                                1,
                                Microsoft.Xna.Framework.Graphics.RenderTargetUsage.PreserveContents);
            return new MONOTexture(textureId, bmp);
        }

        public override Texture GetTexture(string textureId, IFileResolver fileResolver)
        {            
            string assetName = fileResolver.Resolve(textureId);
            var bmp = game.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(assetName);
            return new MONOTexture(textureId, bmp);
        }

        protected override bool NeedsInitGraphics
        {
            get { return view == null || view.Width != viewWidth || view.Height != viewHeight; }
        }
        protected override void InitGraphics()
        {
            Logger.Info("Gfx", $"Init Graphics {viewWidth} x {viewHeight} scale = {viewScale}");
            view?.Dispose();
            view = new Microsoft.Xna.Framework.Graphics.RenderTarget2D(game.GraphicsDevice, viewWidth, viewHeight, false, Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color, Microsoft.Xna.Framework.Graphics.DepthFormat.None, 1, Microsoft.Xna.Framework.Graphics.RenderTargetUsage.PreserveContents);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && view != null)
            {
                view.Dispose();
            }
        }
    }
}
