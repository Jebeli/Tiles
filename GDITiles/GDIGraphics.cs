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

namespace GDITiles
{
    using System;
    using TileEngine.Core;
    using TileEngine.Files;
    using TileEngine.Fonts;
    using TileEngine.Graphics;
    using TileEngine.Logging;

    public class GDIGraphics : AbstractGraphics
    {
        private System.Drawing.Imaging.PixelFormat pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
        private System.Drawing.Bitmap view;
        private System.Drawing.Graphics gfx;
        private System.Drawing.Graphics oldGfx;
        private System.Drawing.Pen gridPen;
        private System.Drawing.Pen selectPen;
        private System.Drawing.Brush textBrush;

        public GDIGraphics(int width, int height, IFontEngine fontEngine, DebugOptions debugOptions = null)
            : base(width, height, fontEngine, debugOptions)
        {
            InitPensAndFonts();
        }

        public void RenderTo(System.Drawing.Graphics graphics, System.Drawing.Rectangle dst)
        {
            System.Drawing.Rectangle src = new System.Drawing.Rectangle(0, 0, view.Width, view.Height);
            graphics.DrawImage(view, dst, src, System.Drawing.GraphicsUnit.Pixel);
        }

        public override void SetTarget(Texture tex)
        {
            var currentView = tex.GetBitmap();
            if (currentView != null)
            {
                oldGfx = gfx;
                gfx = System.Drawing.Graphics.FromImage(currentView);
            }
        }

        public override void ClearTarget()
        {
            if (oldGfx != null)
            {
                gfx.Dispose();
                gfx = oldGfx;
                oldGfx = null;
            }
        }

        public override void ClearScreen()
        {
            if (CheckInFrame())
            {
                gfx.Clear(System.Drawing.Color.Black);
            }
        }

        public override void ClearScreen(Color color)
        {
            if (CheckInFrame())
            {
                gfx.Clear(color.GetColor());
            }
        }

        public override void SetClip(int x, int y, int width, int height)
        {
            if (width * height > 0)
            {
                clipX = x;
                clipY = y;
                clipW = width;
                clipH = height;
                x += transX;
                y += transY;
                gfx.SetClip(new System.Drawing.Rectangle(x, y, width, height), System.Drawing.Drawing2D.CombineMode.Replace);
            }
            else
            {
                ClearClip();
            }
        }

        public override void ClearClip()
        {
            gfx.SetClip(new System.Drawing.Rectangle(0, 0, viewWidth, viewHeight), System.Drawing.Drawing2D.CombineMode.Replace);
        }


