using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;
using SDL2;

namespace SDLTiles
{
    public static class SDLDraw
    {
        private static readonly double M_PI = Math.PI;
        private static double sin(double s) { return Math.Sin(s); }
        private static double cos(double s) { return Math.Cos(s); }

        public static int Pixel(IntPtr renderer, int x, int y)
        {
            return SDL.SDL_RenderDrawPoint(renderer, x, y);
        }
        public static int PixelRGBA(IntPtr renderer, int x, int y, byte r, byte g, byte b, byte a)
        {
            int result = 0;
            result |= SDL.SDL_SetRenderDrawBlendMode(renderer, (a == 255) ? SDL.SDL_BlendMode.SDL_BLENDMODE_NONE : SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            result |= SDL.SDL_SetRenderDrawColor(renderer, r, g, b, a);
            result |= SDL.SDL_RenderDrawPoint(renderer, x, y);
            return result;
        }
        public static int HLine(IntPtr render, int x1, int x2, int y)
        {
            return SDL.SDL_RenderDrawLine(render, x1, y, x2, y);
        }
        public static int HLineRGBA(IntPtr renderer, int x1, int x2, int y, byte r, byte g, byte b, byte a)
        {
            int result = 0;
            result |= SDL.SDL_SetRenderDrawBlendMode(renderer, (a == 255) ? SDL.SDL_BlendMode.SDL_BLENDMODE_NONE : SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            result |= SDL.SDL_SetRenderDrawColor(renderer, r, g, b, a);
            result |= SDL.SDL_RenderDrawLine(renderer, x1, y, x2, y);
            return result;
        }

        public static int VLineRGBA(IntPtr renderer, int x, int y1, int y2, byte r, byte g, byte b, byte a)
        {
            int result = 0;
            result |= SDL.SDL_SetRenderDrawBlendMode(renderer, (a == 255) ? SDL.SDL_BlendMode.SDL_BLENDMODE_NONE : SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            result |= SDL.SDL_SetRenderDrawColor(renderer, r, g, b, a);
            result |= SDL.SDL_RenderDrawLine(renderer, x, y1, x, y2);
            return result;
        }
        public static int BoxRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, byte r, byte g, byte b, byte a)
        {
            int result;
            int tmp;
            SDL.SDL_Rect rect;
            if (x1 > x2)
            {
                tmp = x1;
                x1 = x2;
                x2 = tmp;
            }
            if (y1 > y2)
            {
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }
            rect.x = x1;
            rect.y = y1;
            rect.w = x2 - x1 + 1;
            rect.h = y2 - y1 + 1;
            result = 0;
            result |= SDL.SDL_SetRenderDrawBlendMode(renderer, (a == 255) ? SDL.SDL_BlendMode.SDL_BLENDMODE_NONE : SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            result |= SDL.SDL_SetRenderDrawColor(renderer, r, g, b, a);
            result |= SDL.SDL_RenderFillRect(renderer, ref rect);
            return result;
        }

        public static int RoundedRectangle(IntPtr renderer, int x1, int y1, int x2, int y2, int rad, Color color)
        {
            return RoundedRectangleRGBA(renderer, x1, y1, x2, y2, rad, color.R, color.G, color.B, color.Alpha);
        }
        public static int RoundedRectangleRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, int rad, byte r, byte g, byte b, byte a)
        {
            int result = 0;
            int tmp;
            int w, h;
            int xx1, xx2;
            int yy1, yy2;
            if (renderer == IntPtr.Zero) return -1;
            if (rad < 0) return -1;
            if (x1 > x2)
            {
                tmp = x1;
                x1 = x2;
                x2 = tmp;
            }
            if (y1 > y2)
            {
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }
            w = x2 - x1;
            h = y2 - y1;
            if ((rad * 2) > w)
            {
                rad = w / 2;
            }
            if ((rad * 2) > h)
            {
                rad = h / 2;
            }
            xx1 = x1 + rad;
            xx2 = x2 - rad;
            yy1 = y1 + rad;
            yy2 = y2 - rad;
            result |= ArcRGBA(renderer, xx1, yy1, rad, 180, 270, r, g, b, a);
            result |= ArcRGBA(renderer, xx2, yy1, rad, 270, 360, r, g, b, a);
            result |= ArcRGBA(renderer, xx1, yy2, rad, 90, 180, r, g, b, a);
            result |= ArcRGBA(renderer, xx2, yy2, rad, 0, 90, r, g, b, a);
            if (xx1 <= xx2)
            {
                result |= HLineRGBA(renderer, xx1, xx2, y1, r, g, b, a);
                result |= HLineRGBA(renderer, xx1, xx2, y2, r, g, b, a);
            }
            if (yy1 <= yy2)
            {
                result |= VLineRGBA(renderer, x1, yy1, yy2, r, g, b, a);
                result |= VLineRGBA(renderer, x2, yy1, yy2, r, g, b, a);
            }
            return result;
        }

        public static int RoundedBox(IntPtr renderer, int x1, int y1, int x2, int y2, int rad, Color color)
        {
            return RoundedBoxRGBA(renderer, x1, y1, x2, y2, rad, color.R, color.G, color.B, color.Alpha);
        }
        public static int RoundedBoxRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, int rad, byte r, byte g, byte b, byte a)
        {
            int result;
            int w, h, r2, tmp;
            int cx = 0;
            int cy = rad;
            int ocx = 0xffff;
            int ocy = 0xffff;
            int df = 1 - rad;
            int d_e = 3;
            int d_se = -2 * rad + 5;
            int xpcx, xmcx, xpcy, xmcy;
            int ypcy, ymcy, ypcx, ymcx;
            int x, y, dx, dy;
            if (renderer == IntPtr.Zero) return -1;
            if (rad < 0) return -1;
            if (x1 > x2)
            {
                tmp = x1;
                x1 = x2;
                x2 = tmp;
            }
            if (y1 > y2)
            {
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }
            w = x2 - x1 + 1;
            h = y2 - y1 + 1;
            r2 = rad + rad;
            if (r2 > w)
            {
                rad = w / 2;
                r2 = rad + rad;
            }
            if (r2 > h)
            {
                rad = h / 2;
            }
            x = x1 + rad;
            y = y1 + rad;
            dx = x2 - x1 - rad - rad;
            dy = y2 - y1 - rad - rad;
            result = 0;
            result |= SDL.SDL_SetRenderDrawBlendMode(renderer, (a == 255) ? SDL.SDL_BlendMode.SDL_BLENDMODE_NONE : SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            result |= SDL.SDL_SetRenderDrawColor(renderer, r, g, b, a);
            do
            {
                xpcx = x + cx;
                xmcx = x - cx;
                xpcy = x + cy;
                xmcy = x - cy;
                if (ocy != cy)
                {
                    if (cy > 0)
                    {
                        ypcy = y + cy;
                        ymcy = y - cy;
                        result |= HLine(renderer, xmcx, xpcx + dx, ypcy + dy);
                        result |= HLine(renderer, xmcx, xpcx + dx, ymcy);
                    }
                    else
                    {
                        result |= HLine(renderer, xmcx, xpcx + dx, y);
                    }
                    ocy = cy;
                }
                if (ocx != cx)
                {
                    if (cx != cy)
                    {
                        if (cx > 0)
                        {
                            ypcx = y + cx;
                            ymcx = y - cx;
                            result |= HLine(renderer, xmcy, xpcy + dx, ymcx);
                            result |= HLine(renderer, xmcy, xpcy + dx, ypcx + dy);
                        }
                        else
                        {
                            result |= HLine(renderer, xmcy, xpcy + dx, y);
                        }
                    }
                    ocx = cx;
                }

                /*
                * Update 
                */
                if (df < 0)
                {
                    df += d_e;
                    d_e += 2;
                    d_se += 2;
                }
                else
                {
                    df += d_se;
                    d_e += 2;
                    d_se += 4;
                    cy--;
                }
                cx++;
            } while (cx <= cy);
            if (dx > 0 && dy > 0)
            {
                result |= BoxRGBA(renderer, x1, y1 + rad + 1, x2, y2 - rad, r, g, b, a);
            }
            return result;
        }

        public static int Arc(IntPtr renderer, int x, int y, int rad, int start, int end, Color color)
        {
            return ArcRGBA(renderer, x, y, rad, start, end, color.R, color.G, color.B, color.Alpha);
        }
        public static int ArcRGBA(IntPtr renderer, int x, int y, int rad, int start, int end, byte r, byte g, byte b, byte a)
        {
            int result;
            int cx = 0;
            int cy = rad;
            int df = 1 - rad;
            int d_e = 3;
            int d_se = -2 * rad + 5;
            int xpcx, xmcx, xpcy, xmcy;
            int ypcy, ymcy, ypcx, ymcx;
            int drawoct;
            int startoct, endoct, oct, stopval_start = 0, stopval_end = 0;
            double dstart, dend, temp = 0.0;

            /*
            * Sanity check radius 
            */
            if (rad < 0)
            {
                return (-1);
            }

            /*
            * Special case for rad=0 - draw a point 
            */
            if (rad == 0)
            {
                return (PixelRGBA(renderer, x, y, r, g, b, a));
            }

            /*
             Octant labeling

              \ 5 | 6 /
               \  |  /
              4 \ | / 7
                 \|/
            ------+------ +x
                 /|\
              3 / | \ 0
               /  |  \
              / 2 | 1 \
                  +y

             Initially reset bitmask to 0x00000000
             the set whether or not to keep drawing a given octant.
             For example: 0x00111100 means we're drawing in octants 2-5
            */
            drawoct = 0;

            /*
            * Fixup angles
            */
            start %= 360;
            end %= 360;
            /* 0 <= start & end < 360; note that sometimes start > end - if so, arc goes back through 0. */
            while (start < 0) start += 360;
            while (end < 0) end += 360;
            start %= 360;
            end %= 360;

            /* now, we find which octants we're drawing in. */
            startoct = start / 45;
            endoct = end / 45;
            oct = startoct - 1;

            /* stopval_start, stopval_end; what values of cx to stop at. */
            do
            {
                oct = (oct + 1) % 8;

                if (oct == startoct)
                {
                    /* need to compute stopval_start for this octant.  Look at picture above if this is unclear */
                    dstart = (double)start;
                    switch (oct)
                    {
                        case 0:
                        case 3:
                            temp = sin(dstart * M_PI / 180.0);
                            break;
                        case 1:
                        case 6:
                            temp = cos(dstart * M_PI / 180.0);
                            break;
                        case 2:
                        case 5:
                            temp = -cos(dstart * M_PI / 180.0);
                            break;
                        case 4:
                        case 7:
                            temp = -sin(dstart * M_PI / 180.0);
                            break;
                    }
                    temp *= rad;
                    stopval_start = (int)temp;

                    /* 
                    This isn't arbitrary, but requires graph paper to explain well.
                    The basic idea is that we're always changing drawoct after we draw, so we
                    stop immediately after we render the last sensible pixel at x = ((int)temp).
                    and whether to draw in this octant initially
                    */
                    if ((oct % 2) != 0) drawoct |= (1 << oct);         /* this is basically like saying drawoct[oct] = true, if drawoct were a bool array */
                    else drawoct &= 255 - (1 << oct);   /* this is basically like saying drawoct[oct] = false */
                }
                if (oct == endoct)
                {
                    /* need to compute stopval_end for this octant */
                    dend = (double)end;
                    switch (oct)
                    {
                        case 0:
                        case 3:
                            temp = sin(dend * M_PI / 180);
                            break;
                        case 1:
                        case 6:
                            temp = cos(dend * M_PI / 180);
                            break;
                        case 2:
                        case 5:
                            temp = -cos(dend * M_PI / 180);
                            break;
                        case 4:
                        case 7:
                            temp = -sin(dend * M_PI / 180);
                            break;
                    }
                    temp *= rad;
                    stopval_end = (int)temp;

                    /* and whether to draw in this octant initially */
                    if (startoct == endoct)
                    {
                        /* note:      we start drawing, stop, then start again in this case */
                        /* otherwise: we only draw in this octant, so initialize it to false, it will get set back to true */
                        if (start > end)
                        {
                            /* unfortunately, if we're in the same octant and need to draw over the whole circle, */
                            /* we need to set the rest to true, because the while loop will end at the bottom. */
                            drawoct = 255;
                        }
                        else
                        {
                            drawoct &= 255 - (1 << oct);
                        }
                    }
                    else if ((oct % 2) != 0) drawoct &= 255 - (1 << oct);
                    else drawoct |= (1 << oct);
                }
                else if (oct != startoct)
                { /* already verified that it's != endoct */
                    drawoct |= (1 << oct); /* draw this entire segment */
                }
            } while (oct != endoct);

            /* so now we have what octants to draw and when to draw them. all that's left is the actual raster code. */

            /*
            * Set color 
            */
            result = 0;
            result |= SDL.SDL_SetRenderDrawBlendMode(renderer, (a == 255) ? SDL.SDL_BlendMode.SDL_BLENDMODE_NONE : SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            result |= SDL.SDL_SetRenderDrawColor(renderer, r, g, b, a);

            /*
            * Draw arc 
            */
            do
            {
                ypcy = y + cy;
                ymcy = y - cy;
                if (cx > 0)
                {
                    xpcx = x + cx;
                    xmcx = x - cx;

                    /* always check if we're drawing a certain octant before adding a pixel to that octant. */
                    if ((drawoct & 4) != 0) result |= Pixel(renderer, xmcx, ypcy);
                    if ((drawoct & 2) != 0) result |= Pixel(renderer, xpcx, ypcy);
                    if ((drawoct & 32) != 0) result |= Pixel(renderer, xmcx, ymcy);
                    if ((drawoct & 64) != 0) result |= Pixel(renderer, xpcx, ymcy);
                }
                else
                {
                    if ((drawoct & 96) != 0) result |= Pixel(renderer, x, ymcy);
                    if ((drawoct & 6) != 0) result |= Pixel(renderer, x, ypcy);
                }

                xpcy = x + cy;
                xmcy = x - cy;
                if (cx > 0 && cx != cy)
                {
                    ypcx = y + cx;
                    ymcx = y - cx;
                    if ((drawoct & 8) != 0) result |= Pixel(renderer, xmcy, ypcx);
                    if ((drawoct & 1) != 0) result |= Pixel(renderer, xpcy, ypcx);
                    if ((drawoct & 16) != 0) result |= Pixel(renderer, xmcy, ymcx);
                    if ((drawoct & 128) != 0) result |= Pixel(renderer, xpcy, ymcx);
                }
                else if (cx == 0)
                {
                    if ((drawoct & 24) != 0) result |= Pixel(renderer, xmcy, y);
                    if ((drawoct & 129) != 0) result |= Pixel(renderer, xpcy, y);
                }

                /*
                * Update whether we're drawing an octant
                */
                if (stopval_start == cx)
                {
                    /* works like an on-off switch. */
                    /* This is just in case start & end are in the same octant. */
                    if ((drawoct & (1 << startoct)) != 0) drawoct &= 255 - (1 << startoct);
                    else drawoct |= (1 << startoct);
                }
                if (stopval_end == cx)
                {
                    if ((drawoct & (1 << endoct)) != 0) drawoct &= 255 - (1 << endoct);
                    else drawoct |= (1 << endoct);
                }

                /*
                * Update pixels
                */
                if (df < 0)
                {
                    df += d_e;
                    d_e += 2;
                    d_se += 2;
                }
                else
                {
                    df += d_se;
                    d_e += 2;
                    d_se += 4;
                    cy--;
                }
                cx++;
            } while (cx <= cy);

            return (result);
        }

    }
}
