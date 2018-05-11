using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;

namespace TileEngine.Nano
{
    public class VScrollPanel : Widget
    {
        protected int childPreferredHeight;
        protected float scroll;
        protected bool updateLayout;
        protected Texture bmp;
        public VScrollPanel(Widget parent)
            : base(parent)
        {

        }

        public float Scroll
        {
            get { return scroll; }
            set { scroll = value; }
        }

        public override void PerformLayout(IGraphics gfx)
        {
            base.PerformLayout(gfx);
            if (children.Count == 0) return;
            if (children.Count > 1) throw new InvalidOperationException("VScrollPanel should have only one child.");
            Widget child = children[0];
            childPreferredHeight = child.GetPreferredSize(gfx).Y;
            if (childPreferredHeight > size.Y)
            {
                child.Position = new Vector2(0, (int)(-scroll * (childPreferredHeight - size.Y)));
                child.Size = new Vector2(size.X - 12, childPreferredHeight);
            }
            else
            {
                child.Position = new Vector2();
                child.Size = size;
                scroll = 0.0f;
            }
            bmp?.Dispose();
            bmp = gfx.CreateTexture("vscroll", size.X, size.Y);
            child.PerformLayout(gfx);
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            if (children.Count == 0) return new Vector2();
            return children[0].GetPreferredSize(gfx) + new Vector2(12, 0);
        }

        public override bool MouseDragEvent(Vector2 p, Vector2 rel, MouseButton button)
        {
            if (children.Count > 0 && childPreferredHeight > size.Y)
            {
                float scrollh = Height * Math.Min(1.0f, Height / (float)childPreferredHeight);
                scroll = Math.Max(0.0f, Math.Min(1.0f, scroll + rel.Y / (size.Y - 8 - scrollh)));
                updateLayout = true;
                return true;
            }
            else
            {
                return base.MouseDragEvent(p, rel, button);
            }
        }

        public override bool ScrollEvent(Vector2 p, Vector2 rel)
        {
            if (children.Count > 0 && childPreferredHeight > size.Y)
            {
                float scrollAmount = rel.Y * (size.Y / 20.0f);
                float scrollh = Height * Math.Min(1.0f, Height / (float)childPreferredHeight);
                scroll = Math.Max(0.0f, Math.Min(1.0f, scroll - scrollAmount / (size.Y - 8 - scrollh)));
                updateLayout = true;
                return true;
            }
            else
            {
                return base.ScrollEvent(p, rel);
            }
        }

        public override void Draw(IGraphics gfx)
        {
            if (children.Count == 0) return;
            Widget child = children[0];
            child.Position = new Vector2(0, (int)(-scroll * (childPreferredHeight - size.Y)));
            childPreferredHeight = child.GetPreferredSize(gfx).Y;
            float scrollh = Height * Math.Min(1.0f, Height / (float)childPreferredHeight);
            if (updateLayout) child.PerformLayout(gfx);
            if (child.Visible)
            {
                //var cgfx = gfx.Clone();
                //cgfx.SetTarget(bmp);
                //child.Draw(cgfx);
                //cgfx.ClearTarget();

                //gfx.SetTarget(bmp);
                gfx.SaveState();
                gfx.Translate(pos.X, pos.Y);
                gfx.SetClip(0, 0, size.X, size.Y);
                child.Draw(gfx);
                gfx.ClearClip();
                //gfx.ClearTarget();
                //gfx.SaveState();
                //gfx.Translate(pos.X, pos.Y);
                //gfx.Render(bmp, pos.X, pos.Y);
                gfx.RestoreState();                
            }
            if (childPreferredHeight <= size.Y) return;
            Color b1 = new Color(0, 0, 0, 32);
            Color b2 = new Color(0, 0, 0, 92);
            gfx.FillRectangle(pos.X + size.X - 12 + 1, pos.Y + 4 + 1, 8, size.Y - 8, b1, b2);
            Color b3 = new Color(220, 220, 220, 100);
            Color b4 = new Color(128, 128, 128, 100);
            gfx.FillRectangle(pos.X + size.X - 12 + 1, (int)(pos.Y + 4 + 1 + (size.Y - 8 - scrollh) * scroll), 8 - 2, (int)(scrollh - 2), b3, b4);
            //base.Draw(gfx);
        }


    }
}
