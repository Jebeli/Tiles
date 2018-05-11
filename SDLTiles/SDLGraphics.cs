using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Files;
using TileEngine.Graphics;
using TileEngine.Logging;
using SDL2;
using TileEngine.Fonts;

namespace SDLTiles
{
    public class SDLGraphics : AbstractGraphics
    {
        private SDLGame game;
        private Texture view;
        //private IntPtr icon;

        public SDLGraphics(SDLGame game, int width, int height, IFontEngine fontEngine, DebugOptions debugOptions = null)
            : base(width, height, fontEngine, debugOptions)
        {
            this.game = game;
        }

        public void BeginFrame(SDLGame game)
        {
            BeginFrame();
            SDL.SDL_SetRenderTarget(game.ren, view.GetTexture());
            SDL.SDL_RenderClear(game.ren);
        }

        public void EndFrame(SDLGame game)
        {
            SDL.SDL_SetRenderTarget(game.ren, IntPtr.Zero);
            SDL.SDL_Rect srcRect = new SDL.SDL_Rect();
            srcRect.x = 0;
            srcRect.y = 0;
            srcRect.w = view.Width;
            srcRect.h = view.Height;
            SDL.SDL_Rect dstRect = new SDL.SDL_Rect();
            dstRect.x = 0;
            dstRect.y = 0;
            dstRect.w = width;
            dstRect.h = height;
            SDL.SDL_SetRenderDrawBlendMode(game.ren, SDL.SDL_BlendMode.SDL_BLENDMODE_NONE);
            SDL.SDL_RenderCopy(game.ren, view.GetTexture(), ref srcRect, ref dstRect);
            EndFrame();
        }

        protected override bool NeedsInitGraphics
        {
            get { return (view == null) || (view.Width != viewWidth) || (view.Height != viewHeight); }
        }

        public override void ClearClip()
        {
            clipX = 0;
            clipY = 0;
            clipW = 0;
            clipH = 0;
            SDL.SDL_RenderSetClipRect(game.ren, IntPtr.Zero);
        }

        public override void ClearScreen()
        {
            SDL.SDL_SetRenderDrawColor(game.ren, 0, 0, 0, 255);
            SDL.SDL_RenderClear(game.ren);
        }

        public override void ClearScreen(Color color)
        {
            SDL.SDL_SetRenderDrawColor(game.ren, color.R, color.G, color.B, color.Alpha);
            SDL.SDL_RenderClear(game.ren);
        }

        public override void ClearTarget()
        {
            SDL.SDL_SetRenderTarget(game.ren, view.GetTexture());
        }

        public override Texture CreateTexture(string textureId, int width, int height)
        {
            IntPtr tex = SDL.SDL_CreateTexture(game.ren, SDL.SDL_PIXELFORMAT_BGRA8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, width, height);
            SDLTexture sdlTex = new SDLTexture(textureId, tex);
            return sdlTex;
        }
        public override void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            x1 += transX;
            x2 += transX;
            y1 += transY;
            y2 += transY;
            SDL.SDL_SetRenderDrawBlendMode(game.ren, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_SetRenderDrawColor(game.ren, color.R, color.G, color.B, color.Alpha);
            SDL.SDL_RenderDrawLine(game.ren, x1, y1, x2, y2);
        }

