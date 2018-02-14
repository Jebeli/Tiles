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
    using System;
    using TileEngine.Core;
    using TileEngine.Files;
    using TileEngine.Graphics;
    using TileEngine.Logging;
    using MonoGame.Extended.BitmapFonts;

    public class MONOGraphics : AbstractGraphics
    {
        private MONOGame game;
        private Microsoft.Xna.Framework.Graphics.RenderTarget2D view;
        private ExtendedSpriteBatch batch;
        public MONOGraphics(MONOGame game, int width, int height, DebugOptions debugOptions = null)
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

        public override void DrawTextures(Texture texture, int[] vertices, int offset, int count)
        {
            var bmp = texture.GetTexture();
            if (bmp != null)
            {
                for (int i = 0; i < count; i++)
                {
                    int idx = offset;
                    int x = vertices[idx];
                    int y = vertices[idx + 1];
                    int width = vertices[idx + 2];
                    int height = vertices[idx + 3];
                    int srcX = vertices[idx + 4];
                    int srcY = vertices[idx + 5];
                    int srcWidth = vertices[idx + 6];
                    int srcHeight = vertices[idx + 7];
                    int trans = vertices[idx + 8];
                    int tint = vertices[idx + 9];
                    var dstRect = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
                    var srcRect = new Microsoft.Xna.Framework.Rectangle(srcX, srcY, srcWidth, srcHeight);
                    var c = Microsoft.Xna.Framework.Color.White;
                    if (tint != 0xFFFFFF)
                    {
                        c = new Microsoft.Xna.Framework.Color((uint)tint);
                    }
                    if (trans > 0)
                    {
                        c *= (float)((255 - trans) / 256.0);
                    }
                    batch.Draw(bmp, dstRect, srcRect, c, 0.0f, Microsoft.Xna.Framework.Vector2.Zero, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);
                    offset += NUM_VERTICES;

                }
            }
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

        public override void RenderText(string text, int x, int y, HorizontalTextAlign hAlign = HorizontalTextAlign.Center, VerticalTextAlign vAlign = VerticalTextAlign.Center)
        {
            var c = Microsoft.Xna.Framework.Color.Wheat;
            var fnt = game.smallFont;
            var size = fnt.MeasureString(text);
            var pos = new Microsoft.Xna.Framework.Vector2(x, y);
            if (hAlign == HorizontalTextAlign.Center)
            {
                pos.X -= size.Width / 2;
            }
            else if (hAlign == HorizontalTextAlign.Left)
            {
            }
            else if (hAlign == HorizontalTextAlign.Right)
            {
                pos.X -= size.Width;
            }
            if (vAlign == VerticalTextAlign.Center)
            {
                pos.Y -= size.Height / 2;
            }
            else if (vAlign == VerticalTextAlign.Top)
            {

            }
            else if (vAlign == VerticalTextAlign.Bottom)
            {
                pos.Y -= size.Height;
            }
            batch.DrawString(fnt, text, pos, c);
        }

        public override void RenderWidget(int x, int y, int width, int height, bool enabled, bool hover, bool pressed)
        {
            var rect = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
            if (pressed)
            {
                batch.FillRectangle(rect, Microsoft.Xna.Framework.Color.DimGray * 0.5f);
                batch.DrawRectangle(rect, Microsoft.Xna.Framework.Color.White);
            }
            else if (hover && enabled)
            {
                batch.FillRectangle(rect, Microsoft.Xna.Framework.Color.LightGray * 0.5f);
                batch.DrawRectangle(rect, Microsoft.Xna.Framework.Color.White);
            }
            else if (enabled)
            {
                batch.FillRectangle(rect, Microsoft.Xna.Framework.Color.Gray * 0.5f);
                batch.DrawRectangle(rect, Microsoft.Xna.Framework.Color.White);
            }
            else
            {
                batch.FillRectangle(rect, Microsoft.Xna.Framework.Color.DimGray * 0.5f);
                batch.DrawRectangle(rect, Microsoft.Xna.Framework.Color.White);

            }
        }

        public override void DrawText(string text, int x, int y)
        {

        }

        public override void DrawTileGrid(int x, int y, int width, int height, MapOrientation oriention = MapOrientation.Isometric)
        {
            DrawTile(x, y, width, height, Microsoft.Xna.Framework.Color.Wheat * 0.48f, oriention);
        }

        public override void DrawTileSelected(int x, int y, int width, int height, MapOrientation oriention = MapOrientation.Isometric)
        {
            DrawTile(x, y, width, height, Microsoft.Xna.Framework.Color.Gold * 0.48f, oriention);
        }

        private void DrawTile(int x, int y, int width, int height, Microsoft.Xna.Framework.Color color, MapOrientation oriention = MapOrientation.Isometric)
        {
            if (oriention == MapOrientation.Orthogonal)
            {
                var rect = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
                batch.DrawRectangle(rect, color);
            }
            else
            {
                float x1 = x;
                float x2 = x + width / 2;
                float x3 = x + width;
                float y1 = y;
                float y2 = y + height / 2;
                float y3 = y + height;
                batch.DrawLine(new Microsoft.Xna.Framework.Vector2(x1, y2), new Microsoft.Xna.Framework.Vector2(x2, y1), color);
                batch.DrawLine(new Microsoft.Xna.Framework.Vector2(x2, y1), new Microsoft.Xna.Framework.Vector2(x3, y2), color);
                batch.DrawLine(new Microsoft.Xna.Framework.Vector2(x3, y2), new Microsoft.Xna.Framework.Vector2(x2, y3), color);
                batch.DrawLine(new Microsoft.Xna.Framework.Vector2(x2, y3), new Microsoft.Xna.Framework.Vector2(x1, y2), color);
            }
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
