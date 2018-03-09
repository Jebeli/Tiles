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

    public class Gadget
    {
        private Window window;
        private Requester requester;
        private int leftEdge;
        private int topEdge;
        private int width;
        private int height;
        private GadgetFlags flags;
        private GadgetActivation activation;
        private GadgetType gadgetType;
        private IntuiText gadgetText;
        private Border gadgetRender;
        private Border selectRender;
        private Image gadgetImage;
        private Image selectImage;
        private PropInfo propInfo;
        private StringInfo stringInfo;
        private int gadgetId;
        private Color bgColor;

        public Gadget()
        {

        }

        public Gadget(GadgetType type)
        {
            gadgetType = type;
        }

        public Gadget(string text)
        {
            gadgetText = new IntuiText(text);
        }

        public static Gadget MakeBoolGadget(string text, int width, int height, GadgetFlags flags = 0)
        {
            Gadget gadget = new Gadget(GadgetType.BOOLGADGET);
            gadget.width = width;
            gadget.height = height;
            gadget.gadgetText = new IntuiText(text);
            gadget.GadgetText.LeftEdge = Math.Abs(width) / 2;
            gadget.GadgetText.TopEdge = Math.Abs(height) / 2;
            gadget.Flags = flags;
            gadget.Flags |= GadgetFlags.GADGIMAGE;
            gadget.Activation = GadgetActivation.RELVERIFY;
            gadget.bgColor = Color.Gray;
            Intuition.InitGadget(gadget);
            return gadget;
        }

        public static Gadget MakePropGadget(int vertBody, int horzBody, int width, int height)
        {
            Gadget gadget = new Gadget(GadgetType.PROPGADGET);
            gadget.width = width;
            gadget.height = height;
            gadget.PropInfo = new PropInfo();
            gadget.PropInfo.VertBody = vertBody != 0 ? vertBody : PropInfo.MAXBODY;
            gadget.PropInfo.HorizBody = horzBody != 0 ? horzBody : PropInfo.MAXBODY;
            gadget.PropInfo.Flags = PropFlags.AUTOKNOB;
            if (vertBody < PropInfo.MAXBODY)
            {
                gadget.PropInfo.Flags |= PropFlags.FREEVERT;
            }
            if (horzBody < PropInfo.MAXBODY)
            {
                gadget.PropInfo.Flags |= PropFlags.FREEHORIZ;
            }
            Intuition.InitGadget(gadget);
            return gadget;
        }

        public static Gadget MakeStringGadget(string text, int width, int height)
        {
            Gadget gadget = new Gadget(GadgetType.STRGADGET);
            gadget.width = width;
            gadget.height = height;
            gadget.StringInfo = new StringInfo(text);
            Intuition.InitGadget(gadget);
            return gadget;
        }

        public void SetBounds(int x, int y, int w, int h)
        {
            leftEdge = x;
            topEdge = y;
            width = w;
            height = h;
        }

        public void SetPosition(int x, int y)
        {
            if ((leftEdge != x) || (topEdge != y))
            {
                leftEdge = x;
                topEdge = y;
                if (window != null) window.Invalidate();
            }
        }

        public Color BgColor
        {
            get { return bgColor; }
            set { bgColor = value; }
        }
        public int GadgetId
        {
            get { return gadgetId; }
            set { gadgetId = value; }
        }

        public Window Window
        {
            get { return window; }
            set { window = value; }
        }

        public Requester Requester
        {
            get { return requester; }
            set
            {
                requester = value;
                if (requester == null)
                {
                    gadgetType &= ~GadgetType.REQGADGET;
                }
                else
                {
                    gadgetType |= GadgetType.REQGADGET;
                }
            }
        }

        public int LeftEdge
        {
            get { return leftEdge; }
            set { leftEdge = value; }
        }

        public int TopEdge
        {
            get { return topEdge; }
            set { topEdge = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public GadgetFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public GadgetActivation Activation
        {
            get { return activation; }
            set { activation = value; }
        }

        public GadgetType GadgetType
        {
            get { return gadgetType; }
            set { gadgetType = value; }
        }

        public IntuiText GadgetText
        {
            get { return gadgetText; }
            set { gadgetText = value; }
        }

        public Border GadgetRender
        {
            get { return gadgetRender; }
            set { gadgetRender = value; }
        }
        public Border SelectRender
        {
            get { return selectRender; }
            set { selectRender = value; }
        }

        public bool Selected
        {
            get { return (flags & GadgetFlags.SELECTED) == GadgetFlags.SELECTED; }
            set
            {
                if (value)
                {
                    flags |= GadgetFlags.SELECTED;
                }
                else
                {
                    flags &= ~GadgetFlags.SELECTED;
                }
            }
        }

        public Image Image
        {
            get { return Selected ? selectImage : gadgetImage; }
        }

        public Image GadgetImage
        {
            get { return gadgetImage; }
            set { gadgetImage = value; }
        }

        public Image SelectImage
        {
            get { return selectImage; }
            set { selectImage = value; }
        }

        public PropInfo PropInfo
        {
            get { return propInfo; }
            set { propInfo = value; }
        }

        public StringInfo StringInfo
        {
            get { return stringInfo; }
            set { stringInfo = value; }
        }

        public override string ToString()
        {
            if (gadgetText != null) return gadgetText.IText;
            if (stringInfo != null) return $"StringGadget {stringInfo.Buffer}";
            if (propInfo != null) return $"PropGadget {propInfo.VertPot}";
            return gadgetType.ToString();
        }
    }
}