        public override void DrawRectangle(int x, int y, int width, int height, Color color)
        {
            x += transX;
            y += transY;
            SDL.SDL_Rect rect = new SDL.SDL_Rect();
            rect.x = x;
            rect.y = y;
            rect.w = width;
            rect.h = height;
            SDL.SDL_SetRenderDrawBlendMode(game.ren, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_SetRenderDrawColor(game.ren, color.R, color.G, color.B, color.Alpha);
            SDL.SDL_RenderDrawRect(game.ren, ref rect);
        }

        public override void DrawText(Font font, string text, int x, int y)
        {
            //throw new NotImplementedException();
        }

        public override void DrawTextures(Texture texture, int[] vertices, int offset, int count)
        {
            var bmp = texture.GetTexture();
            if (bmp != IntPtr.Zero)
            {
                for (int i = 0; i < count; i++)
                {
                    int idx = offset;
                    int x = vertices[idx] + transX;
                    int y = vertices[idx + 1] + transY;
                    int width = vertices[idx + 2];
                    int height = vertices[idx + 3];
                    int srcX = vertices[idx + 4];
                    int srcY = vertices[idx + 5];
                    int srcWidth = vertices[idx + 6];
                    int srcHeight = vertices[idx + 7];
                    int trans = vertices[idx + 8];
                    int tint = vertices[idx + 9];
                    if ((width > 0) && height > 0)
                    {
                        var dstRect = new SDL.SDL_Rect()
                        {
                            x = x,
                            y = y,
                            w = width,
                            h = height
                        };
                        var srcRect = new SDL.SDL_Rect()
                        {
                            x = srcX,
                            y = srcY,
                            w = srcWidth,
                            h = srcHeight
                        };
                        //var c = Microsoft.Xna.Framework.Color.White;
                        //if (tint != 0xFFFFFF)
                        //{
                        //    c = new Microsoft.Xna.Framework.Color((uint)tint);
                        //}
                        //if (trans > 0)
                        //{
                        //    c *= (float)((255 - trans) / 256.0);
                        //}
                        //batch.Draw(bmp, dstRect, srcRect, c, 0.0f, Microsoft.Xna.Framework.Vector2.Zero, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);
                        SDL.SDL_SetRenderDrawBlendMode(game.ren, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
                        SDL.SDL_RenderCopy(game.ren, bmp, ref srcRect, ref dstRect);
                    }
                    offset += NUM_VERTICES;

                }
            }

        }

        public override void DrawTileGrid(int x, int y, int width, int height, MapOrientation oriention = MapOrientation.Isometric)
        {
            //throw new NotImplementedException();
        }

        public override void DrawTileSelected(int x, int y, int width, int height, MapOrientation oriention = MapOrientation.Isometric)
        {
            //throw new NotImplementedException();
        }

        public override void ExitRequested()
        {
            game.running = false;
        }

        public override void FillRectangle(int x, int y, int width, int height, Color color)
        {
            x += transX;
            y += transY;
            SDL.SDL_Rect rect = new SDL.SDL_Rect();
            rect.x = x;
            rect.y = y;
            rect.w = width;
            rect.h = height;
            SDL.SDL_SetRenderDrawBlendMode(game.ren, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_SetRenderDrawColor(game.ren, color.R, color.G, color.B, color.Alpha);
            SDL.SDL_RenderFillRect(game.ren, ref rect);
        }

        public override void FillRectangle(int x, int y, int width, int height, Color color, Color color2)
        {
            int dc = color2.G - color.G;
            int da = color2.Alpha - color.Alpha;
            float incC = dc;
            float incA = da;
            incC /= height;
            incA /= height;
            x += transX;
            y += transY;
            float c = color.G;
            float a = color.Alpha;
            for (int i = 0; i < height; i++)
            {
                SDL.SDL_SetRenderDrawBlendMode(game.ren, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
                SDL.SDL_SetRenderDrawColor(game.ren, (byte)c, (byte)c, (byte)c, (byte)a);
                SDL.SDL_RenderDrawLine(game.ren, x, y + i, x + width - 1, y + i);
                c += incC;
                a += incA;
            }
        }

        public override Texture GetTexture(string textureId, IFileResolver fileResolver)
        {
            string assetName = fileResolver.Resolve(textureId);
            IntPtr tex = SDL_image.IMG_LoadTexture(game.ren, assetName);
            return new SDLTexture(textureId, tex);
        }

        public override int MeasureTextWidth(Font font, string text)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            if (font != null)
            {
                int res = SDL_ttf.TTF_SizeText(font.GetFont(), text, out int w, out int h);
                return w;
            }
            else
            {
                return text.Length * 8;
            }
        }

        public override void Render(Texture texture, int x, int y, int width, int height, int srcX, int srcY, int srcWidth, int srcHeight, int trans = 0)
        {
            x += transX;
            y += transY;
            var bmp = texture.GetTexture();
            if (bmp != IntPtr.Zero && width > 0 && height > 0)
            {
                var dstRect = new SDL.SDL_Rect()
                {
                    x = x,
                    y = y,
                    w = width,
                    h = height
                };
                var srcRect = new SDL.SDL_Rect()
                {
                    x = srcX,
                    y = srcY,
                    w = srcWidth,
                    h = srcHeight
                };
                SDL.SDL_RenderCopy(game.ren, bmp, ref srcRect, ref dstRect);
            }
        }

        public override void RenderIcon(int icon, int x, int y)
        {
            var fnt = FontEngine.IconFont;
            if (fnt != null)
            {
                x += transX;
                y += transY;
                SDL.SDL_Color col = new SDL.SDL_Color();
                col.a = 255;
                col.r = 255;
                col.g = 255;
                col.b = 255;
                IntPtr txt = SDL_ttf.TTF_RenderGlyph_Blended(fnt.GetFont(), (ushort)icon, col);
                IntPtr txt2 = SDL.SDL_CreateTextureFromSurface(game.ren, txt);
                SDL.SDL_QueryTexture(txt2, out uint format, out int access, out int w, out int h);
                int tx = x;
                int ty = y;
                SDL.SDL_Rect rect = new SDL.SDL_Rect();
                rect.x = tx;
                rect.y = ty;
                rect.w = w;
                rect.h = h;
                SDL.SDL_RenderCopy(game.ren, txt2, IntPtr.Zero, ref rect);
                SDL.SDL_DestroyTexture(txt2);
                SDL.SDL_FreeSurface(txt);
            }
        }

        public override void RenderIcon(int icon, int x, int y, Color color, HorizontalTextAlign hAlign = HorizontalTextAlign.Center, VerticalTextAlign vAlign = VerticalTextAlign.Center)
        {
            var fnt = FontEngine.IconFont;
            if (fnt != null)
            {
                x += transX;
                y += transY;
                SDL.SDL_Color col = new SDL.SDL_Color();
                col.a = color.Alpha;
                col.r = color.R;
                col.g = color.G;
                col.b = color.B;
                IntPtr txt = SDL_ttf.TTF_RenderGlyph_Blended(fnt.GetFont(), (ushort)icon, col);
                IntPtr txt2 = SDL.SDL_CreateTextureFromSurface(game.ren, txt);
                SDL.SDL_QueryTexture(txt2, out uint format, out int access, out int w, out int h);
                int tx = x;
                int ty = y;
                switch (hAlign)
                {
                    case HorizontalTextAlign.Center:
                        tx -= w / 2;
                        break;
                    case HorizontalTextAlign.Right:
                        tx -= w;
                        break;
                }
                switch (vAlign)
                {
                    case VerticalTextAlign.Center:
                        ty -= h / 2;
                        break;
                    case VerticalTextAlign.Bottom:
                        ty -= h;
                        break;
                }
                SDL.SDL_Rect rect = new SDL.SDL_Rect();
                rect.x = tx;
                rect.y = ty;
                rect.w = w;
                rect.h = h;
                SDL.SDL_RenderCopy(game.ren, txt2, IntPtr.Zero, ref rect);
                SDL.SDL_DestroyTexture(txt2);
                SDL.SDL_FreeSurface(txt);
            }
        }

        public override void RenderText(Font font, string text, int x, int y, Color color, HorizontalTextAlign hAlign = HorizontalTextAlign.Center, VerticalTextAlign vAlign = VerticalTextAlign.Center)
        {
            if (font == null) return;
            x += transX;
            y += transY;
            SDL.SDL_Color col = new SDL.SDL_Color();
            col.a = color.Alpha;
            col.r = color.R;
            col.g = color.G;
            col.b = color.B;

            IntPtr txt = SDL_ttf.TTF_RenderText_Blended(font.GetFont(), text, col);
            IntPtr txt2 = SDL.SDL_CreateTextureFromSurface(game.ren, txt);
            SDL.SDL_QueryTexture(txt2, out uint format, out int access, out int w, out int h);
            int tx = x;
            int ty = y;
            switch (hAlign)
            {
                case HorizontalTextAlign.Center:
                    tx -= w / 2;
                    break;
                case HorizontalTextAlign.Right:
                    tx -= w;
                    break;
            }
            switch (vAlign)
            {
                case VerticalTextAlign.Center:
                    ty -= h / 2;
                    break;
                case VerticalTextAlign.Bottom:
                    ty -= h;
                    break;
            }
            SDL.SDL_Rect rect = new SDL.SDL_Rect();
            rect.x = tx;
            rect.y = ty;
            rect.w = w;
            rect.h = h;
            SDL.SDL_SetRenderDrawBlendMode(game.ren, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_RenderCopy(game.ren, txt2, IntPtr.Zero, ref rect);
            SDL.SDL_DestroyTexture(txt2);
            SDL.SDL_FreeSurface(txt);
        }

        public override void RenderText(Font font, string text, int x, int y, Color color, Color bg, HorizontalTextAlign hAlign = HorizontalTextAlign.Center, VerticalTextAlign vAlign = VerticalTextAlign.Center)
        {
            if (font == null) return;
            x += transX;
            y += transY;
            SDL.SDL_Color col = new SDL.SDL_Color();
            col.a = color.Alpha;
            col.r = color.R;
            col.g = color.G;
            col.b = color.B;
            SDL.SDL_Color bgcol = new SDL.SDL_Color();
            bgcol.a = bg.Alpha;
            bgcol.r = bg.R;
            bgcol.g = bg.G;
            bgcol.b = bg.B;

            IntPtr txt = SDL_ttf.TTF_RenderText_Shaded(font.GetFont(), text, col, bgcol);
            IntPtr txt2 = SDL.SDL_CreateTextureFromSurface(game.ren, txt);
            SDL.SDL_QueryTexture(txt2, out uint format, out int access, out int w, out int h);
            int tx = x;
            int ty = y;
            switch (hAlign)
            {
                case HorizontalTextAlign.Center:
                    tx -= w / 2;
                    break;
                case HorizontalTextAlign.Right:
                    tx -= w;
                    break;
            }
            switch (vAlign)
            {
                case VerticalTextAlign.Center:
                    ty -= h / 2;
                    break;
                case VerticalTextAlign.Bottom:
                    ty -= h;
                    break;
            }
            SDL.SDL_Rect rect = new SDL.SDL_Rect();
            rect.x = tx;
            rect.y = ty;
            rect.w = w;
            rect.h = h;
            SDL.SDL_SetRenderDrawBlendMode(game.ren, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_RenderCopy(game.ren, txt2, IntPtr.Zero, ref rect);
            SDL.SDL_DestroyTexture(txt2);
            SDL.SDL_FreeSurface(txt);
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
                SDL.SDL_Rect rect = new SDL.SDL_Rect();
                rect.x = x;
                rect.y = y;
                rect.w = width;
                rect.h = height;
                int result = SDL.SDL_RenderSetClipRect(game.ren, ref rect);
                if (result != 0)
                {
                    string er = SDL.SDL_GetError();
                    Logger.Error("SDL", $"Set Clip Failed: {er}");
                }
            }
            else
            {
                ClearClip();
            }
        }

        public override void SetTarget(Texture tex)
        {
            SDL.SDL_SetRenderTarget(game.ren, tex.GetTexture());
        }

        protected override void InitGraphics()
        {
            Logger.Info("Gfx", $"Init Graphics {viewWidth} x {viewHeight} scale = {viewScale}");
            view?.Dispose();
            view = CreateTexture("view", viewWidth, viewHeight);

        }
    }
}