        public override void DrawTextures(Texture texture, int[] vertices, int offset, int count)
        {
            var bmp = texture.GetBitmap();
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
                    var dstRect = new System.Drawing.Rectangle(x, y, width, height);
                    var srcRect = new System.Drawing.Rectangle(srcX, srcY, srcWidth, srcHeight);
                    gfx.DrawImage(bmp, dstRect, srcRect, System.Drawing.GraphicsUnit.Pixel);
                    offset += NUM_VERTICES;

                }
            }
        }


        public override void Render(Texture texture, int x, int y, int width, int height, int srcX, int srcY, int srcWidth, int srcHeight, int trans)
        {
            var bmp = texture.GetBitmap();
            if (bmp != null)
            {
                x += transX;
                y += transY;
                System.Drawing.Rectangle dstRect = new System.Drawing.Rectangle(x, y, width, height);
                System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(srcX, srcY, srcWidth, srcHeight);
                gfx.DrawImage(bmp, dstRect, srcRect, System.Drawing.GraphicsUnit.Pixel);
            }
        }

        public override void RenderText(Font font, string text, int x, int y, Color color, HorizontalTextAlign hAlign = HorizontalTextAlign.Center, VerticalTextAlign vAlign = VerticalTextAlign.Center)
        {
            if (font == null) return;
            x += transX;
            y += transY;
            var fnt = font.GetFont();
            float fx = x;
            float fy = y;
            var size = gfx.MeasureString(text, fnt);
            if (hAlign == HorizontalTextAlign.Center)
            {
                fx -= size.Width / 2;
            }
            else if (hAlign == HorizontalTextAlign.Left)
            {
            }
            else if (hAlign == HorizontalTextAlign.Right)
            {
                fx -= size.Width;
            }
            if (vAlign == VerticalTextAlign.Center)
            {
                fy -= size.Height / 2;
            }
            else if (vAlign == VerticalTextAlign.Top)
            {

            }
            else if (vAlign == VerticalTextAlign.Bottom)
            {
                fy -= size.Height;
            }
            using (var brush = new System.Drawing.SolidBrush(color.GetColor()))
            {
                gfx.DrawString(text, fnt, brush, fx, fy);
            }
        }

        public override void RenderText(Font font, string text, int x, int y, Color color, Color bg, HorizontalTextAlign hAlign = HorizontalTextAlign.Center, VerticalTextAlign vAlign = VerticalTextAlign.Center)
        {
            if (font == null) return;
            x += transX;
            y += transY;
            var fnt = font.GetFont();
            float fx = x;
            float fy = y;
            var size = gfx.MeasureString(text, fnt);
            if (hAlign == HorizontalTextAlign.Center)
            {
                fx -= size.Width / 2;
            }
            else if (hAlign == HorizontalTextAlign.Left)
            {
            }
            else if (hAlign == HorizontalTextAlign.Right)
            {
                fx -= size.Width;
            }
            if (vAlign == VerticalTextAlign.Center)
            {
                fy -= size.Height / 2;
            }
            else if (vAlign == VerticalTextAlign.Top)
            {

            }
            else if (vAlign == VerticalTextAlign.Bottom)
            {
                fy -= size.Height;
            }
            using (var brush = new System.Drawing.SolidBrush(color.GetColor()))
            {
                gfx.DrawString(text, fnt, brush, fx, fy);
            }
        }
        public override void RenderIcon(int icon, int x, int y)
        {
            var fnt = FontEngine.IconFont;
            if (fnt != null)
            {
                x += transX;
                y += transY;
                string txt = new string(new char[] { (char)icon });
                gfx.DrawString(txt, fnt.GetFont(), System.Drawing.Brushes.White, x, y);
            }
        }

        public override void RenderIcon(int icon, int x, int y, Color color, HorizontalTextAlign hAlign = HorizontalTextAlign.Center, VerticalTextAlign vAlign = VerticalTextAlign.Center)
        {
            var fnt = FontEngine.IconFont;
            if (fnt != null)
            {
                x += transX;
                y += transY;
                string txt = new string(new char[] { (char)icon });
                float fx = x;
                float fy = y;
                var ft = fnt.GetFont();
                var size = gfx.MeasureString(txt, ft);
                if (hAlign == HorizontalTextAlign.Center)
                {
                    fx -= size.Width / 2;
                }
                else if (hAlign == HorizontalTextAlign.Left)
                {
                }
                else if (hAlign == HorizontalTextAlign.Right)
                {
                    fx -= size.Width;
                }
                if (vAlign == VerticalTextAlign.Center)
                {
                    fy -= size.Height / 2;
                }
                else if (vAlign == VerticalTextAlign.Top)
                {

                }
                else if (vAlign == VerticalTextAlign.Bottom)
                {
                    fy -= size.Height;
                }
                using (var brush = new System.Drawing.SolidBrush(color.GetColor()))
                {
                    gfx.DrawString(txt, fnt.GetFont(), brush, fx, fy);
                }
            }

        }
        public override int MeasureTextWidth(Font font, string text)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            var fnt = font.GetFont();
            if (fnt != null)
            {
                var size = gfx.MeasureString(text, fnt);
                return (int)size.Width;
            }
            return text.Length * 8;
        }

        public override void DrawRectangle(int x, int y, int width, int height, Color color)
        {
            using (var pen = new System.Drawing.Pen(color.GetColor()))
            {
                x += transX;
                y += transY;
                var rect = new System.Drawing.Rectangle(x, y, width - 1, height - 1);
                gfx.DrawRectangle(pen, rect);
            }
        }

        public override void DrawRoundedRectangle(int x, int y, int width, int height, Color color)
        {
            using (var pen = new System.Drawing.Pen(color.GetColor()))
            {
                x += transX;
                y += transY;
                var rect = new System.Drawing.Rectangle(x, y, width - 1, height - 1);
                gfx.DrawRectangle(pen, rect);
            }
        }

        public override void FillRectangle(int x, int y, int width, int height, Color color)
        {
            using (var brush = new System.Drawing.SolidBrush(color.GetColor()))
            {
                x += transX;
                y += transY;
                var rect = new System.Drawing.Rectangle(x, y, width, height);
                gfx.FillRectangle(brush, rect);
            }
        }
        public override void FillRectangle(int x, int y, int width, int height, Color color, Color color2)
        {
            using (var brush = new System.Drawing.SolidBrush(color.GetColor()))
            {
                x += transX;
                y += transY;
                var rect = new System.Drawing.Rectangle(x, y, width, height);
                gfx.FillRectangle(brush, rect);
            }
        }

        public override void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            using (var pen = new System.Drawing.Pen(color.GetColor()))
            {
                x1 += transX;
                y1 += transY;
                x2 += transX;
                y2 += transY;
                gfx.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        public override void DrawText(Font font, string text, int x, int y)
        {
            x += transX;
            y += transY;
            gfx.DrawString(text, font.GetFont(), textBrush, x, y);
        }

        public override void DrawTileGrid(int x, int y, int width, int height, MapOrientation oriention = MapOrientation.Isometric)
        {
            x += transX;
            y += transY;
            DrawTile(x, y, width, height, gridPen, oriention);
        }

        public override void DrawTileSelected(int x, int y, int width, int height, MapOrientation oriention = MapOrientation.Isometric)
        {
            x += transX;
            y += transY;
            DrawTile(x, y, width, height, selectPen, oriention);
        }
        public override Texture CreateTexture(string textureId, int width, int height)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height, pixelFormat);
            return new GDITexture(textureId, bmp);
        }

        public override Texture GetTexture(string textureId, IFileResolver fileResolver)
        {
            string fileName = fileResolver.Resolve(textureId);
            return LoadTexture(textureId, fileName);
        }

        private GDITexture LoadTexture(string textureId, string fileName)
        {
            if (fileName != null)
            {
                System.Drawing.Bitmap bmp = System.Drawing.Image.FromFile(fileName) as System.Drawing.Bitmap;
                if (bmp != null)
                {
                    return new GDITexture(textureId, bmp);
                }
            }
            return null;
        }

        public override void ExitRequested()
        {
            System.Windows.Forms.Application.Exit();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && gfx != null)
            {
                gfx.Dispose();
            }
            if (disposing && view != null)
            {
                view.Dispose();
            }
            if (disposing)
            {
                gridPen.Dispose();
                selectPen.Dispose();
                textBrush.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override bool NeedsInitGraphics
        {
            get { return gfx == null || view == null || view.Width != viewWidth || view.Height != viewHeight; }
        }
        protected override void InitGraphics()
        {
            Logger.Info("Gfx", $"Init Graphics {viewWidth} x {viewHeight} scale = {viewScale}");
            gfx?.Dispose();
            view?.Dispose();
            view = new System.Drawing.Bitmap(viewWidth, viewHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            gfx = System.Drawing.Graphics.FromImage(view);
            InitGraphics(gfx);
        }

        private static void InitGraphics(System.Drawing.Graphics g)
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
        }

        private void InitPensAndFonts()
        {
            gridPen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(96, System.Drawing.Color.Wheat));
            selectPen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(96, System.Drawing.Color.Gold));
            textBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        }
        private void DrawTile(int x, int y, int width, int height, System.Drawing.Pen pen, MapOrientation oriention = MapOrientation.Isometric)
        {
            if (oriention == MapOrientation.Orthogonal)
            {
                var rect = new System.Drawing.Rectangle(x, y, width, height);
                gfx.DrawRectangle(pen, rect);
            }
            else
            {
                float x1 = x;
                float x2 = x + width / 2;
                float x3 = x + width;
                float y1 = y;
                float y2 = y + height / 2;
                float y3 = y + height;
                System.Drawing.PointF[] poly = new System.Drawing.PointF[5];
                poly[0].X = x1;
                poly[0].Y = y2;
                poly[1].X = x2;
                poly[1].Y = y1;
                poly[2].X = x3;
                poly[2].Y = y2;
                poly[3].X = x2;
                poly[3].Y = y3;
                poly[4].X = x1;
                poly[4].Y = y2;
                gfx.DrawPolygon(pen, poly);
            }
        }
    }
}
