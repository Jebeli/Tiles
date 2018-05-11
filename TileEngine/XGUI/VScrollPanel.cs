using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Core;
using TileEngine.Graphics;
using TileEngine.Input;
using TileEngine.Logging;

namespace TileEngine.XGUI
{
    public class VScrollPanel : Widget
    {
        private int childPreferredHeight;
        private bool updateLayout;
        private Scroller scroller;
        private int level;
        public VScrollPanel(Widget parent)
            : base(parent)
        {
            scroller = new Scroller(this, Orientation.Vertical);
            scroller.LevelChanged += Scroller_LevelChanged;
            scroller.Visible = false;
            updateLayout = true;
        }

        private void Scroller_LevelChanged(object sender, IndexEventArgs e)
        {
            level = e.Index;
            updateLayout = true;
        }

        public override void PerformLayout(IGraphics gfx)
        {
            HideScroller();
            base.PerformLayout(gfx);
            if (VisibleChildCount == 0) return;
            if (VisibleChildCount > 1) throw new InvalidOperationException("VScrollPanel should have only one child.");
            Widget child = VisibleChildAt(0);
            childPreferredHeight = child.GetPreferredSize(gfx).Y;
            if (childPreferredHeight > Size.Y)
            {
                SetScrollerValues(childPreferredHeight, Size.Y, level);
                child.Position = new Vector2(0, -level);
                child.Size = new Vector2(Size.X - scroller.Width, childPreferredHeight);
                ShowScroller();
                scroller.PerformLayout(gfx);
            }
            else
            {
                child.Position = new Vector2();
                child.Size = Size;
                level = 0;
            }
            child.PerformLayout(gfx);
        }

        private void HideScroller()
        {
            scroller.Visible = false;
        }

        private void ShowScroller()
        {
            scroller.Visible = true;
            scroller.Height = Height + 4;
            scroller.Position = new Vector2(Left + Width - scroller.Width + 2, 0);
        }

        private void SetScrollerValues(int total, int visible, int level)
        {
            scroller.Total = total;
            scroller.Amount = visible;
            scroller.Level = level;
            Logger.Detail("GUI", $"VSP: {visible}/{total} - {level}");
        }

        public override Vector2 GetPreferredSize(IGraphics gfx)
        {
            updateLayout = false;
            Vector2 pf = new Vector2();
            Vector2 fs = FixedSize;
            Vector2 s = GetValidSize(pf, fs);
            HideScroller();
            if (VisibleChildCount == 1)
            {

                var cpf = VisibleChildAt(0).GetPreferredSize(gfx);
                childPreferredHeight = cpf.Y;
                pf += cpf;
                pf += new Vector2(scroller.Width, 0);
                SetScrollerValues(childPreferredHeight, childPreferredHeight, level);
            }
            return pf;
        }

        //public override bool MouseDragEvent(Vector2 p, Vector2 rel, MouseButton button)
        //{
        //    if (VisibleChildCount > 0 && childPreferredHeight > ClientHeight)
        //    {
        //        float scrollh = Height * Math.Min(1.0f, Height / (float)childPreferredHeight);
        //        //scroll = Math.Max(0.0f, Math.Min(1.0f, scroll + rel.Y / (Height - 8 - scrollh)));
        //        updateLayout = true;
        //        return true;
        //    }
        //    else
        //    {
        //        return base.MouseDragEvent(p, rel, button);
        //    }
        //}

        //public override bool ScrollEvent(Vector2 p, Vector2 rel)
        //{
        //    if (VisibleChildCount > 0 && childPreferredHeight > Height)
        //    {
        //        float scrollAmount = rel.Y * (Height / 20.0f);
        //        float scrollh = Height * Math.Min(1.0f, Height / (float)childPreferredHeight);
        //        //scroll = Math.Max(0.0f, Math.Min(1.0f, scroll - scrollAmount / (Height - 8 - scrollh)));
        //        updateLayout = true;
        //        return true;
        //    }
        //    else
        //    {
        //        return base.ScrollEvent(p, rel);
        //    }
        //}

        public override void Render(IGraphics gfx)
        {
            HideScroller();
            if (VisibleChildCount == 0) return;
            Widget child = VisibleChildAt(0);
            child.Position = new Vector2(0, -level);
            childPreferredHeight = child.GetPreferredSize(gfx).Y;
            if (updateLayout) child.PerformLayout(gfx);
            if (child.Visible)
            {
                gfx.SaveState();
                gfx.Translate(Left, Top);
                gfx.SetClip(0, 0, Width, Height);
                child.Render(gfx);
                gfx.ClearClip();
                gfx.RestoreState();
            }
            if (childPreferredHeight <= Height) return;
            ShowScroller();
            scroller.Render(gfx);
        }


    }
}
