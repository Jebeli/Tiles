using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Graphics;

namespace TileEngine.GUI
{
    public class FrameImage : Image
    {
        public FrameImage() 
        {
        }

        protected override int SetTag(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, (Tags, object) tag)
        {
            switch (tag.Item1)
            {
                case Tags.IA_Recessed:
                    Recessed = tag.GetTagData(false);
                    return 1;
                case Tags.IA_EdgesOnly:
                    EdgesOnly = tag.GetTagData(false);
                    return 1;
                case Tags.IA_FrameType:
                    FrameType = tag.GetTagData(FrameType.Default);
                    return 1;
                case Tags.IA_ReadOnly:
                    ReadOnly = tag.GetTagData(false);
                    return 1;
                default:
                    return base.SetTag(gadgetInfo, set, update, tag);
            }
        }

        protected override int AfterSetTags(GadgetInfo gadgetInfo, SetFlags set, UpdateFlags update, int returnValue)
        {
            switch (FrameType)
            {
                case FrameType.Default:
                    HOffset = 1;
                    VOffset = 1;
                    break;
                case FrameType.Button:
                    HOffset = 1;
                    VOffset = 1;
                    break;
                case FrameType.Ridge:
                    HOffset = 2;
                    VOffset = 2;
                    break;
                case FrameType.IconDropBox:
                    HOffset = 3;
                    VOffset = 3;
                    break;
            }
            return base.AfterSetTags(gadgetInfo, set, update, returnValue);
        }

        public override void Draw(IGraphics rport, int x, int y, ImageState state, DrawInfo drawInfo = null)
        {
            InternalDrawFrame(rport, x, y, Width, Height, state, drawInfo);
        }

        public override void DrawFrame(IGraphics rport, int x, int y, int width, int height, ImageState state, DrawInfo drawInfo = null)
        {
            InternalDrawFrame(rport, x, y, width, height, state, drawInfo);
        }

        private void InternalDrawFrame(IGraphics rport, int x, int y, int width, int height, ImageState state, DrawInfo drawInfo = null)
        {
            if (rport != null)
            {
                bool selected = (state & ImageState.Selected) == ImageState.Selected;
                bool hover = (state & ImageState.Hover) == ImageState.Hover;
                bool disabled = (state & ImageState.Disabled) == ImageState.Disabled;
                if (ReadOnly)
                {
                    selected = false;
                    hover = false;
                }
                Color shine = Color.White;
                Color shadow = Color.Black;
                Color fillColor = Color.Gray;
                Color backColor = Color.Black;
                if (drawInfo != null)
                {
                    shine = drawInfo.ShinePen;
                    shadow = drawInfo.ShadowPen;
                    backColor = drawInfo.BackgoundPen;
                    if (hover && !disabled)
                    {
                        shine = drawInfo.HoverShinePen;
                        shadow = drawInfo.HoverShadowPen;
                        backColor = drawInfo.InactiveHoverBackgroundPen;
                    }
                }
                if (Recessed ^ selected)
                {
                    Color temp = shine;
                    shine = shadow;
                    shadow = temp;
                }
                int left = LeftEdge + x;
                int top = TopEdge + y;
                switch (FrameType)
                {
                    case FrameType.Default:
                        InternalDrawFrame(rport, shine, shadow, left, top, width, height, false);
                        break;
                    case FrameType.Button:
                        InternalDrawFrame(rport, shine, shadow, left, top, width, height, true);
                        break;
                    case FrameType.Ridge:
                        InternalDrawFrame(rport, shine, shadow, left, top, width, height, true);
                        InternalDrawFrame(rport, shadow, shine, left + HOffset / 2, top + VOffset / 2, width - HOffset, height - VOffset, true);
                        break;
                    case FrameType.IconDropBox:
                        InternalDrawFrame(rport, shine, shadow, left, top, width, height, true);
                        int hoffset = HOffset * 2 / 3;
                        int voffset = VOffset * 2 / 3;
                        InternalDrawFrame(rport, shadow, shine, left + hoffset, top + voffset, width - hoffset * 2, height - voffset * 2, true);
                        break;
                }
                if (!EdgesOnly)
                {
                    rport.FillRectangle(left + HOffset, top + VOffset, width - HOffset - 1, height - VOffset - 1, backColor);
                }
                if (disabled)
                {
                    Color color = new Color(128, 128, 128, 128);
                    rport.FillRectangle(left, top, width, height, color);
                }
            }
        }

        internal static void InternalDrawFrame(IGraphics rport, Color shine, Color shadow, int left, int top, int width, int height, bool thicken)
        {
            height -= 1;
            width -= 1;
            rport.DrawLine(left, top + height, left, top, shine);
            rport.DrawLine(left, top, left + width, top, shine);
            rport.DrawLine(left + width, top, left + width, top + height, shadow);
            rport.DrawLine(left + width, top + height, left + 1, top + height, shadow);
            if (thicken)
            {
                rport.DrawLine(left + width - 1, top + height - 1, left + width - 1, top + 1, shadow);
                rport.DrawLine(left + 1, top + height - 1, left + 1, top + 1, shine);
            }
        }

        public bool Recessed { get; set; }
        public bool EdgesOnly { get; set; }
        public bool ReadOnly { get; set; }
        public FrameType FrameType { get; set; }
        public int HOffset { get; set; }
        public int VOffset { get; set; }

    }
}
