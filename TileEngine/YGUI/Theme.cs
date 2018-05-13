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

namespace TileEngine.YGUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Core;
    using TileEngine.Fonts;
    using TileEngine.Graphics;

    public interface ITheme
    {
        Color LinePen { get; set; }
        Color ShinePen { get; set; }
        Color ShadowPen { get; set; }
        Color WindowBorderPen { get; set; }
        Color WindowInactiveBorderPen { get; set; }
        Color WindowBackPen { get; set; }
        Color WindowHoverPen { get; set; }
        Color WindowSelectedPen { get; set; }
        Color GadgetBackPen { get; set; }
        Color GadgetHoverPen { get; set; }
        Color GadgetSelectedPen { get; set; }
        Color TextPen { get; set; }
        Color SelectedTextPen { get; set; }

        Icons ArrowLeft { get; set; }
        Icons ArrowRight { get; set; }
        Icons ArrowUp { get; set; }
        Icons ArrowDown { get; set; }

        Icons CloseIcon { get; set; }
        Icons SizeIcon { get; set; }
        Icons DepthIcon { get; set; }

        void RenderGadget(IGraphics gfx, Gadget gadget);
        void RenderScreen(IGraphics gfx, Screen screen);
        void RenderWindow(IGraphics gfx, Window window);
        void RenderGadget(IGraphics gfx, ButtonGadget gadget);
        void RenderGadget(IGraphics gfx, CheckBoxGadget gadget);
        void RenderGadget(IGraphics gfx, LabelGadget gadget);
        void RenderGadget(IGraphics gfx, ItemGadget gadget);
        void RenderGadget(IGraphics gfx, StrGadget gadget);
        void RenderGadget(IGraphics gfx, PropGadget gadget);
        void RenderGadget(IGraphics gfx, ScrollbarGadget gadget);
        void RenderGadget(IGraphics gfx, ProgressbarGadget gadget);
        void RenderGadget(IGraphics gfx, ChooserGadget gadget);
        void RenderGadget(IGraphics gfx, TabPanelGadget gadget);
        void RenderGadget(IGraphics gfx, TableGadget gadget);
        void RenderGadget(IGraphics gfx, ImageGadget gadget);
    }

    public class BaseTheme : ITheme
    {
        public Color LinePen { get; set; }
        public Color ShinePen { get; set; }
        public Color ShadowPen { get; set; }
        public Color WindowBorderPen { get; set; }
        public Color WindowInactiveBorderPen { get; set; }
        public Color WindowBackPen { get; set; }
        public Color WindowHoverPen { get; set; }
        public Color WindowSelectedPen { get; set; }
        public Color GadgetBackPen { get; set; }
        public Color GadgetHoverPen { get; set; }
        public Color GadgetSelectedPen { get; set; }
        public Color TextPen { get; set; }
        public Color SelectedTextPen { get; set; }

        public Icons ArrowLeft { get; set; }
        public Icons ArrowRight { get; set; }
        public Icons ArrowUp { get; set; }
        public Icons ArrowDown { get; set; }

        public Icons CloseIcon { get; set; }
        public Icons SizeIcon { get; set; }
        public Icons DepthIcon { get; set; }


        public BaseTheme()
        {
            WindowBackPen = new Color(125, 125, 125);
            WindowHoverPen = new Color(125, 125, 125);
            WindowSelectedPen = new Color(125, 125, 125);
            //WindowHoverPen = new Color(130, 130, 130);
            //WindowSelectedPen = new Color(130, 130, 130);
            GadgetBackPen = new Color(125, 125, 125);
            GadgetHoverPen = new Color(130, 130, 130);
            GadgetSelectedPen = new Color(50, 50, 120);
            LinePen = new Color(30, 30, 30);
            ShinePen = new Color(250, 250, 250, 128);
            ShadowPen = new Color(10, 10, 10, 128);
            TextPen = new Color(240, 240, 240);
            SelectedTextPen = new Color(255, 255, 255);
            WindowInactiveBorderPen = new Color(130, 130, 130);
            WindowBorderPen = new Color(103, 136, 187);


            //ArrowLeft = Icons.ENTYPO_ICON_ARROW_LEFT;
            //ArrowRight = Icons.ENTYPO_ICON_ARROW_RIGHT;
            //ArrowDown = Icons.ENTYPO_ICON_ARROW_DOWN;
            //ArrowUp = Icons.ENTYPO_ICON_ARROW_UP;

            ArrowLeft = Icons.ENTYPO_ICON_TRIANGLE_LEFT;
            ArrowRight = Icons.ENTYPO_ICON_TRIANGLE_RIGHT;
            ArrowDown = Icons.ENTYPO_ICON_TRIANGLE_DOWN;
            ArrowUp = Icons.ENTYPO_ICON_TRIANGLE_UP;

            CloseIcon = Icons.ENTYPO_ICON_CROSS;
            SizeIcon = Icons.ENTYPO_ICON_RESIZE_FULL_SCREEN;
            DepthIcon = Icons.ENTYPO_ICON_POPUP;
        }


        public void RenderGadget(IGraphics gfx, Gadget gadget)
        {
        }

        public void RenderScreen(IGraphics gfx, Screen screen)
        {
        }

        public void RenderWindow(IGraphics gfx, Window window)
        {
            var frame = window.Bounds;
            if (!window.Borderless)
            {
                FillBox(gfx, frame, window.Focused ? WindowBorderPen : WindowInactiveBorderPen);
                DrawBox(gfx, frame, ShinePen, ShadowPen);
            }
            var cframe = window.ClientBounds;
            FillBox(gfx, cframe, window.MouseSelected ? WindowSelectedPen : window.Hover ? WindowHoverPen : WindowBackPen);
            if (!window.Borderless)
            {
                DrawBox(gfx, cframe, ShadowPen, ShinePen);
            }
            if (!string.IsNullOrEmpty(window.Label))
            {
                gfx.RenderText(window.Font, window.Label, window.CenterX, window.TopEdge + window.BorderTop / 2, window.Focused ? SelectedTextPen : TextPen);
            }
        }

        public void RenderGadget(IGraphics gfx, ButtonGadget gadget)
        {
            var frame = gadget.Bounds;
            if (!gadget.TransparentBackground)
            {
                FillBox(gfx, frame, gadget.DrawSelected ? GadgetSelectedPen : gadget.Hover ? GadgetHoverPen : GadgetBackPen);
            }
            if (!gadget.Borderless)
            {
                DrawBox(gfx, frame, gadget.DrawSelected ? ShadowPen : ShinePen, gadget.DrawSelected ? ShinePen : ShadowPen);
            }
            if (!string.IsNullOrEmpty(gadget.Label))
            {
                gfx.RenderText(gadget.Font, gadget.Label, gadget.CenterX, gadget.CenterY, gadget.DrawSelected ? SelectedTextPen : TextPen);
            }
            if (gadget.Icon != Icons.NONE)
            {
                int offset = gadget.DrawSelected ? 1 : 0;
                gfx.RenderIcon((int)gadget.Icon, gadget.CenterX + offset, gadget.CenterY + offset, gadget.DrawSelected ? SelectedTextPen : TextPen);
            }
            if (!gadget.Enabled)
            {
                FillBox(gfx, frame, new Color(250, 250, 250, 128));
            }
        }

        public void RenderGadget(IGraphics gfx, CheckBoxGadget gadget)
        {
            var frame = gadget.Bounds;
            var frameB = frame;
            frameB.Width = 24;
            if (!gadget.TransparentBackground)
            {
                FillBox(gfx, frameB, gadget.DrawSelected ? GadgetSelectedPen : gadget.Hover ? GadgetHoverPen : GadgetBackPen);
            }
            DrawBox(gfx, frameB, gadget.DrawSelected ? ShadowPen : ShinePen, gadget.DrawSelected ? ShinePen : ShadowPen);
            if (!string.IsNullOrEmpty(gadget.Label))
            {
                gfx.RenderText(gadget.Font, gadget.Label, frameB.Right + 4, gadget.CenterY, gadget.DrawSelected ? SelectedTextPen : TextPen, HorizontalTextAlign.Left);
            }
            if (gadget.Checked)
            {
                gfx.RenderIcon((int)Icons.ENTYPO_ICON_CHECK, frameB.CenterX, frameB.CenterY, gadget.DrawSelected ? SelectedTextPen : TextPen);
            }
            if (!gadget.Enabled)
            {
                FillBox(gfx, frame, new Color(250, 250, 250, 128));
            }
        }

        public void RenderGadget(IGraphics gfx, LabelGadget gadget)
        {
            var frame = gadget.Bounds;
            if (!gadget.TransparentBackground)
            {
                FillBox(gfx, frame, gadget.DrawSelected ? GadgetSelectedPen : gadget.Hover ? GadgetHoverPen : GadgetBackPen);
            }
            if (gadget.Frame)
            {
                gfx.DrawRectangle(frame.X, frame.Y, frame.Width, frame.Height, ShadowPen);
            }
            if (!string.IsNullOrEmpty(gadget.Label))
            {
                gfx.RenderText(gadget.Font, gadget.Label, frame.CenterX, frame.CenterY, gadget.DrawSelected ? SelectedTextPen : TextPen);
            }
        }

        public void RenderGadget(IGraphics gfx, ItemGadget gadget)
        {
            var frame = gadget.Bounds;
            if (!gadget.TransparentBackground)
            {
                FillBox(gfx, frame, gadget.DrawSelected ? GadgetSelectedPen : gadget.Hover ? GadgetHoverPen : GadgetBackPen);
            }
            if (!string.IsNullOrEmpty(gadget.Label))
            {
                gfx.RenderText(gadget.Font, gadget.Label, gadget.CenterX, gadget.CenterY, gadget.DrawSelected ? SelectedTextPen : TextPen);
            }
        }

        public void RenderGadget(IGraphics gfx, StrGadget gadget)
        {
            var frame = gadget.Bounds;
            var lFrame = gadget.LabelBounds;
            var cFrame = gadget.ContainerBounds;
            if (!string.IsNullOrEmpty(gadget.Label))
            {
                gfx.RenderText(gadget.Font, gadget.Label, lFrame.CenterX, lFrame.CenterY, gadget.MouseSelected ? SelectedTextPen : TextPen);
            }
            DrawBox(gfx, cFrame, ShadowPen, ShinePen);
            string buffer = gadget.Buffer + " ";
            int lineSkip = 20;
            if (!string.IsNullOrEmpty(buffer))
            {
                gfx.SaveState();
                gfx.SetClip(cFrame.X, cFrame.Y, cFrame.Width, cFrame.Height);
                cFrame.Inflate(-2, -2);
                int x = cFrame.X;
                int y = cFrame.Y;
                bool selected = false;
                for (int i = 0; i < buffer.Length; i++)
                {
                    var c = buffer[i];

                    selected = (i >= gadget.BufferSelStart && i < gadget.BufferSelEnd);
                    if (selected)
                    {
                        gfx.RenderText(gadget.Font, "" + c, x, y, SelectedTextPen, GadgetSelectedPen, HorizontalTextAlign.Left, VerticalTextAlign.Top);
                    }
                    else
                    {
                        gfx.RenderText(gadget.Font, "" + c, x, y, TextPen, HorizontalTextAlign.Left, VerticalTextAlign.Top);
                    }
                    if (i == gadget.BufferPos) // drawCursor
                    {
                        if (gadget.Focused)
                        {
                            gfx.DrawLine(x, y, x, y + lineSkip - 1, TextPen);
                        }
                    }
                    int ax = gfx.MeasureTextWidth(gadget.Font, "" + c);
                    x += ax;
                }
                gfx.RestoreState();
            }
        }

        public void RenderGadget(IGraphics gfx, PropGadget gadget)
        {
            var frame = gadget.Bounds;
            if (!gadget.TransparentBackground)
            {
                FillBox(gfx, frame, WindowBackPen);
            }
            if (!gadget.Borderless)
            {
                gfx.DrawRectangle(frame.X, frame.Y, frame.Width, frame.Height, ShadowPen);
            }
            var knob = gadget.Knob;
            FillBox(gfx, knob, gadget.DrawSelected ? GadgetSelectedPen : gadget.Hover ? GadgetHoverPen : GadgetBackPen);
            DrawBox(gfx, knob, gadget.DrawSelected ? ShadowPen : ShinePen, gadget.MouseSelected ? ShinePen : ShadowPen);
        }

        public void RenderGadget(IGraphics gfx, ScrollbarGadget gadget)
        {
            var frame = gadget.Bounds;
            DrawBox(gfx, frame, ShadowPen, ShinePen);
        }

        public void RenderGadget(IGraphics gfx, ProgressbarGadget gadget)
        {
            var frame = gadget.Bounds;
            DrawBox(gfx, frame, ShadowPen, ShinePen);
            var filled = gadget.ProgressRect;
            FillBox(gfx, filled, GadgetSelectedPen);
            if (gadget.ShowPercent)
            {
                gfx.RenderText(gadget.Font, gadget.PercentText, frame.CenterX, frame.CenterY, TextPen);
            }
            else if (gadget.ShowPart)
            {
                gfx.RenderText(gadget.Font, gadget.PartText, frame.CenterX, frame.CenterY, TextPen);
            }
        }

        public void RenderGadget(IGraphics gfx, ChooserGadget gadget)
        {
            var frame = gadget.Bounds;
            if (!gadget.TransparentBackground)
            {
                FillBox(gfx, frame, gadget.DrawSelected ? GadgetSelectedPen : gadget.Hover ? GadgetHoverPen : GadgetBackPen);
            }
            if (!gadget.Borderless)
            {
                DrawBox(gfx, frame, gadget.DrawSelected ? ShadowPen : ShinePen, gadget.DrawSelected ? ShinePen : ShadowPen);
            }
            if (!string.IsNullOrEmpty(gadget.Label))
            {
                gfx.RenderText(gadget.Font, gadget.Label, frame.CenterX, frame.CenterY, gadget.DrawSelected ? SelectedTextPen : TextPen);
            }
            if (gadget.Icon != Icons.NONE)
            {
                gfx.RenderIcon((int)gadget.Icon, frame.CenterX, frame.CenterY, gadget.DrawSelected ? SelectedTextPen : TextPen);
            }
            gfx.RenderIcon((int)Icons.ENTYPO_ICON_CHEVRON_DOWN, frame.Right, frame.CenterY, gadget.DrawSelected ? SelectedTextPen : TextPen, HorizontalTextAlign.Right, VerticalTextAlign.Center);
            if (!gadget.Enabled)
            {
                FillBox(gfx, frame, new Color(250, 250, 250, 128));
            }
        }

        public void RenderGadget(IGraphics gfx, TabPanelGadget gadget)
        {
            var frame = gadget.Bounds;
            gfx.DrawLine(frame.Left, frame.Y + 24, frame.Left, frame.Bottom - 1, ShadowPen);
            gfx.DrawLine(frame.Right, frame.Y + 24, frame.Right, frame.Bottom - 1, ShadowPen);
            gfx.DrawLine(frame.Left, frame.Bottom, frame.Right, frame.Bottom, ShadowPen);

        }

        public void RenderGadget(IGraphics gfx, TableGadget gadget)
        {
            var frame = gadget.Bounds;
            frame.Width -= 20;
            DrawBox(gfx, frame, ShadowPen, ShinePen);
            if (gadget.ShowHeader)
            {
                DrawTableHeader(gfx, gadget);
            }
            DrawTable(gfx, gadget);
        }

        public void DrawTableHeader(IGraphics gfx, TableGadget gadget)
        {
            var frame = gadget.HeaderRect;
            gfx.SaveState();
            gfx.SetClip(frame.X, frame.Y, frame.Width, frame.Height);
            gfx.Translate(frame.X, frame.Y);
            int y = 0;
            int height = frame.Height;
            foreach (var col in gadget.Columns)
            {
                Rect colBox = new Rect(col.X, y, col.PixelWidth, height);
                DrawBox(gfx, colBox, ShinePen, ShadowPen);
                DrawText(gfx, col.Label, gadget.Font, colBox, TextPen, col.HTextAlign, col.VTextAlign);
            }
            gfx.RestoreState();
        }

        public void DrawTable(IGraphics gfx, TableGadget gadget)
        {
            var frame = gadget.TableRect;
            DrawBox(gfx, frame, ShinePen, ShadowPen);
            frame.Inflate(-1, -1);
            gfx.SaveState();
            gfx.SetClip(frame.X, frame.Y, frame.Width, frame.Height);
            int start = gadget.FirstVisibleRow;
            int end = gadget.LastVisibleRow;
            gfx.Translate(frame.X, frame.Y);
            TableGadget.TableRow row = null;
            for (int i = start; i < end && i < gadget.Rows.Count; i++)
            {
                row = gadget.Rows[i];
                DrawRow(gfx, row);
            }
            if (row != null)
            {
                if (row.Y + gadget.RowHeight < frame.Bottom)
                {
                    DrawEmptyRow(gfx, row);
                }
            }
            gfx.RestoreState();
        }

        public void DrawRow(IGraphics gfx, TableGadget.TableRow row)
        {
            foreach (var cell in row.Cells)
            {
                DrawCell(gfx, cell);
            }
        }

        public void DrawEmptyRow(IGraphics gfx, TableGadget.TableRow row)
        {
            foreach (var cell in row.Cells)
            {
                DrawEmptyCell(gfx, cell);
            }
        }

        public void DrawCell(IGraphics gfx, TableGadget.TableCell cell)
        {
            int x = cell.X;
            int y = cell.Y;
            int w = cell.Width;
            int h = cell.Height;
            Rect cellBox = new Rect(x, y, w, h);
            FillBox(gfx, cellBox, cell.DrawSelected ? GadgetSelectedPen : cell.Hover ? GadgetHoverPen : GadgetBackPen);
            if (!cell.IsFirstInRow)
            {
                gfx.DrawLine(x - 1, y, x - 1, y + h, ShinePen);
            }
            gfx.DrawLine(x + w - 2, y, x + w - 2, y + h, ShadowPen);
            if (cell.Image != null)
            {
                DrawImage(gfx, cellBox.Inflated(-1, -1), cell.Image);
            }
            if (!string.IsNullOrEmpty(cell.Label))
            {
                var textBox = cellBox;
                if (cell.Icon != Icons.NONE)
                {
                    textBox.X += 24;
                    textBox.Width -= 24;
                }
                DrawText(gfx, cell.Label, cell.Font, textBox, cell.DrawSelected ? SelectedTextPen : TextPen, cell.HTextAlign, cell.VTextAlign);
            }
            if (cell.Icon != Icons.NONE)
            {
                gfx.RenderIcon((int)cell.Icon, x + 4, cellBox.CenterY, cell.DrawSelected ? SelectedTextPen : TextPen, HorizontalTextAlign.Left);
            }
        }

        public void DrawEmptyCell(IGraphics gfx, TableGadget.TableCell cell)
        {
            int x = cell.X;
            int y = cell.Y + cell.Height;
            int w = cell.Width;
            int h = cell.Table.Height - cell.Y;
            Rect cellBox = new Rect(x, y, w, h);
            FillBox(gfx, cellBox, GadgetBackPen);
            if (!cell.IsFirstInRow)
            {
                gfx.DrawLine(x - 1, y, x - 1, y + h, ShinePen);
            }
            gfx.DrawLine(x + w - 2, y, x + w - 2, y + h, ShadowPen);
        }


        public void RenderGadget(IGraphics gfx, ImageGadget gadget)
        {
            var frame = gadget.Bounds;
            if (gadget.Frame)
            {
                DrawBox(gfx, frame, ShadowPen, ShinePen);
                frame.Inflate(-1, -1);
            }
            if (gadget.Image != null)
            {
                gfx.Render(gadget.Image, frame.X - gadget.Image.OffsetX, frame.Y - gadget.Image.OffsetY);
            }
        }

        public void DrawImage(IGraphics gfx, Rect rect, TextureRegion image)
        {
            int ox = image.OffsetX;
            int oy = image.OffsetY;
            int imgW = image.Width;
            int imgH = image.Height;
            if (imgW <= rect.Width && imgH <= rect.Height)
            {
                gfx.Render(image, rect.X - ox, rect.Y - oy);
            }
            else if (imgW <= rect.Width)
            {
                gfx.Render(image, rect.X - ox, rect.Y - oy, imgW, rect.Height);
            }
            else if (imgH <= rect.Height)
            {
                gfx.Render(image, rect.X - ox, rect.Y - oy, rect.Width, imgH);
            }
            else
            {
                gfx.Render(image, rect.X - ox, rect.Y - oy, rect.Width, rect.Height);
            }
        }

        public void DrawBox(IGraphics gfx, Rect rect, Color shine, Color shadow)
        {
            gfx.DrawLine(rect.Left, rect.Top + 1, rect.Left, rect.Bottom, shine);
            gfx.DrawLine(rect.Left, rect.Top, rect.Right, rect.Top, shine);
            gfx.DrawLine(rect.Right, rect.Top, rect.Right, rect.Bottom - 1, shadow);
            gfx.DrawLine(rect.Left, rect.Bottom, rect.Right, rect.Bottom, shadow);
        }

        public void FillBox(IGraphics gfx, Rect rect, Color color)
        {
            gfx.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height, color);
        }

        public void DrawText(IGraphics gfx, string text, Font font, Rect rect, Color color, HorizontalTextAlign hta, VerticalTextAlign vta)
        {
            int x = rect.CenterX;
            int y = rect.CenterY;
            switch (hta)
            {
                case HorizontalTextAlign.Left:
                    x = rect.Left + 2;
                    break;
                case HorizontalTextAlign.Right:
                    x = rect.Right - 2;
                    break;
            }
            switch (vta)
            {
                case VerticalTextAlign.Top:
                    y = rect.Top + 1;
                    break;
                case VerticalTextAlign.Bottom:
                    y = rect.Bottom - 1;
                    break;
            }
            gfx.RenderText(font, text, x, y, color, hta, vta);
        }
    }
}
