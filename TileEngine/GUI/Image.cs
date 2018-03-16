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

namespace TileEngine.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TileEngine.Graphics;

    public class Image : Root, IBox
    {
        public Image()
        {
        }

        protected override int BeforeSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update)
        {
            return 0;
        }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.IA_Left:
                    LeftEdge = tag.GetTagData(0);
                    return 1;
                case Tags.IA_Top:
                    TopEdge = tag.GetTagData(0);
                    return 1;
                case Tags.IA_Width:
                    Width = tag.GetTagData(0);
                    return 1;
                case Tags.IA_Height:
                    Height = tag.GetTagData(0);
                    return 1;
                case Tags.IA_Data:
                    ImageData = tag.GetTagData<TextureRegion>();
                    return 1;
                default:
                    return 0;
            }
        }

        protected override int AfterSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, int returnValue)
        {
            return returnValue;
        }

        public virtual void Draw(IGraphics rport, int x, int y, ImageState state, DrawInfo drawInfo = null)
        {

        }

        public virtual void DrawFrame(IGraphics rport, int x, int y, int width, int height, ImageState state, DrawInfo drawInfo = null)
        {
            Draw(rport, x, y, state, drawInfo);
        }

        public virtual void Erase(IGraphics rport, int x, int y)
        {
            rport.RectFill(this, Color.Black);
        }

        public virtual void EraseFrame(IGraphics rport, int x, int y, int width, int height)
        {
            Erase(rport, x, y);
        }

        public virtual HitTestResult HitTest(int mouseX, int mouseY)
        {
            if (ContainsPoint(mouseX, mouseY))
            {
                return HitTestResult.ImageHit;
            }
            return HitTestResult.NoHit;
        }

        public virtual HitTestResult HitTestFrame(int mouseX, int mouseY, int width, int height)
        {
            return HitTest(mouseX, mouseY);
        }

        public virtual void FrameBox(ref IBox contentsBox, IBox frameBox, FrameFlags flags, DrawInfo drawInfo = null)
        {
            if (contentsBox == null) contentsBox = new Box();

        }

        public int LeftEdge { get; set; }
        public int TopEdge { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int RightEdge { get { return LeftEdge + Width - 1; } }
        public int BottomEdge { get { return TopEdge + Height - 1; } }
        public int CenterX { get { return LeftEdge + Width / 2; } }
        public int CenterY { get { return TopEdge + Height / 2; } }
        public bool ContainsPoint(int x, int y)
        {
            return ((LeftEdge <= x) && (TopEdge <= y) && (RightEdge >= x) && (BottomEdge >= y));
        }

        public TextureRegion ImageData { get; set; }
        public Image NextImage { get; set; }
    }
}
